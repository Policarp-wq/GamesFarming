using GamesFarming.MVVM.Base;
using System;

namespace GamesFarming.MVVM.Commands
{
    internal class ParamCommand : CommandBase
    {
        public readonly Action<object> Execution;
        public readonly Func<bool> CanExecuteAction;
        public ParamCommand(Action<object> execution, Func<bool> canExecute = null)
        {
            Execution = execution;
            CanExecuteAction = canExecute;
        }

        public override void Execute(object parameter)
        {
            Execution?.Invoke(parameter);
        }
        public override bool CanExecute(object parameter)
        {
            return CanExecuteAction is null ? base.CanExecute(parameter) : CanExecuteAction.Invoke();
        }
    }
}
