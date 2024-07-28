using Service.Model;

namespace Service.Service;

public interface IBookingService
{
    Task<BookingConfirmationResult> BookSettlementSlot(Booking booking);
}