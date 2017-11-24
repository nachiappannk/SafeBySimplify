using NSubstitute;
using SafeModel;

namespace SafeViewModelTests
{
    public static class SafeProviderTestExtententions
    {
        public static void StubUserNameValidity(this ISafeProviderForNonExistingUser safeProvider,
            string username, bool expectedValue, string expectedErrorMessage)
        {
            string errorMessage = "";
            safeProvider.IsUserNameValidForNonExistingUser(username, out errorMessage).Returns(x =>
            {
                x[1] = expectedErrorMessage;
                return expectedValue;
            });
        }

        public static void StubPasswordNameValidity(this ISafeProviderForNonExistingUser safeProvider,
            string password, bool expectedValue, string expectedErrorMessage)
        {
            string errorMessage = "";
            safeProvider.IsPasswordValidForNonExistingUser(password, out errorMessage).Returns(x =>
            {
                x[1] = expectedErrorMessage;
                return expectedValue;
            });
        }

        public static void StubCreateSafeForExistingUser(this ISafeProviderForExistingUser safeProvider, string validUserName, string validPassword, ISafe safe)
        {
            ISafe outSafe;
            safeProvider.TryCreateSafeForExistingUser(validUserName, validPassword, out outSafe)
                .Returns(x =>
                {
                    x[2] = safe;
                    return true;
                });
        }
    }
}