using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository, IBrandRepository, ITypeRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            _context = context;
        }

        public async Task<Pagination<Product>> GetProducts(CatalogSpecParams catalogSpec)
        {
            var builder = Builders<Product>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(catalogSpec.Search))
            {
                var searchFilter = builder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(catalogSpec.Search));
                filter &= searchFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpec.BrandId))
            {
                var brandFilter = builder.Eq(x => x.Brands.Id, catalogSpec.BrandId);
                filter &= brandFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpec.TypeId))
            {
                var typeFilter = builder.Eq(x => x.Types.Id, catalogSpec.TypeId);
                filter &= typeFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpec.Sort))
            {

                var typeFilter = builder.Regex(x => x.Types.Id, new MongoDB.Bson.BsonRegularExpression(catalogSpec.TypeId));
                filter &= typeFilter;
            }
            if (!string.IsNullOrEmpty(catalogSpec.Sort))
            {
                return new Pagination<Product>
                {
                    PageIndex = catalogSpec.PageIndex,
                    PageSize = catalogSpec.PageSize,                    
                    Data = await DataFilter(catalogSpec, filter),
                    Count = await _context.Products.CountDocumentsAsync(p => true)
                };
            }
            return new Pagination<Product>
            {
                PageSize = catalogSpec.PageSize,
                PageIndex = catalogSpec.PageIndex,
                Data = await _context
                            .Products
                            .Find(filter)
                            .Sort(Builders<Product>.Sort.Ascending("Name"))
                            .Skip(catalogSpec.PageSize * (catalogSpec.PageIndex - 1))
                            .Limit(catalogSpec.PageSize)
                            .ToListAsync(),
                Count = await _context.Products.CountDocumentsAsync(p => true)
            };
            //return await _context.Products
            //    .Find(b => true)
            //    .ToListAsync();
        }

        private async Task<IReadOnlyList<Product>> DataFilter(CatalogSpecParams catalogSpec, FilterDefinition<Product> filter)
        {
            switch (catalogSpec.Sort)
            {
                case "priceAsc":
                    return await _context
                             .Products
                             .Find(filter)
                             .Sort(Builders<Product>.Sort.Ascending("Price"))
                             .Skip(catalogSpec.PageSize * (catalogSpec.PageIndex - 1))
                             .Limit(catalogSpec.PageSize)
                             .ToListAsync();
                case "priceDesc":
                    return await _context
                             .Products
                             .Find(filter)
                             .Sort(Builders<Product>.Sort.Descending("Price"))
                             .Skip(catalogSpec.PageSize * (catalogSpec.PageIndex - 1))
                             .Limit(catalogSpec.PageSize)
                             .ToListAsync();
                default:
                    return await _context
                             .Products
                             .Find(filter)
                             .Sort(Builders<Product>.Sort.Ascending("Name"))
                             .Skip(catalogSpec.PageSize * (catalogSpec.PageIndex - 1))
                             .Limit(catalogSpec.PageSize)
                             .ToListAsync();
            }
        }

        public async Task<Product> GetProduct(string id)
        {
            return await _context.Products
                .Find(b => b.Id == id)
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Name, name);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByBrand(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Brands.Name, name);
            return await _context.Products
                .Find(filter)
                .ToListAsync();
        }
        public async Task<Product> CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products
                .ReplaceOneAsync(p => p.Id == product.Id, product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await _context
                .Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<ProductBrand>> GetAllBrands()
        {
            return await _context.Brands
                .Find(b => true)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetAllTypes()
        {
            return await _context.Types
                .Find(b => true)
                .ToListAsync();
        }


    }
}
