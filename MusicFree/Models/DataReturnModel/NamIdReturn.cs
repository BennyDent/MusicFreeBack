namespace MusicFree.Models.DataReturnModel
{
    public class NameIdReturn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public NameIdReturn(Guid id, string name) { 
        
        Id= id;
        Name= name;
        }
    }
}
