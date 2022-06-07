using System.Collections.Generic;
using System.Text;

namespace indexador.Models
{
    public class EventoSaude
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Estrutura { get; set; }
        public List<string> Tags { get; set; }
        
        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(Nome).Append(" ");
            buffer.Append(Estrutura).Append(" ");
            buffer.Append(string.Join(" ", Tags));
            return buffer.ToString();
        }
    }
}