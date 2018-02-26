using AvaliacaoGuiando.Enums;
using AvaliacaoGuiando.Model;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Servicos.Abstrato
{
    public interface IFaturaService
    {
        FaturaModel ExtractData(Stream conteudoFatura);
        Fornecedor DetectarFornecedorFatura(string conteudoFatura);
        string GetConteudoFatura(Stream content);
    }
}
