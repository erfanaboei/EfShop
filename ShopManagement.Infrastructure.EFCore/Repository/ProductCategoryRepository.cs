using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductCategoryRepository:RepositoryBase<long,ProductCategory>,IProductCategoryRepository
    {
        private readonly ShopContext _context;

        public ProductCategoryRepository(ShopContext context):base(context)
        {
            _context = context;
        }
        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            var query = _context.ProductCategories.Select(x => new ProductCategoryViewModel()
            {
                Id = x.Id,
                Picture = x.Picture,
                Name = x.Name,
                CreationDate = x.CreationDate.ToString(),
            });

            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            return query.OrderByDescending(x => x.Id).ToList();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _context.ProductCategories.Select(x => new EditProductCategory()
                {
                    Id = x.Id,
                    Description = x.Description,
                    Keywords = x.Keywords,
                    Name = x.Name,
                    Slug = x.Slug,
                    MetaDescription = x.MetaDescription,
                    Picture = x.Picture,
                    PictureAlt = x.PictureAlt,
                    PictureTitle = x.PictureTitle
                })
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
