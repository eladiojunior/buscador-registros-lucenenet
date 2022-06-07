using System.Collections.Generic;
using System.Text;

namespace indexador.Models
{
    public class Especialidade
    {
        public int Id { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public List<EventoSaude> Eventos { get; set; }
        public List<string> Tags { get; set; }
        
        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(Nome).Append(" ");
            buffer.Append(Descricao).Append(" ");
            buffer.Append(string.Join(" ", Eventos)).Append(" ");
            buffer.Append(string.Join(" ", Tags));
            return buffer.ToString();
        }
        
    }
}