namespace NutriTrack
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ProfilPage", typeof(ProfilPage));
        }
    }
}
