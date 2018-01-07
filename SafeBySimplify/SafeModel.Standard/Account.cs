namespace SafeModel.Standard
{
    public class Account
    {
        public byte[] MasterEncryptedPassBytes { get; set; }
        public string VerifyingWord { get; set; }
        public byte[] VeryifyingWordEncryptedBytes { get; set; }

    }
}
