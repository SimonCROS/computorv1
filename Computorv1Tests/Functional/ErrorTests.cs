namespace Computorv1Tests.Functional;

using System.Globalization;
using computorv1;

[TestClass]
public class ErrorTests
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [TestMethod]
    public void FloatExponent()
    {
        (int code, string output) = CapturedOutput("5 ^ 2.5 = x");
        Assert.AreEqual(1, code);
        Assert.AreEqual("Error: Invalid expression: cannot use a floating number as exponent\n", output);
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
