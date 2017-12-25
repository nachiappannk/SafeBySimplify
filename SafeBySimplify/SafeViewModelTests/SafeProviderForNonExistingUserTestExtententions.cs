/*
MIT License

Copyright(c) 2017 Nachiappan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using NSubstitute;
using SafeModel;

namespace SafeViewModelTests
{
    public static class SafeProviderTestExtententions
    {
        public static void StubUserNameValidity(this ISafeProviderForNonExistingUser safeProvider,
            string username, bool expectedValue, string expectedErrorMessage)
        {
            // ReSharper disable once RedundantAssignment
            string errorMessage = string.Empty;
            safeProvider.IsUserNameValidForNonExistingUser(username, out errorMessage).Returns(x =>
            {
                x[1] = expectedErrorMessage;
                return expectedValue;
            });
        }

        public static void StubPasswordNameValidity(this ISafeProviderForNonExistingUser safeProvider,
            string password, bool expectedValue, string expectedErrorMessage)
        {
            // ReSharper disable once RedundantAssignment
            string errorMessage = string.Empty;
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