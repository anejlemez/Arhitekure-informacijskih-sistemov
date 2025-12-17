using Microsoft.AspNetCore.Mvc;
using Spletne_storive.Podatki;
using Spletne_storive.Models;
using System.Linq;

namespace Spletne_storive.Controllers;

[ApiController]
[Route("api/nakupi")]
public class NakupiController : ControllerBase
{
    // GET /api/nakupi
    // GET /api/nakupi?kupecId=1
    // GET /api/nakupi?avtoId=2
    // GET /api/nakupi?kupecId=1&avtoId=2
    [HttpGet]
    public ActionResult<List<Nakup>> GetAll([FromQuery] int? kupecId, [FromQuery] int? avtoId)
    {
        var query = ProgramPodatki.Nakupi.AsQueryable();

        if (kupecId.HasValue)
            query = query.Where(n => n.KupecId == kupecId.Value);

        if (avtoId.HasValue)
            query = query.Where(n => n.AvtoId == avtoId.Value);

        return Ok(query.ToList());
    }

    // GET /api/nakupi/{id}
    [HttpGet("{id}")]
    public ActionResult<Nakup> GetById(int id)
    {
        var nakup = ProgramPodatki.Nakupi.FirstOrDefault(n => n.Id == id);
        if (nakup == null) return NotFound();
        return Ok(nakup);
    }

    // POST /api/nakupi
    [HttpPost]
    public ActionResult<Nakup> Create([FromBody] Nakup novNakup)
    {
        // osnovna validacija
        if (novNakup.Cena <= 0)
            return BadRequest("Cena mora biti večja od 0.");

        // Datum lahko pustiš obvezen ali pa, če je default, nastaviš na zdaj
        if (novNakup.DatumNakupa == default)
            novNakup.DatumNakupa = DateTime.Now;

        // preveri tuja ključa
        var kupecObstaja = ProgramPodatki.Kupci.Any(k => k.Id == novNakup.KupecId);
        if (!kupecObstaja)
            return BadRequest("KupecId ne obstaja.");

        var avtoObstaja = ProgramPodatki.Avti.Any(a => a.Id == novNakup.AvtoId);
        if (!avtoObstaja)
            return BadRequest("AvtoId ne obstaja.");

        // nov ID
        var newId = ProgramPodatki.Nakupi.Count == 0 ? 1 : ProgramPodatki.Nakupi.Max(n => n.Id) + 1;
        novNakup.Id = newId;

        ProgramPodatki.Nakupi.Add(novNakup);

        return CreatedAtAction(nameof(GetById), new { id = novNakup.Id }, novNakup);
    }

    // PUT /api/nakupi/{id}
    [HttpPut("{id}")]
    public ActionResult<Nakup> Update(int id, [FromBody] Nakup posodobljen)
    {
        if (posodobljen.Cena <= 0)
            return BadRequest("Cena mora biti večja od 0.");

        if (posodobljen.DatumNakupa == default)
            posodobljen.DatumNakupa = DateTime.Now;

        // preveri tuja ključa
        if (!ProgramPodatki.Kupci.Any(k => k.Id == posodobljen.KupecId))
            return BadRequest("KupecId ne obstaja.");

        if (!ProgramPodatki.Avti.Any(a => a.Id == posodobljen.AvtoId))
            return BadRequest("AvtoId ne obstaja.");

        var nakup = ProgramPodatki.Nakupi.FirstOrDefault(n => n.Id == id);
        if (nakup == null) return NotFound();

        nakup.DatumNakupa = posodobljen.DatumNakupa;
        nakup.Cena = posodobljen.Cena;
        nakup.KupecId = posodobljen.KupecId;
        nakup.AvtoId = posodobljen.AvtoId;

        return Ok(nakup);
    }

    // DELETE /api/nakupi/{id}
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var nakup = ProgramPodatki.Nakupi.FirstOrDefault(n => n.Id == id);
        if (nakup == null) return NotFound();

        ProgramPodatki.Nakupi.Remove(nakup);
        return NoContent(); // 204
    }
}
