
using SQLite;

namespace NutriTrack;

public class JournalAlimentaire
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    // Identifiant de l'aliment consommé
    public int AlimentId { get; set; }

    // Quantité consommée en grammes ou en ml
    public double Quantite { get; set; }

    // Date et heure de consommation
    public DateTime DateConsommation { get; set; }

    // Petit-déjeuner, déjeuner, dîner ou collation
    public string Repas { get; set; } = "";
}
