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
            var safeProvider = new SafeProvider();
            return new WorkFlowViewModel(safeProvider);
        }

        public static WorkFlowViewModel CreateWorkFlowViewModelWithTestSetting()
        {
            var safeProvider = new SafeProvider();
            return new WorkFlowViewModel(safeProvider);
        }
    }
}
