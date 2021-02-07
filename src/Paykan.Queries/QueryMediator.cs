﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Paykan.Messaging.Abstractions.Extensions;
using Paykan.Queries.Abstractions;
using Paykan.Registry.Abstractions;

namespace Paykan.Queries
{
    internal class QueryMediator : IQueryMediator
    {
        private readonly IMessageRegistry _messageRegistry;
        private readonly IServiceProvider _serviceProvider;

        public QueryMediator(IServiceProvider serviceProvider,
                             IMessageRegistry messageRegistry)
        {
            _serviceProvider = serviceProvider;
            _messageRegistry = messageRegistry;
        }

        public Task<TQueryResult> QueryAsync<TQuery, TQueryResult>(TQuery query,
                                                                   CancellationToken cancellationToken = default)
            where TQuery : IQuery<TQueryResult>
        {
            var queryType = typeof(TQuery);

            var descriptor = _messageRegistry.GetDescriptor<TQuery>();

            if (descriptor.HandlerTypes.Count > 1) throw new MultipleQueryHandlerFoundException(queryType);

            var handler = _serviceProvider
                .GetHandler<TQuery, Task<TQueryResult>>(descriptor.HandlerTypes.First());

            return handler.HandleAsync(query, cancellationToken);
        }

        public IAsyncEnumerable<TQueryResult> StreamQueryAsync<TQuery, TQueryResult>(TQuery query,
            CancellationToken cancellationToken = default) where TQuery : IStreamQuery<TQueryResult>
        {
            var queryType = typeof(TQuery);

            var descriptor = _messageRegistry.GetDescriptor<TQuery>();

            if (descriptor.HandlerTypes.Count > 1) throw new MultipleQueryHandlerFoundException(queryType);

            var handler = _serviceProvider
                .GetHandler<TQuery, IAsyncEnumerable<TQueryResult>>(descriptor.HandlerTypes.First());

            return handler.HandleAsync(query, cancellationToken);
        }

        public Task<TQueryResult> QueryAsync<TQueryResult>(IQuery<TQueryResult> query,
                                                           CancellationToken cancellationToken = default)
        {
            var queryType = query.GetType();

            var descriptor = _messageRegistry.GetDescriptor(queryType);

            if (descriptor.HandlerTypes.Count > 1) throw new MultipleQueryHandlerFoundException(queryType);

            return _serviceProvider
                   .GetService(descriptor.HandlerTypes.First())
                   .HandleAsync<Task<TQueryResult>>(query, cancellationToken);
        }

        public IAsyncEnumerable<TQueryResult> StreamQueryAsync<TQueryResult>(IStreamQuery<TQueryResult> query,
                                                                             CancellationToken cancellationToken =
                                                                                 default)
        {
            var queryType = query.GetType();

            var descriptor = _messageRegistry.GetDescriptor(queryType);

            if (descriptor.HandlerTypes.Count > 1) throw new MultipleQueryHandlerFoundException(queryType);

            return _serviceProvider
                   .GetService(descriptor.HandlerTypes.First())
                   .HandleAsync<IAsyncEnumerable<TQueryResult>>(query, cancellationToken);
        }
    }
}