using System.Text.RegularExpressions;
using Refit;
using Slayer.Interfaces;


namespace Slayer.Class
{
    public class ConsultaInfo
    {
        ContructJson Json = new ContructJson();
        public string pat = @"PCT\d{3}|DSC\d{3}|SRV\d{3}|PR\d{5}";
        public string packageIdProd = "";
        
        List<string> lost = new List<string>();

        public async Task<List<string>> RegexStringProdutoAsync(string code, string valor)
        {
               
            var catClient = RestService.For<IGetProduct>("http://10.151.2.93:8000");
        
            try
            {
                var produto = await catClient.GetProductAsync(code);
                    
                packageIdProd = produto.PackageId;

                var dataEditada = await LimpaStringAsync(valor);

                if(dataEditada.Count > 0 && packageIdProd != null)
                {
                    await Json.ConstroiDadosJson(code, packageIdProd, dataEditada);

                } else 
                {
                    lost.Add(code); 
                }
            }
            catch (Exception)
            {   
                Console.WriteLine("=====================================================================================");
                Console.WriteLine("======== Ocorreu algum erro, por favor verificar conexão com VPN e Internet =========");
                Console.WriteLine("=====================================================================================");
                Console.Clear();

                GC.Collect();

                await Menu.ValidaArquivoAsync();       
            }

            return lost;

        }       

        public async Task<List<string>> LimpaStringAsync(string vet)
        {
            List<string> dados = new List<string>();

            Regex rg = new Regex(pat);

            MatchCollection pacotes = rg.Matches(vet);

            for(int i=0; i < pacotes.Count; i++)
            {
                
               bool v = await ConsultaPacoteAsync(pacotes[i].Value);
            
               if(v)
               {
                    dados.Add(pacotes[i].Value);
               } 

            }

            async Task<bool> ConsultaPacoteAsync(string code)
            {
               
               var catClient = RestService.For<IGetProduct>("http://10.151.2.93:8000");
               
               try
               {
                    var produto = await catClient.GetProductAsync(code);

                    return true;
               }
               catch (Exception)
               {
                    return false;
               }    
            }

            return dados;
            
        }


        public void CaminhoAsync(int flag)
        {
           if(flag == 2)
           {
               Json.CriaArquivoJson();
           }
            
        }       

    }

}