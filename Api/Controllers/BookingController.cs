using Api.Models;
using BookingAPI.Models.Dto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Service.Service;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IValidator<BookingRequest> _validator;
    private readonly IBookingService _bookingService;
    
    public BookingController(ILogger<BookingController> logger, IValidator<BookingRequest> validator, IBookingService bookingService)
    {
        _logger = logger;
        _validator = validator;
        _bookingService = bookingService;
    }

    [HttpPost()]
    [ProducesResponseType((StatusCodes.Status200OK))]
    [ProducesResponseType((StatusCodes.Status400BadRequest))]
    [ProducesResponseType((StatusCodes.Status409Conflict))]
    public async Task<ActionResult<BookingResult>> Post([FromBody] BookingRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (validationResult.IsValid)
        {
            var bookingResult = await _bookingService.BookSettlementSlot(request.MapToBooking());
            return bookingResult.Confirmed ? Ok(bookingResult.Booking) : Conflict();
        }
        
        var errors = validationResult.Errors;
        return BadRequest(errors);
    }
}

