using BisLeagues.Core.Models;
using BisLeagues.Core.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Presentation.Models.ViewModels
{
    public class ExchangeTableViewModel
    {
        public Result LastMatchsResult;
        public List<ExchangeRow> ExchangeTableRows;
    }
}
