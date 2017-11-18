using System;
using NUnit.Framework;
using Prism.Commands;

namespace SafeViewModelTests
{
    public static  class CommandAssertionExtentions
    {
        public static Func<CommandEventInfo> GetCommandEventInfoFactory(this DelegateCommand command)
        {
            CommandEventInfo eventInfo = new CommandEventInfo();
            eventInfo.WasEventReceived = false;
            eventInfo.WasTheSenderCorrect = false;
            command.CanExecuteChanged += (obj, e) =>
            {
                if (command == obj) eventInfo.WasTheSenderCorrect = true;
                eventInfo.ValueOfCanExecuteOnEvent = command.CanExecute();
                eventInfo.WasEventReceived = true;
            };

            return () => new CommandEventInfo()
            {
                ValueOfCanExecuteOnEvent = eventInfo.ValueOfCanExecuteOnEvent,
                WasTheSenderCorrect = eventInfo.WasTheSenderCorrect,
                WasEventReceived = eventInfo.WasEventReceived
            };
        }

        public static void AssertCommandEventHappenedWithCorrectParameters(this Func<CommandEventInfo> commandEventInfoFactory,
            bool expectedValueOfCanExecute)
        {
            var commandEventInfo = commandEventInfoFactory.Invoke();
            Assert.True(commandEventInfo.WasEventReceived);
            Assert.True(commandEventInfo.WasTheSenderCorrect);
            Assert.AreEqual(expectedValueOfCanExecute, commandEventInfo.ValueOfCanExecuteOnEvent);
        }
    }

    public class CommandEventInfo
    {
        public bool ValueOfCanExecuteOnEvent { get; set; }
        public bool WasEventReceived { get; set; }
        public bool WasTheSenderCorrect { get; set; }
    }
}