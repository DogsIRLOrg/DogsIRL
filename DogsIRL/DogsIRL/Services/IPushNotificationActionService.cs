using System;
using DogsIRL.Models;
using DogsIRL.Services;

namespace DogsIRL.Services
{
    public interface IPushNotificationActionService : INotificationActionService
    {
        event EventHandler<PushAction> ActionTriggered;
    }
}
