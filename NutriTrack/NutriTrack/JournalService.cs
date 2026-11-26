using SQLite;
using Microsoft.Maui.Storage;

namespace NutriTrack;

public static class JournalService
{
    private static SQLiteConnection? _base;

    private static SQLiteConnection Base()
    {
        if (_base == null)
        {
            string chemin = Path.Combine(
 FileSystem.AppDataDirectory,
                "nutritrack.db3");

            _base = new SQLiteConnection(chemin);

            _base.CreateTable<JournalAlimentaire>();
        }

        return _base;
    }

    // Ajouter une consommation avec une date choisie
    public static void Ajouter(
        int alimentId,
        double quantite,
        string repas,
        DateTime date)
    {
        JournalAlimentaire consommation = new JournalAlimentaire
        {
            AlimentId = alimentId,
            Quantite = quantite,
            DateConsommation = date.Date,
            Repas = repas
        };

        Base().Insert(consommation);
    }

    // Récupérer les consommations d'une journée
    public static List<JournalAlimentaire> ObtenirParDate(DateTime date)
    {
        DateTime debut = date.Date;
        DateTime fin = debut.AddDays(1);

        return Base()
            .Table<JournalAlimentaire>()
            .Where(j =>
                j.DateConsommation >= debut &&
                j.DateConsommation < fin)
            .OrderBy(j => j.DateConsommation)
            .ToList();
    }

    // Supprimer une consommation
    public static void Supprimer(int id)
    {
        Base().Delete<JournalAlimentaire>(id);
    }
}