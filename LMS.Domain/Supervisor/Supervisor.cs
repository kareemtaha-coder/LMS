using LMS.Domain.Abstractions;
using LMS.Domain.Supervisor.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Supervisor
{
    public class Supervisor : Entity
    {
        // يجب أن تكون الخصائص للقراءة فقط من الخارج (private set)
        // لفرض التعديل عليها من خلال دوال (methods) داخل الكلاس
        public Guid UserId { get; private set; }

        // Constructor خاص لمنع الإنشاء المباشر
        // نفرض على المطور استخدام الـ Factory Method وهو Create
        private Supervisor(Guid id, Guid userId) : base(id)
        {
            UserId = userId;
        }

        // هذه هي الطريقة الصحيحة لإنشاء كائن Supervisor جديد
        // تسمى Factory Method
        public static Supervisor Create(Guid userId)
        {
            // --- هنا نحمي الثوابت (Protecting Invariants) ---
            if (userId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be empty.");
            }

            // إذا كانت كل القواعد سليمة، ننشئ الكائن
            var supervisor = new Supervisor(Guid.NewGuid(), userId);

        // --- يمكننا إطلاق حدث هنا إذا أردنا ---
            supervisor.RaiseDomainEvent(new SupervisorCreatedEvent(supervisor.Id));

            return supervisor;
        }
    }
}
