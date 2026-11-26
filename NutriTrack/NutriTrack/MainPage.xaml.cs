namespace NutriTrack;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        //execution android 
        InitializeComponent();
    }

    private async void OnCommencerClicked( object sender, EventArgs e)
    {
        Profil? profil = ProfilService.Charger();

        if (profil == null)
        {
            // Aucun profil : ouvrir la page Profil
            await Navigation.PushAsync(new ProfilPage());
        }
        else
        {
            // Profil existant : ouvrir l'accueil
             await Navigation.PushAsync(new AcceuilPage());
        }
    }
}
