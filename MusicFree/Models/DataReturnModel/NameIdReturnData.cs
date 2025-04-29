namespace MusicFree.Models.DataReturnModel
{
    public class NameIdReturnData
    {
        public string name {  get; set; }
        public Guid id { get; set; }

    public NameIdReturnData(string Name, Guid Id) {
            name = Name;
            id = Id;
        }   
    }
}
