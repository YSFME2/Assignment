﻿using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Contracts.v1.Responses.Errors;

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

        [HttpGet(ApiRoutes.Categories.GetAll)]
        [ProducesResponseType(typeof(List<CategoryResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<CategoryResponse>>(await _unitOfWork.CategoryRepository.GetAllAsync()));
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            return category is not null ? 
                Ok(_mapper.Map<CategoryResponse>(category)) 
                : NotFound("Not Found Category");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost(ApiRoutes.Categories.Create)]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        public async Task<IActionResult> CreateAsync(UpsertCategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            await _unitOfWork.CategoryRepository.AddAsync(category);
            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<CategoryResponse>(category))
                : BadRequest((ErrorResponse)"Data not saved!");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut(ApiRoutes.Categories.Update)]
        [ProducesResponseType(typeof(CategoryResponse), 200)]
        public async Task<IActionResult> UpdateAsync(int id, UpsertCategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = id;
            _unitOfWork.CategoryRepository.Update(category);
            return await _unitOfWork.Complete() > 0 ? Ok(_mapper.Map<CategoryResponse>(category)) : BadRequest("Data not saved!");
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete(ApiRoutes.Categories.Delete)]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _unitOfWork.CategoryRepository.Delete(id);
            return await _unitOfWork.Complete() > 0 && result ? Ok("Category deleted successfully!") : BadRequest("Data not saved!");
        }
    }
}
