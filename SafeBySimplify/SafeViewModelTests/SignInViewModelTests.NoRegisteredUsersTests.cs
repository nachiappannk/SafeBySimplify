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
                SignInViewModel = new SignInViewModel(_safeProviderForExistingUser, 
                    (s, n) => { });
            }

            [Test]
            public void When_there_are_no_user_accounts_then_sign_in_is_disabled()
            {
                Assert.AreEqual(false, SignInViewModel.IsEnabled);
            }
        }
    }
}