using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace VSLiveToDo.Services
{
    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string start_sync_settings = "start-sync-settings";

        public static bool HasSyncStarted
        {
            get
            {
                return AppSettings.GetValueOrDefault(start_sync_settings, false);
            }
            set
            {
                AppSettings.AddOrUpdateValue(start_sync_settings, value);
            }
        }
    }
}
