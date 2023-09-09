namespace Computorv1Tests.Unit;

using Computorv1;
using Computorv1.Nodes;

[TestClass]
public class ValidityTests
{
    [TestMethod]
    public void Addition_With_Number_Equality()
    {
        (bool Ok, string Output) = CapturedOutput(
            new EqualNode(
                new AddNode(
                    new NumberNode(4),
                    new NumberNode(2)
                ),
                new NumberNode(6)
            )
        );
        Assert.IsTrue(Ok);
        Assert.AreEqual("", Output);
    }

    [TestMethod]
    public void EqualNotAtTop()
    {
        (bool Ok, string Output) = CapturedOutput(
            new AddNode(
                new NumberNode(4),
                new EqualNode(
                    new NumberNode(2),
                    new NumberNode(2)
                )
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: = should be on the top level\n", Output);
    }

    [TestMethod]
    public void Nested_Multiplication()
    {
        (bool Ok, string Output) = CapturedOutput(
            new MulNode(
                new NumberNode(4),
                new MulNode(
                    new NumberNode(2),
                    new NumberNode(2)
                )
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: non constant nested multiplication\n", Output);
    }

    [TestMethod]
    public void Nested_Addition()
    {
        (bool Ok, string Output) = CapturedOutput(
            new MulNode(
                new NumberNode(4),
                new AddNode(
                    new NumberNode(2),
                    new NumberNode(2)
                )
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: addition or subtraction should not be inside a multiplication\n", Output);
    }

    [TestMethod]
    public void Nested_Subtraction()
    {
        (bool Ok, string Output) = CapturedOutput(
            new MulNode(
                new NumberNode(4),
                new SubNode(
                    new NumberNode(2),
                    new NumberNode(2)
                )
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: addition or subtraction should not be inside a multiplication\n", Output);
    }

    [TestMethod]
    public void Non_Integer_Exponent()
    {
        (bool Ok, string Output) = CapturedOutput(
            new PowNode(
                new NumberNode(4),
                new NumberNode(2.5f)
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: cannot use a floating number as exponent\n", Output);
    }

    [TestMethod]
    public void Non_Const_Exponent()
    {
        (bool Ok, string Output) = CapturedOutput(
            new PowNode(
                new NumberNode(4),
                new IdentifierNode("X")
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: a const number must be used as exponent\n", Output);
    }

    [TestMethod]
    public void Multiple_Identifiers()
    {
        (bool Ok, string Output) = CapturedOutput(
            new AddNode(
                new IdentifierNode("X"),
                new IdentifierNode("Y")
            )
        );
        Assert.IsFalse(Ok);
        Assert.AreEqual("Error: too many identifiers (max 1)\n", Output);
    }

    private static (bool Ok, string Output) CapturedOutput(Node node)
    {
        TextWriter originalStdOut = Console.Out;

        using var newStdOut = new StringWriter();
        Console.SetOut(newStdOut);
        bool ret = new Validator(1).Validate(node);
        Console.SetOut(originalStdOut);

        return (ret, newStdOut.ToString());
    }
}
