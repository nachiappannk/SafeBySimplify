using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class RecordHeaderViewModel
    {
        public RecordHeaderViewModel(RecordHeader recordHeader)
        {
            RecordHeader = recordHeader;
            Name = recordHeader.Name;
            Tags = recordHeader.Tags.ToList();
            SelectCommand = new DelegateCommand(() => { });
        }
        public RecordHeader RecordHeader { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public DelegateCommand SelectCommand { get; set; }
    }
}