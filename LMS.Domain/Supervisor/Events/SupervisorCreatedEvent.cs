﻿using LMS.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Supervisor.Events
{
    public record SupervisorCreatedEvent(Guid SupervisorId) : IDomainEvent;
}
