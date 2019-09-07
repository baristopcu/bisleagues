﻿using BisLeagues.Core.Interfaces.Repositories;
using BisLeagues.Core.Data;
using BisLeagues.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BisLeagues.Core.Services.Repositories
{
    public class TransferRequestRepository : Repository<TransferRequest>, ITransferRequestRepository
    {
        private readonly BisLeaguesContext _dbContext;

        public TransferRequestRepository(BisLeaguesContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }
    }
}
