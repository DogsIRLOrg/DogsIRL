using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogsIRL.Services
{
    public interface INotificationRegistrationService
    {
        Task DeregisterDeviceAsync();
        Task RegisterDeviceAsync(params string[] tags);
        Task RefreshRegistrationAsync();
    }
}
