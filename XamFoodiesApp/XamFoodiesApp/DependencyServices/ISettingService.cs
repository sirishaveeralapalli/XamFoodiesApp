
using System;
namespace XamFoodiesApp.DependencyServices
{
    public interface ISettingService
    {
        T GetValueOrDefault<T>(string key, T defaultValue = default(T));
        bool AddOrUpdateValue(string key, Object value);
        void Save();
        //void Reset();
        void CloseApp();
        void ChangeOrientation();
        void ChangeLandscapeOrientation();
        void UserOrientation();
        string GetVersionNumber();
        void ClearURLCache();
    }
}
