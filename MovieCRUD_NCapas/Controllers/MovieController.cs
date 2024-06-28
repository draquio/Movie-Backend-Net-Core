using Microsoft.AspNetCore.Mvc;
using MovieCRUD_NCapas.DTO.Movie;
using MovieCRUD_NCapas.Models;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace MovieCRUD_NCapas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> GetList(int page = 1, int pageSize = 10)
        {
            var rsp = new Response<List<MovieDTO>>();
            try
            {
                if (pageSize > 20)
                {
                    pageSize = 20;
                }
                rsp.status = true;
                rsp.value = await _movieService.GetMovies(page, pageSize);
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
        public async Task<ActionResult<Response<MovieDTO>>> GetMovie(int id) {
            var rsp = new Response<MovieDTO>();
            try
            {
                rsp.value = await _movieService.GetById(id);
                if (rsp.value == null)
                {
                    rsp.status = false;
                    rsp.msg = $"Movie with ID {id} not found";
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
        public async Task<ActionResult<Response<MovieDTO>>> Create([FromBody] CreateMovieDTO movie)
        {
            var rsp = new Response<MovieDTO>();
            try
            {
                if (movie == null)
                {
                    rsp.status = false;
                    rsp.msg = "movie can't be null";
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
                rsp.value = await _movieService.Create(movie);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return CreatedAtAction(nameof(GetMovie), new { id = rsp.value.Id }, rsp);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] MovieDTO movie, int id)
        {
            var rsp = new Response<bool>();
            try
            {
                if (movie == null || movie.Id != id)
                {
                    rsp.status = false;
                    rsp.msg = "Movie can't be null or ID mismatch";
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
                bool response = await _movieService.Update(movie);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Movie couldn't be updated";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.msg = "Movie updated successfully";
                rsp.value = true;
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
                bool response = await _movieService.Delete(id);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Movie couldn't be deleted";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.value = true;
                rsp.msg = "Movie deleted successfully";
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
