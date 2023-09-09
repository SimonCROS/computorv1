namespace Computorv1Tests;

using System.Globalization;

[TestClass]
public class GlobalTestInitializer
{
    [AssemblyInitialize]
    public static void SetCulture(TestContext _)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }
}
