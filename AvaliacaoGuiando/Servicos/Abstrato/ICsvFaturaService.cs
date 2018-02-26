using AvaliacaoGuiando.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.Servicos.Abstrato
{
    public interface ICsvFaturaService
    {
        FaturaModel ExtractData(string content);
    }
}
