using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Agenda.Models
{
    public class Tarefa
    {
        [PrimaryKey] // A Data continua sendo a chave primária
        public string DataString { get; set; } // Armazena a data como string (YYYY-MM-DD)

        public string Titulo { get; set; } // Pode conter vários títulos separados por ";"
        public string Descricao { get; set; } // Pode conter várias descrições separadas por ";"

        [Ignore] // Essa propriedade não será salva diretamente no banco
        public DateOnly Data
        {
            get => DateOnly.Parse(DataString); // Converte string para DateOnly
            set => DataString = value.ToString("yyyy-MM-dd"); // Converte DateOnly para string
        }

        public Tarefa() { }

        public Tarefa(DateOnly data, string titulo, string descricao)
        {
            Data = data;
            Titulo = titulo;
            Descricao = descricao;
        }
    }
}

