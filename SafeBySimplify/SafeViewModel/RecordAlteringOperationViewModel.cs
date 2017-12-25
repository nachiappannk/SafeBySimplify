﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using SafeModel;

namespace SafeViewModel
{
    public class RecordAlteringOperationViewModel : SingleOperationViewModel
    {
        private readonly ISafe _safe;

        public RecordAlteringOperationViewModel(ISafe safe, string recordId)
        {
            _safe = safe;
            Record = new RecordViewModel(CreateEmptyRecord(recordId), safe, new IdGenerator());
            var record = safe.GetRecord(recordId);
            Record.Name = record.Header.Name;
            Record.Tags = record.Header.Tags;
            Record.Id = record.Header.Id;
            Record.PasswordRecords = new ObservableCollection<PasswordRecordViewModel>();
        }

        public RecordViewModel Record { get; set; }

        private Record CreateEmptyRecord(string recordId)
        {
            var record = new Record
            {
                Header = new RecordHeader(),
                FileRecords = new List<FileRecord>(),
                PasswordRecords = new List<PasswordRecord>()
            };
            record.Header.Id = recordId;
            return record;
        }
    }
}