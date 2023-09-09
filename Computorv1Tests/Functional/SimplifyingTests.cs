namespace Computorv1Tests.Functional;

using Computorv1;
using Computorv1.Tokens;
using Computorv1.Nodes;

[TestClass]
public class SimplifyingTests
{
    [TestMethod]
    public void Subject1()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("5 + 4 * X - 9.3 * X ^ 2 - 1", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("5 + 4 * X - 4", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("8 - 6 * X - 5.6 * X ^ 3 - 3", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Number_Number()
    {
        Assert.IsTrue(new Lexer("2 + 2 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Number_Zero()
    {
        Assert.IsTrue(new Lexer("4 + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Identifier_Zero()
    {
        Assert.IsTrue(new Lexer("X + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X - 4", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Zero_Identifier()
    {
        Assert.IsTrue(new Lexer("0 + X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X - 4", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Number_Number()
    {
        Assert.IsTrue(new Lexer("8 - 4 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Number_Zero()
    {
        Assert.IsTrue(new Lexer("4 - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Identifier_Zero()
    {
        Assert.IsTrue(new Lexer("X - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X - 4", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Zero_Identifier()
    {
        Assert.IsTrue(new Lexer("0 - X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0 - X - 4", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Identifier_Identifier()
    {
        Assert.IsTrue(new Lexer("X - X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_SameParts()
    {
        Assert.IsTrue(new Lexer("1 * X ^ 4 * 7 ^ X - 1 * X ^ 4 * 7 ^ X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Number_Number()
    {
        Assert.IsTrue(new Lexer("6 * 7 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_One_Identifier()
    {
        Assert.IsTrue(new Lexer("1 * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_One()
    {
        Assert.IsTrue(new Lexer("X * 1 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Number_Identifier()
    {
        Assert.IsTrue(new Lexer("6 * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("6 * X - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Number()
    {
        Assert.IsTrue(new Lexer("X * 6 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("6 * X - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Identifier_To_Exponent()
    {
        Assert.IsTrue(new Lexer("X * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X ^ 2 - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Identifier_Identifier_To_Exponent()
    {
        Assert.IsTrue(new Lexer("X * X * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X ^ 3 - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Pow_Zero_Zero()
    {
        Assert.IsTrue(new Lexer("0 ^ 0 = 1").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("0", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void X_Mul_X_Pow_Increment_Exponent()
    {
        Assert.IsTrue(new Lexer("X * X ^ 2 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X ^ 3 - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void X_Pow_Mul_X_Increment_Exponent()
    {
        Assert.IsTrue(new Lexer("X ^ 2 * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("X ^ 3 - 42", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Nested_Multiplication_With_One_Identifier()
    {
        Assert.AreEqual("6 * x", Utils.Simplify(
            new MulNode(
                new MulNode(new NumberNode(3), new IdentifierNode("x")),
                new NumberNode(2)
            )).ToString());
        Assert.AreEqual("6 * x", Utils.Simplify(
            new MulNode(
                new MulNode(new IdentifierNode("x"), new NumberNode(3)),
                new NumberNode(2)
            )).ToString());
        Assert.AreEqual("6 * x", Utils.Simplify(
            new MulNode(
                new NumberNode(2),
                new MulNode(new NumberNode(3), new IdentifierNode("x"))
            )).ToString());
        Assert.AreEqual("6 * x", Utils.Simplify(
            new MulNode(
                new NumberNode(2),
                new MulNode(new IdentifierNode("x"), new NumberNode(3))
            )).ToString());
    }

    [TestMethod]
    public void Nested_Multiplication_With_Two_Identifiers()
    {
        Assert.AreEqual("2 * y * x", Utils.Simplify(
            new MulNode(
                new MulNode(new IdentifierNode("y"), new IdentifierNode("x")),
                new NumberNode(2)
            )).ToString());
        Assert.AreEqual("2 * x * y", Utils.Simplify(
            new MulNode(
                new MulNode(new IdentifierNode("x"), new IdentifierNode("y")),
                new NumberNode(2)
            )).ToString());
        Assert.AreEqual("2 * y * x", Utils.Simplify(
            new MulNode(
                new NumberNode(2),
                new MulNode(new IdentifierNode("y"), new IdentifierNode("x"))
            )).ToString());
        Assert.AreEqual("2 * x * y", Utils.Simplify(
            new MulNode(
                new NumberNode(2),
                new MulNode(new IdentifierNode("x"), new IdentifierNode("y"))
            )).ToString());
    }
}
