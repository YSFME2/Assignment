namespace Web.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        /// <summary>
        /// Get All Categories
        /// </summary>
        [HttpGet(ApiRoutes.Categories.GetAll)]
        [ProducesResponseType<List<CategoryResponse>>(200)]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<IEnumerable<CategoryResponse>>(await _unitOfWork.CategoryRepository.GetAllAsync()));
        }



        /// <summary>
        /// Get Category By Id
        /// </summary>
        [Authorize]
        [HttpGet(ApiRoutes.Categories.Get)]
        [ProducesResponseType<CategoryResponse>(200)]
        [ProducesResponseType<ErrorResponse>(404)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            return category is not null ?
                Ok(_mapper.Map<CategoryResponse>(category))
                : NotFound((ErrorResponse)("Not Found Category with Id " + id));
        }



        /// <summary>
        /// Create new Category
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Categories.Create)]
        [ProducesResponseType<CategoryResponse>(200)]
        [ProducesResponseType<ErrorResponse>(400)]
        public async Task<IActionResult> CreateAsync(UpsertCategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<CategoryResponse>(category))
                : BadRequest((ErrorResponse)"Data not saved!");
        }



        /// <summary>
        /// Update Category Details
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpPut(ApiRoutes.Categories.Update)]
        [ProducesResponseType<CategoryResponse>(200)]
        [ProducesResponseType<ErrorResponse>(400)]
        [ProducesResponseType<ErrorResponse>(404)]
        public async Task<IActionResult> UpdateAsync(int id, UpsertCategoryRequest request)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (category is null)
                return NotFound((ErrorResponse)("Not Found Category with Id " + id));
            
            category.Description = request.Description;
            category.Name = request.Name;

            return await _unitOfWork.Complete() > 0 ? 
                Ok(_mapper.Map<CategoryResponse>(category)) 
                : BadRequest((ErrorResponse)"Data not saved!");
        }





        /// <summary>
        ///  Delete Category
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Categories.Delete)]
        [ProducesResponseType<string>(200)]
        [ProducesResponseType<ErrorResponse>(400)]
        [ProducesResponseType<ErrorResponse>(404)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _unitOfWork.CategoryRepository.AnyAsync(x => x.Id == id))
                return NotFound((ErrorResponse)("Not Found Category with Id " + id));

            var result = await _unitOfWork.CategoryRepository.Delete(id);
            return await _unitOfWork.Complete() > 0 && result ? 
                Ok("Category deleted successfully!") :
                BadRequest((ErrorResponse)"Data not saved!");
        }
    }
}
