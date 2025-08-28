using Library.Domain.Models;
using Xunit;

public class BookTests
{
    [Fact]
    public void TryBorrow_DecrementsAvailable_WhenAvailable()
    {
        var b = new Book { TotalCopies = 2, AvailableCopies = 2 };
        Assert.True(b.TryBorrow());
        Assert.Equal(1, b.AvailableCopies);
    }

    [Fact]
    public void TryBorrow_Fails_WhenNoneAvailable()
    {
        var b = new Book { TotalCopies = 1, AvailableCopies = 0 };
        Assert.False(b.TryBorrow());
        Assert.Equal(0, b.AvailableCopies);
    }

    [Fact]
    public void ReturnCopy_IncrementsButNotBeyondTotal()
    {
        var b = new Book { TotalCopies = 2, AvailableCopies = 1 };
        b.ReturnCopy();
        Assert.Equal(2, b.AvailableCopies);
        b.ReturnCopy();
        Assert.Equal(2, b.AvailableCopies);
    }
}
