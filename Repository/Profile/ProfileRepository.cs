using MongoDB.Driver;
using VulnerableAppForWebinar.Entity.Profile;
using VulnerableAppForWebinar.Utility.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VulnerableAppForWebinar.Repository.Profile
{
    public class ProfileRepository
    {

        private readonly IMongoCollection<ProfileEntity> _profile;

        public ProfileRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _profile = database.GetCollection<ProfileEntity>("Profile");
        }

        public async Task<ProfileEntity> GetProfileByEmail(string email)
        {
            return await _profile.Find(prof => prof.Email == email).FirstOrDefaultAsync();
        }

        public async Task<ProfileEntity> CreateProfile(ProfileEntity entity)
        {
            await _profile.InsertOneAsync(entity);
            return entity;
        }

        public async Task<ProfileEntity> UpdateProfile(string id ,ProfileEntity entity)
        {
            var update = await _profile.FindOneAndReplaceAsync<ProfileEntity>(ent => ent.Id == id, entity);
            return update;
        }

        public async Task<bool> DeleteProfile(string id)
        {
            var deleteResult = await _profile.DeleteOneAsync(ent => ent.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }


    }
}
