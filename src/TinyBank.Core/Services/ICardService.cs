using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyBank.Core.Model;

namespace TinyBank.Core.Services
{
    public interface ICardService
    {
        //public IQueryable<Card> Search(
        //    Options.SearchCardOptions options);

        public bool IsValidCard(
            Options.SearchCardOptions options);

        public ApiResult<Card> Pay(
            Options.SearchCardOptions options);
    }
}
