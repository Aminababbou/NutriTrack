
using System.Globalization;

namespace NutriTrack;

public partial class AjouterJournalPage : ContentPage
{
    private readonly Aliment _aliment;

    public AjouterJournalPage(Aliment aliment)
    {
        InitializeComponent();

        _aliment = aliment;
        BindingContext = aliment;

        // Informations de l'aliment
        LblNom.Text = aliment.Nom;
        LblCategorie.Text = aliment.Categorie;
        LblCalories100.Text = $"{aliment.Calories} kcal";
        LblProteines100.Text = $"{aliment.Proteines} g";

        // Unité selon la catégorie
        LblUnite.Text =
            aliment.Categorie == "Boissons" ? "ml" : "g";

        // Date par défaut
        DateConsommationPicker.Date = DateTime.Today;

        // Repas non sélectionné par défaut
        RepasPicker.SelectedIndex = -1;
    }

    // Calcul automatique des calories et protéines
    private void OnQuantiteChanged(object sender, TextChangedEventArgs e)
    {
        string texte = e.NewTextValue ?? "";

        if (!double.TryParse(
            texte,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out double quantite) || quantite <= 0)
        {
            LblCalories.Text = "0 kcal";
            LblProteines.Text = "0 g";
            return;
        }

        double calories = _aliment.Calories * quantite / 100;
        double proteines = _aliment.Proteines * quantite / 100;

        LblCalories.Text = $"{calories:F1} kcal";
        LblProteines.Text = $"{proteines:F1} g";
    }

    // Ajouter l'aliment au journal
    private async void OnAjouterClicked(object sender, EventArgs e)
    {
        // Vérifier la quantité
        if (!double.TryParse(
            TxtQuantite.Text,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out double quantite) || quantite <= 0)
        {
            await DisplayAlert(
                "Quantité invalide",
                "Veuillez saisir une quantité supérieure à 0.",
                "OK");

            return;
        }

        // Définir la quantité maximale
        double quantiteMax =
            _aliment.Categorie == "Boissons" ? 5000 : 2000;

        string unite =
            _aliment.Categorie == "Boissons" ? "ml" : "g";

        // Vérifier que la quantité ne dépasse pas la limite
        if (quantite > quantiteMax)
        {
            await DisplayAlert(
                "Quantité trop élevée",
                $"La quantité maximale autorisée est de {quantiteMax} {unite}.",
                "OK");

            return;
        }

        // Vérifier le repas
        if (RepasPicker.SelectedIndex == -1)
        {
            await DisplayAlert(
                "Repas manquant",
                "Veuillez choisir un repas.",
                "OK");

            return;
        }

        try
        {
            // Enregistrer dans SQLite
            JournalService.Ajouter(
                _aliment.Id,
                quantite,
                RepasPicker.SelectedItem.ToString()!,
                DateConsommationPicker.Date);

            await DisplayAlert(
                "Succès",
                "L'aliment a été ajouté au journal.",
                "OK");

            // Retour à la page précédente
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Erreur",
                "Impossible d'ajouter l'aliment : " + ex.Message,
                "OK");
        }
    }
}

