using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Application.Abstractions.Messaging
{
    public interface ICommand : IRequest
    {
    }
    public interface ICommand<TResponse> : IRequest<TResponse>
    {
    }
}
