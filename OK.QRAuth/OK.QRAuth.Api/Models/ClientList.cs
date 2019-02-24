using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OK.QRAuth.Api.Models
{
    public class ClientList
    {
        private static List<Client> clients;
        private static IMongoCollection<Client> collection;

        static ClientList()
        {
            clients = new List<Client>();
            return;
            
            var client = new MongoClient(new MongoUrl("mongodb://127.0.0.1:27017"));
            var db = client.GetDatabase("auth");
            collection = db.GetCollection<Client>("clients");
        }

        public List<Client> Get()
        {
            return clients;

            return collection.Find(new JsonFilterDefinition<Client>("{}")).ToList();
        }

        public static Client Find(string token)
        {
            return clients.FirstOrDefault(x => x.Token == token);
        }

        public static void Insert(Client client)
        {
            client.Id = Guid.NewGuid().ToString().Replace("-", "");
            client.ConnectedAt = DateTime.Now;
            clients.Add(client);
            return;

            collection.InsertOne(client);
        }

        public static void Delete(string id)
        {
            clients.RemoveAll(x => x.Id == id);
            return;

            collection.DeleteOne(x => x.Id == id);
        }
    }
}