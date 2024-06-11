
using Microsoft.AspNetCore.Mvc;
using MovieCRUD_NCapas.DTO;
using MovieCRUD_NCapas.Services.Interface;
using MovieCRUD_NCapas.Utility;

namespace MovieCRUD_NCapas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorService _actorService;
        public ActorController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<ActorDTO>> GetList()
        {
            var rsp = new Response<List<ActorDTO>>();
            try
            {
                rsp.status = true;
                rsp.value = await _actorService.GetActors();
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
        public async Task<ActionResult<ActorDTO>> GetActor(int id)
        {
            var rsp = new Response<ActorDTO>();
            try
            {
                rsp.status = true;
                rsp.value = await _actorService.GetById(id);
                if (rsp.value == null)
                {
                    rsp.status = false;
                    rsp.msg = "Actor not found";
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
        public async Task<ActionResult> Create([FromBody] ActorDTO actor) {
            var rsp = new Response<ActorDTO>();
            try
            {
                if (actor == null)
                {
                    rsp.status = false;
                    rsp.msg = "Actor can't be null";
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
                rsp.value = await _actorService.Create(actor);
            }
            catch (Exception ex)
            {
                rsp.status = false;
                rsp.msg = $"An error occurred: {ex.Message}";
                return StatusCode(500, rsp);
            }
            return CreatedAtAction(nameof(GetActor), new { id = actor.Id }, rsp);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] ActorDTO actor, int id) {
            var rsp = new Response<bool>();
            try
            {
                if (actor == null || actor.Id != id)
                {
                    rsp.status = false;
                    rsp.msg = "Actor can't be null or ID mismatch";
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
                bool response = await _actorService.Update(actor);
                if (!response)
                {
                    rsp.status = false;
                    rsp.msg = "Actor couldn't be updated";
                    return NotFound(rsp);
                }
                rsp.status = true;
                rsp.msg = "Actor updated successfully";
                rsp.value = response;
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
                ActorDTO actor = await _actorService.GetById(id);
                if (actor == null)
                {
                    rsp.status = false;
                    rsp.msg = "Actor not found";
                    return NotFound(rsp);
                }
                bool response = await _actorService.Delete(id);
                if (!response)
                {
                    rsp.status=false;
                    rsp.msg = "Actor couldn't be deleted";
                    return StatusCode(500, rsp);
                }
                rsp.status = true;
                rsp.value = response;
                rsp.msg = "Actor deleted successfully";
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
