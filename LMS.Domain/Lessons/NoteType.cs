using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Lessons
{
    public enum NoteType
    {
        Normal,    // The default text
        Important, // ملاحظة هامة
        Warning,   // تحذير
        Tip        // نصيحة
    }
}
