namespace Computorv1Tests.Unit;

using Computorv1;

[TestClass]
public class MonominalTests
{
    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(2, "X", 3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("2 * X^3", result);
    }

    [TestMethod]
    public void Monominal_WithNegativeCoefficientAndPositiveExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(-2, "X", 3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("-2 * X^3", result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndNegativeExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(2, "X", -3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("2 * X^-3", result);
    }

    [TestMethod]
    public void Monominal_WithNegativeCoefficientAndNegativeExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(-2, "X", -3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("-2 * X^-3", result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndZeroExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(2, "X", 0);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("2", result);
    }

    [TestMethod]
    public void Monominal_WithNegativeCoefficientAndZeroExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(-2, "X", 0);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("-2", result);
    }

    [TestMethod]
    public void Monominal_WithZeroCoefficientAndPositiveExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(0, "X", 3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("0", result);
    }

    [TestMethod]
    public void Monominal_WithZeroCoefficientAndNegativeExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(0, "X", -3);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("0", result);
    }

    [TestMethod]
    public void Monominal_WithZeroCoefficientAndZeroExponent_ShouldReturnCorrectString()
    {
        // Arrange
        var monominal = new Monominal(0, "X", 0);

        // Act
        var result = monominal.ToString();

        // Assert
        Assert.AreEqual("0", result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_And_Monominal_WithPositiveCoefficientAndPositiveExponent_ShouldBeEqual()
    {
        // Arrange
        var monominal1 = new Monominal(2, "X", 3);
        var monominal2 = new Monominal(2, "X", 3);

        // Act
        var result = monominal1.Equals(monominal2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_And_Monominal_WithPositiveCoefficientAndNegativeExponent_ShouldNotBeEqual()
    {
        // Arrange
        var monominal1 = new Monominal(2, "X", 3);
        var monominal2 = new Monominal(2, "X", -3);

        // Act
        var result = monominal1.Equals(monominal2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_And_Monominal_WithGreaterExponent_ShouldBeGreater()
    {
        // Arrange
        var monominal1 = new Monominal(2, "X", 3);
        var monominal2 = new Monominal(2, "X", 4);

        // Act
        var result = monominal1.CompareTo(monominal2);

        // Assert
        Assert.AreEqual(1, result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_And_Monominal_WithLowerExponent_ShouldBeLower()
    {
        // Arrange
        var monominal1 = new Monominal(2, "X", 4);
        var monominal2 = new Monominal(2, "X", 3);

        // Act
        var result = monominal1.CompareTo(monominal2);

        // Assert
        Assert.AreEqual(-1, result);
    }

    [TestMethod]
    public void Monominal_WithPositiveCoefficientAndPositiveExponent_And_Monominal_WithSameExponent_ShouldBeEqual()
    {
        // Arrange
        var monominal1 = new Monominal(2, "X", 3);
        var monominal2 = new Monominal(2, "X", 3);

        // Act
        var result = monominal1.CompareTo(monominal2);

        // Assert
        Assert.AreEqual(0, result);
    }
}
