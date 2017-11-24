using Prism.Commands;

namespace SafeViewModel
{
    public class SignInViewModel
    {
        public string SignInUserName { get; set; }
        public string SignInPassword { get; set; }
        public DelegateCommand SignInCommand { get; set; }
    }
}