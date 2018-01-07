namespace SafeModel.Standard
{
    public class PasswordRecord
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public override bool Equals(object obj)
        {
            var passwordRecord = obj as PasswordRecord;
            return Equals(passwordRecord);
        }

        protected bool Equals(PasswordRecord other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return 243;
        }
    }
}
