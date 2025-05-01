using Agenda.Models;
using Agenda.Data;

namespace Agenda.Views;

// -----------------------------
// ARQUIVO TarefaPage.xaml
// -----------------------------
// Interface visual da tela de tarefa.
// - Campos:
//   - Entry: campo para o título da tarefa;
//   - Label: mostra a data (não editável);
//   - Editor: campo para a descrição da tarefa.
// - Botões:
//   - "Excluir": remove a tarefa atual;
//   - "Editar": libera os campos para edição;
//   - "Limpar": limpa os campos de texto;
//   - "Salvar": salva ou atualiza a tarefa.


public partial class TarefaPage : ContentPage
{
    public string DataAtual { get; set; }
    private string data { get; set; }
    private readonly TarefaController _tarefaController;
    Tarefa tarefaAtual { get; set; }
    public TarefaPage(string textoRecebido)
    {
        InitializeComponent();
        _tarefaController = App.TarefaController;
        DataAtual = DateTime.Today.ToString("dd/MM/yyyy");
        BindingContext = this; // Vincula o modelo de dados à página
        data = textoRecebido.Split("--")[0].Trim();
        DataLabel.Text = data;
        CarregarTarefa();
    }

    // botão Excluir
    private async void ExcluirButton_Clicked(object sender, EventArgs e)
    {
        // Aqui você pode implementar a lógica para excluir a tarefa
        bool confirmacao = await DisplayAlert("Confirmar", "Você tem certeza que deseja excluir?", "Sim", "Não");
        if (confirmacao)
        {
            
            _tarefaController.ExcluirTarefa(tarefaAtual);
            TituloEntry.Text = "";
            DescricaoEditor.Text = "";
            await DisplayAlert("Excluído", "A tarefa foi excluída com sucesso!", "OK");
            // Navegar de volta ou limpar os campos
        }
    }

    private void LimparButton_Clicked(object sender, EventArgs e)
    {
        TituloEntry.Text = "";
        DescricaoEditor.Text = "";
    }

   
    private void EditarButton_Clicked(object sender, EventArgs e)
    {
        
        // Exemplo: Habilitar campos de edição ou navegar para outra tela de edição
        // TituloEntry.IsEnabled = true;
        //DescricaoEditor.IsEnabled = true;
        TituloEntry.IsReadOnly = false;
        DescricaoEditor.IsReadOnly = false;
    }

    private async void SalvarButton_Clicked(object sender, EventArgs e)
    {
        string data = DataLabel.Text.Split("(")[0].Trim();
        string titulo = TituloEntry.Text;
        string descricao = DescricaoEditor.Text;  
        Tarefa tarefa = new Tarefa(DateOnly.ParseExact(data, "dd/MM/yyyy", null), titulo, descricao);
        await _tarefaController.SalvarTarefa(tarefa);
        DescricaoEditor.IsReadOnly = true;
        TituloEntry.IsReadOnly = true;
    }

    private async void CarregarTarefa()
    {
        string dataAuax = DataLabel.Text.Split("(")[0].Trim();
        tarefaAtual = await _tarefaController.ObterTarefaPorData(DateOnly.ParseExact(dataAuax, "dd/MM/yyyy", null));
        if (!(tarefaAtual is null))
        {
            TituloEntry.Text = tarefaAtual.Titulo;
            TituloEntry.IsReadOnly = true;
            DescricaoEditor.Text = tarefaAtual.Descricao;
            DescricaoEditor.IsReadOnly = true;
        }
        else
        {
            TituloEntry.Text = "";
            DescricaoEditor.Text = "";
            TituloEntry.IsReadOnly = false;
            DescricaoEditor.IsReadOnly = false;

        }
    }

   


}