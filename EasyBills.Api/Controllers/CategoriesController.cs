using AutoMapper;
using EasyBills.Api.Authorization;
using EasyBills.Api.Models;
using EasyBills.Application.Categories;
using EasyBills.Domain.Entities;
using EasyBills.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyBills.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    [Authorization]
    [HttpGet]
    public async Task<List<CategoryDTO>> GetCategories()
    {
        var categories = await _categoryRepository.GetAll();

        return _mapper.Map<List<CategoryDTO>>(categories);
    }

    [Authorization]
    [HttpGet("{id:guid}")]
    public async Task<CategoryDTO> GetCategoryById(Guid id)
    {
        var category = await _categoryRepository.GetById(id);

        return _mapper.Map<CategoryDTO>(category);
    }

    [Authorization]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
    {
        var existingCategory = await _categoryRepository.GetOne(category => category.Name == createCategoryDTO.Name);

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

    [Authorization]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateCategory(CreateCategoryDTO createCategoryDTO, Guid id)
    {
        var existingCategory = await _categoryRepository.GetById(id);

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

    [Authorization]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        var category = await _categoryRepository.GetById(id);

        if (category is null)
        {
            return NotFound(new ErrorResponse { Error = "La categoria no existe" });
        }

        _categoryRepository.Remove(category);
        await _categoryRepository.SaveChanges();

        return NoContent();
    }
}
