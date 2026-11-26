
using System.Globalization;

namespace NutriTrack;

public partial class ProfilPage : ContentPage
{
    bool _pret = false;

    public ProfilPage()
    {
        InitializeComponent();

        _pret = true;
        VerifierTout();
    }

    // Quand l'utilisateur modifie un champ
    void OnTexteChange(object? sender, TextChangedEventArgs e)
    {
        if (_pret)
            VerifierTout();
    }

    // Quand l'utilisateur choisit le sexe ou l'activité
    void OnChoixChange(object? sender, EventArgs e)
    {
        if (_pret)
            VerifierTout();
    }

    // Quand l'utilisateur choisit son objectif
    void OnObjectifChange(object? sender, EventArgs e)
    {
        if (!_pret) return;

        int objectif = ObjectifPicker.SelectedIndex;

        PoidsViseLayout.IsVisible = (objectif == 1 || objectif == 2);

        PoidsViseEntry.Text = "";

        if (objectif == 1)
            PoidsViseLabel.Text = "Poids visé (kg) : inférieur à votre poids actuel";

        if (objectif == 2)
            PoidsViseLabel.Text = "Poids visé (kg) : supérieur à votre poids actuel";

        VerifierTout();
    }

    // Vérifier tous les champs
    bool VerifierTout()
    {
        string eAge = VerifierAge(AgeEntry.Text);
        string eTaille = VerifierTaille(TailleEntry.Text);
        string ePoids = VerifierPoids(PoidsEntry.Text);

        bool objectifChoisi = ObjectifPicker.SelectedIndex >= 0;

        string eVise = "";

        if (objectifChoisi)
        {
            eVise = VerifierPoidsVise(
                ObjectifPicker.SelectedIndex,
                PoidsEntry.Text,
                TailleEntry.Text,
                PoidsViseEntry.Text);
        }

        // Afficher les erreurs
        AfficherErreur(AgeErreur, eAge, AgeEntry.Text);
        AfficherErreur(TailleErreur, eTaille, TailleEntry.Text);
        AfficherErreur(PoidsErreur, ePoids, PoidsEntry.Text);
        AfficherErreur(PoidsViseErreur, eVise, PoidsViseEntry.Text);

        // Vérifier les choix obligatoires
        bool sexeChoisi = SexePicker.SelectedIndex >= 0;
        bool activiteChoisie = ActivitePicker.SelectedIndex >= 0;

        bool valide =
            eAge == "" &&
            eTaille == "" &&
            ePoids == "" &&
            eVise == "" &&
            sexeChoisi &&
            activiteChoisie &&
            objectifChoisi;

        ValiderButton.IsEnabled = valide;

        return valide;
    }

    // Contrôle de l'âge
    string VerifierAge(string? texte)
    {
        if (string.IsNullOrWhiteSpace(texte))
            return "Champ obligatoire.";

        if (!int.TryParse(texte, out int age))
            return "Entrez un nombre entier.";

        if (age < 18 || age > 100)
            return "L'âge doit être compris entre 18 et 100 ans.";

        return "";
    }

    // Contrôle de la taille
    string VerifierTaille(string? texte)
    {
        if (string.IsNullOrWhiteSpace(texte))
            return "Champ obligatoire.";

        if (!int.TryParse(texte, out int taille))
            return "Entrez un nombre entier.";

        if (taille < 120 || taille > 230)
            return "La taille doit être comprise entre 120 et 230 cm.";

        return "";
    }

    // Contrôle du poids
    string VerifierPoids(string? texte)
    {
        if (string.IsNullOrWhiteSpace(texte))
            return "Champ obligatoire.";

        if (!LireDecimal(texte, out double poids))
            return "Entrez un nombre valide (ex. 72,5).";

        if (poids < 30 || poids > 300)
            return "Le poids doit être compris entre 30 et 300 kg.";

        return "";
    }

    // Contrôle du poids visé
    string VerifierPoidsVise(
        int objectif,
        string? poidsTexte,
        string? tailleTexte,
        string? viseTexte)
    {
        // Garder son poids : pas besoin de poids visé
        if (objectif == 0)
            return "";

        if (string.IsNullOrWhiteSpace(viseTexte))
            return "Champ obligatoire.";

        if (!LireDecimal(viseTexte, out double vise))
            return "Entrez un nombre valide (ex. 68,5).";

        if (vise < 30 || vise > 300)
            return "Le poids visé doit être compris entre 30 et 300 kg.";

        // Il faut d'abord un poids actuel et une taille valides
        if (VerifierPoids(poidsTexte) != "" ||
            VerifierTaille(tailleTexte) != "")
        {
            return "Saisissez d'abord un poids actuel et une taille valides.";
        }

        LireDecimal(poidsTexte, out double poids);
        int taille = int.Parse(tailleTexte!);

        double metres = taille / 100.0;
        double imcVise = vise / (metres * metres);

        if (objectif == 1)
        {
            if (vise > poids - 1)
                return "Pour perdre du poids, le poids visé doit être inférieur d'au moins 1 kg à votre poids actuel.";

            if (imcVise < 18.5)
                return "Objectif trop bas pour votre taille (IMC inférieur à 18,5).";
        }
        else if (objectif == 2)
        {
            if (vise < poids + 1)
                return "Pour prendre du poids, le poids visé doit être supérieur d'au moins 1 kg à votre poids actuel.";

            if (imcVise > 30)
                return "Objectif trop élevé pour votre taille (IMC supérieur à 30).";
        }

        return "";
    }

    // Transformer le texte en nombre décimal
    bool LireDecimal(string? texte, out double valeur)
    {
        valeur = 0;

        if (string.IsNullOrWhiteSpace(texte))
            return false;

        string t = texte.Trim().Replace(',', '.');

        bool ok = double.TryParse(
            t,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out valeur);

        return ok && double.IsFinite(valeur);
    }

    // Afficher le message d'erreur
    void AfficherErreur(Label label, string erreur, string? texte)
    {
        label.Text = erreur;

        label.IsVisible =
            erreur != "" &&
            !string.IsNullOrEmpty(texte);
    }

    // Quand l'utilisateur clique sur Valider
    async void OnValiderClicked(object? sender, EventArgs e)
    {
        // Vérification finale
        if (!VerifierTout())
            return;

        LireDecimal(PoidsEntry.Text, out double poids);

        int objectif = ObjectifPicker.SelectedIndex;

        double vise = poids;

        if (objectif != 0)
            LireDecimal(PoidsViseEntry.Text, out vise);

        // Création de l'objet Model
        Profil profil = new Profil
        {
            Sexe = SexePicker.SelectedItem?.ToString() ?? "",
            Age = int.Parse(AgeEntry.Text),
            TailleCm = int.Parse(TailleEntry.Text),
            PoidsKg = poids,
            Activite = ActivitePicker.SelectedItem?.ToString() ?? "",
            Objectif = objectif,
            PoidsViseKg = vise
        };

        // Envoyer l'objet au Service pour l'enregistrement
        ProfilService.Enregistrer(profil);

        await DisplayAlert("Profil","Votre profil et votre objectif sont enregistrés.","OK");
        await Navigation.PushAsync(new ObjectifPage());

    }
}
