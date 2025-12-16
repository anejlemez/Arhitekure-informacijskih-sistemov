using Microsoft.AspNetCore.Mvc;
using Spletne_storive.Podatki;

namespace Spletne_storive.Controllers;

[ApiController]
[Route("api/kupci")]
class KupciController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(ProgramPodatki.Kupci);
    }
}