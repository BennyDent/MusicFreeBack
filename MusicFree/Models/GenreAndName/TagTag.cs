namespace MusicFree.Models.GenreAndName
{
    public class TagTag
    {
        public Tags first { get; set; }
        public Tags second { get; set; }
        public string first_id { get; set; }
        public string second_id { get; set; }

        public TagTag() { }

        public TagTag(Tags First, Tags Second)
        {

            first = First;
            second = Second;
            first_id = First.Name;
            second_id = First.Name;
        }

    }
}
