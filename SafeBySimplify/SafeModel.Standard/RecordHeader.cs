namespace SafeModel.Standard
{
    public class RecordHeader
    {
        public string Name { get; set; }
        public string Tags { get; set; }
        public string Id { get; set; }

        public override bool Equals(object obj)
        {
            var header = obj as RecordHeader;
            if (header == null) return false;
            return Equals(header);
        }

        protected bool Equals(RecordHeader other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Tags, other.Tags) && string.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return 52322;
        }
    }
}
