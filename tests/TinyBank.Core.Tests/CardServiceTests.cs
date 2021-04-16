using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using Xunit;

namespace TinyBank.Core.Tests
{
    public class CardServiceTests : IClassFixture<TinyBankFixture>
    {
        private readonly ICardService _cards;


        //private readonly TinyBankDbContext _dbContext;

        public CardServiceTests(TinyBankFixture fixture)
        {
            _cards = fixture.GetService<ICardService>();
        }

        //public CardServiceTests(TinyBankFixture fixture)
        //{
        //    _dbContext = fixture.DbContext;
        //}

        [Fact]
        public void ValidateCard()
        {
            // success
            var result = _cards.IsValidCard(new SearchCardOptions() {
                                CardNumber = "12345678901",
                                ExpirationMonth = "09",
                                ExpirationYear = "2025"
                                });
            Assert.True(result);


            // fail
            result = _cards.IsValidCard(new SearchCardOptions() {
                CardNumber = "12345678901",
                ExpirationMonth = "o9",
                ExpirationYear = "2025"
            });   
            Assert.False(result);

            result = _cards.IsValidCard(new SearchCardOptions() {
                CardNumber = " ",
                ExpirationMonth = "09",
                ExpirationYear = "2025"
            });
            Assert.False(result);

            result = _cards.IsValidCard(new SearchCardOptions() {
                CardNumber = "12345678901",
                ExpirationMonth = "09",
                ExpirationYear = "invalid"
            });
            Assert.False(result);
        }
    }


}
