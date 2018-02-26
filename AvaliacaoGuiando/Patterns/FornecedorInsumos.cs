using AvaliacaoGuiando.Enums;
using AvaliacaoGuiando.Model;
using AvaliacaoGuiando.Servicos.Abstrato;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Patterns
{
    public class FornecedorInsumos : ICsvFaturaService
    {
        private const int IDX_COD_DOCUMENTO = 0;
        private const int IDX_VALOR = 1;
        private const int IDX_DATA = 2;
        private const int IDX_DESCRITIVO = 3;
        public FaturaModel ExtractData(string content)
        {
            var cultureInfo = new CultureInfo("pt-BR");

            var linhas = content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            var arrayDados = linhas[1].Split('|');

            return new FaturaModel
            {
                NomeFornecedor = "FornecedorInsumos",
                CodDocumento = arrayDados[IDX_COD_DOCUMENTO],
                ValorTotal = double.Parse(arrayDados[IDX_VALOR], NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.Currency, cultureInfo),
                DataVencimento = DateTime.ParseExact(arrayDados[IDX_DATA], "dd/MM/yyyy", cultureInfo),
                CategoriaFatura = Categorias.MateriaPrima
            };
        }
    }
}
