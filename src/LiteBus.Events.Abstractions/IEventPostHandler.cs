﻿using LiteBus.Messaging.Abstractions;

namespace LiteBus.Events.Abstractions;

public interface IEventPostHandlerBase : IEventConstruct
{
}

/// <summary>
///     Represents an action that is executed on each event post-handle phase
/// </summary>
public interface IEventPostHandler : IEventPostHandlerBase, IMessagePostHandler<IEvent>
{
}

/// <summary>
///     Represents an action that is executed on <typeparamref cref="TEvent" /> post-handle phase
/// </summary>
public interface IEventPostHandler<in TEvent> : IEventPostHandlerBase, IMessagePostHandler<TEvent>
    where TEvent : IEvent
{
}