namespace Computorv1Tests.Unit;

using Computorv1;

[TestClass]
public class MathTests
{
    [TestMethod]
    public void Sqrt_With_Integer()
    {
        Assert.AreEqual(2, MyMathF.Sqrt(4));
        Assert.AreEqual(3, MyMathF.Sqrt(9));
        Assert.AreEqual(4, MyMathF.Sqrt(16));
        Assert.AreEqual(5, MyMathF.Sqrt(25));
        Assert.AreEqual(6, MyMathF.Sqrt(36));
        Assert.AreEqual(7, MyMathF.Sqrt(49));
        Assert.AreEqual(8, MyMathF.Sqrt(64));
        Assert.AreEqual(9, MyMathF.Sqrt(81));
        Assert.AreEqual(10, MyMathF.Sqrt(100));
        Assert.AreEqual(11, MyMathF.Sqrt(121));
        Assert.AreEqual(12, MyMathF.Sqrt(144));
        Assert.AreEqual(13, MyMathF.Sqrt(169));
        Assert.AreEqual(14, MyMathF.Sqrt(196));
        Assert.AreEqual(15, MyMathF.Sqrt(225));
        Assert.AreEqual(16, MyMathF.Sqrt(256));
        Assert.AreEqual(17, MyMathF.Sqrt(289));
        Assert.AreEqual(18, MyMathF.Sqrt(324));
        Assert.AreEqual(19, MyMathF.Sqrt(361));
        Assert.AreEqual(20, MyMathF.Sqrt(400));
        Assert.AreEqual(21, MyMathF.Sqrt(441));
        Assert.AreEqual(22, MyMathF.Sqrt(484));
        Assert.AreEqual(23, MyMathF.Sqrt(529));
        Assert.AreEqual(24, MyMathF.Sqrt(576));
        Assert.AreEqual(25, MyMathF.Sqrt(625));
        Assert.AreEqual(26, MyMathF.Sqrt(676));
        Assert.AreEqual(27, MyMathF.Sqrt(729));
        Assert.AreEqual(28, MyMathF.Sqrt(784));
        Assert.AreEqual(29, MyMathF.Sqrt(841));
        Assert.AreEqual(30, MyMathF.Sqrt(900));
        Assert.AreEqual(31, MyMathF.Sqrt(961));
        Assert.AreEqual(32, MyMathF.Sqrt(1024));
        Assert.AreEqual(33, MyMathF.Sqrt(1089));
        Assert.AreEqual(34, MyMathF.Sqrt(1156));
        Assert.AreEqual(35, MyMathF.Sqrt(1225));
        Assert.AreEqual(36, MyMathF.Sqrt(1296));
        Assert.AreEqual(37, MyMathF.Sqrt(1369));
        Assert.AreEqual(38, MyMathF.Sqrt(1444));
        Assert.AreEqual(39, MyMathF.Sqrt(1521));
        Assert.AreEqual(40, MyMathF.Sqrt(1600));
        Assert.AreEqual(41, MyMathF.Sqrt(1681));
        Assert.AreEqual(42, MyMathF.Sqrt(1764));
        Assert.AreEqual(43, MyMathF.Sqrt(1849));
        Assert.AreEqual(44, MyMathF.Sqrt(1936));
    }

    [TestMethod]
    public void Sqrt_With_Float()
    {
        Assert.AreEqual(2.236068f, MyMathF.Sqrt(5), 0.000001f);
        Assert.AreEqual(2.645751f, MyMathF.Sqrt(7), 0.000001f);
        Assert.AreEqual(3.162278f, MyMathF.Sqrt(10), 0.000001f);
        Assert.AreEqual(3.605551f, MyMathF.Sqrt(13), 0.000001f);
        Assert.AreEqual(4.123106f, MyMathF.Sqrt(17), 0.000001f);
        Assert.AreEqual(4.582576f, MyMathF.Sqrt(21), 0.000001f);
        Assert.AreEqual(5.099019f, MyMathF.Sqrt(26), 0.000001f);
        Assert.AreEqual(5.567764f, MyMathF.Sqrt(31), 0.000001f);
        Assert.AreEqual(6.082762f, MyMathF.Sqrt(37), 0.000001f);
        Assert.AreEqual(6.557439f, MyMathF.Sqrt(43), 0.000001f);
        Assert.AreEqual(7.071068f, MyMathF.Sqrt(50), 0.000001f);
        Assert.AreEqual(7.549834f, MyMathF.Sqrt(57), 0.000001f);
        Assert.AreEqual(8.062258f, MyMathF.Sqrt(65), 0.000001f);
    }

