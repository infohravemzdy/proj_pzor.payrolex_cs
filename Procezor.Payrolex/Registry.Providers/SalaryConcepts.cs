using System;
using System.Collections.Generic;
using System.Linq;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Procezor.Service.Errors;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Providers;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using MaybeMonad;
using ResultMonad;
using HraveMzdy.Legalios.Service.Types;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // PaymentBasis			PAYMENT_BASIS
    class PaymentBasisConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_PAYMENT_BASIS;
        public PaymentBasisConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PaymentBasisConSpec(this.Code.Value);
        }
    }

    class PaymentBasisConSpec : PayrolexConceptSpec
    {
        public PaymentBasisConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN,
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_WORK,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PaymentBasisTarget(month, t.Contract, t.Position, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSalary = GetSalaryPropsResult(ruleset, target, period);
            if (resPrSalary.IsFailure)
            {
                return BuildFailResults(resPrSalary.Error);
            }
            IPropsSalary salaryRules = resPrSalary.Value;

            var resTarget = GetTypedTarget<PaymentBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PaymentBasisTarget evalTarget = resTarget.Value;

            var resWorkPlan = GetPositionResult<PositionWorkPlanResult>(target, period, results,
               target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN));

            var resTimePlan = GetPositionResult<PositionTimePlanResult>(target, period, results,
               target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN));

            var resTimeWork = GetPositionResult<PositionTimeWorkResult>(target, period, results,
               target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_WORK));

            var resCompound = GetFailedOrOk(resWorkPlan.ErrOrOk(), resTimePlan.ErrOrOk(), resTimeWork.ErrOrOk());
            if (resCompound.IsFailure)
            {
                return BuildFailResults(resCompound.Error);
            }

            var evalWorkPlan = resWorkPlan.Value;

            var evalTimePlan = resTimePlan.Value;

            var evalTimeWork = resTimeWork.Value;

            Int32 shiftLiable = OperationsPeriod.TotalWeeksHours(evalWorkPlan.HoursFullWeeks);
            Int32 shiftWorked = OperationsPeriod.TotalWeeksHours(evalWorkPlan.HoursRealWeeks);
            Int32 hoursLiable = OperationsPeriod.TotalMonthHours(evalTimePlan.HoursRealMonth);
            Int32 hoursWorked = OperationsPeriod.TotalMonthHours(evalTimeWork.HoursTermMonth);

            Decimal resValue = salaryRules.PaymentRoundUpWithMonthlyAndFullWeekAndFullAndWorkHours(evalTarget.TargetBasis, 
                shiftLiable, shiftWorked,
                hoursLiable, hoursWorked);
            ITermResult resultsValues = new PaymentBasisResult(target, spec,
                OperationsRound.RoundUp(resValue), evalTarget.TargetBasis);

            return BuildOkResults(resultsValues);
        }
    }


    // PaymentFixed			PAYMENT_FIXED
    class PaymentFixedConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_PAYMENT_FIXED;
        public PaymentFixedConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PaymentFixedConSpec(this.Code.Value);
        }
    }

    class PaymentFixedConSpec : PayrolexConceptSpec
    {
        public PaymentFixedConSpec(Int32 code) : base(code)
        {
            Path = new List<ArticleCode>();

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PaymentFixedTarget(month, t.Contract, t.Position, var, article, this.Code, 0))); 
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSalary = GetSalaryPropsResult(ruleset, target, period);
            if (resPrSalary.IsFailure)
            {
                return BuildFailResults(resPrSalary.Error);
            }
            IPropsSalary salaryRules = resPrSalary.Value;

            var resTarget = GetTypedTarget<PaymentFixedTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PaymentFixedTarget evalTarget = resTarget.Value;

            Decimal resValue = salaryRules.PaymentRoundUpWithAmountFixed(evalTarget.TargetBasis);

            ITermResult resultsValues = new PaymentFixedResult(target, spec,
                OperationsRound.RoundUp(resValue), evalTarget.TargetBasis);

            return BuildOkResults(resultsValues);
        }
    }
}
