using Spletne_storive.Models;
namespace Spletne_storive.Podatki;

class ProgramPodatki
{
    public static List<Kupec> Kupci = new()
    {
        new Kupec { Id = 1, Ime = "Ana", Priimek = "Novak", Email = "ana.novak@gmail.com" },
        new Kupec { Id = 2, Ime = "Boris", Priimek = "Kralj", Email = "boris.kralj@gmail.com"}
    };

    public static List<Avto> Avti = new()
    {
        new Avto { Id = 1, Znamka = "Toyota", Model = "Corolla", Letnik = 2018 },
        new Avto { Id = 2, Znamka = "Honda", Model = "Civic", Letnik = 2020 }
    };

    public static List<Nakup> Nakupi = new()
    {
        new Nakup { Id = 1, DatumNakupa = new DateTime(2023, 5, 1), Cena = 15000, KupecId = 1, AvtoId = 1 },
        new Nakup { Id = 2, DatumNakupa = new DateTime(2023, 6, 15), Cena = 18000, KupecId = 2, AvtoId = 2 }
    };
}