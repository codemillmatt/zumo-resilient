using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace VSLiveToDo.Abstractions
{
    public interface IPlatformProvider
    {
        Task RegisterForPushNotifications(MobileServiceClient client);
    }
}
