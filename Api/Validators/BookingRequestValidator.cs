using System.Text.RegularExpressions;
using Api.Models;
using BookingAPI.Models;
using FluentValidation;

namespace Api.Validators;

public class BookingRequestValidator: AbstractValidator<BookingRequest>
{
    private static readonly Regex SBookingTimeFormatRegex = new Regex("^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$", RegexOptions.Compiled | RegexOptions.NonBacktracking);
    
    public BookingRequestValidator()
    {
        RuleFor(b => b.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        RuleFor(b => b.BookingTime)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .Must(ValidateBookingTimeFormat)
            .Must(ValidateOutOfBusinessHours)
            .WithMessage("Booking Format is incorrect");

    }

    /// <summary>
    /// Validate Out of Business Hours booking time
    /// this includes late booking requests, ie. after 4 PM
    /// </summary>
    /// <param name="bookingTime"></param>
    /// <returns></returns>
    private static bool ValidateOutOfBusinessHours(string bookingTime)
    {
        if (!TimeOnly.TryParse(bookingTime, out var parsedBookingTime)) return false;
        return parsedBookingTime.IsBetween(new TimeOnly(9, 0), new TimeOnly(16, 0));
    }

    private static bool ValidateBookingTimeFormat(string bookingTime)
    {
        return SBookingTimeFormatRegex.IsMatch(bookingTime);
    }
}