
using SQLite;

namespace NutriTrack;

public class Profil
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Sexe { get; set; } = "";

    public int Age { get; set; }

    public int TailleCm { get; set; }

    public double PoidsKg { get; set; }

    public string Activite { get; set; } = "";

    public int Objectif { get; set; }

    public double PoidsViseKg { get; set; }
}
