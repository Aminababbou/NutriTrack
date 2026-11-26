namespace NutriTrack;

public partial class AlimentsMasquesPage : ContentPage
{
	public AlimentsMasquesPage()
	{
		InitializeComponent();
        ChargerAlimentsMasques();
    }
    private void ChargerAlimentsMasques()
    {
        AlimentsMasquesCollection.ItemsSource =
            AlimentService.ObtenirAlimentsMasques();
    }
    private async void OnReafficherClicked(object sender, EventArgs e)
    {
        if (sender is Button bouton &&
            bouton.BindingContext is Aliment aliment)
        {
            bool confirmation = await DisplayAlert(
                "Réafficher l'aliment",
                $"Voulez-vous réafficher {aliment.Nom} ?",
                "Oui",
                "Non");

            if (!confirmation)
                return;

            AlimentService.Reafficher(aliment.Id);

            ChargerAlimentsMasques();

            await DisplayAlert(
                "Succès",
                "L'aliment a été réaffiché.",
                "OK");
        }
    }
}
