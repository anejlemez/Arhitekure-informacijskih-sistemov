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
}