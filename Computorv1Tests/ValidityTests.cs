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
        bool result = new Lexer("X + Y = Z").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.IsTrue(false);
    }

    [TestMethod]
    public void IdentifierAsExponent()
    {
        bool result = new Lexer("2 ^ X = 42").Tokenize(out List<Token> tokens);
        Assert.IsTrue(result);
        result = new Parser(tokens).Parse(out EqualNode? node);
        Assert.IsTrue(result);
        Assert.IsTrue(false);
    }
}
