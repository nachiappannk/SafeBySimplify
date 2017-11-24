using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SafeViewModel;

namespace SafeViewModelTests
{
    public partial class SignInViewModelTests
    {
        public class NoRegisteredUsersTests : SignInViewModelTests
        {
            [SetUp]
            public void Setup()
            {
                _safeProviderForExistingUser.GetUserNames().Returns(new List<string>());
                _signInViewModel = new SignInViewModel(_safeProviderForExistingUser, Substitute.For<IHasSafe>(),
                    () => { });
            }

            [Test]
            public void When_there_are_no_user_accounts_then_sign_in_is_disabled()
            {
                Assert.AreEqual(false, _signInViewModel.IsEnabled);
            }

        }
    }
}