using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestKaniner.Models;

namespace RestKaniner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KaninController : ControllerBase
    {
        // Dependency Injection
        private readonly KaninRepository _repo;

        public KaninController(KaninRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Kanin
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public ActionResult<IEnumerable<Kanin>> Get()
        {
            if (_repo != null)
            {
                var kaniner = _repo.GetAll();
                return Ok(kaniner);
            }

            return NotFound();
        }

        // GET api/Kanin/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Kanin> Get(int id)
        {
            Kanin? kanin = _repo.GetById(id);

            if (kanin == null)
            {
                return NotFound();
            }

            return Ok(kanin);
        }

        // POST api/Kanin
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost]
        public ActionResult<Kanin> Post([FromBody] Kanin newKanin) //data sendes i request body som JSON, og ASP.NET Core konverterer det automatisk til et kanin objekt.
        {
            try
            {
                _repo.Add(newKanin);

                return Created($"api/kanin/{newKanin.Id}", newKanin);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/Kanin/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var deleted = _repo.Delete(id);

            if (deleted == null)
            {
                return NotFound("No kanin with this ID " + id);
            }

            return Ok("Kanin with ID " + id + " has been deleted");
        }


        [HttpGet("filter")]
        public ActionResult<IEnumerable<Kanin>> FilterAndSort(string? farve, string? sort)
        {
            var kaniner = _repo.FilterAndSort(farve, sort);

            return Ok(kaniner);
        }

    }
}