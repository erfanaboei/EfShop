using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var opration = new OperationResult();
            if (_productCategoryRepository.Exists(x=>x.Name == command.Name))
                return opration.Failed(" این نام قبلا ثبت شده است ");
            var slug = command.Slug.Slugify();
            var productCategory = new ProductCategory(command.Name, command.Picture, command.PictureAlt, command.PictureTitle, command.MetaDescription, command.Keywords , slug);
            _productCategoryRepository.Create(productCategory);
            _productCategoryRepository.SaveChanges();
            return opration.Success();
        }

        public OperationResult Edit(EditProductCategory command)
        {
            var opration = new OperationResult();
            var productCategory = _productCategoryRepository.Get(command.Id);
            if (productCategory == null)
                return opration.Failed("رکوردی یافت نشد");
            if(_productCategoryRepository.Exists(x=>x.Name == command.Name && x.Id != command.Id))
                return opration.Failed("رکوردی با این مشخصات ثبت شده است");

            var slug = command.Slug.Slugify();
            productCategory.Edit(command.Name, command.Description, command.Picture, command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            _productCategoryRepository.SaveChanges();
            return opration.Success();
        }

        public EditProductCategory GetDetails(long id)
        {
            return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel searchModel)
        {
            return _productCategoryRepository.Search(searchModel);
        }
    }
}
