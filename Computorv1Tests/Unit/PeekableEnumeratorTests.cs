namespace Computorv1Tests.Unit;

using System.Collections;
using Computorv1.Collections;

[TestClass]
public class PeekableEnumeratorTests
{
    [TestMethod]
    public void PeekableEnumerator_Empty_CurrentIsNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string>());
        Assert.IsNull(enumerator.Current);
        Assert.IsNull(((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_Empty_MoveNextIsFalse()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string>());
        Assert.IsFalse(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_Empty_PeekIsNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string>());
        Assert.IsNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_Empty_PeekDoesNotMove()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string>());
        enumerator.Peek();
        Assert.IsFalse(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_Empty_PeekTwiceIsNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string>());
        enumerator.Peek();
        Assert.IsNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_OneElement_CurrentIsNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a" });
        Assert.IsNull(enumerator.Current);
        Assert.IsNull(((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_OneElement_MoveNextIsTrue()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a" });
        Assert.IsTrue(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_OneElement_PeekIsNotNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a" });
        Assert.IsNotNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_OneElement_PeekDoesNotMove()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a" });
        enumerator.Peek();
        Assert.IsTrue(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_OneElement_PeekTwiceIsNotNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a" });
        enumerator.Peek();
        Assert.IsNotNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_CurrentIsNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        Assert.IsNull(enumerator.Current);
        Assert.IsNull(((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_MoveNextIsTrue()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        Assert.IsTrue(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_PeekIsNotNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        Assert.IsNotNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_PeekTwiceIsNotNull()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.Peek();
        Assert.IsNotNull(enumerator.Peek());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_MoveNextIsTrueAfterPeek()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.Peek();
        Assert.IsTrue(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_CurrentIsNullAfterPeek()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.Peek();
        Assert.IsNull(enumerator.Current);
        Assert.IsNull(((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_CurrentIsFirstElementAfterPeekAndMoveNext()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.Peek();
        enumerator.MoveNext();
        Assert.AreEqual("a", enumerator.Current);
        Assert.AreEqual("a", ((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_CurrentIsSecondElementAfterMoveNextTwice()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.MoveNext();
        enumerator.MoveNext();
        Assert.AreEqual("b", enumerator.Current);
        Assert.AreEqual("b", ((IEnumerator)enumerator).Current);
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_MoveNextIsTrueAfterPeekAndMoveNext()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.Peek();
        enumerator.MoveNext();
        Assert.IsTrue(enumerator.MoveNext());
    }

    [TestMethod]
    public void PeekableEnumerator_TwoElements_CurrentIsNullAfterPeekAndMoveNextThreeTimes()
    {
        using var enumerator = new PeekableEnumerator<string>(new List<string> { "a", "b" });
        enumerator.MoveNext();
        enumerator.MoveNext();
        enumerator.MoveNext();
        Assert.IsNull(enumerator.Current);
        Assert.IsNull(((IEnumerator)enumerator).Current);
    }
}
