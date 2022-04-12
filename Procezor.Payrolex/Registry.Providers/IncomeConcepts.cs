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

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // IncomeGross			INCOME_GROSS
    class IncomeGrossConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_INCOME_GROSS;
        public IncomeGrossConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new IncomeGrossConSpec(this.Code.Value);
        }
    }

    class IncomeGrossConSpec : PayrolexConceptSpec
    {
        public IncomeGrossConSpec(Int32 code) : base(code)
        {
            Path = new List<ArticleCode>();

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var con = ContractCode.Zero;
            var pos = PositionCode.Zero;

            if (targets.Count() != 0)
            {
                return Array.Empty<ITermTarget>();
            }
            return new ITermTarget[] {
                new IncomeGrossTarget(month, con, pos, var, article, this.Code, 0) 
            };
        }
        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<IncomeGrossTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            IncomeGrossTarget evalTarget = resTarget.Value;

            var incomeList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal resValue = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            ITermResult resultsValues = new IncomeGrossResult(target, spec, RoundToInt(resValue), 0);

            return BuildOkResults(resultsValues);
        }
    }


    // IncomeNetto			INCOME_NETTO
    class IncomeNettoConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_INCOME_NETTO;
        public IncomeNettoConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new IncomeNettoConSpec(this.Code.Value);
        }
    }

    class IncomeNettoConSpec : PayrolexConceptSpec
    {
        public IncomeNettoConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYEE,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYEE,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_PAYM_ADVANCES,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_PAYM_WITHHOLD,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_BONUS_CHILD,
                (Int32)PayrolexArticleConst.ARTICLE_INCOME_GROSS,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var con = ContractCode.Zero;
            var pos = PositionCode.Zero;

            if (targets.Count() != 0)
            {
                return Array.Empty<ITermTarget>();
            }
            return new ITermTarget[] {
                new IncomeNettoTarget(month, con, pos, var, article, this.Code, 0)
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<IncomeNettoTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            IncomeNettoTarget evalTarget = resTarget.Value;

            var resIncGross = GetResult<IncomeGrossResult>(target, period, results,
               ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_INCOME_GROSS));

            if (resIncGross.IsFailure)
            {
                return BuildFailResults(resIncGross.Error);
            }

            var evalIncGross = resIncGross.Value;

            decimal resGross = evalIncGross.ResultValue;

            var healthList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYEE))
               .Select((x) => (x as HealthPaymEmployeeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthSum = healthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 paymentHealth  = RoundToInt(healthSum);

            var socialList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYEE))
               .Select((x) => (x as SocialPaymEmployeeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialSum = socialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 paymentSocial = RoundToInt(socialSum);

            var advancesList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_PAYM_ADVANCES))
               .Select((x) => (x as TaxingPaymAdvancesResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal advancesSum = advancesList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 paymentAdvances = RoundToInt(advancesSum);

            var withholdList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_PAYM_WITHHOLD))
               .Select((x) => (x as TaxingPaymWithholdResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal withholdSum = withholdList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 paymentWithhold = RoundToInt(withholdSum);

            var incomeList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal resNetto = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 evalBonusChild = 0;
            var resBonusChild = GetResult<TaxingBonusChildResult>(target, period, results,
                ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_BONUS_CHILD));

            if (resBonusChild.IsSuccess)
            {
                evalBonusChild = resBonusChild.Value.ResultValue;
            }

            decimal resValue = decimal.Add(decimal.Subtract(decimal.Add(resGross, resNetto), 
                decimal.Add(decimal.Add(paymentHealth, paymentSocial), 
                decimal.Add(paymentAdvances, paymentWithhold))), evalBonusChild);

            ITermResult resultsValues = new IncomeNettoResult(target, spec, RoundToInt(resValue), 0);

            return BuildOkResults(resultsValues);
        }
    }

    // EmployerCosts			EMPLOYER_COSTS
    class EmployerCostsConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_EMPLOYER_COSTS;
        public EmployerCostsConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new EmployerCostsConSpec(this.Code.Value);
        }
    }

    class EmployerCostsConSpec : PayrolexConceptSpec
    {
        public EmployerCostsConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYER,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYER,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var con = ContractCode.Zero;
            var pos = PositionCode.Zero;

            if (targets.Count() != 0)
            {
                return Array.Empty<ITermTarget>();
            }
            return new ITermTarget[] {
                new EmployerCostsTarget(month, con, pos, var, article, this.Code, 0) 
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<EmployerCostsTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            EmployerCostsTarget evalTarget = resTarget.Value;

            var incomeList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal resValue = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            var costHealthList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYER))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal costHealthValue = costHealthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            resValue = decimal.Add(resValue, costHealthValue);

            var costSocialList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYER))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal costSocialValue = costSocialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            resValue = decimal.Add(resValue, costSocialValue);

            ITermResult resultsValues = new EmployerCostsResult(target, spec, RoundToInt(resValue), 0);

            return BuildOkResults(resultsValues);
        }
    }

}
