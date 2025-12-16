namespace Spletne_storive.Models;

public class Nakup
{
    public int Id { get; set; }
    public DateTime DatumNakupa { get; set; }
    public decimal Cena { get; set; }

    public int KupecId { get; set; }
    public int AvtoId { get; set; }
}