using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Agenda.Data;
using Agenda.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agenda.Data
{
    public class TarefaController
    {
        private readonly TarefaDatabase _database;

        public TarefaController(TarefaDatabase database)
        {
            _database = database;
        }

        // Método para buscar todas as tarefas
        public async Task<List<Tarefa>> ObterTodasTarefas()
        {
            return await _database.GetTarefasAsync();
        }

        // Método para buscar uma única tarefa por data
        public async Task<Tarefa> ObterTarefaPorData(DateOnly data)
        {
            return await _database.GetTarefaPorDataAsync(data);
        }

        // Método para buscar tarefas entre datas
        public async Task<List<Tarefa>> ObterTarefasEntreDatas(DateOnly dataInicio, DateOnly dataFim)
        {
            return await _database.GetTarefasEntreDatasAsync(dataInicio, dataFim);
        }

        // Método para salvar uma tarefa (se já existir, substitui)
        public async Task SalvarTarefa(Tarefa tarefa)
        {
            await _database.SalvarTarefaAsync(tarefa);
        }

        // Método para excluir uma tarefa
        public async Task ExcluirTarefa(Tarefa tarefa)
        {
            await _database.ExcluirTarefaAsync(tarefa);
        }

        // Método para inicializar o banco de dados
        public static async Task<TarefaController> InicializarBancoDeDados(string dbPath)
        {
            var database = new TarefaDatabase(dbPath);
            return new TarefaController(database);
        }
    }
}
