using NSubstitute;
using NUnit.Framework;
using SafeModel;
using SafeViewModel;

namespace SafeViewModelTests
{
    [TestFixture]
    public partial class SignInViewModelTests
    {
        private ISafeProviderForExistingUser _safeProviderForExistingUser;
        protected SignInViewModel _signInViewModel;

        [SetUp]
        public void SetUp()
        {
            _safeProviderForExistingUser = Substitute.For<ISafeProviderForExistingUser>();
        }
    }
}