using System.Xml.Linq;
using System.Text.RegularExpressions;
using Refit;
using Slayer.Interfaces;


namespace Slayer.Class
{
    public class ColetaPCTS
   {
        public string? localarq {get; set;}
        public List<string> PCTS = new List<string>();
        public List<string> Liaux = new List<string>();

        public List<string> PCTSNaoEncontrados = new List<string>();

        public int flag = 0;

        public bool aviso = true;

        public string pattern = @"PCT\d{3}|DSC\d{3}|SRV\d{3}|PR\d{5}";
        

        public string PCTSArquivo(string dir)
        {

            localarq = dir;

            flag = 1;

            XDocument arquivo = XDocument.Load(dir);
         
            IEnumerable<XElement> cods = from od in arquivo.Descendants("Product")
                                      select od.Element("PartNumber"); 
            if(cods.Count() > 0)
            {
                foreach (XElement i in cods)
                {
                    string valores = string.Concat(i.Nodes());
                    valores.ToString();
                    PCTS.Add(valores);
                }

            } else 
            {
                aviso = false;
            }                          
            
            return Formata(PCTS);
        }

        public string Formata(List<string> values)
        {
                var vetor = "";
                if(values.Count > 0)
                {
                
                    foreach (var item in values)
                    {
                   
                        vetor += item + ",";
                   
                    }

                    vetor = vetor[..^1];

                    
                }
                
                return vetor;
                
        }

        public virtual async Task RegexStringAsync(string vet)
        {
            
            Regex rg = new Regex(pattern);

            MatchCollection pacotes = rg.Matches(vet);

            for(int i=0; i < pacotes.Count; i++)
            {
                
                bool v = await ConsultaPacoteAsync(pacotes[i].Value);

            
                if(v)
                {
                    Liaux.Add(pacotes[i].Value);
                } 
                else if(v != true && flag == 1)
                {
                    PCTSNaoEncontrados.Add(pacotes[i].Value);
                } else 
                {
                    //Não Acontece Nada
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


        }         
   } 

   public class ColetaIncompativeis : ColetaPCTS
   {    
        public List<string> PctsFormatados = new List<string>();
        public List<string> Incompatibilities = new List<string>();

        public Dictionary<string, string> finalGroup = new Dictionary<string, string>();


        public Dictionary<string, string> VetorIncompativeis(ColetaPCTS coletaPCTS)
        {
            flag = 2;
            XDocument diretorio = XDocument.Load(coletaPCTS.localarq);

            coletaPCTS.flag = 0;
            
            foreach (var i in coletaPCTS.Liaux)
            {
                PctsFormatados.Add(i);
            }

            
            foreach (string jo in PctsFormatados)
            {
                string vals = "";
                Incompatibilities.Clear();

                IEnumerable<XElement> incompativeis = from comp in diretorio.Descendants("Product")
                                                      where (string)comp.Element("PartNumber") == jo
                                                      select comp.Element("ListOfCompatibilityRule");

                foreach (XElement comp in incompativeis)
                {
                    IEnumerable<XElement> inpcts = from v in comp.Descendants("CompatibilityRule")
                                                   select v.Element("PartNumber");

                    foreach (XElement v in inpcts)
                    {
                        
                        if(v != null)
                        {   
                            vals = string.Concat(v.Nodes());
                            vals.ToString();
                            Incompatibilities.Add(vals);
                        }
                        
                    }
                }

               
                string incomps = coletaPCTS.Formata(Incompatibilities);
                 
                finalGroup.Add(jo, incomps);
                incomps = "";       
                
            } 
       
              return finalGroup;
        }
      

   }

}

/*

/*
                IEnumerable<XElement> incompativeis = from comp in diretorio.Descendants("Product")
                                                      where (string)comp.Element("PartNumber") == item
                                                      select comp.Element("ListOfCompatibilityRule");

                foreach (XElement j in incompativeis)
                {
                    Console.WriteLine(incompativeis);
                }
            */  

