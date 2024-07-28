namespace Service.Model;

public struct Booking
{
    public required Guid BookingId { get; init; }
    public required TimeOnly BookingTime { get; init; }
    public required string Name { get; init; }
}

public struct BookingConfirmationResult
{
    public bool Confirmed { get; init; }
    public Booking Booking { get; init; }
}