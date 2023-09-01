namespace Tests;

using System.Globalization;
using computorv1;
using computorv1.Nodes;
using computorv1.Tokens;

[TestClass]
public class ParsingTest
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US"); 
    }
    
    public Node Monominal(float coefficient, float exponent)
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
        bool result = new Lexer("5*X^0+4*X^1-9.3*X^2=1*X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new AddNode(
                        Monominal(5f, 0f),
                        Monominal(4f, 1f)),
                    Monominal(9.3f, 2f)),
                Monominal(1f, 0f)),
            node);
    }

    [TestMethod]
    public void Subject2()
    {
        bool result = new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual(
            new EqualNode(
                new AddNode(
                    Monominal(5f, 0f),
                    Monominal(4f, 1f)),
                Monominal(4f, 0f)),
            node);
    }

    [TestMethod]
    public void Subject3()
    {
        bool result = new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual(
            new EqualNode(
                new SubNode(
                    new AddNode(
                        new SubNode(
                            Monominal(8f, 0f),
                            Monominal(6f, 1f)),
                        Monominal(0f, 2f)),
                    Monominal(5.6f, 3f)),
                Monominal(3f, 0f)),
            node);
    }

    [TestMethod]
    public void NoEqualSign()
    {
        bool result = new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 + 3 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void MultipleEqualSign()
    {
        bool result = new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 = 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void DoublePlusSign()
    {
        bool result = new Lexer("2 + + 4 = 8").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void DoubleMulSign()
    {
        bool result = new Lexer("2 * * 4 = 8").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void NoEqualLhs()
    {
        bool result = new Lexer("= 8").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void NoEqualRhs()
    {
        bool result = new Lexer("2 =").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        Assert.IsFalse(new Parser(tokens).Parse(out _));
    }

    [TestMethod]
    public void SimpleScalarAroundEqual()
    {
        bool result = new Lexer("2 = 8").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual(new EqualNode(new NumberNode(2f), new NumberNode(8f)), node);
    }

	// TODO Check in another file
    [TestMethod]
    public void MultipleIdentifiers()
    {
        bool result = new Lexer("X + Y = Z").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
		Assert.IsTrue(false);
    }
}
