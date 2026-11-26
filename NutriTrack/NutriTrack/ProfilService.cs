using SQLite;
using Microsoft.Maui.Storage;

namespace NutriTrack;

public static class ProfilService
{
    static SQLiteConnection? _base;

    // Ouvrir la base et créer la table si nécessaire
    static SQLiteConnection Base()
    {
        if (_base == null)
        {
            string chemin = Path.Combine(
                FileSystem.AppDataDirectory,
                "nutritrack.db3");

            _base = new SQLiteConnection(chemin);

            _base.CreateTable<Profil>();
        }

        return _base;
    }

    // Enregistrer le profil
    public static void Enregistrer(Profil profil)
    {
        profil.Id = 1;

        Base().InsertOrReplace(profil);
    }

    // Charger le profil enregistré
    public static Profil? Charger()
    {
        return Base().Find<Profil>(1);
    }
}
