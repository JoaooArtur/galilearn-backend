﻿using Core.Shared.Results;
using MediatR;

namespace Core.Application.Messaging
{
    /// <summary>
    /// Represents the query interface.
    /// </summary>
    /// <typeparam name="TResponse">The query response type.</typeparam>
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
}
