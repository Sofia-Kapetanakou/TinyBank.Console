using System;
using System.Collections.Generic;

namespace TinyBank.Core.Services.Options
{
    public class SearchCardOptions
    {        
        public string CardNumber { get; set; }
        public string ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Amount { get; set; }        
    }
}
