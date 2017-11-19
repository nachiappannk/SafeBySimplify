using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Prism.Commands;

namespace SafeViewModelTests.TestTools
{
    public static  class CommandAssertionExtentions
    {
        public static CommandObserver GetDelegateCommandObserver(this DelegateCommand command)
        {
            CommandObserver observer = new CommandObserver();
            
            command.CanExecuteChanged += (obj, e) =>
            {
                if (command == obj) observer.WasTheSendersCorrect.Add(true);
                observer.ValueOfCanExecuteOnLatestEvent = command.CanExecute();
                observer.NumberOfEventsRecieved++;
            };
            return observer;
        }


        public static void AssetAllSendersWereCorrect(this CommandObserver commandObserver)
        {
            Assert.AreEqual(0, commandObserver.WasTheSendersCorrect.Count(x => false),"There was atleast one sender for this command that not correct");
        }

        public static void AssetThereWasAtleastOneCanExecuteChangedEventAndCommandIsExecutable(this CommandObserver commandObserver)
        {
            commandObserver.
                AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs(true);
        }

        public static void AssetThereWasAtleastOneCanExecuteChangedEventAndCommandIsNotExecutable(this CommandObserver commandObserver)
        {
            commandObserver.
                AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs(false);
        }

        public static void AssetThereWasAtleastOneCanExecuteChangedEventAndCommandExecutableStateIs
            (this CommandObserver commandObserver, bool executableState)
        {
            commandObserver.AssetAllSendersWereCorrect();
            Assert.AreNotEqual(0, commandObserver.NumberOfEventsRecieved, "No events were recieved");
            Assert.AreEqual(executableState, commandObserver.ValueOfCanExecuteOnLatestEvent, "The command is executable");
        }
    }

    public class CommandObserver
    {
        public CommandObserver()
        {
            ValueOfCanExecuteOnLatestEvent = false;
            NumberOfEventsRecieved = 0;
            WasTheSendersCorrect = new List<bool>();
        }
        public bool ValueOfCanExecuteOnLatestEvent { get; set; }
        public int NumberOfEventsRecieved { get; set; }
        public List<bool> WasTheSendersCorrect { get; set; }

        public void ClearCommandHistory()
        {
            ValueOfCanExecuteOnLatestEvent = false;
            NumberOfEventsRecieved = 0;
            WasTheSendersCorrect.Clear();
        }
    }
}