
using Service.Model;

namespace Api.Models;

public struct BookingRequest
{
    public string BookingTime { get; init; }
    public string Name { get; init; }

    public Booking MapToBooking()
    {
        return new Booking()
        {
            BookingId = Guid.NewGuid(),
            BookingTime = TimeOnly.Parse(BookingTime),
            Name = Name
        };
    }
}