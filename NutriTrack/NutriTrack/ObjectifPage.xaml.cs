
using System.Globalization;

namespace NutriTrack;

public partial class ObjectifPage : ContentPage
{
    public ObjectifPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CalculerBesoins();
    }

    private async Task CalculerBesoins()
    {
        // 1. Récupérer le profil enregistré dans SQLite
        Profil? profil = ProfilService.Charger();

        if (profil == null)
        {
            await DisplayAlert(
                "Erreur",
                "Aucun profil n'a été trouvé.",
                "OK");

            return;
        }

        // 2. Récupérer les informations du profil
        double poids = profil.PoidsKg;
        double poidsCible = profil.PoidsViseKg;
        double taille = profil.TailleCm;
        int age = profil.Age;
        string sexe = profil.Sexe;
        string activite = profil.Activite;
        int objectif = profil.Objectif;

        // 3. Calculer le métabolisme de base
        double mb;

        if (sexe == "Femme")
        {
            mb = (10 * poids)
                 + (6.25 * taille)
                 - (5 * age)
                 - 161;
        }
        else
        {
            mb = (10 * poids)
                 + (6.25 * taille)
                 - (5 * age)
                 + 5;
        }

        // 4. Déterminer le coefficient d'activité
        double coefficient;

        if (activite.StartsWith("Sédentaire"))
        {
            coefficient = 1.2;
        }
        else if (activite.StartsWith("Légèrement actif"))
        {
            coefficient = 1.375;
        }
        else if (activite.StartsWith("Actif"))
        {
            coefficient = 1.55;
        }
        else if (activite.StartsWith("Très actif"))
        {
            coefficient = 1.725;
        }
        else
        {
            await DisplayAlert(
                "Erreur",
                $"Niveau d'activité inconnu : {activite}",
                "OK");

            return;
        }

        // 5. Calculer les calories de maintien
        double caloriesMaintien = mb * coefficient;

        // 6. Ajuster selon l'objectif
        double caloriesCibles;

        if (objectif == 0)
        {
            caloriesCibles = caloriesMaintien;
            ObjectifLabel.Text = "Maintenir le poids";
        }
        else if (objectif == 1)
        {
            caloriesCibles = caloriesMaintien - 400;
            ObjectifLabel.Text = "Perdre du poids";
        }
        else if (objectif == 2)
        {
            caloriesCibles = caloriesMaintien + 300;
            ObjectifLabel.Text = "Prendre du poids";
        }
        else
        {
            await DisplayAlert(
                "Erreur",
                "L'objectif enregistré est inconnu.",
                "OK");

            return;
        }

        // 7. Calculer les protéines selon le poids cible
        double coefficientProteines;

        if (objectif == 0)
        {
            coefficientProteines = 1.0;
        }
        else if (objectif == 1)
        {
            coefficientProteines = 1.5;
        }
        else
        {
            coefficientProteines = 1.8;
        }

        double proteines = poidsCible * coefficientProteines;

        // 8. Afficher les résultats
        CultureInfo culture = CultureInfo.GetCultureInfo("fr-FR");

        CaloriesLabel.Text =
            $"{Math.Round(caloriesCibles).ToString("N0", culture)} kcal";

        ProteinesLabel.Text =
            $"{Math.Round(proteines).ToString("N0", culture)} g";

        MetabolismeLabel.Text =
            $"Métabolisme de base : {Math.Round(mb).ToString("N0", culture)} kcal";

        ActiviteLabel.Text =
            $"Coefficient d'activité : {coefficient}";
    }

    private async void OnCommencerClicked(object sender, EventArgs e)
    {
         await Navigation.PushAsync(new AcceuilPage());
    }
}

