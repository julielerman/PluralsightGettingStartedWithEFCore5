using Microsoft.AspNetCore.Mvc;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SamuraiAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SamuraisSoCController : ControllerBase
  {
    private readonly BusinessLogicData _bizdata;

    public SamuraisSoCController(BusinessLogicData bizdata)
    {
      _bizdata = bizdata;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Samurai>>> GetSamurais()
    {
      var samurais = await _bizdata.GetAllSamurais();
      return Ok(samurais);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Samurai>> GetSamurai(int id)
    {
      var samurai = await _bizdata.GetSamuraiById(id);
      if (samurai == null)
      {
        return NotFound();
      }
      return Ok(samurai);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSamurai(int id, Samurai samurai)
    {
      if (id != samurai.Id)
      {
        return BadRequest();
      }
      try
      {
        bool result = await _bizdata.UpdateSamurai(samurai);

        if (result == false)
        {
          return NotFound();
        }
      }
      catch (Exception)
      {
        throw;
      }

      return NoContent();
    }

   

    // POST: api/Samurais
    [HttpPost]
    public async Task<ActionResult<Samurai>> PostSamurai(Samurai samurai)
    {
      var addedSamurai = await _bizdata.AddSamurai(samurai);

      return CreatedAtAction("GetSamurai", new { id = addedSamurai.Id }, addedSamurai);
    }

    // DELETE: api/Samurais/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Samurai>> DeleteSamurai(int id)
    {
      return await _bizdata.DeleteSamurai(id);
    }
  }
}