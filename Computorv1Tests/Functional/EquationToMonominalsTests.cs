namespace Computorv1Tests.Functional;

using Computorv1;
using Computorv1.Nodes;
using Computorv1.Tokens;

[TestClass]
public class EquationToMonominalsTests
{
    [TestMethod]
    public void Subject1()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(-9.3f, "X", 2), new(4, "X"), new(4) },
            Utils.ListMonominals(node));
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(4, "X"), new(1) },
            Utils.ListMonominals(node));
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        node = Utils.Simplify(node);
        CollectionAssert.AreEqual(
            new List<Monominal>()
                { new(-5.6f, "X", 3), new(-6, "X"), new(5) },
            Utils.ListMonominals(node));
    }

    [TestMethod]
    public void Equal_In_ListMonominals()
    {
        Assert.ThrowsException<NotSupportedException>(() => Utils.ListMonominals(new EqualNode(new NumberNode(1), new NumberNode(2))));
    }
}
