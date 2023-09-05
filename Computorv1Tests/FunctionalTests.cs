namespace Computorv1Tests.Functional;

using System.Globalization;

[TestClass]
public class FunctionalTests
{
    [TestInitialize]
    public void SetCulture()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
    }
}
