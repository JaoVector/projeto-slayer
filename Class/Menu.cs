using System.Xml.Linq;
using Figgle;

namespace Slayer.Class
{
    public static class Menu
    {
        public static string? arq;

        public static async Task ValidaArquivoAsync()
        {
            Console.WriteLine(FiggleFonts.Cosmike.Render("Slayer 1"));
            Console.WriteLine("Aplicação que gera um arquivo Json para cadastro das Incompatibilidades no SIGMA");
            Console.WriteLine("Dev by: João Victor Carneiro Aureliano");
            Console.WriteLine("======================================================================================");
            Console.WriteLine("============================ Informe o XSLT das Promoções ============================");
            Console.WriteLine("===============================  Digite 1 para sair ==================================");
            Console.Write("Dir: ");
            arq = Console.ReadLine();
            Console.Clear();

            if(arq == "1")
            {
                System.Environment.Exit(0);
            }

            try
            {
                XDocument test = XDocument.Load(arq);
                test.Save(arq, SaveOptions.None);
                Programa pro = new Programa();
                await pro.ExecutaAsync(arq);
            }
            catch (Exception)
            {
                Programa erro = new Programa();  
                await erro.ErroMSGAsync();
            }

        }
    }

    public class Programa
    {
        public async Task ExecutaAsync(string arquivo)
        {

            Dictionary<string, string> DicioProdutos = new Dictionary<string, string>();
            var lost = new List<string>();   
            ColetaPCTS obj = new ColetaPCTS();

            try
            {
                var a = obj.PCTSArquivo(arquivo);
                
                if(obj.aviso && a != null)
                {
                    await obj.RegexStringAsync(a);
                    Console.WriteLine(Figgle.FiggleFonts.Cosmike.Render("Slayer 1"));
                    ColetaIncompativeis obj2 = new ColetaIncompativeis();
                    DicioProdutos = obj2.VetorIncompativeis(obj);
                    ConsultaInfo info = new ConsultaInfo();

                    if(DicioProdutos.Count > 0)
                    {  
                        Console.WriteLine("==================================================================================");
                        Console.WriteLine("=============================== Aguarde ==========================================");
                        foreach (KeyValuePair<string, string> kvp in DicioProdutos)
                        {
                            lost = await info.RegexStringProdutoAsync(kvp.Key, kvp.Value); 
                        }

                  
                        info.CaminhoAsync(obj2.flag);
                        await Resultados(lost, obj);

                    } else 
                    {
                        await ErroMSGAsync();
                    }
                    

                } else 
                {
                    await ErroMSGAsync();
                }

            }

            catch (System.Exception)
            {
                    
                await ErroMSGAsync();
            }   
            
        }

        public async Task Resultados(List<string> pctsInvalidos, ColetaPCTS objIn)
        {
            Console.Clear();
            Console.WriteLine(Figgle.FiggleFonts.Cosmike.Render("Slayer 1"));
            if (pctsInvalidos.Count > 0)
            {

                Console.WriteLine("=====================================================================================");
                Console.WriteLine("========================== Produtos sem Incompatibilidades ==========================");
                foreach (var i in pctsInvalidos)
                {
                    Console.WriteLine($"Produto: {i}");
                }

            }
            else
            {
                Console.WriteLine("================= Todos Produtos Possuem Incompatibilidades =========================");
            }

            if (objIn.PCTSNaoEncontrados.Count > 0)
            {
                Console.WriteLine("=====================================================================================");
                Console.WriteLine("============================ Produtos Não Encontrados ===============================");
                

                foreach (var v in objIn.PCTSNaoEncontrados)
                {
                    Console.WriteLine($"Produto: {v}");
                }

            }
            else
            {
                Console.WriteLine("============================ Todos os Produtos Existem ==============================");
            }

            Console.WriteLine("=====================================================================================");
            Console.WriteLine("============================ Clique Enter Para Encerrar =============================");
            Console.WriteLine("=====================================================================================");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }
            Console.Clear();

            GC.Collect();

            await Menu.ValidaArquivoAsync();
        }

        public async Task ErroMSGAsync()
        {
            Console.WriteLine("================================ EXCEPTION =======================================");
            Console.WriteLine("===== Verifique se o Diretório informado leva até o arquivo XSLT da Promoção =====");
            Console.WriteLine("================= Verifique se o arquivo XSLT pode ser editado ===================");
            Console.WriteLine("==================================================================================");
            Console.WriteLine("======================== Tecle Enter Para Continuar ==============================");
            while (Console.ReadKey().Key != ConsoleKey.Enter){}
            Console.Clear();

            GC.Collect();

            await Menu.ValidaArquivoAsync();
        }
    }
}