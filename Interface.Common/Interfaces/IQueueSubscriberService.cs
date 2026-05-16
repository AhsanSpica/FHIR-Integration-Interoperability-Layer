using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Misc.Interfaces
{
    public interface IQueueSubscriberService
    {
        Task StartProcessingAsync();
        Task StopProcessingAsync();
        List<ServiceBusReceivedMessage> GetMessages();
        bool ServiceBusIsActive();
    }
}