    [TestMethod]
    public void Pow_With_Integer_Two()
    {
        Assert.AreEqual(4, MyMathF.Pow(2, 2));
        Assert.AreEqual(4, MyMathF.Pow(2, 2));
        Assert.AreEqual(9, MyMathF.Pow(3, 2));
        Assert.AreEqual(16, MyMathF.Pow(4, 2));
        Assert.AreEqual(25, MyMathF.Pow(5, 2));
        Assert.AreEqual(36, MyMathF.Pow(6, 2));
        Assert.AreEqual(49, MyMathF.Pow(7, 2));
        Assert.AreEqual(64, MyMathF.Pow(8, 2));
        Assert.AreEqual(81, MyMathF.Pow(9, 2));
        Assert.AreEqual(100, MyMathF.Pow(10, 2));
        Assert.AreEqual(121, MyMathF.Pow(11, 2));
        Assert.AreEqual(144, MyMathF.Pow(12, 2));
        Assert.AreEqual(169, MyMathF.Pow(13, 2));
        Assert.AreEqual(196, MyMathF.Pow(14, 2));
        Assert.AreEqual(225, MyMathF.Pow(15, 2));
        Assert.AreEqual(256, MyMathF.Pow(16, 2));
        Assert.AreEqual(289, MyMathF.Pow(17, 2));
        Assert.AreEqual(324, MyMathF.Pow(18, 2));
        Assert.AreEqual(361, MyMathF.Pow(19, 2));
        Assert.AreEqual(400, MyMathF.Pow(20, 2));
        Assert.AreEqual(441, MyMathF.Pow(21, 2));
        Assert.AreEqual(484, MyMathF.Pow(22, 2));
        Assert.AreEqual(529, MyMathF.Pow(23, 2));
        Assert.AreEqual(576, MyMathF.Pow(24, 2));
        Assert.AreEqual(625, MyMathF.Pow(25, 2));
        Assert.AreEqual(676, MyMathF.Pow(26, 2));
        Assert.AreEqual(729, MyMathF.Pow(27, 2));
        Assert.AreEqual(784, MyMathF.Pow(28, 2));
        Assert.AreEqual(841, MyMathF.Pow(29, 2));
        Assert.AreEqual(900, MyMathF.Pow(30, 2));
        Assert.AreEqual(961, MyMathF.Pow(31, 2));
    }

    [TestMethod]
    public void Pow_With_Float_Two()
    {
        Assert.AreEqual(6.25f, MyMathF.Pow(2.5f, 2));
        Assert.AreEqual(6.25f, MyMathF.Pow(2.5f, 2));
        Assert.AreEqual(12.25f, MyMathF.Pow(3.5f, 2));
        Assert.AreEqual(20.25f, MyMathF.Pow(4.5f, 2));
        Assert.AreEqual(30.25f, MyMathF.Pow(5.5f, 2));
        Assert.AreEqual(42.25f, MyMathF.Pow(6.5f, 2));
    }

    [TestMethod]
    public void Pow_With_Float_One()
    {
        Assert.AreEqual(2.5f, MyMathF.Pow(2.5f, 1));
        Assert.AreEqual(3.5f, MyMathF.Pow(3.5f, 1));
        Assert.AreEqual(4.5f, MyMathF.Pow(4.5f, 1));
        Assert.AreEqual(5.5f, MyMathF.Pow(5.5f, 1));
        Assert.AreEqual(6.5f, MyMathF.Pow(6.5f, 1));
    }

    [TestMethod]
    public void Pow_With_Float_Zero()
    {
        Assert.AreEqual(1, MyMathF.Pow(2.5f, 0));
        Assert.AreEqual(1, MyMathF.Pow(3.5f, 0));
        Assert.AreEqual(1, MyMathF.Pow(4.5f, 0));
        Assert.AreEqual(1, MyMathF.Pow(5.5f, 0));
        Assert.AreEqual(1, MyMathF.Pow(6.5f, 0));
    }

    [TestMethod]
    public void Pow_With_Float()
    {
        Assert.AreEqual(3031.20718096, MyMathF.Pow(7.42f, 4), 0.0001f);
    }

    [TestMethod]
    public void Min_With_Integer()
    {
        Assert.AreEqual(1, MyMathF.Min(1, 2));
        Assert.AreEqual(1, MyMathF.Min(2, 1));
        Assert.AreEqual(1, MyMathF.Min(1, 1));
    }

    [TestMethod]
    public void Max_With_Integer()
    {
        Assert.AreEqual(2, MyMathF.Max(1, 2));
        Assert.AreEqual(2, MyMathF.Max(2, 1));
        Assert.AreEqual(1, MyMathF.Max(1, 1));
    }
}
