
using System.Globalization;
namespace NutriTrack;
using System.Text;
using System.Text.Json;

public partial class JournalPage : ContentPage
{
    private readonly CultureInfo culture = new("fr-FR");

    private List<JournalItemAffichage> listeJournal = new();

    private double objectifCalories;
    private double objectifProteines;

    public JournalPage()
    {
        InitializeComponent();

        DateJournalPicker.Date = DateTime.Today;

        ChargerObjectifs();
        ChargerJournal();
    }

    // 1. Charger les objectifs du profil
    private void ChargerObjectifs()
    {
        Profil? profil = ProfilService.Charger();

        if (profil == null)
        {
            LblObjectifCalories.Text = "Objectif indisponible";
            LblObjectifProteines.Text = "Objectif indisponible";
            return;
        }

        double metabolisme;

        if (profil.Sexe.Equals("Femme", StringComparison.OrdinalIgnoreCase))
        {
            metabolisme = 10 * profil.PoidsKg
                + 6.25 * profil.TailleCm
                - 5 * profil.Age - 161;
        }
        else
        {
            metabolisme = 10 * profil.PoidsKg
                + 6.25 * profil.TailleCm
                - 5 * profil.Age + 5;
        }

        double coefficient;

        if (profil.Activite.StartsWith("Sédentaire"))
            coefficient = 1.2;
        else if (profil.Activite.StartsWith("Légèrement actif"))
            coefficient = 1.375;
        else if (profil.Activite.StartsWith("Actif"))
            coefficient = 1.55;
        else if (profil.Activite.StartsWith("Très actif"))
            coefficient = 1.725;
        else
        {
            LblObjectifCalories.Text = "Objectif indisponible";
            LblObjectifProteines.Text = "Objectif indisponible";
            return;
        }

        double caloriesMaintien = metabolisme * coefficient;

        switch (profil.Objectif)
        {
            case 0:
                objectifCalories = caloriesMaintien;
                objectifProteines = profil.PoidsViseKg * 1.0;
                break;

            case 1:
                objectifCalories = caloriesMaintien - 400;
                objectifProteines = profil.PoidsViseKg * 1.5;
                break;

            case 2:
                objectifCalories = caloriesMaintien + 300;
                objectifProteines = profil.PoidsViseKg * 1.8;
                break;

            default:
                LblObjectifCalories.Text = "Objectif indisponible";
                LblObjectifProteines.Text = "Objectif indisponible";
                return;
        }

        LblObjectifCalories.Text =
            $"Objectif : {objectifCalories.ToString("N0", culture)} kcal";

        LblObjectifProteines.Text =
            $"Objectif : {objectifProteines.ToString("N0", culture)} g";
    }

    // 2. Charger les consommations
    private void ChargerJournal()
    {
        DateTime date = DateJournalPicker.Date;

        List<JournalAlimentaire> consommations =
            JournalService.ObtenirParDate(date);

        List<Aliment> aliments = AlimentService.ObtenirTous();

        listeJournal.Clear();

        double totalCalories = 0;
        double totalProteines = 0;

        foreach (JournalAlimentaire consommation in consommations)
        {
            Aliment? aliment = aliments.FirstOrDefault(
                a => a.Id == consommation.AlimentId);

            if (aliment == null)
                continue;

            double calories =
                aliment.Calories * consommation.Quantite / 100;

            double proteines =
                aliment.Proteines * consommation.Quantite / 100;

            totalCalories += calories;
            totalProteines += proteines;

            string unite =
                aliment.Categorie == "Boissons" ? "ml" : "g";

            listeJournal.Add(new JournalItemAffichage
            {
                JournalId = consommation.Id,
                Repas = consommation.Repas,
                NomAliment = aliment.Nom,
                QuantiteTexte =
                    $"{consommation.Quantite:N0} {unite}",
                DetailsNutrition =
                    $"{calories:N0} kcal | Protéines : {proteines:F1} g"
            });
        }

        listeJournal = listeJournal
            .OrderBy(j => OrdreRepas(j.Repas))
            .ToList();

        JournalCollection.ItemsSource = listeJournal;

        LblAucunAliment.IsVisible = listeJournal.Count == 0;

        AfficherBilan(totalCalories, totalProteines);
    }

    // 3. Ordre des repas
    private int OrdreRepas(string repas)
    {
        return repas switch
        {
            "Petit-déjeuner" => 1,
            "Déjeuner" => 2,
            "Dîner" => 3,
            "Collation" => 4,
            _ => 5
        };
    }

