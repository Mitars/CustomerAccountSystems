using Cas.Account.Core.Dtos;
using Cas.Account.Core.Interfaces;
using Cas.Account.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cas.Account.DataAccess
{
    public class AccountAccessor : IAccountAccessor
    {
        private readonly DataContext dataContext;

        public AccountAccessor(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public Task<AccountModel> Get(long id) =>
            this.dataContext.Accounts.FirstOrDefaultAsync(account => account.Id == id);

        public Task<List<AccountModel>> GetByCustomerId(long customerId) =>
            this.dataContext.Accounts
                .Where(account => account.CustomerId == customerId)
                .OrderBy(account => account.Name)
                .ToListAsync();

        public async Task<AccountModel> Create(AccountToCreateDto accountToCreate)
        {
            var account = new AccountModel
            {
                CustomerId = accountToCreate.CustomerId,
                Name = accountToCreate.Name,
                Created = DateTime.UtcNow,
            };

            await this.dataContext.Accounts.AddAsync(account);

            var success = await this.dataContext.SaveChangesAsync() > 0;
            if (!success)
            {
                throw new Exception("Problem saving changes");
            }

            return account;
        }
    }
}
