using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "User")]
    public class CartItemsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartItemsController(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all cart items for current user
        /// </summary>
        /// <response code="200">Returns the cart items collection Successfully</response>
        /// <response code="401">Not Signed in</response>
        /// <response code="403">Current user Not have 'User' Role</response>
        [HttpGet(ApiRoutes.CartItems.GetAll)]
        [ProducesResponseType<List<CartItemResponse>>(200)]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(_mapper.Map<List<CartItemResponse>>(await _unitOfWork.CartItemRepository.GetUserItemsAsync(_currentUserService.UserId!)));
        }


        /// <summary>
        /// Get cart item by id for current user
        /// </summary>
        /// <response code="200">Returns the cart item Successfully</response>
        /// <response code="401">Not Signed in</response>
        /// <response code="403">Current user Not have 'User' Role</response>
        /// <response code="404">Not found or didn't belong to user</response>
        [HttpGet(ApiRoutes.CartItems.Get)]
        [ProducesResponseType<CartItemResponse>(200)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var cartItem = await _unitOfWork.CartItemRepository.GetUserItemByIdAsync(id, _currentUserService.UserId!);
            return cartItem is not null ? Ok(_mapper.Map<CartItemResponse>(cartItem)) : NotFound((ErrorResponse)"Not found cart item or didn't belong to user");
        }


        /// <summary>
        /// Create new cart item for current user
        /// </summary>
        /// <response code="200">Returns the cart item Successfully</response>
        /// <response code="401">Not Signed in</response>
        /// <response code="403">Current user Not have 'User' Role</response>
        /// <response code="404">Not found product</response>
        /// <response code="409">Product is already in cart items</response>
        [HttpPost(ApiRoutes.CartItems.Create)]
        [ProducesResponseType<CartItemResponse>(200)]
        public async Task<IActionResult> CreateAsync(CreateCartItemRequest request)
        {
            var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);
            if (product is null)
                return NotFound((ErrorResponse)"Not found product");

            if (await _unitOfWork.CartItemRepository.AnyAsync(x => x.UserId == _currentUserService.UserId && x.ProductId == request.ProductId))
                return Conflict((ErrorResponse)"The selected product is already in cart items");

            var cartItem = new CartItem()
            {
                Product = product,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UserId = _currentUserService.UserId!
            };
            await _unitOfWork.CartItemRepository.AddAsync(cartItem);

            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<CartItemResponse>(cartItem))
                : BadRequest((ErrorResponse)"Data not saved");
        }


        /// <summary>
        /// Update cart item quantity for current user
        /// </summary>
        /// <param name="request">New Quantity (if less than 1 the cart item will be deleted)</param>
        /// <response code="200">Returns the cart item Successfully</response>
        /// <response code="401">Not Signed in</response>
        /// <response code="403">Current user Not have 'User' Role</response>
        /// <response code="404">Not found or didn't belong to user</response>
        [HttpPut(ApiRoutes.CartItems.UpdateQuantity)]
        [ProducesResponseType<CartItemResponse>(200)]
        public async Task<IActionResult> UpdateAsync(int id, UpdateCartItemQuantityRequest request)
        {
            var cartItem = await _unitOfWork.CartItemRepository.GetUserItemByIdAsync(id, _currentUserService.UserId!);
            if (cartItem is null)
                return NotFound((ErrorResponse)"Not found cart item or didn't belong to user");


            cartItem!.Quantity = request.Quantity;
            if (request.Quantity <= 0)
                cartItem!.IsDeleted = true;


            return await _unitOfWork.Complete() > 0 ?
                Ok(_mapper.Map<CartItemResponse>(cartItem))
                : BadRequest((ErrorResponse)"Data not saved");
        }


        /// <summary>
        /// Delete cart item for current user
        /// </summary>
        /// <response code="200">cart item deleted Successfully</response>
        /// <response code="401">Not Signed in</response>
        /// <response code="403">Current user Not have 'User' Role</response>
        /// <response code="404">Not found or didn't belong to user</response>
        [HttpDelete(ApiRoutes.CartItems.Delete)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var cartItem = await _unitOfWork.CartItemRepository.GetUserItemByIdAsync(id, _currentUserService.UserId!);
            if (cartItem is null) return NotFound((ErrorResponse)"Not found cart item or didn't belong to user");

            cartItem!.IsDeleted = true;

            return await _unitOfWork.Complete() > 0 ?
                Ok("Deleted Successfully!")
                : BadRequest((ErrorResponse)"Data not saved");
        }
    }
}
