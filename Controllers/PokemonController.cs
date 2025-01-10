using Microsoft.AspNetCore.Mvc;
using PokedexAPI.Services;

namespace PokedexAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public PokemonController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }

        [HttpGet]
        public async Task<List<Pokemon>> Get() =>
            await _mongoDBService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Pokemon>> Get(string id)
        {
            var pokemon = await _mongoDBService.GetAsync(id);

            if (pokemon is null)
            {
                return NotFound();
            }

            return pokemon;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Pokemon newPokemon)
        {
            await _mongoDBService.CreateAsync(newPokemon);

            return CreatedAtAction(nameof(Get), new { id = newPokemon.Id }, newPokemon);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Pokemon updatedPokemon)
        {
            var pokemon = await _mongoDBService.GetAsync(id);

            if (pokemon is null)
            {
                return NotFound();
            }

            updatedPokemon.Id = pokemon.Id;

            await _mongoDBService.UpdateAsync(id, updatedPokemon);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var pokemon = await _mongoDBService.GetAsync(id);

            if (pokemon is null)
            {
                return NotFound();
            }

            await _mongoDBService.RemoveAsync(id);

            return NoContent();
        }
    }
}
