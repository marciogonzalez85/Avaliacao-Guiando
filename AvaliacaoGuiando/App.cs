using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Reflection;
using AvaliacaoGuiando.DI;
using Ninject;
using AvaliacaoGuiando.Servicos.Abstrato;
using AvaliacaoGuiando.Patterns;
using AvaliacaoGuiando.Model;

namespace AvaliacaoGuiando
{
    public class App
    {
        static void Main(string[] args)
        {
            StandardKernel _kernal = new StandardKernel();
            _kernal.Load(Assembly.GetExecutingAssembly());
            
            var pathRelativo = ConfigurationManager.AppSettings["PathFaturas"];
            var pathAbsoluto = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}{pathRelativo}";

            //Crio o diretório de entrada de faturas caso ele ainda não exista
            if (!Directory.Exists(pathAbsoluto))
                Directory.CreateDirectory(pathAbsoluto);

            //Lista de faturas processadas
            List<FaturaModel> Faturas = new List<FaturaModel>();

            //Varro os arquivos da pasta de entrada
            foreach(var file in Directory.GetFiles(pathAbsoluto))
            {
                //Recupero o serviço específico do tipo do arquivo [file]
                var faturaService = _kernal.Get<IFaturaService>(Path.GetExtension(file));
                using(var sr = new StreamReader(file))
                {
                    //Extraio o conteúdo da fatura
                    var faturaModel = faturaService.ExtractData(sr.BaseStream);

                    if(faturaModel != null)
                    {
                        Faturas.Add(faturaModel);

                        //TODO: Mover a fatura para uma pasta de processados
                    }
                        
                }
                
            }
        }
    }
}
