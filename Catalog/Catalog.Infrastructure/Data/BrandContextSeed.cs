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
    public static class BrandContextSeed
    {
        public static void SeedData(IMongoCollection<ProductBrand> brandCollection)
        {
            //bool checkBrands = brandCollection.Find(b => true).Any();
            ////string path = Path.Combine("Data", "SeedData", "brands.json");
            //string path = Path.Combine("../Catalog.Infrastructure/Data/SeedData/brands.json");

            //if (!checkBrands)
            //{
            //    var brandData = File.ReadAllText(path);
            //    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
            //    if (brands != null)
            //    {
            //        foreach (var brand in brands)
            //        {
            //            brandCollection.InsertOneAsync(brand);
            //        }
            //    }

            //}
            bool checkBrands = brandCollection.Find(b => true).Any();
            //string path = Path.Combine("Data", "SeedData", "brands.json");
            //string path = Path.Combine("../Catalog.Infrastructure/Data/SeedData/brands.json");

            if (!checkBrands)
            {
                var brandData = GetProductData();
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);
                if (brands != null)
                {
                    foreach (var brand in brands)
                    {
                        brandCollection.InsertOneAsync(brand);
                    }
                }

            }
        }

        private static string GetProductData()
        {
            return
                "[\r\n  {\r\n    \"Id\": \"63ca5e40e0aa3968b549af53\",\r\n    \"Name\": \"Adidas\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e4c455900b990b43bc1\",\r\n    \"Name\": \"ASICS\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e59065163c16451bd73\",\r\n    \"Name\": \"Victor\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e655ec1fdc49bd9327d\",\r\n    \"Name\": \"Yonex\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e728c4cff9708ada2a6\",\r\n    \"Name\": \"Puma\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e7ec90ff5c8f44d5ac8\",\r\n    \"Name\": \"Nike\"\r\n  },\r\n  {\r\n    \"Id\": \"63ca5e8d6110a9c56ee7dc48\",\r\n    \"Name\": \"Babolat\"\r\n  }\r\n]";
        }
    }
}
