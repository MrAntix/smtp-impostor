using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Data.Xml.Dom;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace SMTP.Impostor.Worker
{
    public class UWPHelper
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder packageFullName);
        const long ERROR_NO_PACKAGE = 15700L;

        public static bool IsUwp()
        {
            if (IsWindows7OrLower()) return false;

            var length = 0;
            var result = GetCurrentPackageFullName(ref length, new StringBuilder(1024));

            return result != ERROR_NO_PACKAGE;
        }

        public static bool IsWindows7OrLower()
        {
            int versionMajor = Environment.OSVersion.Version.Major;
            int versionMinor = Environment.OSVersion.Version.Minor;
            var version = versionMajor + (double)versionMinor / 10;
            return version <= 6.1;
        }

        public static bool PrimaryTileSupport = ApiInformation.IsTypePresent("Windows.UI.StartScreen.StartScreenManager");
        public static AppListEntry ListEntry;

        public static async Task<bool> TryAddPrimaryTile()
        {
            if (!IsUwp() && !PrimaryTileSupport) return false;

            ListEntry = (await Package.Current.GetAppListEntriesAsync())[0];

            if (!StartScreenManager.GetDefault().SupportsAppListEntry(ListEntry)) return false;

            return await StartScreenManager.GetDefault().RequestAddAppListEntryAsync(ListEntry);
        }

        const string START_NOTIFICATION_ID = "SMTP_START_ID";
        const string OPEN_UI_ACTION = "OPEN_UI_ACTION";


        public static void SendStartupMessage(
            ToastNotifier notifier,
            ISMTPImpostorWorkerSettings settings)
        {
            var content = new ToastContent
            {
                Actions = new ToastActionsCustom
                {                    
                    Buttons = {
                        new ToastButtonDismiss(),
                        new ToastButton("Open UI", OPEN_UI_ACTION)
                    }
                },
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children = {
                            new AdaptiveText
                            {
                                Text = "Worker is running"
                            },
                            new AdaptiveText
                            {
                                Text = settings.StartupMessageLink
                            }
                        }
                    }
                }
            };

            var doc = new XmlDocument();
            doc.LoadXml(content.GetContent());

            var notification = new ToastNotification(doc);
            notification.Tag = START_NOTIFICATION_ID;
            //notification.ExpirationTime = DateTimeOffset.Now.AddMinutes(5);

            notification.Activated += async (_, o) =>
             {
                 var args = o as ToastActivatedEventArgs;
                 if (args.Arguments == OPEN_UI_ACTION)
                     await LaunchLink(settings.StartupMessageLink);
             };

            notifier.Show(notification);
        }

        public static async Task LaunchLink(string link)
        {
            await Launcher.LaunchUriAsync(new Uri(link));
        }
    }
}
