using System.Collections.Concurrent;
using Service.Model;

namespace Service.Service;

public class BookingService: IBookingService
{
    private ConcurrentDictionary<int, int> _bookingSyncHour = new();
    private const int MaxBookingsPerSettlementSlot = 4;
    
    public Task<BookingConfirmationResult> BookSettlementSlot(Booking booking)
    {
        if (_bookingSyncHour.TryGetValue(booking.BookingTime.Hour, out var currentBookingsForSettlementSlot))
        {
            if (currentBookingsForSettlementSlot < MaxBookingsPerSettlementSlot)
            {
                _bookingSyncHour.TryUpdate(booking.BookingTime.Hour, currentBookingsForSettlementSlot + 1, currentBookingsForSettlementSlot);
                return Task.FromResult(new BookingConfirmationResult()
                {
                    Confirmed = true,
                    Booking = booking
                });
            }
            
            return Task.FromResult(new BookingConfirmationResult()
            {
                Confirmed = false,
                Booking = booking
            }); 
        }
        else
        {
            if (_bookingSyncHour.TryAdd(booking.BookingTime.Hour, default(int) + 1))
            {
                return Task.FromResult(new BookingConfirmationResult()
                {
                    Confirmed = true,
                    Booking = booking
                });
            }
            
        }

        return Task.FromResult(new BookingConfirmationResult()
        {
            Confirmed = false,
            Booking = booking
        });
    }
}