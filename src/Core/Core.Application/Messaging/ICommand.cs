﻿using Core.Shared.Results;
using MediatR;

namespace Core.Application.Messaging
{
    /// <summary>
    /// Represents the command interface.
    /// </summary>
    public interface ICommand : IRequest<Result> { }

    /// <summary>
    /// Represents the command interface.
    /// </summary>
    /// <typeparam name="TResponse">The command response type.</typeparam>
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
}
