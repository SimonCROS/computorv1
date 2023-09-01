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
        bool result = new Lexer("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(((5 + (4 * X)) - (9.3 * (X ^ 2))) = 1)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject2()
    {
        bool result = new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("((5 + (4 * X)) = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void Subject3()
    {
        bool result = new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(((8 - (6 * X)) - (5.6 * (X ^ 3))) = 3)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_NumberPlusNumber()
    {
        bool result = new Lexer("2 + 2 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_NumberPlusZero()
    {
        bool result = new Lexer("4 + 0 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_IdentifierPlusZero()
    {
        bool result = new Lexer("X + 0 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleAddition_ZeroPlusIdentifier()
    {
        bool result = new Lexer("0 + X = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_NumberMinusNumber()
    {
        bool result = new Lexer("8 - 4 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_NumberMinusZero()
    {
        bool result = new Lexer("4 - 0 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(4 = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_IdentifierMinusZero()
    {
        bool result = new Lexer("X - 0 = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(X = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_ZeroMinusIdentifier()
    {
        bool result = new Lexer("0 - X = 4").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("((0 - X) = 4)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_IdentifierMinusIdentifier()
    {
        bool result = new Lexer("X - X = 0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }

    [TestMethod]
    public void SimpleSubtraction_SameParts()
    {
        bool result = new Lexer("1 * X ^ 4 * 7 ^ X - 1 * X ^ 4 * 7 ^ X = 0").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.AreEqual("(0 = 0)", Utils.Simplify(node!).ToString());
    }
}
