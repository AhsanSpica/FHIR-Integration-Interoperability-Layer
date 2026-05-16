using Interface.Misc.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GlobalHelpers;
using Microsoft.Extensions.Options;

namespace Interface.Misc.Implementation
{
    public class QueueSubscriberService : IQueueSubscriberService
    {
        private readonly ILogger<QueueSubscriberService> _logger;
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private List<ServiceBusReceivedMessage> _messages;
        private readonly IOptions<AzureServiceBus> _optionsServiceBus;

        public QueueSubscriberService(
            ILogger<QueueSubscriberService> logger,
            IOptions<AzureServiceBus> optionsServiceBus
            )
        {
            _logger = logger;
            _optionsServiceBus = optionsServiceBus;

            string connectionString = _optionsServiceBus.Value.ConnectionString; 
            string queueName = _optionsServiceBus.Value.QueueName;

            if (!string.IsNullOrEmpty(connectionString))
            {
                _client = new ServiceBusClient(connectionString);
                _processor = _client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

                _processor.ProcessMessageAsync += MessageHandler;
                _processor.ProcessErrorAsync += ErrorHandler;
                _messages = new List<ServiceBusReceivedMessage>();
            }
        }
        public bool ServiceBusIsActive()
        {
           var truth = _processor != null ? true : false;
            return truth;
        }

        public async Task StartProcessingAsync()
        {
            await _processor.StartProcessingAsync();
        }

        public async Task StopProcessingAsync()
        {
            await _processor.StopProcessingAsync();
        }

        private Task MessageHandler(ProcessMessageEventArgs args)
        {
            _messages.Add(args.Message);
            return args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            _logger.LogError(args.Exception, "Error processing message");
            return Task.CompletedTask;
        }

        public List<ServiceBusReceivedMessage> GetMessages()
        {
            return new List<ServiceBusReceivedMessage>(_messages);
        }
    }
}
