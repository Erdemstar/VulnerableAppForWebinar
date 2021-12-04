using MongoDB.Driver;
using VulnerableAppForWebinar.Entity.Account;
using VulnerableAppForWebinar.Utility.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VulnerableAppForWebinar.Dto.Account;

namespace VulnerableAppForWebinar.Repository.Account
{
    public class AccountRepository
    {
        private readonly IMongoCollection<AccountEntity> _account;

        public AccountRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _account = database.GetCollection<AccountEntity>("Account");
        }

        public async Task<List<AccountEntity>> GetAccounts()
        {
            return await _account.Find(ac => true).ToListAsync();
        }

        public async Task<AccountEntity> GetAccountByEmail(string email)
        {
            return await _account.Find(ac => ac.Email == email).FirstOrDefaultAsync();
        }

        public async Task<AccountEntity> GetAccountByEmailPassword(string email,string password)
        {
            return await _account.Find(ac => ac.Email == email && ac.Password == password).FirstOrDefaultAsync();
        }

        public async Task<AccountEntity> CreateAccount(AccountEntity account)
        {
            await _account.InsertOneAsync(account);
            return account;
        }

        public async Task<AccountEntity> UpdateNameSurname(string id, AccountEntity entity)
        {
            await _account.FindOneAndReplaceAsync<AccountEntity>(ac => ac.Id == id, entity);
            return entity;
        }

        public async Task<bool> DeleteAccount(string email)
        {
            var status = await _account.DeleteOneAsync(ac => ac.Email == email);
            return status.DeletedCount > 0;
        }
    }
}
