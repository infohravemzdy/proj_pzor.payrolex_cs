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
    // PositionWorkPlan			POSITION_WORK_PLAN
    class PositionWorkPlanConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_PLAN;
        public PositionWorkPlanConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PositionWorkPlanConSpec(this.Code.Value);
        }
    }

    class PositionWorkPlanConSpec : PayrolexConceptSpec
    {
        public PositionWorkPlanConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            Int32 RulesetWDays = 5;
            Int32 RulesetShift = 8;
            IPropsSalary salaryRules = GetSalaryProps(ruleset, period);
            if (salaryRules != null)
            {
                RulesetWDays = ruleset.SalaryProps.WorkingShiftWeek;
                RulesetShift = ruleset.SalaryProps.WorkingShiftTime;
            }

            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PositionWorkPlanTarget(month, t.Contract, t.Position, var, article, this.Code,
                    WorkScheduleType.SCHEDULE_NORMALY_WEEK, RulesetWDays, RulesetShift, RulesetShift)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<PositionWorkPlanTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PositionWorkPlanTarget evalTarget = resTarget.Value;

            Int32[] hoursFullWeeks = Array.Empty<Int32>();
            Int32[] hoursRealWeeks = Array.Empty<Int32>();
            Int32[] hoursFullMonth = Array.Empty<Int32>();
            Int32[] hoursRealMonth = Array.Empty<Int32>();

            if (evalTarget.WorkType == WorkScheduleType.SCHEDULE_NORMALY_WEEK)
            {
                Int32 weekShiftPlaned = evalTarget.WeekShiftPlaned;
                Int32 weekShiftLiable = evalTarget.WeekShiftLiable * evalTarget.WeekShiftPlaned;
                hoursFullWeeks = OperationsPeriod.TimesheetWeekSchedule(period, weekShiftLiable, (Byte)weekShiftPlaned);
                hoursRealWeeks = OperationsPeriod.TimesheetWeekSchedule(period, weekShiftLiable, (Byte)weekShiftPlaned);
                hoursFullMonth = OperationsPeriod.TimesheetFullSchedule(period, hoursFullWeeks);
                hoursRealMonth = OperationsPeriod.TimesheetFullSchedule(period, hoursRealWeeks);
            }
            else
            {
                var error = NoImplementationError.CreateError(period, target, $"WorkScheduleType.{evalTarget.WorkType}");
                return BuildFailResults(error);
            }
            ITermResult resultsValues = new PositionWorkPlanResult(target, spec,
                evalTarget.WorkType, hoursFullWeeks, hoursRealWeeks, hoursFullMonth, hoursRealMonth);

            return BuildOkResults(resultsValues);
        }
    }


    // PositionTimePlan			POSITION_TIME_PLAN
    class PositionTimePlanConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_POSITION_TIME_PLAN;
        public PositionTimePlanConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PositionTimePlanConSpec(this.Code.Value);
        }
    }

    class PositionTimePlanConSpec : PayrolexConceptSpec
    {
        public PositionTimePlanConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN,
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM,
            });

            ResultDelegate = ConceptEval;
        }
        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PositionTimePlanTarget(month, t.Contract, t.Position, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<PositionTimePlanTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PositionTimePlanTarget evalTarget = resTarget.Value;

            var resWorkPlan = GetPositionResult<PositionWorkPlanResult>(target, period, results,
                target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN));

            var resWorkTerm = GetPositionResult<PositionWorkTermResult>(target, period, results,
                target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM));

            var resCompound = GetFailedOrOk(resWorkPlan.ErrOrOk(), resWorkTerm.ErrOrOk());
            if (resCompound.IsFailure) {
                return BuildFailResults(resCompound.Error);
            }

            var evalWorkPlan = resWorkPlan.Value;

            var evalWorkTerm = resWorkTerm.Value;

            Int32[] hoursRealMonth = evalWorkPlan.HoursRealMonth.ToArray();
            Int32[] hoursTermMonth = OperationsPeriod.TimesheetWorkSchedule(hoursRealMonth, 
                evalWorkTerm.TermDayFrom, evalWorkTerm.TermDayStop);

            ITermResult resultsValues = new PositionTimePlanResult(target, spec,
                evalWorkTerm.TermDayFrom, evalWorkTerm.TermDayStop, hoursRealMonth, hoursTermMonth);

            return BuildOkResults(resultsValues);
        }
    }


    // PositionTimeWork			POSITION_TIME_WORK
    class PositionTimeWorkConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_POSITION_TIME_WORK;
        public PositionTimeWorkConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PositionTimeWorkConSpec(this.Code.Value);
        }
    }

    class PositionTimeWorkConSpec : PayrolexConceptSpec
    {
        public PositionTimeWorkConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN,
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_ABSC,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PositionTimeWorkTarget(month, t.Contract, t.Position, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<PositionTimeWorkTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PositionTimeWorkTarget evalTarget = resTarget.Value;

            var resTimePlan = GetPositionResult<PositionTimePlanResult>(target, period, results,
                target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN));

            var resTimeAbsc = GetPositionResult<PositionTimeAbscResult>(target, period, results,
                target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_ABSC));

            var resCompound = GetFailedOrOk(resTimePlan.ErrOrOk(), resTimeAbsc.ErrOrOk());
            if (resCompound.IsFailure)
            {
                return BuildFailResults(resCompound.Error);
            }

            var evalTimePlan = resTimePlan.Value;

            var evalTimeAbsc = resTimeAbsc.Value;

            Int32[] hoursWorkMonth = OperationsPeriod.ScheduleBaseSubtract(
                evalTimePlan.HoursTermMonth, evalTimeAbsc.HoursTermMonth, 1, 31);

            ITermResult resultsValues = new PositionTimeWorkResult(target, spec,
                evalTimePlan.TermDayFrom, evalTimePlan.TermDayStop, hoursWorkMonth);

            return BuildOkResults(resultsValues);
        }
    }


    // PositionTimeAbsc			POSITION_TIME_ABSC
    class PositionTimeAbscConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_POSITION_TIME_ABSC;
        public PositionTimeAbscConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PositionTimeAbscConSpec(this.Code.Value);
        }
    }

    class PositionTimeAbscConSpec : PayrolexConceptSpec
    {
        public PositionTimeAbscConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN,
                (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_TIME_ABSC,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();

            return ter.Select((t) => (new PositionTimeAbscTarget(month, t.Contract, t.Position, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<PositionTimeAbscTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PositionTimeAbscTarget evalTarget = resTarget.Value;

            var resTimePlan = GetPositionResult<PositionTimePlanResult>(target, period, results,
                target.Contract, target.Position, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN));

            if (resTimePlan.IsFailure)
            {
                return BuildFailResults(resTimePlan.Error);
            }

            var evalTimePlan = resTimePlan.Value;

            Int32[] hoursAbscMonth = OperationsPeriod.EmptyMonthSchedule();

            ITermResult resultsValues = new PositionTimeAbscResult(target, spec,
                evalTimePlan.TermDayFrom, evalTimePlan.TermDayStop, hoursAbscMonth);

            return BuildOkResults(resultsValues);
        }
    }


    // ContractTimePlan			CONTRACT_TIME_PLAN
    class ContractTimePlanConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_CONTRACT_TIME_PLAN;
        public ContractTimePlanConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new ContractTimePlanConSpec(this.Code.Value);
        }
    }

    class ContractTimePlanConSpec : PayrolexConceptSpec
    {
        public ContractTimePlanConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new ContractTimePlanTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<ContractTimePlanTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            ContractTimePlanTarget evalTarget = resTarget.Value;

            Int32[] zerMonth = OperationsPeriod.EmptyMonthSchedule();

            ContractCode posContract = evalTarget.Contract;
            Int32 posArticle = (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_PLAN;
            var positionList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == posArticle && v.Contract.Equals(posContract)))
                .Select((tr) => (tr as PositionTimePlanResult))
                .Where((pr) => (pr!=null)).ToArray();

            Int32[] resValue = positionList.Aggregate(zerMonth, (agr, item) =>
            {
                var agrResult = OperationsPeriod.TimesheetWorkContract(agr, item.HoursTermMonth, item.TermDayFrom, item.TermDayStop);
                return agrResult;
            });
            ITermResult resultsValues = new ContractTimePlanResult(target, spec, resValue);

            return BuildOkResults(resultsValues);
        }
    }


    // ContractTimeWork			CONTRACT_TIME_WORK
    class ContractTimeWorkConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_CONTRACT_TIME_WORK;
        public ContractTimeWorkConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new ContractTimeWorkConSpec(this.Code.Value);
        }
    }

    class ContractTimeWorkConSpec : PayrolexConceptSpec
    {
        public ContractTimeWorkConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_WORK,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new ContractTimeWorkTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<ContractTimeWorkTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            ContractTimeWorkTarget evalTarget = resTarget.Value;

            Int32[] zerMonth = OperationsPeriod.EmptyMonthSchedule();

            ContractCode posContract = evalTarget.Contract;
            Int32 posArticle = (Int32)PayrolexArticleConst.ARTICLE_POSITION_TIME_WORK;
            var positionList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == posArticle && v.Contract.Equals(posContract)))
                .Select((tr) => (tr as PositionTimeWorkResult))
                .Where((pr) => (pr != null)).ToArray();

            Int32[] resValue = positionList.Aggregate(zerMonth, (agr, item) =>
            {
                var agrResult = OperationsPeriod.TimesheetWorkContract(agr, item.HoursTermMonth, item.TermDayFrom, item.TermDayStop);
                return agrResult;
            });
            ITermResult resultsValues = new ContractTimeWorkResult(target, spec, resValue);

            return BuildOkResults(resultsValues);
        }
    }


    // ContractTimeAbsc			CONTRACT_TIME_ABSC
    class ContractTimeAbscConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_CONTRACT_TIME_ABSC;
        public ContractTimeAbscConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new ContractTimeAbscConSpec(this.Code.Value);
        }
    }

    class ContractTimeAbscConSpec : PayrolexConceptSpec
    {
        public ContractTimeAbscConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_TIME_PLAN,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new ContractTimeAbscTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<ContractTimeAbscTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            ContractTimeAbscTarget evalTarget = resTarget.Value;

            Int32[] absMonth = OperationsPeriod.EmptyMonthSchedule();

            ITermResult resultsValues = new ContractTimeAbscResult(target, spec, absMonth);

            return BuildOkResults(resultsValues);
        }
    }
}
