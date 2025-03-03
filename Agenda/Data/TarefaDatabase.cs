using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agenda.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Agenda.Data
{
    public class TarefaDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        // Construtor que inicializa o banco de dados
        public TarefaDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Tarefa>().Wait(); // Cria a tabela se não existir
        }

        // Inserir ou atualizar uma tarefa
        public Task<int> SalvarTarefaAsync(Tarefa tarefa)
        {
            return _database.InsertOrReplaceAsync(tarefa);
        }

        // Excluir uma tarefa
        public Task<int> ExcluirTarefaAsync(Tarefa tarefa)
        {
            return _database.DeleteAsync(tarefa);
        }

        // Buscar todas as tarefas
        public Task<List<Tarefa>> GetTarefasAsync()
        {
            string query = "SELECT * FROM Tarefa";
            return _database.QueryAsync<Tarefa>(query);
        }

        // Buscar tarefa específica por data
        public async Task<Tarefa> GetTarefaPorDataAsync(DateOnly data)
        {
            string dataFormatada = data.ToString("yyyy-MM-dd");
            string query = "SELECT * FROM Tarefa WHERE DataString = ?"; // Ajustado para DataString
            var resultado = await _database.QueryAsync<Tarefa>(query, dataFormatada);
            return resultado.FirstOrDefault(); // Retorna a primeira ou null caso não encontre
        }

        // Buscar tarefas entre um intervalo de datas
        public Task<List<Tarefa>> GetTarefasEntreDatasAsync(DateOnly dataInicio, DateOnly dataFim)
        {
            string inicio = dataInicio.ToString("yyyy-MM-dd");
            string fim = dataFim.ToString("yyyy-MM-dd");

            string query = "SELECT * FROM Tarefa WHERE Data BETWEEN ? AND ?";
            return _database.QueryAsync<Tarefa>(query, inicio, fim);
        }
    }
}
