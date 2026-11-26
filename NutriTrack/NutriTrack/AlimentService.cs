
using SQLite;
using Microsoft.Maui.Storage;

namespace NutriTrack;

public static class AlimentService
{
    private static SQLiteConnection? _base;

    // 1. Connexion à la base de données
    private static SQLiteConnection Base()
    {
        if (_base == null)
        {
            string chemin = Path.Combine(
                FileSystem.AppDataDirectory,
                "nutritrack.db3");

            _base = new SQLiteConnection(chemin);

            _base.CreateTable<Aliment>();

            // Vérifier si la colonne Image existe
            int colonneImage = _base.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM pragma_table_info('Aliment') WHERE name = 'Image'");

            if (colonneImage == 0)
            {
                _base.Execute(
                    "ALTER TABLE Aliment ADD COLUMN Image TEXT NOT NULL DEFAULT ''");
            }

            // Vérifier si la colonne EstMasque existe
            int colonneMasque = _base.ExecuteScalar<int>(
                "SELECT COUNT(*) FROM pragma_table_info('Aliment') WHERE name = 'EstMasque'");

            if (colonneMasque == 0)
            {
                _base.Execute(
                    "ALTER TABLE Aliment ADD COLUMN EstMasque INTEGER NOT NULL DEFAULT 0");
            }

            InitialiserAliments();
        }

        return _base;
    }

    // 2. Créer un aliment
    private static Aliment A(
        string nom,
        string categorie,
        double calories,
        double proteines,
        double glucides,
        double lipides,
        string image)
    {
        return new Aliment
        {
            Nom = nom,
            Categorie = categorie,
            Calories = calories,
            Proteines = proteines,
            Glucides = glucides,
            Lipides = lipides,
            Image = image
        };
    }

