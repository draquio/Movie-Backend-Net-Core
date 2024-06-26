﻿

using Microsoft.AspNetCore.Mvc;
using MovieCRUD_NCapas.DTO.Category;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace MovieCRUD_NCapas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> GetList(int page = 1, int pageSize = 10) {
            var rsp = new Response<List<CategoryDTO>>();
            try
            {
                if (pageSize > 20)
                {
                    pageSize = 20;
                }
                rsp.status = true;
                rsp.value = await _categoryService.GetCategories(page, pageSize);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var rsp = new Response<CategoryDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _categoryService.GetById(id);
                if (rsp.value == null)
                {
                    rsp.status = false;
                    rsp.msg = $"Category with ID {id} not found";
                    return NotFound(rsp);
                }
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Create([FromBody] CategoryDTO category)
        {
            var rsp = new Response<CategoryDTO>();
            try
            {
                if (category == null)
                {
                    rsp.status = false;
                    rsp.msg = "Category can't be null";
                    return BadRequest(rsp);
                }
                if (!ModelState.IsValid)
                {
                    rsp.status = false;
                    rsp.msg = "Invalid data";
                    rsp.errors = ModelState.Values
                        .SelectMany(err => err.Errors)
                        .Select(err => err.ErrorMessage)
                        .ToList();
                    return BadRequest(rsp);
                }
                rsp.status = true;
                rsp.value = await _categoryService.Create(category);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, rsp);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] CategoryDTO category, int id)
        {
            var rsp = new Response<bool>();
            try
            {
                if (category == null || category.Id != id)
                {
                    rsp.status = false;
                    rsp.msg = "Category can't be null or ID mismatch";
                    return BadRequest(rsp);
                }
                if (!ModelState.IsValid)
                {
                    rsp.status = false;
                    rsp.msg = "Invalid data";
                    rsp.errors = ModelState.Values
                        .SelectMany(err => err.Errors)
                        .Select(err => err.ErrorMessage)
                        .ToList();
                    return BadRequest(rsp);
                }
                bool response = await _categoryService.Update(category);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Category couldn't be updated";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = response;
                rsp.msg = "Category updated successfully";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Delete(int id)
        {
            var rsp = new Response<bool>();
            try
            {
                bool response = await _categoryService.Delete(id);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Category couldn't be deleted";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = response;
                rsp.msg = "Category deleted successfully";
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return Ok(rsp);
        }
    }
}
