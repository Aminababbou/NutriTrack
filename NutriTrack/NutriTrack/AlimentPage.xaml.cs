namespace NutriTrack;

public partial class AlimentPage : ContentPage
{
    private List<Aliment> listeAliments = new();

    public AlimentPage()
    {
        InitializeComponent();

        RechercheAliment.TextChanged += OnRechercheChanged;

        ChargerAliments();
    }

    private void ChargerAliments()
    {
        // Charger uniquement les aliments non masqués
        listeAliments = AlimentService.ObtenirAlimentsVisibles();

        CategoriePicker.SelectedIndex = 0;

        FiltrerAliments();
    }

    private void OnRechercheChanged(object sender, TextChangedEventArgs e)
    {
        FiltrerAliments();
    }

    private void OnCategorieChanged(object sender, EventArgs e)
    {
        FiltrerAliments();
    }

    private void FiltrerAliments()
    {
        string recherche = RechercheAliment.Text?.Trim() ?? "";

        string categorie = "Toutes les catégories";

        if (CategoriePicker.SelectedIndex != -1)
        {
            categorie = CategoriePicker.SelectedItem?.ToString()
                ?? "Toutes les catégories";
        }

        List<Aliment> resultat = listeAliments
            .Where(a =>
                (a.Nom ?? "").Contains(
                    recherche,
                    StringComparison.OrdinalIgnoreCase)
                &&
                (categorie == "Toutes les catégories"
                    || a.Categorie == categorie)
            )
            .ToList();

        AlimentsCollection.ItemsSource = resultat;
    }

    // Masquer un aliment
    private async void OnMasquerClicked(object sender, EventArgs e)
    {
        if (sender is Button bouton &&
            bouton.BindingContext is Aliment aliment)
        {
            bool confirmation = await DisplayAlert(
                "Masquer l'aliment",
                $"Voulez-vous masquer {aliment.Nom} ?",
                "Oui",
                "Non");

            if (!confirmation)
                return;

            AlimentService.Masquer(aliment.Id);

            ChargerAliments();

            await DisplayAlert(
                "Succès",
                "L'aliment a été masqué.",
                "OK");
        }
    }

    // Ajouter au journal
    private async void OnAjouterJournalClicked(object sender, EventArgs e)
    {
        if (sender is Button bouton &&
            bouton.BindingContext is Aliment aliment)
        {
            await Navigation.PushAsync(
                new AjouterJournalPage(aliment));
        }
    }
    private async void OnAlimentsMasquesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlimentsMasquesPage());
    }
}

