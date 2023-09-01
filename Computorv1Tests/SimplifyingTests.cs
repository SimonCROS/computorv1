namespace Tests;

using System.Globalization;
using computorv1;
using computorv1.Tokens;
using computorv1.Nodes;

[TestClass]
public class SimplifyingTests
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [TestMethod]
    public void Subject1()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(((5 + (4 * X)) - (9.3 * (X ^ 2))) = 1)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((5 + (4 * X)) = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(((8 - (6 * X)) - (5.6 * (X ^ 3))) = 3)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Number_Number()
    {
        Assert.IsTrue(new Lexer("2 + 2 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Number_Zero()
    {
        Assert.IsTrue(new Lexer("4 + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Identifier_Zero()
    {
        Assert.IsTrue(new Lexer("X + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Addition_Zero_Identifier()
    {
        Assert.IsTrue(new Lexer("0 + X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Number_Number()
    {
        Assert.IsTrue(new Lexer("8 - 4 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Number_Zero()
    {
        Assert.IsTrue(new Lexer("4 - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Identifier_Zero()
    {
        Assert.IsTrue(new Lexer("X - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Zero_Identifier()
    {
        Assert.IsTrue(new Lexer("0 - X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((0 - X) = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_Identifier_Identifier()
    {
        Assert.IsTrue(new Lexer("X - X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subtraction_SameParts()
    {
        Assert.IsTrue(new Lexer("1 * X ^ 4 * 7 ^ X - 1 * X ^ 4 * 7 ^ X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Number_Number()
    {
        Assert.IsTrue(new Lexer("6 * 7 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(42 = 42)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Number_Identifier()
    {
        Assert.IsTrue(new Lexer("6 * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((6 * X) = 42)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Number()
    {
        Assert.IsTrue(new Lexer("X * 6 = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((6 * X) = 42)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Identifier_To_Exponent()
    {
        Assert.IsTrue(new Lexer("X * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((X ^ 2) = 42)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Multiplication_Identifier_Identifier_Identifier_To_Exponent()
    {
        Assert.IsTrue(new Lexer("X * X * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((X ^ 3) = 42)", Utils.Simplify(node!).ToString());
    }
}
