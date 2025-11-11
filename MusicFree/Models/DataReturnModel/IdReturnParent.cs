namespace MusicFree.Models.DataReturnModel
{
    public class IdReturnParent: ReturnParent
    {
        public Guid Id { get; set; }


        public IdReturnParent() { }
        
        public IdReturnParent(string name, Guid id) : base(name)
        {
            Id = id;
        }
    }
}
