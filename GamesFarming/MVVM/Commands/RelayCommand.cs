using GamesFarming.MVVM.Base;
using System;

namespace GamesFarming.MVVM.Commands
{
    internal class RelayCommand : CommandBase
    {
        public readonly Action Execution;
        public readonly Func<bool> CanExecuteAction;

        public RelayCommand(Action action, Func<bool> canExecute = null)
        {
            Execution = action;
            CanExecuteAction = canExecute;
        }
        public override void Execute(object parameter)
        {
            Execution?.Invoke();
        }

        public override bool CanExecute(object parameter)
        {
            return CanExecuteAction is null ? base.CanExecute(parameter) : CanExecuteAction.Invoke();
        }
    }
}