    // 4. Afficher le bilan
    private void AfficherBilan(double calories, double proteines)
    {
        LblCalories.Text =
            $"{calories.ToString("N0", culture)} kcal";

        LblProteines.Text =
            $"{proteines.ToString("N1", culture)} g";

        if (objectifCalories > 0)
        {
            double restantes = objectifCalories - calories;

            ProgressCalories.Progress =
                Math.Clamp(calories / objectifCalories, 0, 1);

            LblCaloriesRestantes.Text = restantes >= 0
                ? $"{restantes:N0} kcal restantes"
                : $"Dépassé de {Math.Abs(restantes):N0} kcal";
        }

        if (objectifProteines > 0)
        {
            double restantes = objectifProteines - proteines;

            ProgressProteines.Progress =
                Math.Clamp(proteines / objectifProteines, 0, 1);

            LblProteinesRestantes.Text = restantes >= 0
                ? $"{restantes:N1} g restantes"
                : $"Dépassé de {Math.Abs(restantes):N1} g";
        }
    }

    // 5. Changer la date
    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        ChargerJournal();
    }

    // 6. Supprimer une consommation
    private async void OnSupprimerClicked(object sender, EventArgs e)
    {
        if (sender is not Button bouton ||
            bouton.BindingContext is not JournalItemAffichage item)
            return;

        bool confirmation = await DisplayAlert(
            "Confirmation",
            $"Voulez-vous supprimer {item.NomAliment} du journal ?",
            "Supprimer",
            "Annuler");

        if (!confirmation)
            return;

        try
        {
            JournalService.Supprimer(item.JournalId);

            await DisplayAlert(
                "Succès",
                "La consommation a été supprimée.",
                "OK");

            ChargerJournal();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Erreur",
                "Impossible de supprimer la consommation : " + ex.Message,
                "OK");
        }
    }

    // 7. Accéder au catalogue
    private async void OnAjouterAlimentClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlimentPage());
    }
    private List<Aliment> ObtenirAlimentsPourSuggestion()
    {
        List<Aliment> aliments = AlimentService.ObtenirAlimentsVisibles();

        return aliments
            .GroupBy(a => a.Categorie)
            .SelectMany(g => g
                .OrderBy(a => Random.Shared.Next())
                .Take(5))
            .ToList();
    }

    private async void OnSuggestionClicked(object sender, EventArgs e)
    {
        try
        {
            Label label = this.FindByName<Label>("SuggestionLabel");

            label.IsVisible = true;
            label.Text = "Génération des suggestions...";

            List<Aliment> aliments = ObtenirAlimentsPourSuggestion();

            string resultat = await DemanderSuggestionGemini(aliments);

            label.Text = resultat;
        }
        catch (Exception ex)
        {
            Label label = this.FindByName<Label>("SuggestionLabel");
            label.IsVisible = true;
            label.Text = "Erreur : " + ex.Message;
        }
    }

    private async Task<string> DemanderSuggestionGemini(List<Aliment> aliments)
    {
        string cleApi = "AQ.Ab8RN6JJ3xc7wBNCU8gefPO1btQRA2dCFS1zCNYPbvKMSm5g4g";
        string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-3.5-flash-lite:generateContent?key={cleApi}";
        string listeAliments = string.Join("\n",
            aliments.Select(a =>
                $"{a.Nom} ({a.Categorie}) : {a.Calories} kcal, {a.Proteines} g de protéines, {a.Glucides} g de glucides, {a.Lipides} g de lipides pour 100 g"));

        string prompt = $"""
    Tu es un assistant spécialisé dans la nutrition.

    Voici les aliments disponibles :
    {listeAliments}

    Propose 3 idées de plats en utilisant uniquement les aliments de cette liste.
    Pour chaque plat, indique :
    - Le nom du plat
    - Les ingrédients
    - Les quantités proposées
    - Une courte préparation

    Réponds en français.
    """;

        using HttpClient client = new HttpClient();

        var requete = new
        {
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new { text = prompt }
                }
            }
        }
        };

        string json = JsonSerializer.Serialize(requete);

        using StringContent contenu = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        HttpResponseMessage reponse = await client.PostAsync(url, contenu);

        string resultat = await reponse.Content.ReadAsStringAsync();

        if (!reponse.IsSuccessStatusCode)
            throw new Exception(resultat);

        using JsonDocument document = JsonDocument.Parse(resultat);

        return document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "Aucune suggestion reçue.";
    }



}

// Modèle destiné à l'affichage
public class JournalItemAffichage
{
    public int JournalId { get; set; }

    public string Repas { get; set; } = "";

    public string NomAliment { get; set; } = "";

    public string QuantiteTexte { get; set; } = "";

    public string DetailsNutrition { get; set; } = "";
}
