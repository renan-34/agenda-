using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Agenda.Views;
using Agenda.Data;
using Agenda.Models;
using System;
using System.Threading.Tasks;

// RESUMO DO FUNCIONAMENTO DO APLICATIVO
// -----------------------------------------

// Este projeto é um aplicativo de agenda desenvolvido com .NET MAUI e SQLite.
// Ele permite cadastrar, editar, excluir e visualizar tarefas associadas a datas específicas.



namespace Agenda
{   
    public partial class MainPage : ContentPage
    {
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await AtualizarListaDiasAsync(); // Chama a função que atualiza a lista
        }

        // Armazena a data atual exibida na agenda
        private DateTime dataAtual = DateTime.Today;
        private readonly TarefaController _tarefaController;

        // Lista de dias exibidos no calendário (ObservableCollection permite atualização automática da interface)
        public ObservableCollection<DiaItem> Dias { get; set; } = new ObservableCollection<DiaItem>();

        // Construtor da classe MainPage
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this; // Define o contexto de dados para a interface          
            _tarefaController = App.TarefaController;
            // AtualizarListaDiasAsync();  // Atualiza a lista de dias
        }

        // Método para atualizar a lista de dias no calendário
        private async Task AtualizarListaDiasAsync()
        {
            Dias.Clear(); // Limpa a lista antes de adicionar novos dias
            
            // Gera os dias no intervalo de 2 dias para trás e 7 dias para frente
            for (int i = -2; i <= 7; i++)
            {
                DateOnly data = DateOnly.FromDateTime(dataAtual.AddDays(i)); // Convertendo para DateOnly
                string titulo = "Nada a fazer"; // Valor padrão
                //vai buscar no banco 
                var cor = Colors.Black;
                Tarefa tarefa = await _tarefaController.ObterTarefaPorData(data);
                // Tarefa tarefa = await _tarefaController.ObterTarefaPorData(DateOnly.ParseExact(dataAtual.AddDays(i), "dd/MM/yyyy", null));
                // Verifica se _tarefaController não é null
                if (tarefa != null)
                {
                    try
                    {
                        // Busca a tarefa no banco
                        //var tarefa = await _tarefaController.ObterTarefaPorData(data);
                        if (tarefa != null && !string.IsNullOrWhiteSpace(tarefa.Titulo))
                        {
                            titulo = tarefa.Titulo; // Substitui "Nada a fazer" pelo título da tarefa
                            cor = Colors.DarkRed;
                            // cor = Colors.Green;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao buscar tarefa para {data:dd/MM/yyyy}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Erro: _tarefaController está null.");
                }

                // Adiciona o dia formatado na lista com estilo apropriado
                Dias.Add(new DiaItem
                {
                    TextoBotao = $"{data:dd/MM/yyyy} ({data:dddd}) -- {titulo}", // Exibe a data, dia da semana e título/tarefa
                    CorFundo = ObterCorDeFundo(data.ToDateTime(TimeOnly.MinValue)), // Define a cor do botão com base no tipo do dia
                    CorTexto = data == DateOnly.FromDateTime(DateTime.Today) ? Colors.White : cor, // Define a cor do texto                    
                });
            }

            // Notifica que a coleção foi alterada
            OnPropertyChanged(nameof(Dias));
        }




        // Método para definir a cor de fundo do botão baseado no tipo do dia
        private Color ObterCorDeFundo(DateTime data)
        {
            if (data == DateTime.Today) return Colors.Black; // O dia atual fica preto
            return (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                ? Colors.Orange // Finais de semana ficam laranja
                : Colors.DarkGray; // Dias normais ficam cinza escuro
        }

        // Evento acionado ao clicar no botão "←" (retroceder 7 dias)
        private void OnAnteriorClicked(object sender, EventArgs e)
        {
            dataAtual = dataAtual.AddDays(-7); // Retrocede a semana
            AtualizarListaDiasAsync(); // Atualiza a interface
        }

        // Evento acionado ao clicar no botão "→" (avançar 7 dias)
        private void OnProximoClicked(object sender, EventArgs e)
        {
            dataAtual = dataAtual.AddDays(7); // Avança a semana
            AtualizarListaDiasAsync(); // Atualiza a interface
        }

        // Evento acionado ao clicar em um botão de dia no calendário
        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button botao)
            {
                string textoBotao = botao.Text;
                await Navigation.PushAsync(new TarefaPage(textoBotao));
            }
        }
    }

    // Classe que representa um item de dia no calendário
    public class DiaItem
    {
        public string TextoBotao { get; set; } // Texto exibido no botão (ex: "Nada a fazer 01/03/2025 (Sábado)")
        public Color CorFundo { get; set; } // Cor do fundo do botão (depende se é dia útil, final de semana ou hoje)
        public Color CorTexto { get; set; } // Cor do texto do botão
    }



}
