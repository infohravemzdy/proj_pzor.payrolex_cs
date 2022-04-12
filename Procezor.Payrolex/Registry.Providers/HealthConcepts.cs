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
using MaybeMonad;
using ResultMonad;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // HealthDeclare			HEALTH_DECLARE
    class HealthDeclareConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_DECLARE;
        public HealthDeclareConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthDeclareConSpec(this.Code.Value);
        }
    }

    class HealthDeclareConSpec : PayrolexConceptSpec
    {
        public HealthDeclareConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract))))==false).ToArray();
            
            return ter.Select((t) => (new HealthDeclareTarget(month, t.Contract, pos, var, article, this.Code, 1, WorkHealthTerms.HEALTH_TERM_BY_CONTRACT, 1)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<HealthDeclareTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthDeclareTarget evalTarget = resTarget.Value;

            var resContract = GetContractResult<ContractWorkTermResult>(target, period, results,
             target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM));

            if (resContract.IsFailure)
            {
                return BuildFailResults(resContract.Error);
            }

            var evalContract = resContract.Value;

            var evalContractType = evalTarget.ContractType;

            if (evalContractType == WorkHealthTerms.HEALTH_TERM_BY_CONTRACT)
            {
                switch (evalContract.TermType)
                {
                    case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                        evalContractType = WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_A:
                        evalContractType = WorkHealthTerms.HEALTH_TERM_AGREEM_WORK;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_T:
                        evalContractType = WorkHealthTerms.HEALTH_TERM_AGREEM_TASK;
                        break;
                    case WorkContractTerms.WORKTERM_PARTNER_STAT:
                        evalContractType = WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS;
                        break;
                }
            }
            ITermResult resultsValues = new HealthDeclareResult(target, spec, 
                evalTarget.InterestCode, evalContractType, evalTarget.MandatorBase);

            return BuildOkResults(resultsValues);
        }
    }

    // HealthIncome			HEALTH_INCOME
    class HealthIncomeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_INCOME;
        public HealthIncomeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthIncomeConSpec(this.Code.Value);
        }
    }

    class HealthIncomeConSpec : PayrolexConceptSpec
    {
        public HealthIncomeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE,
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
                new HealthIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            Int32 marginIncomeEmp = healthRules.MarginIncomeEmp;
            Int32 marginIncomeAgr = healthRules.MarginIncomeAgr;

            var resTarget = GetTypedTarget<HealthIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthIncomeTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE))
                .Select((tr) => (tr as HealthDeclareResult)).ToArray();

            var incomeResultInit = Array.Empty<HealthIncomeResult>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var evalSubjectsType = x.ContractType;
                var evalInterestCode = x.InterestCode;
                var evalMandatorBase = x.MandatorBase;

                var contractResult = agr.FirstOrDefault((a) => (a.Contract.Equals(x.Contract)));
                if (contractResult == null)
                {
                    contractResult = new HealthIncomeResult(evalTarget, x.Contract, spec,
                        evalInterestCode, evalSubjectsType, evalMandatorBase, VALUE_ZERO, VALUE_ZERO, BASIS_ZERO);
                    agr = agr.Concat(new HealthIncomeResult[] { contractResult }).ToArray();
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

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), HealthIncomeResult.ResultComparator()).ToArray();

            var resultOrdersInit = Array.Empty<HealthIncomeResult>();

            var resultOrdersList = incomeOrdersList.Aggregate(resultOrdersInit,
                (agr, x) => {
                    Int32 sumTermIncome = incomeResultList.Where((c) => (c.InterestCode != 0 && c.IncomeTerm().Equals(x.IncomeTerm())))
                        .Aggregate(0, (sum, c) => (sum + c.ResultValue));
                    Int32 conTermIncome = x.ResultValue;

                    Int16 particyCode = 0;
                    if (x.InterestCode != 0)
                    {
                        if (healthRules.HasParticy(x.IncomeTerm(), sumTermIncome, conTermIncome))
                        {
                            particyCode = 1;
                        }
                    }
                    x.SetParticyCode(particyCode);

                    return agr.Concat(new HealthIncomeResult[] { x }).ToArray();
                });

            return BuildOkResults(resultOrdersList);
        }
    }

    // HealthBase			HEALTH_BASE
    class HealthBaseConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_BASE;
        public HealthBaseConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthBaseConSpec(this.Code.Value);
        }
    }

    class HealthBaseConSpec : PayrolexConceptSpec
    {
        public HealthBaseConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_INCOME,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new HealthBaseTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<HealthBaseTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthBaseTarget evalTarget = resTarget.Value;

            var resDeclare = GetContractResult<HealthDeclareResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE));
            var resIncomes = GetContractResult<HealthIncomeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_INCOME));

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

            ITermResult resultsValues = new HealthBaseResult(target, spec,
                evalIncomes.InterestCode, evalIncomes.SubjectType, evalIncomes.MandatorBase, evalIncomes.ParticyCode,
                evalTarget.AnnuityBase, resGeneralBase, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // HealthBaseEmployee			HEALTH_BASE_EMPLOYEE
    class HealthBaseEmployeeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_BASE_EMPLOYEE;
        public HealthBaseEmployeeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthBaseEmployeeConSpec(this.Code.Value);
        }
    }

    class HealthBaseEmployeeConSpec : PayrolexConceptSpec
    {
        public HealthBaseEmployeeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_MANDATE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new HealthBaseEmployeeTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<HealthBaseEmployeeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthBaseEmployeeTarget evalTarget = resTarget.Value;

            Int32 baseEmployee = 0;

            Int32 baseMandated = 0;

            var resBaseMandated = GetContractResult<HealthBaseMandateResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_MANDATE));

            if (resBaseMandated.IsSuccess)
            {
                baseMandated = resBaseMandated.Value.ResultValue;
            }

            ITermResult resultsValues = new HealthBaseEmployeeResult(target, spec, baseMandated + baseEmployee, baseEmployee);

            return BuildOkResults(resultsValues);
        }
    }

    // HealthBaseEmployer			HEALTH_BASE_EMPLOYER
    class HealthBaseEmployerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_BASE_EMPLOYER;
        public HealthBaseEmployerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthBaseEmployerConSpec(this.Code.Value);
        }
    }

    class HealthBaseEmployerConSpec : PayrolexConceptSpec
    {
        public HealthBaseEmployerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new HealthBaseEmployerTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<HealthBaseEmployerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthBaseEmployerTarget evalTarget = resTarget.Value;

            Int32 baseEmployer = 0;

            ITermResult resultsValues = new HealthBaseEmployerResult(target, spec, baseEmployer, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // HealthBaseMandate			HEALTH_BASE_MANDATE
    class HealthBaseMandateConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_BASE_MANDATE;
        public HealthBaseMandateConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthBaseMandateConSpec(this.Code.Value);
        }
    }

    class HealthBaseMandateConSpec : PayrolexConceptSpec
    {
        public HealthBaseMandateConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE,
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
                new HealthBaseMandateTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            Int32 minMonthlyBasis = healthRules.MinMonthlyBasis;

            if (minMonthlyBasis == 0)
            {
                return BuildEmptyResults();
            }
            var resTarget = GetTypedTarget<HealthBaseMandateTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthBaseMandateTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE))
                .Select((tr) => (tr as HealthBaseResult)).ToArray();

            var incomeResultInit = Array.Empty<HealthBaseMandateResult>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var evalInterestCode = x.InterestCode;
                var evalSubjectType = x.SubjectType;
                var evalMandatorBase = x.MandatorBase;
                var evalParticyCode = x.ParticyCode;

                var contractResult = agr.FirstOrDefault((a) => (a.Contract.Equals(x.Contract)));
                if (contractResult == null)
                {
                    contractResult = new HealthBaseMandateResult(evalTarget, x.Contract, spec,
                        evalInterestCode, evalSubjectType, evalMandatorBase, evalParticyCode,
                        VALUE_ZERO, BASIS_ZERO);
                    agr = agr.Concat(new HealthBaseMandateResult[] { contractResult }).ToArray();
                }
                contractResult.AddResultBasis(x.ResultValue);
                return agr;
            });


            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), HealthBaseMandateResult.ResultComparator()).ToArray();

            Int32 sumMonthlyBasis = Math.Max(0, incomeResultList.Aggregate(0, (agr, item) => (agr + item.ResultBasis)));
            Int32 resMandateBasis = Math.Max(0, minMonthlyBasis - sumMonthlyBasis);

            var resultOrdersInit = new Tuple<Int32, Int32, HealthBaseMandateResult[]>(
                minMonthlyBasis, resMandateBasis, Array.Empty<HealthBaseMandateResult>());

            var resultOrdersList = incomeOrdersList.Aggregate(resultOrdersInit,
                (agr, x) =>
                {
                    Int32 resMandateBase = agr.Item2;
                    Int32 curMandateBase = 0;
                    if (x.InterestCode != 0)
                    {
                        if (x.ParticyCode != 0)
                        {
                            Int32 resGeneralBase = x.ResultBasis;

                            if (x.MandatorBase != 0)
                            {
                                curMandateBase = Math.Max(0, resMandateBase);
                                resMandateBase = Math.Max(0, resMandateBase - curMandateBase);
                            }
                        }
                    }
                    if (curMandateBase > 0)
                    {
                        x.SetResultValue(curMandateBase);

                        return new Tuple<Int32, Int32, HealthBaseMandateResult[]>(
                            agr.Item1, resMandateBase, agr.Item3.Concat(new HealthBaseMandateResult[] { x }).ToArray());
                    }
                    return new Tuple<Int32, Int32, HealthBaseMandateResult[]>(agr.Item1, resMandateBase, agr.Item3);
                });

            return BuildOkResults(resultOrdersList.Item3);
        }
    }

    // HealthBaseOvercap			HEALTH_BASE_OVERCAP
    class HealthBaseOvercapConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_BASE_OVERCAP;
        public HealthBaseOvercapConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthBaseOvercapConSpec(this.Code.Value);
        }
    }

    class HealthBaseOvercapConSpec : PayrolexConceptSpec
    {
        public HealthBaseOvercapConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE,
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
                new HealthBaseOvercapTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            Int32 maxAnnualsBasis = healthRules.MaxAnnualsBasis;

            var resTarget = GetTypedTarget<HealthBaseOvercapTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthBaseOvercapTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE))
                .Select((tr) => (tr as HealthBaseResult)).ToArray();

            var incomeResultInit = Array.Empty<HealthBaseOvercapResult>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var evalInterestCode = x.InterestCode;
                var evalSubjectType = x.SubjectType;
                var evalMandatorBase = x.MandatorBase;
                var evalParticyCode = x.ParticyCode;

                var contractResult = agr.FirstOrDefault((a) => (a.Contract.Equals(x.Contract)));
                if (contractResult == null)
                {
                    contractResult = new HealthBaseOvercapResult(evalTarget, x.Contract, spec,
                        evalInterestCode, evalSubjectType, evalMandatorBase, evalParticyCode,
                        VALUE_ZERO, BASIS_ZERO);
                    agr = agr.Concat(new HealthBaseOvercapResult[] { contractResult }).ToArray();
                }
                contractResult.AddResultBasis(x.ResultValue);
                return agr;
            });

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), HealthBaseOvercapResult.ResultComparator()).ToArray();

            Int32 perAnnuityBasis = 0;
            Int32 perAnnualsBasis = Math.Max(0, maxAnnualsBasis - perAnnuityBasis);
            var resultOrdersInit = new Tuple<Int32, Int32, HealthBaseOvercapResult[]>(
                maxAnnualsBasis, perAnnualsBasis, Array.Empty<HealthBaseOvercapResult>());

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

                        return new Tuple<Int32, Int32, HealthBaseOvercapResult[]>(
                            agr.Item1, remAnnualsBasis, agr.Item3.Concat(new HealthBaseOvercapResult[] { x }).ToArray());
                    }
                    return new Tuple<Int32, Int32, HealthBaseOvercapResult[]>(agr.Item1, remAnnualsBasis, agr.Item3);
                });

            return BuildOkResults(resultOrdersList.Item3);
        }
    }

    // HealthPaymEmployee			HEALTH_PAYM_EMPLOYEE
    class HealthPaymEmployeeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_PAYM_EMPLOYEE;
        public HealthPaymEmployeeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthPaymEmployeeConSpec(this.Code.Value);
        }
    }

    class HealthPaymEmployeeConSpec : PayrolexConceptSpec
    {
        public HealthPaymEmployeeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_OVERCAP,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYEE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new HealthPaymEmployeeTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<HealthPaymEmployeeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthPaymEmployeeTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<HealthBaseResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 valBaseGenerals = evalBaseVal.ResultValue;

            Int32 valBaseEmployee = 0;
            Int32 valBaseOvercaps = 0;
            var resBaseEmployee = GetContractResult<HealthBaseEmployeeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYEE));

            if (resBaseEmployee.IsSuccess)
            {
                valBaseEmployee = resBaseEmployee.Value.ResultValue;
            }
            
            var resBaseOvercaps = GetContractResult<HealthBaseOvercapResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_OVERCAP));

            if (resBaseOvercaps.IsSuccess)
            {
                valBaseOvercaps = resBaseOvercaps.Value.ResultValue;
            }

            Int32 maxBaseEmployee = Math.Max(0, valBaseEmployee - valBaseOvercaps);
            Int32 empBaseOvercaps = Math.Max(0, (valBaseEmployee - maxBaseEmployee));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - empBaseOvercaps);

            Int32 maxBaseGenerals = Math.Max(0, valBaseGenerals - valBaseOvercaps);
            Int32 genBaseOvercaps = Math.Max(0, (valBaseGenerals - maxBaseGenerals));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - genBaseOvercaps);

            Int32 employeePayment = healthRules.RoundedAugmentEmployeePaym(maxBaseGenerals, maxBaseEmployee);

            ITermResult resultsValues = new HealthPaymEmployeeResult(target, spec, maxBaseEmployee, maxBaseGenerals, employeePayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // HealthPaymEmployer			HEALTH_PAYM_EMPLOYER
    class HealthPaymEmployerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_HEALTH_PAYM_EMPLOYER;
        public HealthPaymEmployerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new HealthPaymEmployerConSpec(this.Code.Value);
        }
    }

    class HealthPaymEmployerConSpec : PayrolexConceptSpec
    {
        public HealthPaymEmployerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_OVERCAP,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYEE,
                (Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYER,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new HealthPaymEmployerTarget(month, t.Contract, pos, var, article, this.Code, 0)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<HealthPaymEmployerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            HealthPaymEmployerTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<HealthBaseResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 valBaseGenerals = evalBaseVal.ResultValue;

            Int32 valBaseEmployee = 0;
            Int32 valBaseEmployer = 0;
            Int32 valBaseOvercaps = 0;
            var resBaseEmployee = GetContractResult<HealthBaseEmployeeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYEE));

            if (resBaseEmployee.IsSuccess)
            {
                valBaseEmployee = resBaseEmployee.Value.ResultValue;
            }
            var resBaseEmployer = GetContractResult<HealthBaseEmployeeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYER));

            if (resBaseEmployer.IsSuccess)
            {
                valBaseEmployer = resBaseEmployer.Value.ResultValue;
            }

            var resBaseOvercaps = GetContractResult<HealthBaseOvercapResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_BASE_OVERCAP));

            if (resBaseOvercaps.IsSuccess)
            {
                valBaseOvercaps = resBaseOvercaps.Value.ResultValue;
            }

            Int32 maxBaseEmployee = Math.Max(0, valBaseEmployee - valBaseOvercaps);
            Int32 empBaseOvercaps = Math.Max(0, (valBaseEmployee - maxBaseEmployee));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - empBaseOvercaps);

            Int32 maxBaseGenerals = Math.Max(0, valBaseGenerals - valBaseOvercaps);
            Int32 genBaseOvercaps = Math.Max(0, (valBaseGenerals - maxBaseGenerals));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - genBaseOvercaps);

            Int32 maxBaseEmployer = Math.Max(0, valBaseEmployer - valBaseOvercaps);
            Int32 emrBaseOvercaps = Math.Max(0, (valBaseEmployer - maxBaseEmployer));
            valBaseOvercaps = Math.Max(0, valBaseOvercaps - emrBaseOvercaps);

            Int32 employerPayment = healthRules.RoundedAugmentEmployerPaym(maxBaseGenerals, maxBaseEmployee, maxBaseEmployer);

            ITermResult resultsValues = new HealthPaymEmployerResult(target, spec, maxBaseEmployer, maxBaseGenerals, employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }
}
