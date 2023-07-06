namespace Slayer.DTOS
{

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Definition
    {
        public string ClassType { get; set; }
        public string Intent { get; set; }
        public string PrivateID { get; set; }
        public string PublicID { get; set; }
        public string Name { get; set; }
        public List<ItemElement> ItemElements { get; set; }
    }

    public class ItemElement
    {
        public string Name { get; set; }
        public List<Values> Values { get; set; }
        public string Type { get; set; }
        public string Intent { get; set; }
    }

    public class Values
    {
        public string Value { get; set; }
    }

    public class ItemElemento
    {
        public string Name { get; set; }
        public List<Incompatibilidade> Values { get; set; }
        public string Type { get; set; }
        public string Coupling { get; set; }
        public string Intent { get; set; }
    }

    public class KRoot
    {
        public string ClassType { get; set; }
        public string Intent { get; set; }
        public string PublicID { get; set; }
        public List<ItemElemento> ItemElements { get; set; }
    } 

    public class Incompatibilidade
    {
        public string Value { get; set; }
        public Definition Definition { get; set; } 
    }

}