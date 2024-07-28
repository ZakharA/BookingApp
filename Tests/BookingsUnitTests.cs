using System.Collections;

namespace Tests;


public class Tests
{
    private IBookingService _bookingService; 
    [SetUp]
    public void Setup()
    {
        _bookingService = new BookingService();
    }

    [Test]
    [TestCaseSource(typeof(NonOverlappingBookingCaseSource))]
    public void BookSettlementSlot_ShouldReturnBookingConfirmed_WhenSlotsAreAvailable(Booking booking)
    {
        var result = _bookingService.BookSettlementSlot(booking).Result;
        Assert.That(true, Is.EqualTo(result.Confirmed));
    }
    
    [Test]
    [TestCaseSource(typeof(BookingSingleSlotCaseSource))]
    public void BookSettlementSlot_ShouldReturnFail_WhenSlotsAreNotAvailable(IEnumerable<Booking> bookings)
    {
        var result = new List<bool>();
        
        foreach (var booking in bookings)
        {
            result.Add(_bookingService.BookSettlementSlot(booking).Result.Confirmed);
        }
        
        Assert.That(result.Where(r => r == false).Count() == 1, Is.EqualTo(true));
    }
}


public class NonOverlappingBookingCaseSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] { new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "John Doe"} };
        yield return new object[] { new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(10, 0), Name = "Bill Doe"} };
        yield return new object[] { new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(11, 0), Name = "Sam Doe"} };
    }
}

public class BookingSingleSlotCaseSource : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new List<Booking> { 
            new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "John Doe"}, 
            new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "Bill Doe"},
            new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "Sam Doe"},
            new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "Sam Doe"},
            new Booking() { BookingId = Guid.NewGuid(), BookingTime = new TimeOnly(9, 0), Name = "Sam Doe"}
        };
    }
}