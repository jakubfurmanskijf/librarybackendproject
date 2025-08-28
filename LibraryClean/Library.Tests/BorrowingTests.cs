using Library.Domain.Models;
using Xunit;

public class BorrowingTests
{
    [Fact]
    public void IsOverdue_True_WhenPastDueAndNotReturned()
    {
        var br = new Borrowing { DueAtUtc = DateTime.UtcNow.AddDays(-1) };
        Assert.True(br.IsOverdue);
    }

    [Fact]
    public void IsOverdue_False_WhenReturned()
    {
        var br = new Borrowing { DueAtUtc = DateTime.UtcNow.AddDays(-1), ReturnedAtUtc = DateTime.UtcNow };
        Assert.False(br.IsOverdue);
    }
}
