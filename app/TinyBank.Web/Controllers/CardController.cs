using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using TinyBank.Core.Implementation.Data;
using TinyBank.Core.Model;
using TinyBank.Core.Services;
using TinyBank.Core.Services.Options;
using TinyBank.Web.Models;

namespace TinyBank.Web.Controllers
{
    [Route("card")]
    public class CardController : Controller
    {
        private readonly ICardService _cards;
        private readonly ILogger<HomeController> _logger;
        private readonly TinyBankDbContext _dbContext;

        // Path: '/card'
        public CardController(
            TinyBankDbContext dbContext,
            ILogger<HomeController> logger,
            ICardService cards)
        {
            _logger = logger;
            _cards = cards;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("checkout")]
        public IActionResult Pay(
           [FromBody] SearchCardOptions options)
        {
            var result = _cards.Pay(options);
            if (result.IsSuccessful()) {
                return Json(new {success = true});
            }

            return Json(new {
                success = false,
                responseText = result.ErrorText
            });

            
        }
    }
}
