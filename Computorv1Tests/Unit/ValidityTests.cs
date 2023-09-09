namespace Computorv1Tests.Unit;

using System.Globalization;
using Computorv1;
using Computorv1.Nodes;
using Computorv1.Tokens;

[TestClass]
public class ValidityTests
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture; 
    }

    [TestMethod]
    public void MultipleIdentifiers()
    {
        Assert.IsTrue(new Lexer("X + Y = Z").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.IsFalse(new Validator(1).Validate(node!));
    }

    [TestMethod]
    public void IdentifierAsExponent()
    {
        Assert.IsTrue(new Lexer("2 ^ X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.IsFalse(new Validator(1).Validate(node!));
    }

    [TestMethod]
    public void NestedNonConstMultiplication()
    {
        Assert.IsTrue(new Lexer("2 * X * X = 42").Tokenize(out List<Token> tokens));
        Assert.IsTrue(new Parser(tokens).Parse(out Node? node));
        Assert.IsFalse(new Validator(1).Validate(node!));
    }
}
