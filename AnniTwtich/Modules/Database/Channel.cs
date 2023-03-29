using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anni.Modules.Database
{
    public class ChannelEntry
    {
        [BsonId]
        public string ChannelId { get; set; } = string.Empty;
        public PeriodicMessage? message { get; set; } = null;
        [BsonIgnore]
        public Task<bool> exists { 
            get {
                return Task.Run(async () => { return (await Get(ChannelId)) != null; });
            } 
        }
        public static async Task<ChannelEntry?> Get(string id)
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                if (collection.CountDocuments(_ => true) <= 0) return null;
                var item = (await collection.FindAsync(s => s.ChannelId == id)).ToList().First();
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await Get(id);
            }
        }
        public static async Task<List<ChannelEntry>> GetAll()
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                var item = (await collection.FindAsync(_ => true)).ToList();
                return item;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await GetAll();
            }
        }
        public async Task<bool> RemoveOne()
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                var res = await collection.DeleteOneAsync(s => s == this);
                return res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await RemoveOne();
            }
        }
        public async Task<bool> UploadOne()
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                long colcount = collection.CountDocuments(_ => true);
                await collection.InsertOneAsync(this);
                long newcol = collection.CountDocuments(_ => true);
                return colcount != newcol;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await UploadOne();
            }
        }
        public async Task<bool> UpdateOneAsync()
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                return (await collection.ReplaceOneAsync(d => d == this, this)).IsAcknowledged;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await UpdateOneAsync();
            }
        }
        public static async Task<bool> UpdateMany(List<ChannelEntry> data)
        {
            try
            {
                IMongoDatabase db = DatabaseController._database;
                IMongoCollection<ChannelEntry> collection = db.GetCollection<ChannelEntry>("ChannelData");
                List<bool> results = new List<bool>();
                for (int i = 0; i < data.Count; i++)
                {
                    results.Add((await collection.ReplaceOneAsync(d => d.ChannelId == data[i].ChannelId, data[i])).IsAcknowledged);
                }
                return results.All(r => r == true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Task.Delay(100);
                return await UpdateMany(data);
            }
        }
    }
    public class PeriodicMessage
    {
        public string text { get; set; } = "Hi";
        public int duration { get; set; } = 5;
    }
}
