namespace Computorv1Tests.Unit;

using Computorv1;
using Computorv1.Nodes;
using Computorv1.Tokens;

[TestClass]
public class ParsingTests
{
    public static Node Mono(float coefficient, float exponent)
    {
        return new MulNode(
            new NumberNode(coefficient),
            new PowNode(
                new IdentifierNode("X"),
                new NumberNode(exponent)));
    }

    [TestMethod]
    public void Subject1()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new AddNode(
                        Mono(5f, 0f),
                        Mono(4f, 1f)),
                    Mono(9.3f, 2f)),
                Mono(1f, 0f)),
            node);
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual(
            new EqualNode(
                new AddNode(
                    Mono(5f, 0f),
                    Mono(4f, 1f)),
                Mono(4f, 0f)),
            node);
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new AddNode(
                        new SubNode(
                            Mono(8f, 0f),
                            Mono(6f, 1f)),
                        Mono(0f, 2f)),
                    Mono(5.6f, 3f)),
                Mono(3f, 0f)),
            node);
    }

    [TestMethod]
    public void No_Equal()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(5f),
            new MulToken(),
            new IdentifierToken("X"),
            new PowToken(),
            new NumberToken(0f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected end of file\n", Output);
    }

    [TestMethod]
    public void Multiple_Equal()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(5f),
            new MulToken(),
            new IdentifierToken("X"),
            new PowToken(),
            new NumberToken(0f),
            new EqualToken(),
            new NumberToken(5f),
            new EqualToken(),
            new MulToken(),
            new IdentifierToken("X"),
            new PowToken(),
            new NumberToken(0f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: =\n", Output);
    }

    [TestMethod]
    public void Consecutive_Mul_Sign()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(0f),
            new MulToken(),
            new MulToken(),
            new NumberToken(5f),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: *\n", Output);
    }

    [TestMethod]
    public void Consecutive_Plus_Sign()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(0f),
            new AddToken(),
            new AddToken(),
            new NumberToken(5f),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: +\n", Output);
    }

    [TestMethod]
    public void Consecutive_Sub_Sign()
    {
        (bool Ok, string _) = CapturedOutput(new List<Token>
        {
            new NumberToken(0f),
            new SubToken(),
            new SubToken(),
            new NumberToken(5f),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? node);
        Assert.IsTrue(Ok);
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new NumberNode(0f),
                    new NumberNode(-5f)),
                new NumberNode(5f)
            ),
            node);
    }

    [TestMethod]
    public void Consecutive_Pow_Sign()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(0f),
            new PowToken(),
            new PowToken(),
            new NumberToken(5f),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: ^\n", Output);
    }

    [TestMethod]
    public void Add_After_Sub()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(0f),
            new SubToken(),
            new AddToken(),
            new NumberToken(5f),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: +\n", Output);
    }

    [TestMethod]
    public void Nothing_Before_Equal()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected token: =\n", Output);
    }

    [TestMethod]
    public void Nothing_After_Equal()
    {
        (bool Ok, string Output) = CapturedOutput(new List<Token>
        {
            new NumberToken(5f),
            new EqualToken(),
        }, out Node? _);
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: unexpected end of file\n", Output);
    }

    [TestMethod]
    public void Minus_Identifier()
    {
        (bool Ok, string _) = CapturedOutput(new List<Token>
        {
            new SubToken(),
            new IdentifierToken("X"),
            new EqualToken(),
            new NumberToken(5f),
        }, out Node? node);
        Assert.IsTrue(Ok);
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new NumberNode(0f),
                    new IdentifierNode("X")),
                new NumberNode(5f)
            ),
            node);
    }

    private static (bool Ok, string Output) CapturedOutput(List<Token> tokens, out Node? result)
    {
        TextWriter originalStdOut = Console.Out;

        using var newStdOut = new StringWriter();
        Console.SetOut(newStdOut);
        bool ret = new Parser(tokens).Parse(out result);
        Console.SetOut(originalStdOut);

        return (ret, newStdOut.ToString());
    }
}
