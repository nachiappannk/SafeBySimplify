using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SafeModel.Standard;

namespace SafeModelTests
{
    [TestFixture]
    public partial class SafeProviderTests
    {
        private ISettingGateway _settingGateway;
        private string _initialWorkingDirectory = "C:\\SomeDirectory";
        private ISafeProvider _safeProvider;
        private IAccountGateway _accountGateway;
        private ICryptor _cryptor;

        [SetUp]
        public void SetUp()
        {
            _settingGateway = new SettingGatewayForTests();
            _settingGateway.SetWorkingDirectory(_initialWorkingDirectory);
            var safeProvider = new SafeProvider();
            _accountGateway = Substitute.For<IAccountGateway>();
            _cryptor = Substitute.For<ICryptor>();

            safeProvider.SettingGateway = _settingGateway;
            safeProvider.AccountGateway = _accountGateway;
            safeProvider.Cryptor = _cryptor;
            _safeProvider = safeProvider;
        }
    }



}
