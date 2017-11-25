using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeModel;
using SafeViewModel;

namespace SafeBySimplify.BootStrapper
{
    public static class  ViewModelFactory
    {

        public static WorkFlowViewModel CreateWorkFlowViewModel()
        {
            return new WorkFlowViewModel(new SafeProvider(new SettingGateway()));
        }

        public static WorkFlowViewModel CreateWorkFlowViewModelWithTestSetting()
        {
            var settings = new SettingGatewayForTests();
            settings.SetWorkingDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            return new WorkFlowViewModel(new SafeProvider(settings));
        }
    }
}
