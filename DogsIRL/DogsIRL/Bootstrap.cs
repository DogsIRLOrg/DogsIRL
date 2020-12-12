using DogsIRL.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL
{
    public static class Bootstrap
    {
        public static void Begin(Func<IDeviceInstallationService> deviceInstallationService)
        {
            ServiceContainer.Register(deviceInstallationService);

            ServiceContainer.Register<IPushNotificationActionService>(()
                => new PushNotificationActionService());

            ServiceContainer.Register<INotificationRegistrationService>(()
                => new NotificationRegistrationService(
                    App.ApiUrl,
                    App.Token));
        }
    }
}
