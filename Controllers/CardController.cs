using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CargoPayWebApi.Services;
using CargoPayWebApi.Models;

[Authorize]
[ApiController]
[Route("/[controller]")]
public class CardController : ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet("create")]
    public async Task<ActionResult<object>> CreateCard()
    {
        try
        {
            var cardNumber = await GenerateUniqueCardNumber();
            var card = await _cardService.CreateCardAsync(cardNumber);
            
            return Ok(new 
            { 
                Success = true,
                Message = "Card created successfully",
                CardNumber = cardNumber,
                Balance = card.Balance,
                CreatedAt = card.CreatedAt
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new 
            { 
                Success = false,
                Message = ex.Message 
            });
        }
    }

    private async Task<string> GenerateUniqueCardNumber()
    {
        Random random = new();
        string cardNumber;
        bool isUnique = false;

        do
        {
            cardNumber = string.Empty;
            for (int i = 0; i < 15; i++)
            {
                cardNumber += random.Next(0, 10).ToString();
            }

            try
            {
                
                await _cardService.GetBalanceAsync(cardNumber);
            }
            catch (ArgumentException)
            {
                
                isUnique = true;
            }
        } while (!isUnique);

        return cardNumber;
    }

    [HttpPost("pay")]
    public async Task<ActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        try
        {
            var result = await _cardService.ProcessPaymentAsync(request.CardNumber, request.Amount);
            return Ok(new 
            { 
                Success = true,
                Message = "Payment processed successfully"
            });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Insufficient funds"))
        {
            return Ok(new 
            { 
                Success = false,
                Message = "Insufficient funds to process the payment",
                CurrentBalance = await _cardService.GetBalanceAsync(request.CardNumber)
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new 
            { 
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new 
            { 
                Success = false,
                Message = "An error occurred while processing the payment"
            });
        }
    }

    
    [HttpPost("balance")]
    public async Task<ActionResult> GetBalance([FromBody] CardNumberRequest request)
    {
        try
        {
            var balance = await _cardService.GetBalanceAsync(request.CardNumber);
            return Ok(new
            {
                Success = true,
                Message = "Balance retrieved successfully",
                CardNumber = request.CardNumber,
                Balance = balance,
                RetrievedAt = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "An error occurred while retrieving the balance"
            });
        }
    }

    [HttpPost("load")]
    public async Task<ActionResult> LoadCard([FromBody] PaymentRequest request)
    {
        try
        {
            var result = await _cardService.LoadCardAsync(request.CardNumber, request.Amount);
            return Ok(new 
            { 
                Success = true,
                Message = "Card loaded successfully",
                NewBalance = result
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new 
            { 
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new 
            { 
                Success = false,
                Message = "An error occurred while loading the card"
            });
        }
    }
}