using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // PositionWorkPlan		POSITION_WORK_PLAN
    public class PositionWorkPlanResult : PayrolexTermResult
    {
        public WorkScheduleType WorkType { get; private set; }
        public Int32[] HoursFullWeeks { get; private set; }
        public Int32[] HoursRealWeeks { get; private set; }
        public Int32[] HoursFullMonth { get; private set; }
        public Int32[] HoursRealMonth { get; private set; }

        public PositionWorkPlanResult(ITermTarget target, IArticleSpec spec, 
            WorkScheduleType workType, Int32[] fullWeeks, Int32[] realWeeks, Int32[] fullMonth, Int32[] realMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            WorkType = workType;
            HoursFullWeeks = fullWeeks.ToArray();
            HoursRealWeeks = realWeeks.ToArray();
            HoursFullMonth = fullMonth.ToArray();
            HoursRealMonth = realMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"{TotalFullWeeks()}/{TotalRealWeeks()} => {TotalFullMonth()}/{TotalRealMonth()}";
        }
        public Int32 TotalFullWeeks()
        {
            return HoursFullWeeks.Aggregate(0, (agr, x) => agr + x);
        }
        public Int32 TotalRealWeeks()
        {
            return HoursRealWeeks.Aggregate(0, (agr, x) => agr + x);
        }
        public Int32 TotalFullMonth()
        {
            return HoursFullMonth.Aggregate(0, (agr, x) => agr + x);
        }
        public Int32 TotalRealMonth()
        {
            return HoursRealMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // PositionTimePlan		POSITION_TIME_PLAN
    public class PositionTimePlanResult : PayrolexTermResult
    {
        public Byte TermDayFrom { get; private set; }
        public Byte TermDayStop { get; private set; }
        public Int32[] HoursRealMonth { get; private set; }
        public Int32[] HoursTermMonth { get; private set; }
        public PositionTimePlanResult(ITermTarget target, IArticleSpec spec, Byte dayTermFrom, Byte dayTermStop, 
            Int32[] realMonth, Int32[] termMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            TermDayFrom = dayTermFrom;
            TermDayStop = dayTermStop;
            HoursRealMonth = realMonth.ToArray();
            HoursTermMonth = termMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"{TermDayFrom} - {TermDayStop} => {TotalRealMonth()}/{TotalTermMonth()}";
        }
        public Int32 TotalRealMonth()
        {
            return HoursRealMonth.Aggregate(0, (agr, x) => agr + x);
        }
        public Int32 TotalTermMonth()
        {
            return HoursTermMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // PositionTimeWork		POSITION_TIME_WORK
    public class PositionTimeWorkResult : PayrolexTermResult
    {
        public Byte TermDayFrom { get; private set; }
        public Byte TermDayStop { get; private set; }
        public Int32[] HoursTermMonth { get; private set; }
        public PositionTimeWorkResult(ITermTarget target, IArticleSpec spec, Byte dayTermFrom, Byte dayTermStop, Int32[] termMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            TermDayFrom = dayTermFrom;
            TermDayStop = dayTermStop;
            HoursTermMonth = termMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"{TermDayFrom} - {TermDayStop} => {TotalTermMonth()}";
        }
        public Int32 TotalTermMonth()
        {
            return HoursTermMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // PositionTimeAbsc		POSITION_TIME_ABSC
    public class PositionTimeAbscResult : PayrolexTermResult
    {
        public Byte TermDayFrom { get; private set; }
        public Byte TermDayStop { get; private set; }
        public Int32[] HoursTermMonth { get; private set; }
        public PositionTimeAbscResult(ITermTarget target, IArticleSpec spec, Byte dayTermFrom, Byte dayTermStop, Int32[] termMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            TermDayFrom = dayTermFrom;
            TermDayStop = dayTermStop;
            HoursTermMonth = termMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"{TermDayFrom} - {TermDayStop} => {TotalTermMonth()}";
        }
        public Int32 TotalTermMonth()
        {
            return HoursTermMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // ContractTimePlan		CONTRACT_TIME_PLAN
    public class ContractTimePlanResult : PayrolexTermResult
    {
        public Int32[] HoursTimeMonth { get; private set; }
        public ContractTimePlanResult(ITermTarget target, IArticleSpec spec, Int32[] timeMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            HoursTimeMonth = timeMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"Total Work Schedule => {TotalTimeMonth()}";
        }
        public Int32 TotalTimeMonth()
        {
            return HoursTimeMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // ContractTimeWork		CONTRACT_TIME_WORK
    public class ContractTimeWorkResult : PayrolexTermResult
    {
        public Int32[] HoursTimeMonth { get; private set; }
        public ContractTimeWorkResult(ITermTarget target, IArticleSpec spec, Int32[] timeMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            HoursTimeMonth = timeMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"Total worked => {TotalTimeMonth()}";
        }
        public Int32 TotalTimeMonth()
        {
            return HoursTimeMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

    // ContractTimeAbsc		CONTRACT_TIME_ABSC
    public class ContractTimeAbscResult : PayrolexTermResult
    {
        public Int32[] HoursTimeMonth { get; private set; }
        public ContractTimeAbscResult(ITermTarget target, IArticleSpec spec, Int32[] timeMonth)
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            HoursTimeMonth = timeMonth.ToArray();
        }
        public override string ResultMessage()
        {
            return $"Total absence => {TotalTimeMonth()}";
        }
        public Int32 TotalTimeMonth()
        {
            return HoursTimeMonth.Aggregate(0, (agr, x) => agr + x);
        }
    }

}
