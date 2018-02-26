using AvaliacaoGuiando.Patterns;
using AvaliacaoGuiando.Servicos.Abstrato;
using AvaliacaoGuiando.Servicos.Concreto;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaliacaoGuiando.DI
{
    public class NinjectBindings : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IFaturaService>().To<FaturaPdfService>().Named(".pdf");
            Bind<IFaturaService>().To<FaturaCSVService>().Named(".csv");

            Bind<IPdfFaturaService>().To<Acessa>().Named("Acessa");
            Bind<IPdfFaturaService>().To<Cemig>().Named("Cemig");
            Bind<ICsvFaturaService>().To<FornecedorInsumos>().Named("FornecedorInsumos");
        }
    }
}
