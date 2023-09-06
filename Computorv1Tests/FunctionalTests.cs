namespace Computorv1Tests.Functional;

using System.Globalization;
using computorv1;

[TestClass]
public class FunctionalTests
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }

    [TestMethod]
    public void Subject1()
    {
        (int code, string output) = CapturedOutput("5 * X^0 + 4 * X^1 - 9.3 * X^2 = 1 * X^0");
        Assert.AreEqual(0, code);
        Assert.AreEqual(
            ExpectedOutput("-9.3 * X^2 + 4 * X + 4 = 0", 2, true, "0.9052389", "-0.47513145"),
            output);
    }

    [TestMethod]
    public void Subject2()
    {
        (int code, string output) = CapturedOutput("5 * X^0 + 4 * X^1 = 4 * X^0");
        Assert.AreEqual(0, code);
        Assert.AreEqual(
            ExpectedOutput("4 * X + 1 = 0", 1, "-0.25"),
            output);
    }

    [TestMethod]
    public void Subject3()
    {
        (int code, string output) = CapturedOutput("8 * X^0 - 6 * X^1 + 0 * X^2 - 5.6 * X^3 = 3 * X^0");
        Assert.AreEqual(1, code);
        Assert.AreEqual(
            ExpectedNoSolutionOutput("-5.6 * X^3 - 6 * X + 5 = 0", 3, "The polynomial degree is strictly greater than 2, I can't solve."),
            output);
    }

    [TestMethod]
    public void NegativeDiscriminant()
    {
        (int code, string output) = CapturedOutput("5 * X^2 + 20 * X + 32 = 0");
        Assert.AreEqual(0, code);
        Assert.AreEqual(
            ExpectedOutput("5 * X^2 + 20 * X + 32 = 0", 2, false, "-2 + 1.5491934i", "-2 - 1.5491934i"),
            output);
    }

    [TestMethod]
    public void SubjectBonus()
    {
        (int code, string output) = CapturedOutput("5 + 4 * X + X^2= X^2");
        Assert.AreEqual(0, code);
        Assert.AreEqual(
            ExpectedOutput("4 * X + 5 = 0", 1, "-1.25"),
            output);
    }

    [TestMethod]
    public void X_Equal_X_Plus_2_NoSolution()
    {
        (int code, string output) = CapturedOutput("x + 2 = x");
        Assert.AreEqual(0, code);
        Assert.AreEqual(
            ExpectedNoSolutionOutput("2 = 0", 0, "No solution"),
            output);
    }

    private static string ExpectedNoSolutionOutput(string reduced, int degree, string error)
    {
        return @$"Reduced form: {reduced}
Polynomial degree: {degree}
{error}
";
    }

    private static string ExpectedOutput(string reduced, int degree, string solution)
    {
        return @$"Reduced form: {reduced}
Polynomial degree: {degree}
The solution is:
{solution}
";
    }

    private static string ExpectedOutput(string reduced, int degree, bool discriminantPositive, string solution1, string solution2)
    {
        return @$"Reduced form: {reduced}
Polynomial degree: {degree}
Discriminant is strictly {(discriminantPositive ? "positive" : "negative")}, the two solutions are:
{solution1}
{solution2}
";
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
