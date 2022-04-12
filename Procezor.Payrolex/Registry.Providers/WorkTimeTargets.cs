using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // PositionWorkPlan		POSITION_WORK_PLAN
    public class PositionWorkPlanTarget : PayrolexTermTarget
    {
        public WorkScheduleType WorkType { get; private set; }
        public Int32 WeekShiftPlaned { get; set; }
        public Int32 WeekShiftLiable { get; set; }
        public Int32 WeekShiftActual { get; set; }

        public PositionWorkPlanTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            WorkScheduleType workType, Int32 shiftPlaned, Int32 shiftLiable, Int32 shiftActual) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            WorkType = workType;
            WeekShiftPlaned = shiftPlaned;
            WeekShiftLiable = shiftLiable;
            WeekShiftActual = shiftActual;
        }
    }

    // PositionTimePlan		POSITION_TIME_PLAN
    public class PositionTimePlanTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public PositionTimePlanTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // PositionTimeWork		POSITION_TIME_WORK
    public class PositionTimeWorkTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public PositionTimeWorkTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // PositionTimeAbsc		POSITION_TIME_ABSC
    public class PositionTimeAbscTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public PositionTimeAbscTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // ContractTimePlan		CONTRACT_TIME_PLAN
    public class ContractTimePlanTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public ContractTimePlanTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // ContractTimeWork		CONTRACT_TIME_WORK
    public class ContractTimeWorkTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public ContractTimeWorkTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // ContractTimeAbsc		CONTRACT_TIME_ABSC
    public class ContractTimeAbscTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public ContractTimeAbscTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

}
