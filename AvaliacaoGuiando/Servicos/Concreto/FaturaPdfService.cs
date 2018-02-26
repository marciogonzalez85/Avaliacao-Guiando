using AvaliacaoGuiando.Config;
using AvaliacaoGuiando.Model;
using AvaliacaoGuiando.Servicos.Abstrato;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Newtonsoft.Json;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Servicos.Concreto
{
    public class FaturaPdfService : IFaturaService
    {
        readonly IResolutionRoot ResolutionRoot;

        public FaturaPdfService(IResolutionRoot resolutionRoot)
        {
            this.ResolutionRoot = resolutionRoot;
        }

        public string GetConteudoFatura(Stream content)
        {
            StringBuilder text = new StringBuilder();
            
            using(var pdfReader = new PdfReader(content))
                for(int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
                    text.Append(currentText);
                }
            
            return text.ToString();
        }

        public FaturaModel ExtractData(Stream content)
        {
            var conteudoFatura = this.GetConteudoFatura(content);
            var fornecedorFatura = this.DetectarFornecedorFatura(conteudoFatura);

            //Retorno caso não seja encontrado um pattern para o fornecedor (Fornecedor não encontrado)
            if (fornecedorFatura == Enums.Fornecedor.NotFound)
                return null;

            //Recupero o serviço de pdf específico do fornecedor
            var pdfFaturaService = ResolutionRoot.Get<IPdfFaturaService>(fornecedorFatura.ToString());
            
            //Extraio os dados da fatura para o modelo
            var data = pdfFaturaService.ExtractData(conteudoFatura);
            
            return data;
        }

        public Enums.Fornecedor DetectarFornecedorFatura(string conteudoFatura)
        {
            var keywordsSection = (KeyWordsSection)ConfigurationManager.GetSection("KeyWordsSection");

            foreach(var fornecedor in keywordsSection.Fornecedores)
            {
                //Caso alguma keyword não seja encontrada, vai para o próximo fornecedor
                if (fornecedor.KeyWordsList.Any(a => !conteudoFatura.Contains(a)))
                    continue;

                return (Enums.Fornecedor)Enum.Parse(typeof(Enums.Fornecedor), fornecedor.Name);
            }

            return Enums.Fornecedor.NotFound;
        }
    }
}
