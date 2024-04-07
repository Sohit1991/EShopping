using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class TypeContextSeed
    {
        public static void SeedData(IMongoCollection<ProductType> typeCollection)
        {
            //bool checkTypes = typeCollection.Find(b => true).Any();
            ////string path = Path.Combine("Data", "SeedData", "types.json");
            //string path = Path.Combine("../Catalog.Infrastructure/Data/SeedData/types.json");
            //if (!checkTypes)
            //{
            //    var typesData = File.ReadAllText(path);
            //    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            //    if (types != null)
            //    {
            //        foreach (var item in types)
            //        {
            //            typeCollection.InsertOneAsync(item);
            //        }
            //    }

            //}
            bool checkTypes = typeCollection.Find(b => true).Any();
            //string path = Path.Combine("Data", "SeedData", "types.json");
            //string path = Path.Combine("../Catalog.Infrastructure/Data/SeedData/types.json");
            if (!checkTypes)
            {
                var typesData = GetTypes();
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
                if (types != null)
                {
                    foreach (var item in types)
                    {
                        typeCollection.InsertOneAsync(item);
                    }
                }

            }
        }
        private static string GetTypes()
        {
            return "[\r\n  {\r\n    \"Id\": \"63ca5d4bc3a8a58f47299f97\",\r\n    \"Name\": \"Shoes\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5d6d958e43ee1cd375fe\",\r\n    \"Name\": \"Rackets\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5d7d380402dce7f06ebc\",\r\n    \"Name\": \"Football\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5d8849bc19321b8be5f1\",\r\n    \"Name\": \"Kit Bags\"\r\n  }\r\n]";
        }
    }
}
