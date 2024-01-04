using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Web.Api.Controllers
{
    /// <summary>
    /// Manage Products
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Get All Products
        /// </summary>
        /// <response code="200">List of products</response>
        [HttpGet(ApiRoutes.Products.GetAll)]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<ProductResponse>>(await _unitOfWork.ProductRepository.GetAllAsync()));
        }

        /// <summary>
        /// Get Filtered Products
        /// </summary>
        /// <response code="200">List of products</response>
        [HttpGet(ApiRoutes.Products.GetFiltered)]
        public async Task<IActionResult> GetFilteredAsync([FromQuery]ProductFilterRequest filter)
        {
            return Ok(_mapper.Map<IEnumerable<ProductResponse>>(await _unitOfWork.ProductRepository.GetFilterAsync(filter.FilterText,filter.CategoryId,filter.PriceFrom,filter.PriceTo)));
        }


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <response code="200">Product Founded Successfully</response>
        /// <response code="404">Product not found!</response>
        [Authorize]
        [HttpGet(ApiRoutes.Products.Get)]
        public async Task<IActionResult> GetAsync(int id)
        {
           var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if(product is null)
                return NotFound((ErrorResponse)$"Product with id {id} is not found!");

            return Ok(_mapper.Map<ProductResponse>(product));
        }


        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="request">New Product</param>
        /// <response code="200">Created Successfully</response>
        /// <response code="400">Data not saved!</response>
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Products.Create)]
        public async Task<IActionResult> CreateAsync(ProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            await _unitOfWork.ProductRepository.AddAsync(product);
            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<ProductResponse>(product))
                : BadRequest((ErrorResponse)"Data not saved!");
        }


        /// <summary>
        /// Update Product
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <param name="request">New Product Details</param>
        /// <response code="200">Updated Successfully</response>
        /// <response code="400">Data not saved!</response>
        /// <response code="404">Product not found!</response>
        [Authorize(Roles = "Admin")]
        [HttpPut(ApiRoutes.Products.Update)]
        public async Task<IActionResult> UpdateAsync(int id, ProductRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
                return NotFound((ErrorResponse)$"Product with id {id} is not found!");
            
            product.Description = request.Description;
            product.DiscountPercent = request.DiscountPercent;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.Name = request.Name;

            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<ProductResponse>(product))
                : BadRequest((ErrorResponse)"Data not saved!");
        }


        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <response code="200">Deleted Successfully</response>
        /// <response code="400">Data not saved!</response>
        /// <response code="404">Product not found!</response>
        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Products.Delete)]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _unitOfWork.ProductRepository.AnyAsync(x => x.Id == id))
                return NotFound((ErrorResponse)$"Product with id {id} is not found!");

            var result = await _unitOfWork.ProductRepository.Delete(id);
            return await _unitOfWork.Complete() > 0 && result ?
                Ok("Product deleted successfully!") :
                BadRequest((ErrorResponse)"Data not saved!");
        }
    }
}