    // 3. Initialiser le catalogue des aliments
    private static void InitialiserAliments()
    {
        var aliments = new List<Aliment>
        {
            // FRUITS
            A("Pomme", "Fruits", 52, 0.3, 14, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSw6FmXoeGDyhKwMw3aYDFAOL-hLcZP0GhWYvxdzfC95g&s=10"),
            A("Banane", "Fruits", 89, 1.1, 23, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSWDX7v0bVtIQlf67dr7K3_l8Y4g73uoQBKVIosAQRtUw&s=10"),
            A("Orange", "Fruits", 47, 0.9, 12, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTyWojjxk7iFw1KAD88W1W91HrEroq5hSgTXCsNG6-8IQ&s=10"),
            A("Fraise", "Fruits", 32, 0.7, 7.7, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSIfKXEIYHqTSW-JSeOsaKah2-360QyZ678_D0rrrnrrw&s=10"),
            A("Raisin", "Fruits", 69, 0.7, 18, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSr9tYL_sfW6VJjtexdtcBhm9k86sL5_VSW3H9P-xhD8w&s=10"),
            A("Poire", "Fruits", 57, 0.4, 15, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTN5DrzUiuMu78iIha-Roag58giIzIloJqyJF9JKmCo6A&s=10"),
            A("Pêche", "Fruits", 39, 0.9, 10, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRXZlNSwiGXu88SmEoKKySbdawdbb2d8h2BbywG4peyJg&s=10"),
            A("Ananas", "Fruits", 50, 0.5, 13, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR8L7Y3zgU9TtcAbgzBD5mF9Em9yhlwF_fTb9Wu_Hqn3Q&s=10"),
            A("Kiwi", "Fruits", 61, 1.1, 15, 0.5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNz4CUHLW3U-vK-aNxkVMxfgFvSudryE9MTvG2mtjBqw&s=10"),
            A("Pastèque", "Fruits", 30, 0.6, 8, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSnzstEx1gVU740KFjmI3XUtoRys0PufifihX2KKQSWMA&s=10"),
            A("Melon", "Fruits", 34, 0.8, 8, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTg7lhV08obLhodi1jpVka5cDDDDyS37HQCfO49rTBzQA&s=10"),
            A("Mangue", "Fruits", 60, 0.8, 15, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTQDkJ2-TbHd7_LXpfNvQazgDuunfXqQCY95E657hOenQ&s=10"),
            A("Avocat", "Fruits", 160, 2, 9, 15, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRypnzsZJEoSOgudGWRArjAW8LqNAg2jmH0N7jRmyMy4w&s=10"),
            A("Citron", "Fruits", 29, 1.1, 9, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQut4TOXifvE3z6z0KG5ClBTKn5-4teIuFpSjjAxWFTSg&s=10"),
            A("Cerise", "Fruits", 63, 1.1, 16, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQFS6FSPpM09rxE8RfgRTdeDO6xCDw70UVIogfVIDm1nw&s=10"),

            // LÉGUMES
            A("Tomate", "Légumes", 18, 0.9, 3.9, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSqo0HGHs66aaBhcLnQqlmG8f1lhx5EJz4qG1lk6FemEA&s=10"),
            A("Carotte", "Légumes", 41, 0.9, 10, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHTZx5hTpAavBxFLJVT5X3Gmkl8Hv5jDYNGN4HWOBcZQ&s=10"),
            A("Concombre", "Légumes", 15, 0.7, 3.6, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTGjfUsTGeU5J0TnAkZe8mQUztMeVp1a3ZzD3fD-JxdFw&s=10"),
            A("Pomme de terre", "Légumes", 77, 2, 17, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTMY-6qmxPtBfvjjxDqqIxahV8Nw6ixHy3qyNos_t5SSg&s=10"),
            A("Oignon", "Légumes", 40, 1.1, 9.3, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSsnUO_ksRc3zggPwXkfPSpXtODZ5mx9GirmTv3lhN4kg&s=10"),
            A("Brocoli", "Légumes", 34, 2.8, 7, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRXgA8mYYu-BnMEu3GxsWtuEvWKSPTcLarFTYa0cvBVbg&s=10"),
            A("Courgette", "Légumes", 17, 1.2, 3.1, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSYbBwv70Aje1M6GRPXt5VdL9ZbQU3vSHjuqHlEOvx44Q&s"),
            A("Poivron", "Légumes", 31, 1, 6, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-RXwXogty7AgNZ_drzokZyUSnYqgDlP5dYIqU4q8oXQ&s=10"),
            A("Épinards", "Légumes", 23, 2.9, 3.6, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRjuz7M8NcLvuy-fgLmAxM5sSG7IdF6NeU2OI2N0-YzGg&s=10"),
            A("Laitue", "Légumes", 15, 1.4, 2.9, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSmXhAPNtPqoa9RtQeu4ouDv99MEdNXPILcMM8ulsaVQ&s=10"),
            A("Aubergine", "Légumes", 25, 1, 6, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTm44vrauJJhBibujOjbIBfdK9u7fxnvB2DJN3yBCe5QQ&s=10"),
            A("Chou-fleur", "Légumes", 25, 1.9, 5, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQZ37DLlvxEIR95MLQOv67kI0DwIkVCrMfv4vw0s6yoRA&s=10"),
            A("Haricots verts", "Légumes", 31, 1.8, 7, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSPK1LZszrmj9u7RBukH7blj_9ZUW8wMQL0rSzTGy6nXg&s=10"),

            // PRODUITS LAITIERS
            A("Lait entier", "Produits laitiers", 61, 3.2, 4.8, 3.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTLd_3SfPWVGIYtAyKtI7-cXW58VjxbDFtzIVuZ1ZdHeQ&s=10"),
            A("Lait écrémé", "Produits laitiers", 34, 3.4, 5, 0.1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSPSB5AgH6SX7GxKrJE6rpeCt387I5waAg_X3Mlh2e7rw&s=10"),
            A("Yaourt nature", "Produits laitiers", 61, 3.5, 4.7, 3.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQYYO_86Wkp36SRUXt34Xp01yG_OQRYv0vMl710N2NpSQ&s=10"),
            A("Yaourt grec", "Produits laitiers", 97, 9, 3.6, 5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRT9uqCHMVkzqKyvKaNw5viX-REtArOiYgmReZ1dvVjvA&s=10"),
            A("Fromage blanc", "Produits laitiers", 72, 8, 4, 3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTarlbURervqpEAZTTXvgDugdfnq22oUj1qgJ2plCsxyQ&s=10"),
            A("Mozzarella", "Produits laitiers", 280, 28, 2.2, 17, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQvSmO_V1D7Hcm1YM3Z2M7jsyPmCC-rc8GhHXAXODwS9w&s=10"),
            A("Fromage cheddar", "Produits laitiers", 403, 25, 1.3, 33, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNtEyCgJuDIvxnmGDpDV0NkgB3qn1JkIlw40bwA-58OA&s=10"),
            A("Camembert", "Produits laitiers", 300, 20, 0.5, 24, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQvJLjIS5vftAqydcmEpQUeBzKNZcBpziJXnVLpXhCHdg&s"),

            // VIANDES ET POISSONS
            A("Poulet cuit", "Viandes et poissons", 165, 31, 0, 3.6, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSjKwjPSgUKbQeTqT9Lq7lOVviu9lS-k2T0QAgAVKxm3A&s=10"),
            A("Dinde", "Viandes et poissons", 135, 29, 0, 1.5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTftvIra3gaYSjQYXvGymwQvr2gOiKzEZmiPGGcGLk-wA&s=10"),
            A("Bœuf", "Viandes et poissons", 250, 26, 0, 15, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRIWBftm9Q-jP6fHGeQCbrOiRU5o2zTTB4AZbukg7vUrA&s=10"),
            A("Viande hachée", "Viandes et poissons", 254, 17, 0, 20, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRGLFxEcPGVG4jNZWWtGe7d3IcxJ1tHiY0NadDX0dH_Qg&s=10"),
            A("Thon naturel", "Viandes et poissons", 116, 26, 0, 1, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFSQBs6zXeiz4XiLfvWYBRmEMDqg2sUIJuNZb1EnB_Lg&s=10"),
            A("Saumon", "Viandes et poissons", 208, 20, 0, 13, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdG-6TQSjz-YCIp3kzNDTcP4CiqiwH_Lsf2QTYuFySew&s=10"),
            A("Sardines", "Viandes et poissons", 208, 25, 0, 11, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRQk-ksZ7jDPYmw26l3PAlEkNW1CLWNmSCJsG537L4c6g&s=10"),
            A("Œuf entier", "Viandes et poissons", 155, 13, 1.1, 11, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTy8TE812KdLMQoWrIZJKKBo1Fnum59GVxjaRyu1gShIw&s"),
            A("Blanc d'œuf", "Viandes et poissons", 52, 11, 0.7, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQw8Ze3AHRwKFWKEiu57t3DbWwrY5dWSDC1E5QKNWJ5lg&s"),

            // FÉCULENTS ET LÉGUMINEUSES
            A("Riz cuit", "Féculents et légumineuses", 130, 2.7, 28, 0.3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSfhlmPfZkaMIeJ0hof6BOgDzxOdK-AAeQP7BgYotuucw&s=10"),
            A("Pâtes cuites", "Féculents et légumineuses", 157, 5.8, 31, 0.9, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcS9bYDbGds4ZzHGJlIBAuEmxhhKTHJv_PHMrDsljTqgTw&s=10"),
            A("Couscous cuit", "Féculents et légumineuses", 112, 3.8, 23, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSQlZfLuQk2ZqUkzAkfk9Tu7S3SX10CiiaVccJxI95wkg&s"),
            A("Pain complet", "Féculents et légumineuses", 247, 13, 41, 4.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSMLwZ9h1-WlQhssQ9huJaaCY8Wa161DNyBuGbMzpdGA&s=10"),
            A("Pain blanc", "Féculents et légumineuses", 266, 9, 49, 3.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQWyOTUmEen2Z7tL7y77OCpTq69UinZ3KZj2io5SuGFLw&s=10"),
            A("Flocons d'avoine", "Féculents et légumineuses", 389, 16.9, 66, 6.9, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsrQbpeZFqxQZ1tbZi4vMc3LMlsTuYGnvUbDBPw6bMJw&s=10"),
            A("Lentilles cuites", "Féculents et légumineuses", 116, 9, 20, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT6qSyt7FJRwCvuhgJ-37zveKY9GryWx_fBVdDT9w1Zeg&s=10"),
            A("Pois chiches cuits", "Féculents et légumineuses", 164, 8.9, 27, 2.6, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRtlh4O_gxMZm__uIKHcSdSpOij1P4VCFV39B0VFz5Xiw&s=10"),
            A("Haricots blancs cuits", "Féculents et légumineuses", 139, 9.7, 25, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsR-SvHWVedjYVizbE6bdInKkd0g7KCr3dWciEtgAKEg&s=10"),

            // FRUITS SECS ET GRAINES
            A("Amandes", "Fruits secs et graines", 579, 21, 22, 50, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQSqYs9IBVfdHUmW2p6ghJpuk1IdfqH1bmyns1VST-rog&s=10"),
            A("Noix", "Fruits secs et graines", 654, 15, 14, 65, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQqrLmAdYmRJ10gvqBFnOrTG3s-N7rh23WVwjc88b5XUQ&s=10"),
            A("Dattes", "Fruits secs et graines", 282, 2.5, 75, 0.4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQTgMy2GailqrAf4g6mqNo__JF_87zdE2QN8oYqqKTItQ&s=10"),
            A("Raisins secs", "Fruits secs et graines", 299, 3.1, 79, 0.5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSGuQIa5xcoSj1UiDcrq_0of-OP_g2zZlI4ZxS2B0yDSQ&s=10"),
            A("Graines de chia", "Fruits secs et graines", 486, 16.5, 42, 30, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTEzcD8c-Kz8vSe85WLUqFkmL6oxIcUghE-1OIEeG2_Og&s=10"),
            A("Cacahuètes", "Fruits secs et graines", 567, 26, 16, 49, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSnsppj6MfXggwu1rGsx2pS3TQg9SR4-FQvCp9KD_wRCA&s=10"),

            // BOISSONS
            A("Eau", "Boissons", 0, 0, 0, 0, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT5in4WHdPb2g9vxcUBgRsMCSEku2hOD1Cb30UXiIu_NQ&s=10"),
            A("Jus d'orange", "Boissons", 45, 0.7, 10, 0.2, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQpwsYpPeHy15iKZii3rIAdTfNF_IjED7c9hgC6SGlt8A&s=10"),
            A("Café sans sucre", "Boissons", 1, 0.1, 0, 0, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRqGuJNaxhBaEv4Hc0qvZ-0NNkIHKlumPeBtkDhFxscvw&s=10"),
            A("Thé sans sucre", "Boissons", 1, 0, 0.2, 0, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQm-5991LG__xEKtqKuz1jenKxNaEnpmi7cK4EFn9W7UA&s=10"),
            A("Lait chocolaté", "Boissons", 75, 3.5, 10, 2.5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQeeZ7aJzPGolZ2UyJx0_gLRFOR4Tk6NN0JQhos81yITw&s=10"),

            // PLATS PRÉPARÉS
            A("Couscous tunisien", "Plats préparés", 150, 6, 25, 3, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2oRrHIrScmUFtJl8K-OnUVNcM6CGE72q_PERWR03lnA&s=10"),
            A("Makrouna (pâtes à la sauce)", "Plats préparés", 180, 6, 28, 5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSklSncnBXhKhL2Ap6IOWDhojmJYOlbuRVfhuio_LxZxA&s=10"),
            A("Brik à l'œuf", "Plats préparés", 250, 9, 25, 13, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTimdk4I2pA_iBbo8vg3nn3VkdXGgYp1vyYdTvWAPUt4A&s=10"),
            A("Salade tunisienne", "Plats préparés", 80, 2, 8, 5, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTHhfnBw2BbsAhtH4AzmlPCkC12kPMPYSgeiObkWNilEw&s=10"),
            A("Ojja", "Plats préparés", 120, 7, 5, 8, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRV69k93V5B_MJY0DLcb9OaZw7_lBvlZiziS1KnORN7-g&s=10"),
            A("Lablabi", "Plats préparés", 140, 7, 20, 4, "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRE949U-4OFB4exGPcOx-LfAYx-lTdcTrju6HGHJ6cvIg&s=10")
        };

        AjouterDansBase(aliments);
    }

    // 4. Insérer ou actualiser les aliments
    private static void AjouterDansBase(List<Aliment> aliments)
    {
        foreach (Aliment aliment in aliments)
        {
            var existant = _base!.Find<Aliment>(
                a => a.Nom == aliment.Nom);

            if (existant == null)
            {
                _base.Insert(aliment);
            }
            else
            {
                existant.Categorie = aliment.Categorie;
                existant.Calories = aliment.Calories;
                existant.Proteines = aliment.Proteines;
                existant.Glucides = aliment.Glucides;
                existant.Lipides = aliment.Lipides;
                existant.Image = aliment.Image;

                // Ne pas modifier EstMasque ici :
                // on conserve le choix de l'utilisateur.

                _base.Update(existant);
            }
        }
    }

    // 5. Récupérer tous les aliments
    public static List<Aliment> ObtenirTous()
    {
        return Base()
            .Table<Aliment>()
            .OrderBy(a => a.Categorie)
            .ThenBy(a => a.Nom)
            .ToList();
    }

    // 6. Récupérer les aliments visibles
    public static List<Aliment> ObtenirAlimentsVisibles()
    {
        return Base()
            .Table<Aliment>()
            .Where(a => !a.EstMasque)
            .OrderBy(a => a.Categorie)
            .ThenBy(a => a.Nom)
            .ToList();
    }

    // 7. Récupérer les aliments masqués
    public static List<Aliment> ObtenirAlimentsMasques()
    {
        return Base()
            .Table<Aliment>()
            .Where(a => a.EstMasque)
            .OrderBy(a => a.Nom)
            .ToList();
    }

    // 8. Masquer un aliment
    public static void Masquer(int id)
    {
        Aliment? aliment = Base().Find<Aliment>(id);

        if (aliment != null)
        {
            aliment.EstMasque = true;
            Base().Update(aliment);
        }
    }
    public static Aliment? ObtenirParId(int id)
    {
        return Base().Table<Aliment>().FirstOrDefault(a => a.Id == id);
    }

    // 9. Réafficher un aliment
    public static void Reafficher(int id)
    {
        Aliment? aliment = Base().Find<Aliment>(id);

        if (aliment != null)
        {
            aliment.EstMasque = false;
            Base().Update(aliment);
        }
    }

    // 10. Ajouter un aliment
    public static void Ajouter(Aliment aliment)
    {
        Base().Insert(aliment);
    }

    // 11. Ajouter plusieurs aliments
    public static void AjouterPlusieurs(List<Aliment> aliments)
    {
        Base().InsertAll(aliments);
    }
}