
using System;
namespace XamFoodiesApp.DependencyServices
{
    public interface IProgressService
    {
        void ShowProgress(string message);
        void HideProgress();
        void ShowToast(string message);
        void ShowAlertWithTextField(string message, string HintText, Action<bool, string> Selection);
    }
}
