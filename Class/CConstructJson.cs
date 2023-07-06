using System.Text.Json;
using Refit;
using Slayer.DTOS;
using Slayer.Interfaces;
using System.Text.RegularExpressions;

namespace Slayer.Class
{
    public class ContructJson
    {
   
        public List<KRoot> produtoJson = new List<KRoot>();
        public string classType = "";

    
        public async Task ConstroiDadosJson(string code, string publicId, List<string> incompa)
        {
           
            switch (code)
            {
                case var ps when new Regex(@"PCT\d{3}|SRV\d{3}").IsMatch(ps):
                    classType = "Template_Oferta_Movel_Plugin";
                    break;
                case var d when new Regex(@"DSC\d{3}").IsMatch(d):
                    classType = "Template_Oferta_Movel_Desconto";
                    break;
                case var o when new Regex(@"PR\d{5}").IsMatch(o):
                    classType = "Template_Oferta_Movel_Legado";
                    break;
                default:
                    break;
            }


            var item = new ItemElemento
            {
                Name = "Incompatible_with",
                Values = await ConsultaAPI(incompa),
                Type = "Any",
                Coupling = "Tight",
                Intent = "Modify"
            };

            var produto = new KRoot
            {
                ClassType = $"{classType}",
                Intent = "Modify",
                PublicID = $"{publicId}",
                ItemElements = new List<ItemElemento>()
                {
                    item
                }

            };

           produtoJson.Add(produto);
           

        }
       
       public async Task<List<Incompatibilidade>> ConsultaAPI(List<string> incomps)
       {
            
            var catClient = RestService.For<IGetProduct>("http://10.151.2.93:8000");

            var incompatibilidade = new List<Incompatibilidade>();

                foreach (var bp in incomps)
                {
                   
                   var Guid = GeraGUID();
                   var produto = await catClient.GetProductAsync(bp); 
                
                   var valor = new Values
                   {
                       Value = $"{produto.PackageId}|Element_Guid"
                   };

                       var elem = new ItemElement
                   {
                       Name = "Entity_Reference",
                       Values = new List<Values>()
                   {
                       valor
                   },
                       Type = "ElementReference",
                       Intent = "New"

                   };

                   var incompatibilidades = new Incompatibilidade
                   {
                       Value = $"{Guid}",
                       Definition = new Definition
                       {
                           ClassType = "Sigma_Entity_Reference_List",
                           Intent = "New",
                           PrivateID = $"{Guid}",
                           PublicID = $"{Guid}",
                           Name = $"{Guid}",
                           ItemElements = new List<ItemElement>()
                           {
                               elem
                           }
                       }
                   };

                   incompatibilidade.Add(incompatibilidades);
                }

            return incompatibilidade;
        }

        public string GeraGUID()
        {   

            Guid uuid = Guid.NewGuid();
            string uuidString = uuid.ToString();
              
            return uuidString;
        }

        public async void CriaArquivoJson()
        {
            Console.Clear();
            Console.WriteLine(Figgle.FiggleFonts.Cosmike.Render("Slayer 1"));
            Console.WriteLine("================================== Gravando Arquivo =====================================");
            Console.WriteLine("======= Informe o nome do Arquivo com a extensão .json junto ao diretório destino =======");
            Console.WriteLine("============================= <diretorio><arquivo.json> =================================");
            Console.Write("Dir: ");
            var nomearquivo = Console.ReadLine();
            Console.Clear();

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(produtoJson, options);
                File.WriteAllText(nomearquivo, json);

                Console.WriteLine(Figgle.FiggleFonts.Cosmike.Render("Slayer 1"));
                Console.WriteLine("=====================================================================================");
                Console.WriteLine("========================= Arquivo Gerado com Sucesso!!! =============================");
                Console.WriteLine("=========================== Tecle Enter Para Continuar ==============================");
                while (Console.ReadKey().Key != ConsoleKey.Enter){}
                Console.Clear();
            }
            catch (Exception)
            {
                Console.WriteLine("=====================================================================================");
                Console.WriteLine("============================= Erro ao Gerar o Arquivo ===============================");
                Console.WriteLine("============================ Tecle Enter Para Continuar =============================");
                while (Console.ReadKey().Key != ConsoleKey.Enter){}
                Console.Clear();

                GC.Collect();

                await Menu.ValidaArquivoAsync();
            }
            
        }
     
    }
}


   
