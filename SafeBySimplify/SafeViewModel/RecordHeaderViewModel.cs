using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
using SafeModel;

namespace SafeViewModel
{
    public class RecordHeaderViewModel
    {
        public RecordHeaderViewModel(RecordHeader recordHeader, Action selectAction)
        {
            RecordHeader = recordHeader;
            Name = recordHeader.Name;
            Tags = recordHeader.Tags;
            SelectCommand = new DelegateCommand(selectAction);
            Id = recordHeader.Id;
        }
        public RecordHeader RecordHeader { get; set; }
        public string Name { get; set; }
        public string Tags { get; set; }
        public DelegateCommand SelectCommand { get; set; }
        public string Id { get; set; }
    }
}