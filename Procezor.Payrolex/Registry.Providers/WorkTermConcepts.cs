using System;
using System.Collections.Generic;
using System.Linq;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Service.Errors;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Providers;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using MaybeMonad;
using ResultMonad;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // ContractTerm			CONTRACT_TERM
    class ContractWorkTermConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_CONTRACT_WORK_TERM;
        public ContractWorkTermConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new ContractWorkTermConSpec(this.Code.Value);
        }
    }

    class ContractWorkTermConSpec : PayrolexConceptSpec
    {
        public ContractWorkTermConSpec(Int32 code) : base(code)
        {
            Path = new List<ArticleCode>();

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();
            return ter.Select((t) => (new ContractWorkTermTarget(month, t.Contract, pos, var, article, this.Code, WorkContractTerms.WORKTERM_EMPLOYMENT_1, null, null)));
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<ContractWorkTermTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            ContractWorkTermTarget evalTarget = resTarget.Value;

            Byte termDayFrom = OperationsPeriod.DateFromInPeriod(period, evalTarget.DateFrom);
            Byte termDayStop = OperationsPeriod.DateStopInPeriod(period, evalTarget.DateStop);

            ITermResult resultsValues = new ContractWorkTermResult(target, spec, evalTarget.TermType, termDayFrom, termDayStop);

            return BuildOkResults(resultsValues);
        }
    }


    // PositionTerm			POSITION_TERM
    class PositionWorkTermConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_TERM;
        public PositionWorkTermConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new PositionWorkTermConSpec(this.Code.Value);
        }
    }

    class PositionWorkTermConSpec : PayrolexConceptSpec
    {
        public PositionWorkTermConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM,
            });

            ResultDelegate = ConceptEval;
        }
        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var ter = posTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract) && x.Position.Equals(t.Position)))) == false).ToArray();
            return ter.Select((t) => (new PositionWorkTermTarget(month, t.Contract, t.Position, var, article, this.Code, "position unknown", null, null))); 
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<PositionWorkTermTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            PositionWorkTermTarget evalTarget = resTarget.Value;

            var resContract = GetContractResult<ContractWorkTermResult>(target, period, results, 
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM));

            if (resContract.IsFailure)
            {
                return BuildFailResults(resContract.Error);
            }

            var evalContract = resContract.Value;

            Byte termDayFrom = OperationsPeriod.DateFromInPeriod(period, evalTarget.DateFrom);
            if (termDayFrom < evalContract.TermDayFrom)
            {
                termDayFrom = evalContract.TermDayFrom;
            }
            Byte termDayStop = OperationsPeriod.DateStopInPeriod(period, evalTarget.DateStop);
            if (termDayStop > evalContract.TermDayStop)
            {
                termDayStop = evalContract.TermDayStop;
            }
            ITermResult resultsValues = new PositionWorkTermResult(target, spec, evalTarget.TermName, termDayFrom, termDayStop);

            return BuildOkResults(resultsValues);
        }
    }
}
