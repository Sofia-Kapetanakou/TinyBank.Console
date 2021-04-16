using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Constants;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;

namespace TinyBank.Core.Implementation.Services
{
    public class CardService : ICardService
    {
        private readonly TinyBankDbContext _dbContext;

        public CardService(TinyBankDbContext dbContext)
        {
            _dbContext = dbContext;         
        }

        public bool IsValidCard(SearchCardOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.CardNumber)) {
                return false;
            }

            if(!int.TryParse(options.ExpirationMonth, out _)) {
                return false;
            }
                
            if(!int.TryParse(options.ExpirationYear, out _)) {
                return false;
            }

            if(!decimal.TryParse(options.Amount, out _)) {
                return false;
            }

            return true;
        }

        public ApiResult<Card> Pay(SearchCardOptions options)
        {
            //check if the user entered valid values
            if (!IsValidCard(options)) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Invalid card info"
                };
            }

            //find the card and it's account
            var card = _dbContext.Set<Card>()
                .Include(c => c.Accounts)
                .Where(c => c.CardNumber == options.CardNumber)
                .FirstOrDefault();

            if(card == null) {
                return new ApiResult<Card>() {
                    Code = ApiResultCode.NotFound,
                    ErrorText = $"Invalid card number"
                };

            }
            //check validity of the card
            var validationResult = Validate(card, options);
            if (validationResult.IsSuccessful()) {
                //pay here

                var account = card.Accounts.FirstOrDefault();
                account.Balance -= decimal.Parse(options.Amount);
                _dbContext.SaveChanges();
            }

            return validationResult;
        }


        private ApiResult<Card> Validate (
            Card card, SearchCardOptions options)
        {
            //just to be sure
            if (!card.CardNumber.Equals(options.CardNumber))
                return ApiResult<Card>.CreateFailed(
                    ApiResultCode.NotFound,
                    $"Invalid card number {options.CardNumber}");

            //check if card is active
            if (!card.Active) {
                return ApiResult<Card>.CreateFailed(
                    ApiResultCode.Forbidden,
                    "Card is not Active");
            }
                      
            //check if expiration date matches
            try {
                DateTime expirationDate = new(int.Parse(options.ExpirationYear),
                           int.Parse(options.ExpirationMonth), card.Expiration.Day);
            
                if(card.Expiration.Date != expirationDate.Date) {
                    throw new Exception();
                }
            }
            catch {
                return ApiResult<Card>.CreateFailed(
                    ApiResultCode.BadRequest,
                    "Invalid date");
            }

            //check if there is sufficient balance

           var account = card.Accounts.FirstOrDefault();
           if(account == null) {
                return ApiResult<Card>.CreateFailed(
                   ApiResultCode.NotFound,
                   "No Account for this card");
            }

           if(account.Balance < decimal.Parse(options.Amount)) {
                return ApiResult<Card>.CreateFailed(
                   ApiResultCode.BadRequest,
                   "Insufficient funds");
            }

            //everything checks out
            return new ApiResult<Card>();
        }

        
    }
}
