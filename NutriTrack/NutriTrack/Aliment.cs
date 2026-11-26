
using SQLite;

namespace NutriTrack;

public class Aliment
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nom { get; set; } = "";
    public string Categorie { get; set; } = "";

    public double Calories { get; set; }
    public double Proteines { get; set; }
    public double Glucides { get; set; }
    public double Lipides { get; set; }

    public string Image { get; set; } = "";
    public bool EstMasque { get; set; } = false;
}
