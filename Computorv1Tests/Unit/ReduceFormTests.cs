namespace Computorv1Tests.Unit;

using Computorv1;

[TestClass]
public class ReduceFormTest
{
    [TestMethod]
    public void ReducedForm_Zero_Coeff()
    {
        Assert.AreEqual("0", Solver.GetReducedForm(new List<Monominal> { new(0, "X", 2) }));
    }
}
