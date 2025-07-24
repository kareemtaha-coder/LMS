using LMS.Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Abstractions.Messaging
{
    public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : Result
    {
    }

    // IQueryHandler for queries that return a value
    public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : Result
    {
    }
}
