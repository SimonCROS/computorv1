namespace Tests;

using System.Globalization;
using computorv1;
using computorv1.Nodes;
using computorv1.Tokens;

[TestClass]
public class EquationToMonominalsTests
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
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(5), new(4, "X"), new(-9.3f, "X", 2) },
            Utils.ListMonominals(((EqualNode)node).Left));
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(1) },
            Utils.ListMonominals(((EqualNode)node).Right));
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(5), new(4, "X") },
            Utils.ListMonominals(((EqualNode)node).Left));
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(4) },
            Utils.ListMonominals(((EqualNode)node).Right));
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(8), new(-6, "X"), new(-5.6f, "X", 3) },
            Utils.ListMonominals(((EqualNode)node).Left));
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(3) },
            Utils.ListMonominals(((EqualNode)node).Right));
    }

    [TestMethod]
    public void LongExample()
    {
        Assert.IsTrue(new Lexer("3 * x^5 - 7 * x^4 + 2 * x^3 + 6 * x^2 - 5 * x + 9 = 4 * x^5 - 2 * x^4 + 8 * x^3 - 6 * x^2 + 7 * x - 12").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        CollectionAssert.AreEqual(
            new List<string>()
                { "(3x ^ 5)", "(-7x ^ 4)", "(2x ^ 3)", "(6x ^ 2)", "-5x", "9" },
            Utils.ListMonominals(((EqualNode)node).Left).ConvertAll(c => c.ToString()));
        CollectionAssert.AreEqual(
            new List<string>()
                { "(4x ^ 5)", "(-2x ^ 4)", "(8x ^ 3)", "(-6x ^ 2)", "7x", "-12" },
            Utils.ListMonominals(((EqualNode)node).Right).ConvertAll(c => c.ToString()));
    }
}
