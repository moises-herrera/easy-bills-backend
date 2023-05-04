using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Categories;
using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EasyBills.Api.Controllers;

/// <summary>
/// The controller to handle all categories actions.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    /// <summary>
    /// The category repository to handle category actions.
    /// </summary>
    private readonly ICategoryRepository _categoryRepository;

    /// <summary>
    /// User repository to handle all the user actions.
    /// </summary>
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// The mapper used to transform an object into a different type.
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoriesController"/> class.
    /// </summary>
    /// <param name="categoryRepository">The category repository instance.</param>
    /// <param name="userRepository">The user repository instance.</param>
    /// <param name="mapper">Mapper instance.</param>
    public CategoriesController(ICategoryRepository categoryRepository, IUserRepository userRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all categories.
    /// </summary>
    /// <returns>A list of categories.</returns>
    [ProducesResponseType(typeof(List<CategoryDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [Authorization]
    [HttpGet]
    public async Task<List<CategoryDTO>> GetCategories()
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        IEnumerable<Category> categories;

        if (isUserAdmin)
        {
            categories = await _categoryRepository.GetAll();
        }
        else
        {
            categories = await _categoryRepository.GetAll(c => c.UserId == userId || c.UserId == null);
        }

        return _mapper.Map<List<CategoryDTO>>(categories);
    }

    /// <summary>
    /// Get a category by id.
    /// </summary>
    /// <param name="id">Category id.</param>
    /// <returns>The category.</returns>
    /// <response code="404">If the category does not exist.</response>
    [ProducesResponseType(typeof(CategoryDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<CategoryDTO> GetCategoryById(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var category = await _categoryRepository.GetOne(c => c.Id == id && ((c.UserId == userId && c.UserId == null) || isUserAdmin));

        return _mapper.Map<CategoryDTO>(category);
    }

    /// <summary>
    /// Create a category.
    /// </summary>
    /// <param name="createCategoryDTO">Object with category data.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the category was created.</response>
    /// <response code="400">If the category already exists.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    [Authorization]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);
        var existingCategory = await _categoryRepository.GetOne(c => c.Name == createCategoryDTO.Name && ((c.UserId == userId && c.UserId == null) || isUserAdmin));

        if (existingCategory is not null)
        {
            return BadRequest(
                new ErrorResponse { Error = "Ya existe una categoria con ese nombre" }
            );
        }

        var category = _mapper.Map<Category>(createCategoryDTO);

        _categoryRepository.Add(category);
        await _categoryRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Update a category by id.
    /// </summary>
    /// <param name="createCategoryDTO">Object with category data.</param>
    /// <param name="id">Category id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the category was updated.</response>
    /// <response code="400">If the category does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCategory(CreateCategoryDTO createCategoryDTO, Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var existingCategory = await _categoryRepository.GetOne(c => c.Id == id && ((c.UserId == userId && c.UserId == null) || isUserAdmin));

        if (existingCategory is null)
        {
            return NotFound(new ErrorResponse { Error = "La categoria no existe" });
        }

        var category = _mapper.Map<Category>(createCategoryDTO);
        category.Id = id;
        _categoryRepository.Update(category);
        await _categoryRepository.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Delete a category by id.
    /// </summary>
    /// <param name="id">Category id.</param>
    /// <returns>No content response</returns>
    /// <response code="204">If the category was deleted.</response>
    /// <response code="400">If the category does not exist.</response>
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var userId = Guid.Parse(Request.HttpContext.Items["UserId"].ToString());
        var isUserAdmin = await _userRepository.IsUserAdmin(userId);

        var category = await _categoryRepository.GetOne(c => c.Id == id && ((c.UserId == userId && c.UserId == null) || isUserAdmin));

        if (category is null)
        {
            return NotFound(new ErrorResponse { Error = "La categoria no existe" });
        }

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChanges();

        return NoContent();
    }
}
