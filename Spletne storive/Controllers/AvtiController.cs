using Microsoft.AspNetCore.Mvc;
using Spletne_storive.Podatki;
using Spletne_storive.Models;
using System.Linq;

namespace Spletne_storive.Controllers;

[ApiController]
[Route("api/avti")]
public class AvtiController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Avto>> GetAll([FromQuery] string? znamka, [FromQuery] string? model)
    {
        var query = ProgramPodatki.Avti.AsQueryable();

        if (!string.IsNullOrWhiteSpace(znamka))
            query = query.Where(a => a.Znamka.Equals(znamka, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(model))
            query = query.Where(a => a.Model.Equals(model, StringComparison.OrdinalIgnoreCase));

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Avto> GetById(int id)
    {
        var avto = ProgramPodatki.Avti.FirstOrDefault(a => a.Id == id);
        if (avto == null) return NotFound();
        return Ok(avto);
    }

    [HttpPost]
    public ActionResult<Avto> Create([FromBody] Avto novAvto)
    {
        if (string.IsNullOrWhiteSpace(novAvto.Znamka) ||
            string.IsNullOrWhiteSpace(novAvto.Model) ||
            novAvto.Letnik <= 0)
        {
            return BadRequest("Znamka, model in veljaven letnik so obvezni.");
        }

        var newId = ProgramPodatki.Avti.Count == 0 ? 1 : ProgramPodatki.Avti.Max(a => a.Id) + 1;
        novAvto.Id = newId;

        ProgramPodatki.Avti.Add(novAvto);

        return CreatedAtAction(nameof(GetById), new { id = novAvto.Id }, novAvto);
    }

    [HttpPost("list")]
    public ActionResult<List<Avto>> CreateMany([FromBody] List<Avto> noviAvti)
    {
        if (noviAvti == null || noviAvti.Count == 0)
            return BadRequest("Seznam avtov ne sme biti prazen.");

        foreach (var a in noviAvti)
        {
            if (string.IsNullOrWhiteSpace(a.Znamka) ||
                string.IsNullOrWhiteSpace(a.Model) ||
                a.Letnik <= 0)
            {
                return BadRequest("Vsak avto mora imeti znamko, model in veljaven letnik.");
            }

            var newId = ProgramPodatki.Avti.Count == 0 ? 1 : ProgramPodatki.Avti.Max(x => x.Id) + 1;
            a.Id = newId;
            ProgramPodatki.Avti.Add(a);
        }

        return Ok(ProgramPodatki.Avti);
    }

    [HttpPut("{id}")]
    public ActionResult<Avto> Update(int id, [FromBody] Avto posodobljen)
    {
        if (string.IsNullOrWhiteSpace(posodobljen.Znamka) ||
            string.IsNullOrWhiteSpace(posodobljen.Model) ||
            posodobljen.Letnik <= 0)
        {
            return BadRequest("Znamka, model in veljaven letnik so obvezni.");
        }

        var avto = ProgramPodatki.Avti.FirstOrDefault(a => a.Id == id);
        if (avto == null) return NotFound();

        avto.Znamka = posodobljen.Znamka;
        avto.Model = posodobljen.Model;
        avto.Letnik = posodobljen.Letnik;

        return Ok(avto);
    }

    
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var avto = ProgramPodatki.Avti.FirstOrDefault(a => a.Id == id);
        if (avto == null) return NotFound();

        ProgramPodatki.Avti.Remove(avto);
        return NoContent(); // 204
    }
}
