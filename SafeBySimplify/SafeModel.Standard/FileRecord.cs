namespace SafeModel.Standard
{
    public class FileRecord
    {
        public string Name { get; set; }
        public string Extention { get; set; }
        public string Description { get; set; }
        public string FileId { get; set; }
        public string AssociatedRecordId { get; set; }
        public override bool Equals(object obj)
        {
            var fileRecord = obj as FileRecord;
            return Equals(fileRecord);
        }

        protected bool Equals(FileRecord fileRecord)
        {
            return string.Equals(Name, fileRecord.Name)
                   && string.Equals(Extention, fileRecord.Extention)
                   && string.Equals(Description, fileRecord.Description)
                   && string.Equals(FileId, fileRecord.FileId)
                   && string.Equals(AssociatedRecordId, fileRecord.AssociatedRecordId);
        }

        public override int GetHashCode()
        {
            return 2445;
        }
    }
}
