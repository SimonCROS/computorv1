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
    public void SimpleAddition_NumberPlusNumber()
    {
        Assert.IsTrue(new Lexer("2 + 2 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_NumberPlusZero()
    {
        Assert.IsTrue(new Lexer("4 + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_IdentifierPlusZero()
    {
        Assert.IsTrue(new Lexer("X + 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_ZeroPlusIdentifier()
    {
        Assert.IsTrue(new Lexer("0 + X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_NumberMinusNumber()
    {
        Assert.IsTrue(new Lexer("8 - 4 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_NumberMinusZero()
    {
        Assert.IsTrue(new Lexer("4 - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_IdentifierMinusZero()
    {
        Assert.IsTrue(new Lexer("X - 0 = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_ZeroMinusIdentifier()
    {
        Assert.IsTrue(new Lexer("0 - X = 4").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("((0 - X) = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_IdentifierMinusIdentifier()
    {
        Assert.IsTrue(new Lexer("X - X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_SameParts()
    {
        Assert.IsTrue(new Lexer("1 * X ^ 4 * 7 ^ X - 1 * X ^ 4 * 7 ^ X = 0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }
}
