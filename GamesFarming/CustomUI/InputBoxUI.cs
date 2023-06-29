using GamesFarming.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesFarming.CustomUI
{
    public static class InputBoxUI
    {
        public static void ShowInputBox(string inputSign, Action<string> actionWithInput)
        {
            var inputBox = new InputBox() {Width=400, Height=400 };
            var inputBoxVm = new InputBoxViewModel(inputSign, (s) => { inputBox.Close(); actionWithInput?.Invoke(s); });
            inputBox.DataContext = inputBoxVm;
            inputBox.Show();
        }
    }
}
