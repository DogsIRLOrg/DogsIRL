using System;
using System.Collections.Generic;
using System.Text;

namespace DogsIRL.Services
{
    public interface INotificationActionService
    {
        void TriggerAction(string action);
    }
}
