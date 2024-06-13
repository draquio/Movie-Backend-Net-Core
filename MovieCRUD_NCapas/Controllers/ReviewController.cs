using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCRUD_NCapas.DTO.Review;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace MovieCRUD_NCapas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ReviewDTO>>> GetList()
        {
            var rsp = new Response<List<ReviewDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _reviewService.GetReviews();

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
        public async Task<ActionResult<ReviewDTO>> GetReview(int id)
        {
            var rsp = new Response<ReviewDTO>();
            try
            {
                ReviewDTO reviewDTO = await _reviewService.GetById(id);
                if (reviewDTO == null)
                {
                    rsp.status = false;
                    rsp.msg = "Category not found";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = reviewDTO;
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
        public async Task<ActionResult<ReviewDTO>> Create([FromBody] ReviewDTO review)
        {
            var rsp = new Response<ReviewDTO>();
            try
            {
                if (review == null)
                {
                    rsp.status = false;
                    rsp.msg = "Review can't be null";
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
                rsp.value = await _reviewService.Create(review);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, rsp);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> Update([FromBody] ReviewDTO review, int id) {
            var rsp = new Response<bool>();
            try
            {
                if (review == null || review.Id != id)
                {
                    rsp.status = false;
                    rsp.msg = "Review can't be null or ID mismatch";
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
                bool response = await _reviewService.Update(review);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Review couldn't be updated";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = true;
                rsp.msg = "Review updated successfully";
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
                bool response = await _reviewService.Delete(id);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Review couldn't be deleted";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = true;
                rsp.msg = "Review deleted successfully";
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
