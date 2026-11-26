namespace NutriTrack;

public partial class AcceuilPage : ContentPage
{
    private string _calories = "0 kcal";
    private string _proteines = "0 g";
    private string _objectif = "Votre objectif";
    private string _motivation = "Chaque petit effort compte !";
    public string Calories
    {
        get => _calories;
        set { _calories = value; OnPropertyChanged(); }
    }

    
    public string Proteines
    {
        get => _proteines;
        set { _proteines = value; OnPropertyChanged(); }
    }

    
    public string Objectif
    {
        get => _objectif;
        set { _objectif = value; OnPropertyChanged(); }
    }
    public string Motivation
    {
        get => _motivation;
        set { _motivation = value; OnPropertyChanged(); }
    }

    public AcceuilPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ChargerResume();
        ChargerObjectif();
    }

    private void ChargerResume()
    {
        double calories = 0;
        double proteines = 0;

        List<JournalAlimentaire> consommations =
            JournalService.ObtenirParDate(DateTime.Today);

        foreach (JournalAlimentaire c in consommations)
        {
            Aliment? aliment = AlimentService.ObtenirParId(c.AlimentId);
            if (aliment == null)
                continue;

            double facteur = c.Quantite / 100.0;
            calories += aliment.Calories * facteur;
            proteines += aliment.Proteines * facteur;
        }

        Calories = $"{calories:F0} kcal";
        Proteines = $"{proteines:F1} g";
    }

    private void ChargerObjectif()
    {
        Profil? profil = ProfilService.Charger();
        if (profil == null)
            return;

        switch (profil.Objectif)
        {
            case 1:
                Objectif = "🎯 Perdre du poids";
                Motivation = "Chaque petit effort compte. Continuez à progresser !";
                break;
            case 2:
                Objectif = "🎯 Maintenir mon poids";
                Motivation = "Gardez vos bonnes habitudes et votre équilibre !";
                break;
            case 3:
                Objectif = "🎯 Gagner du poids";
                Motivation = "Avancez progressivement vers votre objectif !";
                break;
        }
    }

    private async void OnJournalClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new JournalPage());
    }

    private async void OnAlimentsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AlimentPage());
    }

    private async void OnProfilClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfilPage());
    }
}
