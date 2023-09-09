namespace Computorv1Tests.Unit;

using Computorv1.Nodes;

[TestClass]
public class NodeTests
{
    [TestMethod]
    public void NumberNode_Should_ShouldReturnCorrectString()
    {
        var node = new NumberNode(2);

        var result = node.ToString();

        Assert.AreEqual("2", result);
    }

    [TestMethod]
    public void IdentifierNode_Should_ShouldReturnCorrectString()
    {
        var node = new IdentifierNode("X");

        var result = node.ToString();

        Assert.AreEqual("X", result);
    }

    [TestMethod]
    public void AddNode_Of_NumberNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new AddNode(new NumberNode(2), new NumberNode(3));

        var result = node.ToString();

        Assert.AreEqual("2 + 3", result);
    }

    [TestMethod]
    public void AddNode_Of_NumberNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new AddNode(new NumberNode(2), new AddNode(new NumberNode(3), new NumberNode(4)));

        var result = node.ToString();

        Assert.AreEqual("2 + 3 + 4", result);
    }

    [TestMethod]
    public void AddNode_Of_AddNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new AddNode(new AddNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4));

        var result = node.ToString();

        Assert.AreEqual("2 + 3 + 4", result);
    }

    [TestMethod]
    public void AddNode_Of_AddNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new AddNode(new AddNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 + 3 + 4 + 5", result);
    }

    [TestMethod]
    public void SubNode_Of_NumberNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new SubNode(new NumberNode(2), new NumberNode(3));

        var result = node.ToString();

        Assert.AreEqual("2 - 3", result);
    }

    [TestMethod]
    public void SubNode_Of_NumberNode_SubNode_ShouldReturnCorrectString()
    {
        var node = new SubNode(new NumberNode(2), new SubNode(new NumberNode(3), new NumberNode(4)));

        var result = node.ToString();

        Assert.AreEqual("2 - 3 - 4", result);
    }

    [TestMethod]
    public void SubNode_Of_SubNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new SubNode(new SubNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4));

        var result = node.ToString();

        Assert.AreEqual("2 - 3 - 4", result);
    }

    [TestMethod]
    public void SubNode_Of_SubNode_SubNode_ShouldReturnCorrectString()
    {
        var node = new SubNode(new SubNode(new NumberNode(2), new NumberNode(3)), new SubNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 - 3 - 4 - 5", result);
    }

    [TestMethod]
    public void MulNode_Of_NumberNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new NumberNode(2), new NumberNode(3));

        var result = node.ToString();

        Assert.AreEqual("2 * 3", result);
    }

    [TestMethod]
    public void MulNode_Of_NumberNode_MulNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new NumberNode(2), new MulNode(new NumberNode(3), new NumberNode(4)));

        var result = node.ToString();

        Assert.AreEqual("2 * 3 * 4", result);
    }

    [TestMethod]
    public void MulNode_Of_MulNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new MulNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4));

        var result = node.ToString();

        Assert.AreEqual("2 * 3 * 4", result);
    }

    [TestMethod]
    public void MulNode_Of_MulNode_MulNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new MulNode(new NumberNode(2), new NumberNode(3)), new MulNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 * 3 * 4 * 5", result);
    }

    [TestMethod]
    public void MulNode_Of_AddNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new AddNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 + 3) * (4 + 5)", result);
    }

    [TestMethod]
    public void MulNode_Of_AddNode_MulNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new AddNode(new NumberNode(2), new NumberNode(3)), new MulNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 + 3) * 4 * 5", result);
    }

    [TestMethod]
    public void MulNode_Of_MulNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new MulNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 * 3 * (4 + 5)", result);
    }

    [TestMethod]
    public void MulNode_Of_SubNode_SubNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new SubNode(new NumberNode(2), new NumberNode(3)), new SubNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 - 3) * (4 - 5)", result);
    }

    [TestMethod]
    public void MulNode_Of_PowNode_PowNode_ShouldReturnCorrectString()
    {
        var node = new MulNode(new PowNode(new NumberNode(2), new NumberNode(3)), new PowNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3 * 4 ^ 5", result);
    }

    [TestMethod]
    public void PowNode_Of_NumberNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new NumberNode(2), new NumberNode(3));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3", result);
    }

    [TestMethod]
    public void PowNode_Of_NumberNode_PowNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new NumberNode(2), new PowNode(new NumberNode(3), new NumberNode(4)));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3 ^ 4", result);
    }

    [TestMethod]
    public void PowNode_Of_PowNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new PowNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3 ^ 4", result);
    }

    [TestMethod]
    public void PowNode_Of_PowNode_PowNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new PowNode(new NumberNode(2), new NumberNode(3)), new PowNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3 ^ 4 ^ 5", result);
    }

    [TestMethod]
    public void PowNode_Of_AddNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new AddNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 + 3) ^ (4 + 5)", result);
    }

    [TestMethod]
    public void PowNode_Of_AddNode_PowNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new AddNode(new NumberNode(2), new NumberNode(3)), new PowNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 + 3) ^ 4 ^ 5", result);
    }

    [TestMethod]
    public void PowNode_Of_PowNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new PowNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 ^ 3 ^ (4 + 5)", result);
    }

    [TestMethod]
    public void PowNode_Of_SubNode_SubNode_ShouldReturnCorrectString()
    {
        var node = new PowNode(new SubNode(new NumberNode(2), new NumberNode(3)), new SubNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("(2 - 3) ^ (4 - 5)", result);
    }

    [TestMethod]
    public void EqualNode_Of_NumberNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new NumberNode(2), new NumberNode(3));

        var result = node.ToString();

        Assert.AreEqual("2 = 3", result);
    }

    [TestMethod]
    public void EqualNode_Of_NumberNode_EqualNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new NumberNode(2), new EqualNode(new NumberNode(3), new NumberNode(4)));

        var result = node.ToString();

        Assert.AreEqual("2 = 3 = 4", result);
    }

    [TestMethod]
    public void EqualNode_Of_EqualNode_NumberNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new EqualNode(new NumberNode(2), new NumberNode(3)), new NumberNode(4));

        var result = node.ToString();

        Assert.AreEqual("2 = 3 = 4", result);
    }

    [TestMethod]
    public void EqualNode_Of_EqualNode_EqualNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new EqualNode(new NumberNode(2), new NumberNode(3)), new EqualNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 = 3 = 4 = 5", result);
    }

    [TestMethod]
    public void EqualNode_Of_AddNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new AddNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 + 3 = 4 + 5", result);
    }

    [TestMethod]
    public void EqualNode_Of_AddNode_EqualNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new AddNode(new NumberNode(2), new NumberNode(3)), new EqualNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 + 3 = 4 = 5", result);
    }

    [TestMethod]
    public void EqualNode_Of_EqualNode_AddNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new EqualNode(new NumberNode(2), new NumberNode(3)), new AddNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 = 3 = 4 + 5", result);
    }

    [TestMethod]
    public void EqualNode_Of_SubNode_SubNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new SubNode(new NumberNode(2), new NumberNode(3)), new SubNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 - 3 = 4 - 5", result);
    }

    [TestMethod]
    public void EqualNode_Of_SubNode_EqualNode_ShouldReturnCorrectString()
    {
        var node = new EqualNode(new SubNode(new NumberNode(2), new NumberNode(3)), new EqualNode(new NumberNode(4), new NumberNode(5)));

        var result = node.ToString();

        Assert.AreEqual("2 - 3 = 4 = 5", result);
    }
}
