using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VulnerableAppForWebinar.Entity.Card;
using VulnerableAppForWebinar.Utility.Database;

namespace VulnerableAppForWebinar.Repository.Card
{
    public class CardRepository
    {
        private readonly IMongoCollection<CardEntity> _card;

        public CardRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _card = database.GetCollection<CardEntity>("Card");
        }

        public async Task<List<CardEntity>> GetCard(string UserId)
        {
            return await _card.Find(card => card.UserID == UserId).ToListAsync();
        }

        public async Task<CardEntity> CreateCard(CardEntity card)
        {
            await _card.InsertOneAsync(card);
            return card;
        }
    }
}
