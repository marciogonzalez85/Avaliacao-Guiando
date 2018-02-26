using AvaliacaoGuiando.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Servicos.Abstrato
{
    public interface IPdfFaturaService
    {
        FaturaModel ExtractData(string content);
    }
}
