using System.Collections.Generic;
using System.Text;

namespace indexador.Models
{
    public class Prestador
    {
        public int Id { get; set; }
        public long Codigo { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string TipoPrestador { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public List<Especialidade> Especialidades { get; set; }

        public override string ToString()
        {
            var buffer = new StringBuilder();
            buffer.Append(Codigo).Append(" ");
            buffer.Append(Nome).Append(" ");
            buffer.Append(CpfCnpj).Append(" ");
            buffer.Append(TipoPrestador).Append(" ");
            buffer.Append(Cidade).Append(" ");
            buffer.Append(Estado).Append(" ");
            buffer.Append(string.Join(" ", Especialidades));
            return buffer.ToString();
        }
    }
}