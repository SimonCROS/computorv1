namespace Computorv1Tests.Functional;

using Computorv1;

[TestClass]
public class ErrorTests
{
    [TestMethod]
    public void FloatExponent()
    {
        (int code, string output) = CapturedOutput("5 ^ 2.5 = x");
        Assert.AreEqual(1, code);
        Assert.AreEqual("Error: cannot use a floating number as exponent\n", output);
    }

    [TestMethod]
    public void NegativeExponent()
    {
        (int code, string output) = CapturedOutput("5 ^ -2 = x");
        Assert.AreEqual(1, code);
        Assert.AreEqual("Error: cannot use a negative number as exponent\n", output);
    }

    [TestMethod]
    public void TokenizerError()
    {
        (int code, string output) = CapturedOutput("5 _^ 2 = x");
        Assert.AreEqual(1, code);
        Assert.AreEqual("Error: unexpected token at pos 3: _\n", output);
    }

    [TestMethod]
    public void ParsingError()
    {
        (int code, string output) = CapturedOutput("1 = 2 = 3");
        Assert.AreEqual(1, code);
        Assert.AreEqual("Error: unexpected token: =\n", output);
    }

    private static (int Code, string Output) CapturedOutput(string equation)
    {
        TextWriter originalStdOut = Console.Out;

        using var newStdOut = new StringWriter();
        Console.SetOut(newStdOut);
        int ret = Solver.Solve(equation);
        Console.SetOut(originalStdOut);

        return (ret, newStdOut.ToString());
    }
}
