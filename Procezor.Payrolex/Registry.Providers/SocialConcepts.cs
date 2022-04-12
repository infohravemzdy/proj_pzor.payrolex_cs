using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Service.Errors;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Providers;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using ResultMonad;
using MaybeMonad;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // SocialDeclare			SOCIAL_DECLARE
    class SocialDeclareConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_DECLARE;
        public SocialDeclareConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialDeclareConSpec(this.Code.Value);
        }
    }

    class SocialDeclareConSpec : PayrolexConceptSpec
    {
        public SocialDeclareConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialDeclareTarget(month, t.Contract, pos, var, article, this.Code, 1, WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<SocialDeclareTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialDeclareTarget evalTarget = resTarget.Value;

            var resContract = GetContractResult<ContractWorkTermResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM));

            if (resContract.IsFailure)
            {
                return BuildFailResults(resContract.Error);
            }

            var evalContract = resContract.Value;

            var evalContractType = evalTarget.ContractType;

            if (evalContractType == WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT)
            {
                switch (evalContract.TermType)
                {
                    case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                        evalContractType = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_A:
                        evalContractType = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_T:
                        evalContractType = WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK;
                        break;
                    case WorkContractTerms.WORKTERM_PARTNER_STAT:
                        evalContractType = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
                        break;
                }
            }
            ITermResult resultsValues = new SocialDeclareResult(target, spec,
                evalTarget.InterestCode, evalContractType);

            return BuildOkResults(resultsValues);
        }
    }

    // SocialIncome			SOCIAL_INCOME
    class SocialIncomeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_INCOME;
        public SocialIncomeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialIncomeConSpec(this.Code.Value);
        }
    }

    class SocialIncomeConSpec : PayrolexConceptSpec
    {
        public SocialIncomeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE,
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
                new SocialIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSocial = GetSocialPropsResult(ruleset, target, period);
            if (resPrSocial.IsFailure)
            {
                return BuildFailResults(resPrSocial.Error);
            }
            IPropsSocial socialRules = resPrSocial.Value;

            Int32 marginIncomeEmp = socialRules.MarginIncomeEmp;
            Int32 marginIncomeAgr = socialRules.MarginIncomeAgr;

            var resTarget = GetTypedTarget<SocialIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialIncomeTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE))
                .Select((tr) => (tr as SocialDeclareResult)).ToArray();

            var incomeResultInit = Array.Empty<SocialIncomeResult>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var evalSubjectsType = x.ContractType;
                var evalInterestCode = x.InterestCode;

                var contractResult = agr.FirstOrDefault((a) => (a.Contract.Equals(x.Contract)));
                if (contractResult == null)
                {
                    contractResult = new SocialIncomeResult(evalTarget, x.Contract, spec,
                        evalInterestCode, evalSubjectsType, VALUE_ZERO, VALUE_ZERO, BASIS_ZERO);
                    agr = agr.Concat(new SocialIncomeResult[] { contractResult }).ToArray();
                }
                var incomeList = results
                    .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                    .Where((v) => (v.Contract.Equals(x.Contract) && v.Spec.Sums.Contains(evalTarget.Article)))
                    .Select((v) => (v as PayrolexTermResult))
                    .Select((tr) => (tr.ResultValue)).ToArray();

                decimal resValue = incomeList.Aggregate(decimal.Zero,
                    (agr, item) => decimal.Add(agr, item));

                contractResult.AddResultValue(RoundToInt(resValue));
                return agr;
            });

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), SocialIncomeResult.ResultComparator()).ToArray();

            var resultOrdersInit = Array.Empty<SocialIncomeResult>();

            var resultOrdersList = incomeOrdersList.Aggregate(resultOrdersInit,
                (agr, x) => {
                    Int32 sumTermIncome = incomeResultList.Where((c) => (c.InterestCode!=0 && c.IncomeTerm().Equals(x.IncomeTerm())))
                        .Aggregate(0, (sum, c) => (sum + c.ResultValue));
                    Int32 conTermIncome = x.ResultValue;

                    Int16 particyCode = 0;
                    if (x.InterestCode != 0)
                    {
                        if (socialRules.HasParticy(x.IncomeTerm(), sumTermIncome, conTermIncome))
                        {
                            particyCode = 1;
                        }
                    }

                    x.SetParticyCode(particyCode);

                    return agr.Concat(new SocialIncomeResult[] { x }).ToArray();
                });

            return BuildOkResults(resultOrdersList);
        }
    }

    // SocialBase			SOCIAL_BASE
    class SocialBaseConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_BASE;
        public SocialBaseConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialBaseConSpec(this.Code.Value);
        }
    }

    class SocialBaseConSpec : PayrolexConceptSpec
    {
        public SocialBaseConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_INCOME,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialBaseTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<SocialBaseTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialBaseTarget evalTarget = resTarget.Value;

            var resDeclare = GetContractResult<SocialDeclareResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE));
            var resIncomes = GetContractResult<SocialIncomeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_INCOME));

            var resCompound = GetFailedOrOk(resDeclare.ErrOrOk(), resIncomes.ErrOrOk());
            if (resCompound.IsFailure)
            {
                return BuildFailResults(resCompound.Error);
            }

            var evalDeclare = resDeclare.Value;
            var evalIncomes = resIncomes.Value;

            Int32 resGeneralBase = 0;
            if (evalDeclare.InterestCode != 0)
            {
                if (evalIncomes.ParticyCode != 0)
                {
                    resGeneralBase = evalIncomes.ResultValue;
                }
            }

            ITermResult resultsValues = new SocialBaseResult(target, spec,
                evalIncomes.InterestCode, evalIncomes.SubjectType, evalIncomes.ParticyCode, 
                evalTarget.AnnuityBase, resGeneralBase, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // SocialBaseEmployee			SOCIAL_BASE_EMPLOYEE
    class SocialBaseEmployeeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_BASE_EMPLOYEE;
        public SocialBaseEmployeeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialBaseEmployeeConSpec(this.Code.Value);
        }
    }

    class SocialBaseEmployeeConSpec : PayrolexConceptSpec
    {
        public SocialBaseEmployeeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialBaseEmployeeTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<SocialBaseEmployeeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialBaseEmployeeTarget evalTarget = resTarget.Value;

            Int32 resBasis = 0;

            ITermResult resultsValues = new SocialBaseEmployeeResult(target, spec, resBasis, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // SocialBaseEmployer			SOCIAL_BASE_EMPLOYER
    class SocialBaseEmployerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_BASE_EMPLOYER;
        public SocialBaseEmployerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialBaseEmployerConSpec(this.Code.Value);
        }
    }

    class SocialBaseEmployerConSpec : PayrolexConceptSpec
    {
        public SocialBaseEmployerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialBaseEmployerTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<SocialBaseEmployerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialBaseEmployerTarget evalTarget = resTarget.Value;

            Int32 resBasis = 0;

            ITermResult resultsValues = new SocialBaseEmployerResult(target, spec, resBasis, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // SocialBaseOvercap			SOCIAL_BASE_OVERCAP
    class SocialBaseOvercapConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_BASE_OVERCAP;
        public SocialBaseOvercapConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialBaseOvercapConSpec(this.Code.Value);
        }
    }

    class SocialBaseOvercapConSpec : PayrolexConceptSpec
    {
        public SocialBaseOvercapConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE,
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
                new SocialBaseOvercapTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSocial = GetSocialPropsResult(ruleset, target, period);
            if (resPrSocial.IsFailure)
            {
                return BuildFailResults(resPrSocial.Error);
            }
            IPropsSocial socialRules = resPrSocial.Value;

            Int32 maxAnnualsBasis = socialRules.MaxAnnualsBasis;

            var resTarget = GetTypedTarget<SocialBaseOvercapTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialBaseOvercapTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE))
                .Select((tr) => (tr as SocialBaseResult)).ToArray();

            var incomeResultInit = Array.Empty<SocialBaseOvercapResult>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var evalInterestCode = x.InterestCode;
                var evalSubjectType = x.SubjectType;
                var evalParticyCode = x.ParticyCode;

                var contractResult = agr.FirstOrDefault((a) => (a.Contract.Equals(x.Contract)));
                if (contractResult == null)
                {
                    contractResult = new SocialBaseOvercapResult(evalTarget, x.Contract, spec,
                        evalInterestCode, evalSubjectType, evalParticyCode, 
                        VALUE_ZERO, BASIS_ZERO);
                    agr = agr.Concat(new SocialBaseOvercapResult[] { contractResult }).ToArray();
                }
                contractResult.AddResultBasis(x.ResultValue);
                return agr;
            });

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), SocialBaseOvercapResult.ResultComparator()).ToArray();

            Int32 perAnnuityBasis = 0;
            Int32 perAnnualsBasis = Math.Max(0, maxAnnualsBasis - perAnnuityBasis);
            var resultOrdersInit = new Tuple<Int32, Int32, SocialBaseOvercapResult[]>(
                maxAnnualsBasis, perAnnualsBasis, Array.Empty<SocialBaseOvercapResult>());

            var resultOrdersList = incomeOrdersList.Aggregate(resultOrdersInit,
                (agr, x) => {
                    Int32 ovrAnnualsBasis = 0;
                    Int32 rawAnnualsBasis = x.ResultBasis;
                    Int32 cutAnnualsBasis = x.ResultBasis;
                    if (agr.Item1 > 0)
                    {
                        ovrAnnualsBasis = Math.Max(0, rawAnnualsBasis - agr.Item2);
                        cutAnnualsBasis = (rawAnnualsBasis - ovrAnnualsBasis);
                    }

                    Int32 remAnnualsBasis = Math.Max(0, (agr.Item2 - cutAnnualsBasis));

                    if (ovrAnnualsBasis > 0)
                    {
                        x.SetResultValue(ovrAnnualsBasis);

                        return new Tuple<Int32, Int32, SocialBaseOvercapResult[]>(
                            agr.Item1, remAnnualsBasis, agr.Item3.Concat(new SocialBaseOvercapResult[] { x }).ToArray());
                    }
                    return new Tuple<Int32, Int32, SocialBaseOvercapResult[]>(agr.Item1, remAnnualsBasis, agr.Item3);
                });

            return BuildOkResults(resultOrdersList.Item3);
        }
    }

    // SocialPaymEmployee			SOCIAL_PAYM_EMPLOYEE
    class SocialPaymEmployeeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_PAYM_EMPLOYEE;
        public SocialPaymEmployeeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialPaymEmployeeConSpec(this.Code.Value);
        }
    }

    class SocialPaymEmployeeConSpec : PayrolexConceptSpec
    {
        public SocialPaymEmployeeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_OVERCAP,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYEE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialPaymEmployeeTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSocial = GetSocialPropsResult(ruleset, target, period);
            if (resPrSocial.IsFailure)
            {
                return BuildFailResults(resPrSocial.Error);
            }
            IPropsSocial socialRules = resPrSocial.Value;


            var resTarget = GetTypedTarget<SocialPaymEmployeeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialPaymEmployeeTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<SocialBaseResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 valBaseGenerals = evalBaseVal.ResultValue;

            Int32 valBaseEmployee = 0;
            Int32 valBaseOvercaps = 0;
            var resBaseEmployee = GetContractResult<SocialBaseEmployeeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYEE));

            if (resBaseEmployee.IsSuccess)
            {
                valBaseEmployee = resBaseEmployee.Value.ResultValue;
            }

            var resBaseOvercaps = GetContractResult<SocialBaseOvercapResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_OVERCAP));

            if (resBaseOvercaps.IsSuccess)
            {
                valBaseOvercaps = resBaseOvercaps.Value.ResultValue;
            }

            var (maxBaseEmployee, empBaseOvercaps) = socialRules.ResultOvercaps(valBaseEmployee, valBaseOvercaps);
            var (maxBaseGenerals, genBaseOvercaps) = socialRules.ResultOvercaps(valBaseGenerals, empBaseOvercaps);

            Int32 sumBaseEmployee = (maxBaseEmployee + maxBaseGenerals);

            Int32 employeePayment = socialRules.RoundedEmployeePaym(sumBaseEmployee);

            ITermResult resultsValues = new SocialPaymEmployeeResult(target, spec, maxBaseEmployee, maxBaseGenerals, employeePayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // SocialPaymEmployer			SOCIAL_PAYM_EMPLOYER
    class SocialPaymEmployerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_SOCIAL_PAYM_EMPLOYER;
        public SocialPaymEmployerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new SocialPaymEmployerConSpec(this.Code.Value);
        }
    }

    class SocialPaymEmployerConSpec : PayrolexConceptSpec
    {
        public SocialPaymEmployerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_OVERCAP,
                (Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYER,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new SocialPaymEmployerTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrSocial = GetSocialPropsResult(ruleset, target, period);
            if (resPrSocial.IsFailure)
            {
                return BuildFailResults(resPrSocial.Error);
            }
            IPropsSocial socialRules = resPrSocial.Value;

            var resTarget = GetTypedTarget<SocialPaymEmployerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            SocialPaymEmployerTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<SocialBaseResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 valBaseGenerals = evalBaseVal.ResultValue;

            Int32 valBaseEmployee = 0;
            Int32 valBaseEmployer = 0;
            Int32 valBaseOvercaps = 0;
            var resBaseEmployee = GetContractResult<SocialBaseEmployeeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYEE));

            if (resBaseEmployee.IsSuccess)
            {
                valBaseEmployee = resBaseEmployee.Value.ResultValue;
            }
            var resBaseEmployer = GetContractResult<SocialBaseEmployerResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYER));

            if (resBaseEmployer.IsSuccess)
            {
                valBaseEmployer = resBaseEmployer.Value.ResultValue;
            }

            var resBaseOvercaps = GetContractResult<SocialBaseOvercapResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_BASE_OVERCAP));

            if (resBaseOvercaps.IsSuccess)
            {
                valBaseOvercaps = resBaseOvercaps.Value.ResultValue;
            }

            Int32 maxBaseEmployer = Math.Max(0, valBaseEmployer - valBaseOvercaps);
            Int32 emrBaseOvercaps = Math.Max(0, (valBaseEmployer - maxBaseEmployer));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - emrBaseOvercaps);

            Int32 maxBaseGenerals = Math.Max(0, valBaseGenerals - valBaseOvercaps);
            Int32 genBaseOvercaps = Math.Max(0, (valBaseGenerals - maxBaseGenerals));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - genBaseOvercaps);

            Int32 sumBaseEmployer = (maxBaseEmployer + maxBaseGenerals);

            Int32 employerPayment = socialRules.RoundedEmployerPaym(sumBaseEmployer);

            ITermResult resultsValues = new SocialPaymEmployerResult(target, spec, maxBaseEmployer, maxBaseGenerals, employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

}
