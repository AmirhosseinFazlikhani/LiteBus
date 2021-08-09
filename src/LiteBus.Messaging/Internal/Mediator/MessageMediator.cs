using System;
using LiteBus.Messaging.Abstractions;

namespace LiteBus.Messaging.Internal.Mediator
{
    internal class MessageMediator : IMessageMediator
    {
        private readonly IMessageRegistry _messageRegistry;
        private readonly IServiceProvider _serviceProvider;

        public MessageMediator(IMessageRegistry messageRegistry,
                               IServiceProvider serviceProvider)
        {
            _messageRegistry = messageRegistry;
            _serviceProvider = serviceProvider;
        }

        public TMessageResult Mediate<TMessage, TMessageResult>(TMessage message,
                                                                IMessageResolveStrategy messageResolveStrategy,
                                                                IMessageMediationStrategy<TMessage, TMessageResult> messageMediationStrategy)
        {
            var descriptor = messageResolveStrategy.Find(message.GetType(), _messageRegistry);

            var context = new MessageContext(descriptor, _serviceProvider);

            return messageMediationStrategy.Mediate(message, context);
        }
    }
}