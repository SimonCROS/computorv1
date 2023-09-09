namespace Computorv1Tests.Functional;

using Computorv1;
using Computorv1.Tokens;

[TestClass]
public class LexingTests
{
    [TestMethod]
    public void Subject1NoBlanks()
    {
        Assert.IsTrue(new Lexer("5*X^0+4*X^1-9.3*X^2=1*X^0").Tokenize(out List<Token> tokens));
        CollectionAssert.AreEqual(
            new List<Token>()
            {
                new NumberToken(5f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
                new AddToken(),
                new NumberToken(4f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(1f),
                new SubToken(),
                new NumberToken(9.3f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(2f),
                new EqualToken(),
                new NumberToken(1f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
            },
            tokens);
    }

    [TestMethod]
    public void Subject1LotOfBlanks()
    {
        Assert.IsTrue(new Lexer("   5 *X^   0+ 4 * X^1 -9.3*X^  2=1*X^ 0    ").Tokenize(out List<Token> tokens));
        CollectionAssert.AreEqual(
            new List<Token>()
            {
                new NumberToken(5f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
                new AddToken(),
                new NumberToken(4f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(1f),
                new SubToken(),
                new NumberToken(9.3f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(2f),
                new EqualToken(),
                new NumberToken(1f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
            },
            tokens);
    }

    [TestMethod]
    public void Subject2()
    {
        Assert.IsTrue(new Lexer("5 * X^0 + 4 * X^1 = 4 * X^0").Tokenize(out List<Token> tokens));
        CollectionAssert.AreEqual(
            new List<Token>()
            {
                new NumberToken(5f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
                new AddToken(),
                new NumberToken(4f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(1f),
                new EqualToken(),
                new NumberToken(4f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
            },
            tokens);
    }

    [TestMethod]
    public void Subject3()
    {
        Assert.IsTrue(new Lexer("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0").Tokenize(out List<Token> tokens));
        CollectionAssert.AreEqual(
            new List<Token>()
            {
                new NumberToken(8f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
                new SubToken(),
                new NumberToken(6f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(1f),
                new AddToken(),
                new NumberToken(0f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(2f),
                new SubToken(),
                new NumberToken(5.6f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(3f),
                new EqualToken(),
                new NumberToken(3f),
                new MulToken(),
                new IdentifierToken("X"),
                new PowToken(),
                new NumberToken(0f),
            },
            tokens);
    }

    [TestMethod]
    public void UnknownToken1()
    {
        Assert.IsFalse(new Lexer("°_°").Tokenize(out _));
    }

    [TestMethod]
    public void UnknownToken2()
    {
        Assert.IsFalse(new Lexer("x(2 + 2)").Tokenize(out _));
    }

    [TestMethod]
    public void BadFloat()
    {
        Assert.IsFalse(new Lexer("12.34.56").Tokenize(out _));
    }

    [TestMethod]
    public void FloatOverflow()
    {
        Assert.IsFalse(new Lexer("1234567890123456789012345678901234567890").Tokenize(out _));
    }
}
