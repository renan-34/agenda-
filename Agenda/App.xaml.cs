namespace Agenda
{
    using Agenda.Data;
    public partial class App : Application
    {
        public static TarefaDatabase Database { get; private set; }
        public static TarefaController TarefaController { get; private set; }

        private string dbPath = Path.Combine(@"C:\Users\R\source\repos\Agenda", "tarefas.db");
        public App()
        {
            InitializeComponent();

            // Inicializa o banco de dados e o controller
            Database = new TarefaDatabase(dbPath);
            TarefaController = new TarefaController(Database);

            // Envolve MainPage dentro de um NavigationPage
            // MainPage = new NavigationPage(new MainPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new MainPage())) { Title = "Agenda" };
        }
    }
}
