namespace Tests;

using System.Globalization;
using computorv1;
using computorv1.Nodes;
using computorv1.Tokens;

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
}
