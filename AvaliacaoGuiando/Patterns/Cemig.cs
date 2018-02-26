using AvaliacaoGuiando.Model;
using AvaliacaoGuiando.Servicos.Abstrato;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Patterns
{
    public class Cemig : IPdfFaturaService
    {
        public const string HEADER_LINHA_VENCIMENTO_VALOR = @"^VENCIMENTO VALOR A PAGAR$\n([0-9]{2}/[0-9]{2}/[0-9]{4})\sR\$\s([0-9,.]+)";
        public const string TEMPLATE_NUMERO_MEDICAO = @"Energia\skWh\s([a-zA-Z0-9]+)";

        public FaturaModel ExtractData(string content)
        {
            var cultureInfo = new CultureInfo("pt-BR");

            var vencimentoValor = Regex.Match(content, HEADER_LINHA_VENCIMENTO_VALOR, RegexOptions.Multiline);

            var numeroMedicao = Regex.Match(content, TEMPLATE_NUMERO_MEDICAO, RegexOptions.Multiline);

            var model = new FaturaModel
            {
                NomeFornecedor = "CEMIG",
                CodDocumento = numeroMedicao.Groups[1].Value,
                DataVencimento = DateTime.ParseExact(vencimentoValor.Groups[1].Value, "dd/MM/yyyy", cultureInfo),
                ValorTotal = double.Parse(vencimentoValor.Groups[2].Value, cultureInfo),
                CategoriaFatura = Enums.Categorias.Energia
            };
            
            return model;
        }
    }
}