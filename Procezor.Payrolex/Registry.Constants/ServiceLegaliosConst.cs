using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HraveMzdy.Procezor.Payrolex.Registry.Constants
{
    public enum WorkScheduleType : UInt16
    {
        SCHEDULE_NORMALY_WEEK = 0,
        SCHEDULE_SPECIAL_WEEK = 1,
        SCHEDULE_SPECIAL_TURN = 2,
        SCHEDULE_NONEDAY_WORK = 9,
    };
}
