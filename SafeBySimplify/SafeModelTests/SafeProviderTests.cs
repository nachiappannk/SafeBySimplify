using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using SafeModel;

namespace SafeModelTests
{
    [TestFixture]
    public class SafeProviderTests
    {
        //[Test]
        //public void
        //    When_settings_gateway_is_not_having_working_directory_then_current_directory_is_used_as_working_directory()
        //{
        //    ISettingGateway settingGateway = Substitute.For<ISettingGateway>();
        //    settingGateway.IsWorkingDirectoryAvailable().Returns(false);
        //    var currentDirectory = @"C:\Temp";
        //    settingGateway.GetCurrentDirectory().Returns(currentDirectory);

        //    ISafeProvider safeProvider = new SafeProvider(settingGateway);

        //    Assert.AreEqual(currentDirectory,safeProvider.WorkingDirectory);

        //}

        //[Test]
        //public void
        //    When_settings_gateway_is_having_working_directory_then_settings_working_directory_is_used_as_working_directory()
        //{
        //    ISettingGateway settingGateway = Substitute.For<ISettingGateway>();
        //    settingGateway.IsWorkingDirectoryAvailable().Returns(true);
        //    var currentDirectory = @"C:\Temp";
        //    var workingDirectory = @"C:\Temp1234";
        //    settingGateway.GetCurrentDirectory().Returns(currentDirectory);
        //    settingGateway.GetWorkingDirectory().Returns(workingDirectory);
        //    ISafeProvider safeProvider = new SafeProvider(settingGateway);
        //    Assert.AreEqual(workingDirectory, safeProvider.WorkingDirectory);
        //}

        //[Test]
        //public void When_working_directory_is_updated_then_setting_is_updated_with_the_value()
        //{
        //    ISettingGateway settingGateway = Substitute.For<ISettingGateway>();
        //    settingGateway.IsWorkingDirectoryAvailable().Returns(true);
        //    var workingDirectory = @"C:\Temp";
        //    settingGateway.GetWorkingDirectory().Returns(workingDirectory);

        //    var newWorkingDirectory = @"C:\NewWorkingDirectory";
        //    settingGateway.PutWorkingDirectory(newWorkingDirectory);

        //    settingGateway.Received(1).PutWorkingDirectory(newWorkingDirectory);


        //}

    }
}
