
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieCRUD_NCapas.DTO;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;
using System;

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
        public async Task<ActionResult<CategoryDTO>> GetList() {
            var rsp = new Response<List<CategoryDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _categoryService.GetCategories();
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
                    rsp.msg = "Category not found";
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
        public async Task<ActionResult<CategoryDTO>> Update([FromBody] CategoryDTO category, int id)
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
                    rsp.msg = "Person couldn't be edited";
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
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
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
            return NoContent();
        }
    }
}
