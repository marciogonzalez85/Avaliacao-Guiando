using AvaliacaoGuiando.Config;
using AvaliacaoGuiando.Enums;
using AvaliacaoGuiando.Model;
using AvaliacaoGuiando.Servicos.Abstrato;
using Ninject;
using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Servicos.Concreto
{
    class FaturaCSVService : IFaturaService
    {
        private IResolutionRoot ResolutionRoot;

        public FaturaCSVService(IResolutionRoot resolutionRoot)
        {
            this.ResolutionRoot = resolutionRoot;
        }

        private string GetConteudoFatura(Stream content)
        {
            List<int> codigos = new List<int>();
            List<double> valor = new List<double>();
            List<string> descricao = new List<string>();

            string csvStringContent;

            using (var sr = new StreamReader(content))
                csvStringContent = sr.ReadToEnd();

            return csvStringContent;
        }

        public Fornecedor DetectarFornecedorFatura(string conteudoFatura)
        {
            var linhas = conteudoFatura.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            if(linhas.Length > 0)
            {
                //Tento encontrar o fornecedor pelas informações da linha header do csv
                var header = linhas[0];
                var fornecedor = this.TryDetectFornecedorByHeader(header);

                return fornecedor != Fornecedor.NotFound ? (Fornecedor)Enum.Parse(typeof(Fornecedor), fornecedor.ToString()) : Fornecedor.NotFound;
            }

            return Fornecedor.NotFound;
        }

        private Fornecedor TryDetectFornecedorByHeader(string header)
        {
            var campos = header.Split('|').ToArray();

            var keywordsSection = (KeyWordsSection)ConfigurationManager.GetSection("KeyWordsSection");

            var fornecedor = keywordsSection.Fornecedores.FirstOrDefault(a => a.KeyWordsList.All(x => campos.Contains(x.ToString())));

            return fornecedor != null ? (Enums.Fornecedor)Enum.Parse(typeof(Enums.Fornecedor), fornecedor.Name) : Fornecedor.NotFound;
            
        }

        public FaturaModel ExtractData(Stream content)
        {
            var data = this.GetConteudoFatura(content);
            var fornecedor = this.DetectarFornecedorFatura(data);

            var csvFaturaService = this.ResolutionRoot.Get<ICsvFaturaService>(fornecedor.ToString());
            var model = csvFaturaService.ExtractData(data);

            return model;
        }

        

        string IFaturaService.GetConteudoFatura(Stream content)
        {
            throw new NotImplementedException();
        }
    }
}
