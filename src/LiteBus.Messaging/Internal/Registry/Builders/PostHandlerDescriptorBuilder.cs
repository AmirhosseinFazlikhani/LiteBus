using System;
using System.Collections.Generic;
using LiteBus.Messaging.Abstractions;
using LiteBus.Messaging.Abstractions.Descriptors;
using LiteBus.Messaging.Internal.Extensions;
using LiteBus.Messaging.Internal.Registry.Abstractions;
using LiteBus.Messaging.Internal.Registry.Descriptors;

namespace LiteBus.Messaging.Internal.Registry.Builders;

public class PostHandlerDescriptorBuilder : IDescriptorBuilder<IPostHandlerDescriptor>
{
    public bool CanBuild(Type type)
    {
        return type.IsAssignableTo(typeof(IMessagePostHandler));
    }

    public IEnumerable<IPostHandlerDescriptor> Build(Type handlerType)
    {
        //TODO: refactor the post handler
        foreach (var postHandlerDescriptor in WithResponse(handlerType))
        {
            yield return postHandlerDescriptor;
        }

        foreach (var postHandlerDescriptor in WithoutResponse(handlerType))
        {
            yield return postHandlerDescriptor;
        }
    }

    public IEnumerable<IPostHandlerDescriptor> WithResponse(Type handlerType)
    {
        var interfaces = handlerType.GetInterfacesEqualTo(typeof(IMessagePostHandler<,>));
        var order = handlerType.GetOrderFromAttribute();

        foreach (var @interface in interfaces)
        {
            var messageType = @interface.GetGenericArguments()[0];
            var messageResultType = @interface.GetGenericArguments()[1];

            yield return new PostHandlerDescriptor(handlerType, messageType, messageResultType, order);
        }
    }

    public IEnumerable<IPostHandlerDescriptor> WithoutResponse(Type handlerType)
    {
        var interfaces = handlerType.GetInterfacesEqualTo(typeof(IMessagePostHandler<>));
        var order = handlerType.GetOrderFromAttribute();

        foreach (var @interface in interfaces)
        {
            var messageType = @interface.GetGenericArguments()[0];

            yield return new PostHandlerDescriptor(handlerType, messageType, null, order);
        }
    }
}