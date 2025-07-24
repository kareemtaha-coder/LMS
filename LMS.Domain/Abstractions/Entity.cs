using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Abstractions
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> domainEvents = new List<IDomainEvent>();
        protected Entity(Guid id)
        {
            Id = id;
        }
        protected Entity()
        {

        }
        public Guid Id { get; init; }

        public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        {
            return domainEvents.ToList();
        }
        public void ClearDomainEvents()
        {
            domainEvents.Clear();
        }
        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            domainEvents.Add(domainEvent);
        }
    }
}
