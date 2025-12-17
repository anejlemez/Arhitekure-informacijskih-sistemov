using Microsoft.AspNetCore.Mvc;
using Spletne_storive.Podatki;
using Spletne_storive.Models;
using System.Linq;

namespace Spletne_storive.Controllers;

[ApiController]
[Route("api/kupci")]
public class KupciController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<Kupec>> GetAll(
    [FromQuery] string? ime,
    [FromQuery] string? priimek)
    {
        var query = ProgramPodatki.Kupci.AsQueryable();

        if (!string.IsNullOrEmpty(ime))
        {
            query = query.Where(k => k.Ime.Equals(ime, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(priimek))
        {
            query = query.Where(k => k.Priimek.Equals(priimek, StringComparison.OrdinalIgnoreCase));
        }

        return Ok(query.ToList());
    }

    [HttpGet("{id}")]
    public ActionResult<Kupec> GetById(int id)
    {
        var Kupec = ProgramPodatki.Kupci.FirstOrDefault(k => k.Id == id);
        if (Kupec == null)
        {
            return NotFound();
        }
        return Ok(Kupec);
    }

    [HttpPost]
    public ActionResult<Kupec> Create([FromBody] Kupec novKupec)
    {
        if (string.IsNullOrWhiteSpace(novKupec.Ime) ||
        string.IsNullOrWhiteSpace(novKupec.Priimek) ||
        string.IsNullOrWhiteSpace(novKupec.Email))
        {
            return BadRequest("Ime, priimek in email so obvezni.");
        }

        var newId = ProgramPodatki.Kupci.Count == 0 ? 1 : ProgramPodatki.Kupci.Max(k => k.Id) + 1;

        novKupec.Id = newId;

        ProgramPodatki.Kupci.Add(novKupec);

        return CreatedAtAction(nameof(GetById), new { id = novKupec.Id }, novKupec);
    }

    [HttpPut("{id}")]
    public ActionResult<Kupec> Update(int id, [FromBody] Kupec posodobljen)
    {
        if (string.IsNullOrWhiteSpace(posodobljen.Ime) ||
            string.IsNullOrWhiteSpace(posodobljen.Priimek) ||
            string.IsNullOrWhiteSpace(posodobljen.Email))
        {
            return BadRequest("Ime, priimek in email so obvezni.");
        }

        var kupec = ProgramPodatki.Kupci.FirstOrDefault(k => k.Id == id);
        if (kupec == null)
            return NotFound();

        kupec.Ime = posodobljen.Ime;
        kupec.Priimek = posodobljen.Priimek;
        kupec.Email = posodobljen.Email;

        return Ok(kupec);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var kupec = ProgramPodatki.Kupci.FirstOrDefault(k => k.Id == id);
        if (kupec == null)
            return NotFound();

        ProgramPodatki.Kupci.Remove(kupec);

        return NoContent();
    }
}