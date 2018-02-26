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
    public class Acessa : IPdfFaturaService
    {
        public const string TEMPLATE_DADOS_DOCUMENTO = @"^([0-9]+)\s([0-9./-]+)\s([0-9]{2}/[0-9]{2}/[0-9]{4})\s([0-9.,]+)\s$";
        public const string HEADER_LINHA_VALOR_COBRADO = @"\(=\) Valor cobrado\n([0-9.,]+)";

        public FaturaModel ExtractData(string content)
        {
            var dadosDocumento = Regex.Match(content, TEMPLATE_DADOS_DOCUMENTO, RegexOptions.Multiline);
            var linhaValorCobrado = Regex.Match(content, HEADER_LINHA_VALOR_COBRADO, RegexOptions.Multiline);

            var cultureInfo = new CultureInfo("pt-BR");

            var model = new FaturaModel
            {
                NomeFornecedor = "ACESSA TELECOMUNICACOES LTDA",
                CodDocumento = dadosDocumento.Groups[1].Value,
                DataVencimento = DateTime.ParseExact(dadosDocumento.Groups[3].Value, "dd/MM/yyyy", cultureInfo),
                ValorTotal = double.Parse(linhaValorCobrado.Groups[1].Value, cultureInfo),
                CategoriaFatura = Enums.Categorias.Internet
            };

            return model;
        }
    }
}
