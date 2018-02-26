using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Model
{
    public class FaturaModel
    {
        public string NomeFornecedor { get; set; }
        public string CodDocumento { get; set; }
        public DateTime DataVencimento { get; set; }
        public double ValorTotal { get; set; }
        public Enums.Categorias CategoriaFatura { get; set; }
    }
}
