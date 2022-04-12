using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Props;
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
    // TaxingDeclare			TAXING_DECLARE
    class TaxingDeclareConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_DECLARE;
        public TaxingDeclareConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingDeclareConSpec(this.Code.Value);
        }
    }

    class TaxingDeclareConSpec : PayrolexConceptSpec
    {
        public TaxingDeclareConSpec(Int32 code) : base(code)
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

            return ter.Select((t) => (new TaxingDeclareTarget(month, t.Contract, pos, var, article, this.Code, 1, WorkTaxingTerms.TAXING_TERM_BY_CONTRACT)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingDeclareTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingDeclareTarget evalTarget = resTarget.Value;

            var resContract = GetContractResult<ContractWorkTermResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM));

            if (resContract.IsFailure)
            {
                return BuildFailResults(resContract.Error);
            }

            var evalContract = resContract.Value;

            var evalContractType = evalTarget.ContractType;

            if (evalContractType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT)
            {
                switch (evalContract.TermType)
                {
                    case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                        evalContractType = WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_A:
                        evalContractType = WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS;
                        break;
                    case WorkContractTerms.WORKTERM_CONTRACTER_T:
                        evalContractType = WorkTaxingTerms.TAXING_TERM_AGREEM_TASK;
                        break;
                    case WorkContractTerms.WORKTERM_PARTNER_STAT:
                        evalContractType = WorkTaxingTerms.TAXING_TERM_STATUT_PART;
                        break;
                }
            }
            ITermResult resultsValues = new TaxingDeclareResult(target, spec, 
                evalTarget.InterestCode, evalContractType);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingSigning			TAXING_SIGNING
    class TaxingSigningConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_SIGNING;
        public TaxingSigningConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingSigningConSpec(this.Code.Value);
        }
    }

    class TaxingSigningConSpec : PayrolexConceptSpec
    {
        public TaxingSigningConSpec(Int32 code) : base(code)
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
                new TaxingSigningTarget(month, con, pos, var, article, this.Code, TaxDeclSignOption.DECL_TAX_DO_SIGNED, TaxNoneSignOption.NOSIGN_TAX_WITHHOLD),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingSigningTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingSigningTarget evalTarget = resTarget.Value;

            ITermResult resultsValues = new TaxingSigningResult(target, spec, 
                evalTarget.DeclSignOpts, evalTarget.NoneSignOpts);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingIncomeSubject			TAXING_INCOME_SUBJECT
    class TaxingIncomeSubjectConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_INCOME_SUBJECT;
        public TaxingIncomeSubjectConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingIncomeSubjectConSpec(this.Code.Value);
        }
    }

    class TaxingIncomeSubjectConSpec : PayrolexConceptSpec
    {
        public TaxingIncomeSubjectConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            var pos = PositionCode.Zero;

            var ter = conTerms.Where((t) => (targets.Any((x) => (x.Contract.Equals(t.Contract)))) == false).ToArray();

            return ter.Select((t) => (new TaxingIncomeSubjectTarget(month, t.Contract, pos, var, article, this.Code, WorkTaxingTerms.TAXING_TERM_BY_CONTRACT)));
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingIncomeSubjectTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingIncomeSubjectTarget evalTarget = resTarget.Value;

            var resTaxDeclare = GetContractResult<TaxingDeclareResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE));

            if (resTaxDeclare.IsFailure)
            {
                return BuildFailResults(resTaxDeclare.Error);
            }

            var evalTaxDeclare = resTaxDeclare.Value;

            var evalSubjectsType = evalTaxDeclare.ContractType;

            var incomeList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Contract.Equals(evalTarget.Contract) && v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            ITermResult resultsValues = new TaxingIncomeSubjectResult(target, spec,
                evalSubjectsType, incomeRes, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingIncomeHealth			TAXING_INCOME_HEALTH
    class TaxingIncomeHealthConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_INCOME_HEALTH;
        public TaxingIncomeHealthConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingIncomeHealthConSpec(this.Code.Value);
        }
    }

    class TaxingIncomeHealthConSpec : PayrolexConceptSpec
    {
        public TaxingIncomeHealthConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE,
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
                new TaxingIncomeHealthTarget(month, con, pos, var, article, this.Code),
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

            var resTarget = GetTypedTarget<TaxingIncomeHealthTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingIncomeHealthTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.Contract, tr.ResultValue)).ToArray();

            var incomeResultInit = Array.Empty<ParticyHealthTarget>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var resTaxDeclare = GetContractResult<TaxingDeclareResult>(target, period, results,
                    x.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE));

                if (resTaxDeclare.IsFailure)
                {
                    return agr;
                }

                var evalTaxDeclare = resTaxDeclare.Value;

                var evalSubjectsType = evalTaxDeclare.ContractType;

                var resHealthInc = GetContractResult<HealthIncomeResult>(target, period, results,
                    x.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_INCOME));

                if (resHealthInc.IsFailure)
                {
                    return agr;
                }

                var evalHealthInc = resHealthInc.Value;

                var evalInterestCode = evalHealthInc.InterestCode;
                var evalSubjectsTerm = evalHealthInc.SubjectType;
                var evalParticyCode = evalHealthInc.ParticyCode;

                if (evalInterestCode != 0)
                {
                    var contractResult = agr.FirstOrDefault((a) => (a.ContractCode.Equals(x.Contract.Value)));
                    if (contractResult == null)
                    {
                        contractResult = new ParticyHealthTarget(x.Contract.Value, 
                            evalSubjectsType, evalInterestCode, evalSubjectsTerm, evalParticyCode, BASIS_ZERO);
                        agr = agr.Concat(new ParticyHealthTarget[] { contractResult }).ToArray();
                    }
                    contractResult.AddTargetsBase(x.ResultValue);
                }
                return agr;
            });

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), ParticyHealthTarget.ResultComparator()).ToArray();

            Int32 perAnnuityBasis = 0;

            var resultOrdersList = healthRules.AnnualsBasisCut(incomeOrdersList, perAnnuityBasis);

            return BuildOkResults(resultOrdersList.Item3.Select((x) => new TaxingIncomeHealthResult(evalTarget, 
                ContractCode.Get(x.ContractCode), spec, 
                x.SubjectType, x.InterestCode, x.SubjectTerm, x.ParticyCode,
                x.ResultsBase, x.TargetsBase)).ToArray());
        }
    }

    // TaxingIncomeSocial			TAXING_INCOME_SOCIAL
    class TaxingIncomeSocialConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_INCOME_SOCIAL;
        public TaxingIncomeSocialConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingIncomeSocialConSpec(this.Code.Value);
        }
    }

    class TaxingIncomeSocialConSpec : PayrolexConceptSpec
    {
        public TaxingIncomeSocialConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE,
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
                new TaxingIncomeSocialTarget(month, con, pos, var, article, this.Code),
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

            var resTarget = GetTypedTarget<TaxingIncomeSocialTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingIncomeSocialTarget evalTarget = resTarget.Value;

            var incomeContractList = results
                .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
                .Where((v) => (v.Spec.Sums.Contains(evalTarget.Article)))
                .Select((v) => (v as PayrolexTermResult))
                .Select((tr) => (tr.Contract, tr.ResultValue)).ToArray();

            var incomeResultInit = Array.Empty<ParticySocialTarget>();
            var incomeResultList = incomeContractList.Aggregate(incomeResultInit, (agr, x) =>
            {
                var resTaxDeclare = GetContractResult<TaxingDeclareResult>(target, period, results,
                    x.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE));

                if (resTaxDeclare.IsFailure)
                {
                    return agr;
                }

                var evalTaxDeclare = resTaxDeclare.Value;

                var evalSubjectsType = evalTaxDeclare.ContractType;

                var resSocialInc = GetContractResult<SocialIncomeResult>(target, period, results,
                    x.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_INCOME));

                if (resSocialInc.IsFailure)
                {
                    return agr;
                }

                var evalSocialInc = resSocialInc.Value;

                var evalInterestCode = evalSocialInc.InterestCode;
                var evalSubjectsTerm = evalSocialInc.SubjectType;
                var evalParticyCode = evalSocialInc.ParticyCode;

                if (evalInterestCode != 0)
                {
                    var contractResult = agr.FirstOrDefault((a) => (a.ContractCode.Equals(x.Contract.Value)));
                    if (contractResult == null)
                    {
                        contractResult = new ParticySocialTarget(x.Contract.Value, 
                            evalSubjectsType, evalInterestCode, evalSubjectsTerm, evalParticyCode, BASIS_ZERO);
                        agr = agr.Concat(new ParticySocialTarget[] { contractResult }).ToArray();
                    }
                    contractResult.AddTargetsBase(x.ResultValue);
                }
                return agr;
            });

            var incomeOrdersList = incomeResultList.OrderBy((x) => (x), ParticySocialTarget.ResultComparator()).ToArray();

            Int32 perAnnuityBasis = 0;

            var resultOrdersList = socialRules.AnnualsBasisCut(incomeOrdersList, perAnnuityBasis);

            return BuildOkResults(resultOrdersList.Item3.Select((x) => new TaxingIncomeSocialResult(evalTarget, 
                ContractCode.Get(x.ContractCode), spec, 
                x.SubjectType, x.InterestCode, x.SubjectTerm, x.ParticyCode,
                x.ResultsBase, x.TargetsBase)).ToArray());
        }
    }

    // TaxingAdvancesIncome			TAXING_ADVANCES_INCOME
    class TaxingAdvancesIncomeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES_INCOME;
        public TaxingAdvancesIncomeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingAdvancesIncomeConSpec(this.Code.Value);
            }
            return new TaxingAdvancesIncomeCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesIncomeCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesIncomeCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
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
                new TaxingAdvancesIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesIncomeTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value==(Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            Int32 resValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT, 
                evalDeclSignOpts, evalNoneSignOpts, incomeRes)==false)
            {
                resValue = incomeRes;
            }
            ITermResult resultsValues = new TaxingAdvancesIncomeResult(target, spec,
                resValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesIncomeConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesIncomeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
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
                new TaxingAdvancesIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesIncomeTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value==(Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value==(Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value==(Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value==(Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            Int32 resValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK, 
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes)==false)
            {
                resValue += incomeDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS, 
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes)==false)
            {
                resValue += incomeLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART, 
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes)==false)
            {
                resValue += incomeStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT, 
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes)==false)
            {
                resValue += incomeRestRes;
            }
            ITermResult resultsValues = new TaxingAdvancesIncomeResult(target, spec,
                resValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAdvancesHealth			TAXING_ADVANCES_HEALTH
    class TaxingAdvancesHealthConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES_HEALTH;
        public TaxingAdvancesHealthConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingAdvancesHealthConSpec(this.Code.Value);
            }
            return new TaxingAdvancesHealthCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesHealthCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesHealthCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH,
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
                new TaxingAdvancesHealthTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesHealthTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesHealthTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            var healthList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthSum = healthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthRes = RoundToInt(healthSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRes) == false)
            {
                baseValue = healthRes;
            }

            Int32 compoundPayment = healthRules.RoundedCompoundPaym(baseValue);
            Int32 employeePayment = healthRules.RoundedEmployeePaym(baseValue);
            Int32 employerPayment = healthRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingAdvancesHealthResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesHealthConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesHealthConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH,
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
                new TaxingAdvancesHealthTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesHealthTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesHealthTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            var healthDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthDppsSum = healthDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthLowsSum = healthLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthStatSum = healthStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthRestSum = healthRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthDppsRes = RoundToInt(healthDppsSum);
            Int32 healthLowsRes = RoundToInt(healthLowsSum);
            Int32 healthStatRes = RoundToInt(healthStatSum);
            Int32 healthRestRes = RoundToInt(healthRestSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK,
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes) == false)
            {
                baseValue += healthDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS,
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes) == false)
            {
                baseValue += healthLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART,
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes) == false)
            {
                baseValue += healthStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes) == false)
            {
                baseValue += healthRestRes;
            }

            Int32 compoundPayment = healthRules.RoundedCompoundPaym(baseValue);
            Int32 employeePayment = healthRules.RoundedEmployeePaym(baseValue);
            Int32 employerPayment = healthRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingAdvancesHealthResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAdvancesSocial			TAXING_ADVANCES_SOCIAL
    class TaxingAdvancesSocialConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES_SOCIAL;
        public TaxingAdvancesSocialConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingAdvancesSocialConSpec(this.Code.Value);
            }
            return new TaxingAdvancesSocialCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesSocialCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesSocialCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL,
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
                new TaxingAdvancesSocialTarget(month, con, pos, var, article, this.Code, 0),
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

            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesSocialTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesSocialTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            var socialList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialSum = socialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialRes = RoundToInt(socialSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRes) == false)
            {
                baseValue = socialRes;
            }

            Int32 employerPayment = socialRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingAdvancesSocialResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesSocialConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesSocialConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL,
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
                new TaxingAdvancesSocialTarget(month, con, pos, var, article, this.Code, 0),
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

            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesSocialTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesSocialTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            var socialDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialDppsSum = socialDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialLowsSum = socialLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialStatSum = socialStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialRestSum = socialRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialDppsRes = RoundToInt(socialDppsSum);
            Int32 socialLowsRes = RoundToInt(socialLowsSum);
            Int32 socialStatRes = RoundToInt(socialStatSum);
            Int32 socialRestRes = RoundToInt(socialRestSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK,
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes) == false)
            {
                baseValue += socialDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS,
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes) == false)
            {
                baseValue += socialLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART,
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes) == false)
            {
                baseValue += socialStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes) == false)
            {
                baseValue += socialRestRes;
            }

            Int32 employerPayment = socialRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingAdvancesSocialResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAdvancesBasis			TAXING_ADVANCES_BASIS
    class TaxingAdvancesBasisConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES_BASIS;
        public TaxingAdvancesBasisConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2021)
            {
                return new TaxingAdvancesBasisConSpec(this.Code.Value);
            }
            return new TaxingAdvancesBasisCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesBasisCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesBasisCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_HEALTH,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_SOCIAL,
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
                new TaxingAdvancesBasisTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesBasisTarget evalTarget = resTarget.Value;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME))
               .Select((x) => (x as TaxingAdvancesIncomeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(Math.Max(0, incomeSum));

            var healthList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_HEALTH))
               .Select((x) => (x as TaxingAdvancesHealthResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthSum = healthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthRes = RoundToInt(Math.Max(0, healthSum));

            var socialList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_SOCIAL))
               .Select((x) => (x as TaxingAdvancesSocialResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialSum = socialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialRes = RoundToInt(Math.Max(0, socialSum));

            Int32 taxableSuper = taxingRules.TaxableIncomeSupers(incomeRes, healthRes, socialRes);

            Int32 advancesBase = taxingRules.RoundedBaseAdvances(incomeRes, healthRes, socialRes);

            ITermResult resultsValues = new TaxingAdvancesBasisResult(target, spec, advancesBase, taxableSuper);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesBasisConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesBasisConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME,
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
                new TaxingAdvancesBasisTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesBasisTarget evalTarget = resTarget.Value;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME))
               .Select((x) => (x as TaxingAdvancesIncomeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(Math.Max(0, incomeSum));

            Int32 taxableBasis = taxingRules.TaxableIncomeBasis(incomeRes);

            Int32 advancesBase = taxingRules.RoundedBaseAdvances(incomeRes);

            ITermResult resultsValues = new TaxingAdvancesBasisResult(target, spec, advancesBase, taxableBasis);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingSolidaryBasis			TAXING_SOLIDARY_BASIS
    class TaxingSolidaryBasisConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_SOLIDARY_BASIS;
        public TaxingSolidaryBasisConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingSolidaryBasisConSpec(this.Code.Value);
        }
    }

    class TaxingSolidaryBasisConSpec : PayrolexConceptSpec
    {
        public TaxingSolidaryBasisConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME,
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
                new TaxingSolidaryBasisTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingSolidaryBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingSolidaryBasisTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<TaxingAdvancesIncomeResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 taxableIncome = evalBaseVal.ResultValue;

            Int32 solidaryBase = taxingRules.RoundedBaseSolidary(taxableIncome);

            ITermResult resultsValues = new TaxingSolidaryBasisResult(target, spec, solidaryBase, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAdvances			TAXING_ADVANCES
    class TaxingAdvancesConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES;
        public TaxingAdvancesConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2021)
            {
                return new TaxingAdvancesConSpec(this.Code.Value);
            }
            return new TaxingAdvancesCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS,
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
                new TaxingAdvancesTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<TaxingAdvancesBasisResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 advanceBasis = evalBaseVal.ResultBasis;

            Int32 advanceSuper = evalBaseVal.ResultValue;

            Int32 advancePaym = taxingRules.RoundedAdvancesPaym(advanceSuper, advanceBasis);

            ITermResult resultsValues = new TaxingAdvancesResult(target, spec, advancePaym, advanceSuper);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS,
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
                new TaxingAdvancesTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAdvancesTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<TaxingAdvancesBasisResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 advanceBasis = evalBaseVal.ResultBasis;

            Int32 advanceFound = evalBaseVal.ResultValue;

            Int32 advancePaym = taxingRules.RoundedAdvancesPaym(advanceFound, advanceBasis);

            ITermResult resultsValues = new TaxingAdvancesResult(target, spec, advancePaym, advanceFound);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingSolidary			TAXING_SOLIDARY
    class TaxingSolidaryConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_SOLIDARY;
        public TaxingSolidaryConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingSolidaryConSpec(this.Code.Value);
        }
    }

    class TaxingSolidaryConSpec : PayrolexConceptSpec
    {
        public TaxingSolidaryConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY_BASIS,
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
                new TaxingSolidaryTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingSolidaryTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingSolidaryTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<TaxingSolidaryBasisResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY_BASIS));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 solidaryBasis = evalBaseVal.ResultValue;

            Int32 solidaryPaym = taxingRules.RoundedSolidaryPaym(solidaryBasis);

            if (solidaryPaym != 0)
            {
                ITermResult resultsValues = new TaxingSolidaryResult(target, spec, solidaryPaym, 0);

                return BuildOkResults(resultsValues);
            }
            return BuildEmptyResults();
        }
    }

    // TaxingAdvancesTotal			TAXING_ADVANCES_TOTAL
    class TaxingAdvancesTotalConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ADVANCES_TOTAL;
        public TaxingAdvancesTotalConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2021)
            {
                return new TaxingAdvancesTotalConSpec(this.Code.Value);
            }
            return new TaxingAdvancesTotalCon2010Spec(this.Code.Value);
        }
    }

    class TaxingAdvancesTotalCon2010Spec : PayrolexConceptSpec
    {
        public TaxingAdvancesTotalCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY,
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
                new TaxingAdvancesTotalTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingAdvancesTotalTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesTotalTarget evalTarget = resTarget.Value;

            var advancesList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES))
               .Select((x) => (x as TaxingAdvancesResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal advancesSum = advancesList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 advancesTaxing = RoundToInt(advancesSum);

            var solidaryList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY))
               .Select((x) => (x as TaxingSolidaryResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal solidarySum = solidaryList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 solidaryTaxing = RoundToInt(solidarySum);

            Int32 advancesTotals = (advancesTaxing + solidaryTaxing);

            ITermResult resultsValues = new TaxingAdvancesTotalResult(target, spec, advancesTotals, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingAdvancesTotalConSpec : PayrolexConceptSpec
    {
        public TaxingAdvancesTotalConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES,
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
                new TaxingAdvancesTotalTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingAdvancesTotalTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAdvancesTotalTarget evalTarget = resTarget.Value;

            var advancesList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES))
               .Select((x) => (x as TaxingAdvancesResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal advancesSum = advancesList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 advancesTaxing = RoundToInt(advancesSum);

            Int32 advancesTotals = (advancesTaxing);

            ITermResult resultsValues = new TaxingAdvancesTotalResult(target, spec, advancesTotals, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingWithholdIncome			TAXING_WITHHOLD_INCOME
    class TaxingWithholdIncomeConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_WITHHOLD_INCOME;
        public TaxingWithholdIncomeConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingWithholdIncomeConSpec(this.Code.Value);
            }
            return new TaxingWithholdIncomeCon2010Spec(this.Code.Value);
        }
    }

    class TaxingWithholdIncomeCon2010Spec : PayrolexConceptSpec
    {
        public TaxingWithholdIncomeCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL,
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
                new TaxingWithholdIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdIncomeTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            Int32 resValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRes))
            {
                resValue = incomeRes;
            }

            ITermResult resultsValues = new TaxingWithholdIncomeResult(target, spec,
                resValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingWithholdIncomeConSpec : PayrolexConceptSpec
    {
        public TaxingWithholdIncomeConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
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
                new TaxingWithholdIncomeTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdIncomeTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdIncomeTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            Int32 resValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK,
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes))
            {
                resValue += incomeDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS,
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes))
            {
                resValue += incomeLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART,
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes))
            {
                resValue += incomeStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes))
            {
                resValue += incomeRestRes;
            }

            ITermResult resultsValues = new TaxingWithholdIncomeResult(target, spec,
                resValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingWithholdHealth			TAXING_WITHHOLD_HEALTH
    class TaxingWithholdHealthConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_WITHHOLD_HEALTH;
        public TaxingWithholdHealthConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingWithholdHealthConSpec(this.Code.Value);
            }
            return new TaxingWithholdHealthCon2010Spec(this.Code.Value);
        }
    }

    class TaxingWithholdHealthCon2010Spec : PayrolexConceptSpec
    {
        public TaxingWithholdHealthCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH,
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
                new TaxingWithholdHealthTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<TaxingWithholdHealthTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdHealthTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            var healthList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthSum = healthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthRes = RoundToInt(healthSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRes))
            {
                baseValue = healthRes;
            }

            Int32 compoundPayment = healthRules.RoundedCompoundPaym(baseValue);
            Int32 employeePayment = healthRules.RoundedEmployeePaym(baseValue);
            Int32 employerPayment = healthRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingWithholdHealthResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }

    }

    class TaxingWithholdHealthConSpec : PayrolexConceptSpec
    {
        public TaxingWithholdHealthConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH,
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
                new TaxingWithholdHealthTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resPrHealth = GetHealthPropsResult(ruleset, target, period);
            if (resPrHealth.IsFailure)
            {
                return BuildFailResults(resPrHealth.Error);
            }
            IPropsHealth healthRules = resPrHealth.Value;

            var resTarget = GetTypedTarget<TaxingWithholdHealthTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdHealthTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            var healthDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var healthRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH))
               .Select((x) => (x as TaxingIncomeHealthResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthDppsSum = healthDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthLowsSum = healthLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthStatSum = healthStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal healthRestSum = healthRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthDppsRes = RoundToInt(healthDppsSum);
            Int32 healthLowsRes = RoundToInt(healthLowsSum);
            Int32 healthStatRes = RoundToInt(healthStatSum);
            Int32 healthRestRes = RoundToInt(healthRestSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK,
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes))
            {
                baseValue += healthDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS,
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes))
            {
                baseValue += healthLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART,
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes))
            {
                baseValue += healthStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes))
            {
                baseValue += healthRestRes;
            }

            Int32 compoundPayment = healthRules.RoundedCompoundPaym(baseValue);
            Int32 employeePayment = healthRules.RoundedEmployeePaym(baseValue);
            Int32 employerPayment = healthRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingWithholdHealthResult(target, spec,
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }

    }

    // TaxingWithholdSocial			TAXING_WITHHOLD_SOCIAL
    class TaxingWithholdSocialConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_WITHHOLD_SOCIAL;
        public TaxingWithholdSocialConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2014)
            {
                return new TaxingWithholdSocialConSpec(this.Code.Value);
            }
            return new TaxingWithholdSocialCon2010Spec(this.Code.Value);
        }
    }

    class TaxingWithholdSocialCon2010Spec : PayrolexConceptSpec
    {
        public TaxingWithholdSocialCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL,
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
                new TaxingWithholdSocialTarget(month, con, pos, var, article, this.Code, 0),
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

            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdSocialTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdSocialTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            var socialList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialSum = socialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialRes = RoundToInt(socialSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRes))
            {
                baseValue = socialRes;
            }

            Int32 employerPayment = socialRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingWithholdSocialResult(target, spec, 
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingWithholdSocialConSpec : PayrolexConceptSpec
    {
        public TaxingWithholdSocialConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL,
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
                new TaxingWithholdSocialTarget(month, con, pos, var, article, this.Code, 0),
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

            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdSocialTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdSocialTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;
            var evalNoneSignOpts = evalTaxSigning.NoneSignOpts;

            var incomeDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var incomeRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeDppsSum = incomeDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeLowsSum = incomeLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeStatSum = incomeStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal incomeRestSum = incomeRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeDppsRes = RoundToInt(incomeDppsSum);
            Int32 incomeLowsRes = RoundToInt(incomeLowsSum);
            Int32 incomeStatRes = RoundToInt(incomeStatSum);
            Int32 incomeRestRes = RoundToInt(incomeRestSum);

            var socialDppsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_AGREEM_TASK))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialLowsList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialStatList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_STATUT_PART))
               .Select((r) => (r.ResultValue)).ToArray();

            var socialRestList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL))
               .Select((x) => (x as TaxingIncomeSocialResult))
               .Where((v) => (v is not null && v.SubjectType == WorkTaxingTerms.TAXING_TERM_BY_CONTRACT))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialDppsSum = socialDppsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialLowsSum = socialLowsList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialStatSum = socialStatList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));
            decimal socialRestSum = socialRestList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialDppsRes = RoundToInt(socialDppsSum);
            Int32 socialLowsRes = RoundToInt(socialLowsSum);
            Int32 socialStatRes = RoundToInt(socialStatSum);
            Int32 socialRestRes = RoundToInt(socialRestSum);

            Int32 baseValue = 0;
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_AGREEM_TASK,
                evalDeclSignOpts, evalNoneSignOpts, incomeDppsRes))
            {
                baseValue += socialDppsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_EMPLOYMENTS,
                evalDeclSignOpts, evalNoneSignOpts, incomeLowsRes))
            {
                baseValue += socialLowsRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_STATUT_PART,
                evalDeclSignOpts, evalNoneSignOpts, incomeStatRes))
            {
                baseValue += socialStatRes;
            }
            if (taxingRules.HasWithholdIncome(WorkTaxingTerms.TAXING_TERM_BY_CONTRACT,
                evalDeclSignOpts, evalNoneSignOpts, incomeRestRes))
            {
                baseValue += socialRestRes;
            }

            Int32 employerPayment = socialRules.RoundedEmployerPaym(baseValue);

            ITermResult resultsValues = new TaxingWithholdSocialResult(target, spec, 
                employerPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingWithholdBasis			TAXING_WITHHOLD_BASIS
    class TaxingWithholdBasisConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_WITHHOLD_BASIS;
        public TaxingWithholdBasisConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            if (period.Year >= 2021)
            {
                return new TaxingWithholdBasisConSpec(this.Code.Value);
            }
            return new TaxingWithholdBasisCon2010Spec(this.Code.Value);
        }
    }

    class TaxingWithholdBasisCon2010Spec : PayrolexConceptSpec
    {
        public TaxingWithholdBasisCon2010Spec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_INCOME,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_HEALTH,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_SOCIAL,
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
                new TaxingWithholdBasisTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdBasisTarget evalTarget = resTarget.Value;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_INCOME))
               .Select((x) => (x as TaxingWithholdIncomeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            var healthList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_HEALTH))
               .Select((x) => (x as TaxingWithholdHealthResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal healthSum = healthList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 healthRes = RoundToInt(healthSum);

            var socialList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_SOCIAL))
               .Select((x) => (x as TaxingWithholdSocialResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal socialSum = socialList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 socialRes = RoundToInt(socialSum);

            Int32 taxableSuper = incomeRes + healthRes + socialRes;

            Int32 withholdBase = taxingRules.RoundedBaseWithhold(taxableSuper);

            ITermResult resultsValues = new TaxingWithholdBasisResult(target, spec, withholdBase, taxableSuper);

            return BuildOkResults(resultsValues);
        }
    }

    class TaxingWithholdBasisConSpec : PayrolexConceptSpec
    {
        public TaxingWithholdBasisConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_INCOME,
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
                new TaxingWithholdBasisTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdBasisTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdBasisTarget evalTarget = resTarget.Value;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_INCOME))
               .Select((x) => (x as TaxingWithholdIncomeResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 incomeRes = RoundToInt(incomeSum);

            Int32 taxableBasis = incomeRes;

            Int32 withholdBase = taxingRules.RoundedBaseWithhold(taxableBasis);

            ITermResult resultsValues = new TaxingWithholdBasisResult(target, spec, withholdBase, taxableBasis);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingWithholdTotal			TAXING_WITHHOLD_TOTAL
    class TaxingWithholdTotalConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_WITHHOLD_TOTAL;
        public TaxingWithholdTotalConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingWithholdTotalConSpec(this.Code.Value);
        }
    }

    class TaxingWithholdTotalConSpec : PayrolexConceptSpec
    {
        public TaxingWithholdTotalConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_BASIS,
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
                new TaxingWithholdTotalTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingWithholdTotalTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingWithholdTotalTarget evalTarget = resTarget.Value;

            var resBaseVal = GetContractResult<TaxingWithholdBasisResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_BASIS));

            if (resBaseVal.IsFailure)
            {
                return BuildFailResults(resBaseVal.Error);
            }

            var evalBaseVal = resBaseVal.Value;

            Int32 withholdBasis = evalBaseVal.ResultBasis;

            Int32 withholdSuper = evalBaseVal.ResultValue;

            Int32 withholdPaym = taxingRules.RoundedWithholdPaym(withholdSuper, withholdBasis);

            ITermResult resultsValues = new TaxingWithholdTotalResult(target, spec, withholdPaym, withholdSuper);

            return BuildOkResults(resultsValues);
        }
    }
    // TaxingAllowancePayer			TAXING_ALLOWANCE_PAYER
    class TaxingAllowancePayerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_PAYER;
        public TaxingAllowancePayerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingAllowancePayerConSpec(this.Code.Value);
        }
    }

    class TaxingAllowancePayerConSpec : PayrolexConceptSpec
    {
        public TaxingAllowancePayerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            return Array.Empty<ITermTarget>();
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            Int32 allowancePayer = taxingRules.AllowancePayer;

            var resTarget = GetTypedTarget<TaxingAllowancePayerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAllowancePayerTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;

            Int32 benefitValue = taxingRules.BenefitAllowancePayer(evalDeclSignOpts, evalTarget.BenefitApply);

            ITermResult resultsValues = new TaxingAllowancePayerResult(target, spec,
                evalTarget.BenefitApply, benefitValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAllowanceChild			TAXING_ALLOWANCE_CHILD
    class TaxingAllowanceChildConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_CHILD;
        public TaxingAllowanceChildConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingAllowanceChildConSpec(this.Code.Value);
        }
    }

    class TaxingAllowanceChildConSpec : PayrolexConceptSpec
    {
        public TaxingAllowanceChildConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            return Array.Empty<ITermTarget>();
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAllowanceChildTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAllowanceChildTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;

            Int32 benefitValue = taxingRules.BenefitAllowanceChild(evalDeclSignOpts, 
                evalTarget.BenefitApply, evalTarget.BenefitOrder, evalTarget.BenefitDisab);

            ITermResult resultsValues = new TaxingAllowanceChildResult(target, spec,
                evalTarget.BenefitApply, evalTarget.BenefitDisab, evalTarget.BenefitOrder, benefitValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAllowanceDisab			TAXING_ALLOWANCE_DISAB
    class TaxingAllowanceDisabConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_DISAB;
        public TaxingAllowanceDisabConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingAllowanceDisabConSpec(this.Code.Value);
        }
    }

    class TaxingAllowanceDisabConSpec : PayrolexConceptSpec
    {
        public TaxingAllowanceDisabConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            return Array.Empty<ITermTarget>();
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAllowanceDisabTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAllowanceDisabTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;

            Int32 benefitValue = taxingRules.BenefitAllowanceDisab(evalDeclSignOpts, evalTarget.BenefitApply);

            ITermResult resultsValues = new TaxingAllowanceDisabResult(target, spec,
                evalTarget.BenefitApply, benefitValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingAllowanceStudy			TAXING_ALLOWANCE_STUDY
    class TaxingAllowanceStudyConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_STUDY;
        public TaxingAllowanceStudyConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingAllowanceStudyConSpec(this.Code.Value);
        }
    }

    class TaxingAllowanceStudyConSpec : PayrolexConceptSpec
    {
        public TaxingAllowanceStudyConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING,
            });

            ResultDelegate = ConceptEval;
        }

        public override IEnumerable<ITermTarget> DefaultTargetList(ArticleCode article, IPeriod period, IBundleProps ruleset, MonthCode month, IEnumerable<IContractTerm> conTerms, IEnumerable<IPositionTerm> posTerms, IEnumerable<ITermTarget> targets, VariantCode var)
        {
            return Array.Empty<ITermTarget>();
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingAllowanceStudyTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingAllowanceStudyTarget evalTarget = resTarget.Value;

            var resTaxSigning = GetContractResult<TaxingSigningResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING));

            if (resTaxSigning.IsFailure)
            {
                return BuildFailResults(resTaxSigning.Error);
            }

            var evalTaxSigning = resTaxSigning.Value;

            var evalDeclSignOpts = evalTaxSigning.DeclSignOpts;

            Int32 benefitValue = taxingRules.BenefitAllowanceStudy(evalDeclSignOpts, evalTarget.BenefitApply);

            ITermResult resultsValues = new TaxingAllowanceStudyResult(target, spec, 
                evalTarget.BenefitApply, benefitValue, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingRebatePayer			TAXING_REBATE_PAYER
    class TaxingRebatePayerConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_REBATE_PAYER;
        public TaxingRebatePayerConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingRebatePayerConSpec(this.Code.Value);
        }
    }

    class TaxingRebatePayerConSpec : PayrolexConceptSpec
    {
        public TaxingRebatePayerConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_PAYER,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_STUDY,
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
                new TaxingRebatePayerTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingRebatePayerTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingRebatePayerTarget evalTarget = resTarget.Value;

            var resAdvancesTotal = GetContractResult<TaxingAdvancesTotalResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL));

            if (resAdvancesTotal.IsFailure)
            {
                return BuildFailResults(resAdvancesTotal.Error);
            }

            var evalAdvancesTotal = resAdvancesTotal.Value;

            var rebatePayerList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_PAYER))
               .Select((x) => (x as TaxingAllowancePayerResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebatePayerSum = rebatePayerList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            var rebateDisabList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB))
               .Select((x) => (x as TaxingAllowanceDisabResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebateDisabSum = rebateDisabList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            var rebateStudyList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_STUDY))
               .Select((x) => (x as TaxingAllowanceStudyResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebateStudySum = rebateStudyList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 allowancePayer = RoundToInt(rebatePayerSum);
            Int32 allowanceDisab = RoundToInt(rebateDisabSum);
            Int32 allowanceStudy = RoundToInt(rebateStudySum);

            Int32 advancesTotal = evalAdvancesTotal.ResultValue;

            Int32 taxPayerRebat = TaxPayerRebate(advancesTotal, allowancePayer + allowanceDisab + allowanceStudy, 0);

            ITermResult resultsValues = new TaxingRebatePayerResult(target, spec, taxPayerRebat, 0);

            return BuildOkResults(resultsValues);
        }
        private Int32 TaxPayerRebate(Int32 taxingBasis, Int32 taxingAllow, Int32 taxingRebat)
        {
            decimal taxAfterRebat = decimal.Subtract(taxingBasis, taxingRebat);
            decimal taxRebatValue = decimal.Subtract(taxingAllow,
                Math.Max(0m, decimal.Subtract(taxingAllow, taxAfterRebat)));

            return RoundToInt(taxRebatValue);
        }
    }

    // TaxingRebateChild			TAXING_REBATE_CHILD
    class TaxingRebateChildConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_REBATE_CHILD;
        public TaxingRebateChildConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingRebateChildConSpec(this.Code.Value);
        }
    }

    class TaxingRebateChildConSpec : PayrolexConceptSpec
    {
        public TaxingRebateChildConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL,
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
                new TaxingRebateChildTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingRebateChildTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingRebateChildTarget evalTarget = resTarget.Value;

            var resAdvancesTotal = GetContractResult<TaxingAdvancesTotalResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL));

            if (resAdvancesTotal.IsFailure)
            {
                return BuildFailResults(resAdvancesTotal.Error);
            }

            var evalAdvancesTotal = resAdvancesTotal.Value;

            var resRebPayerTotal = GetContractResult<TaxingRebatePayerResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER));

            if (resRebPayerTotal.IsFailure)
            {
                return BuildFailResults(resRebPayerTotal.Error);
            }

            var evalRebPayerTotal = resRebPayerTotal.Value;

            var rebateChildList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD))
               .Select((x) => (x as TaxingAllowanceChildResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebateChildSum = rebateChildList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 allowanceChild = RoundToInt(rebateChildSum);

            Int32 rebPayerTotal = evalRebPayerTotal.ResultValue;

            Int32 advancesTotal = evalAdvancesTotal.ResultValue;

            Int32 taxChildRebat = TaxChildRebate(advancesTotal, allowanceChild, rebPayerTotal);

            ITermResult resultsValues = new TaxingRebateChildResult(target, spec, taxChildRebat, allowanceChild);

            return BuildOkResults(resultsValues);
        }
        private Int32 TaxChildRebate(Int32 taxingBasis, Int32 taxingAllow, Int32 taxingRebat)
        {
            decimal taxAfterRebat = decimal.Subtract(taxingBasis, taxingRebat);
            decimal taxRebatValue = decimal.Subtract(taxingAllow,
                Math.Max(0m, decimal.Subtract(taxingAllow, taxAfterRebat)));

            return RoundToInt(taxRebatValue);
        }
    }

    // TaxingBonusChild			TAXING_BONUS_CHILD
    class TaxingBonusChildConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_BONUS_CHILD;
        public TaxingBonusChildConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingBonusChildConSpec(this.Code.Value);
        }
    }

    class TaxingBonusChildConSpec : PayrolexConceptSpec
    {
        public TaxingBonusChildConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_CHILD,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT,
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
                new TaxingBonusChildTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resPrTaxing = GetTaxingPropsResult(ruleset, target, period);
            if (resPrTaxing.IsFailure)
            {
                return BuildFailResults(resPrTaxing.Error);
            }
            IPropsTaxing taxingRules = resPrTaxing.Value;

            var resTarget = GetTypedTarget<TaxingBonusChildTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingBonusChildTarget evalTarget = resTarget.Value;

            var resAdvancesTotal = GetContractResult<TaxingAdvancesTotalResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL));

            if (resAdvancesTotal.IsFailure)
            {
                return BuildFailResults(resAdvancesTotal.Error);
            }

            var evalAdvancesTotal = resAdvancesTotal.Value;

            var resRebPayerTotal = GetContractResult<TaxingRebatePayerResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER));

            if (resRebPayerTotal.IsFailure)
            {
                return BuildFailResults(resRebPayerTotal.Error);
            }

            var evalRebPayerTotal = resRebPayerTotal.Value;

            var resRebChildTotal = GetContractResult<TaxingRebateChildResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_CHILD));

            if (resRebChildTotal.IsFailure)
            {
                return BuildFailResults(resRebChildTotal.Error);
            }

            var evalRebChildTotal = resRebChildTotal.Value;

            var incomeList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT))
               .Select((x) => (x as TaxingIncomeSubjectResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal incomeSum = incomeList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 rebateChild = evalRebChildTotal.ResultValue;
            Int32 summarChild = evalRebChildTotal.ResultBasis;

            Int32 incomeTaxs = RoundToInt(incomeSum);

            Int32 rawBonusValue = taxingRules.BonusChildRaw(incomeTaxs, summarChild, rebateChild);

            Int32 fixBonusValue = taxingRules.BonusChildFix(incomeTaxs, summarChild, rebateChild);

            ITermResult resultsValues = new TaxingBonusChildResult(target, spec, fixBonusValue, rawBonusValue);

            return BuildOkResults(resultsValues);
        }
    }
    // TaxingPaymAdvances			TAXING_PAYM_ADVANCES
    class TaxingPaymAdvancesConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_PAYM_ADVANCES;
        public TaxingPaymAdvancesConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingPaymAdvancesConSpec(this.Code.Value);
        }
    }

    class TaxingPaymAdvancesConSpec : PayrolexConceptSpec
    {
        public TaxingPaymAdvancesConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER,
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_CHILD,
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
                new TaxingPaymAdvancesTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingPaymAdvancesTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingPaymAdvancesTarget evalTarget = resTarget.Value;

            var resAdvancesTotal = GetContractResult<TaxingAdvancesTotalResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL));

            if (resAdvancesTotal.IsFailure)
            {
                return BuildFailResults(resAdvancesTotal.Error);
            }

            var evalAdvancesTotal = resAdvancesTotal.Value;

            var rebatePayerList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER))
               .Select((x) => (x as TaxingRebatePayerResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebatePayerSum = rebatePayerList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 rebatePayer = RoundToInt(rebatePayerSum);

            var rebateChildList = results
               .Where((x) => (x.IsSuccess)).Select((r) => (r.Value))
               .Where((v) => (v.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_TAXING_REBATE_CHILD))
               .Select((x) => (x as TaxingRebateChildResult))
               .Where((v) => (v is not null))
               .Select((r) => (r.ResultValue)).ToArray();

            decimal rebateChildSum = rebateChildList.Aggregate(decimal.Zero,
                (agr, item) => decimal.Add(agr, item));

            Int32 rebateChild = RoundToInt(rebateChildSum);

            Int32 advancesPayment = (evalAdvancesTotal.ResultValue - rebatePayer - rebateChild);

            ITermResult resultsValues = new TaxingPaymAdvancesResult(target, spec, advancesPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }

    // TaxingPaymWithhold			TAXING_PAYM_WITHHOLD
    class TaxingPaymWithholdConProv : ConceptSpecProvider
    {
        const Int32 CONCEPT_CODE = (Int32)PayrolexConceptConst.CONCEPT_TAXING_PAYM_WITHHOLD;
        public TaxingPaymWithholdConProv() : base(CONCEPT_CODE)
        {
        }

        public override IConceptSpec GetSpec(IPeriod period, VersionCode version)
        {
            return new TaxingPaymWithholdConSpec(this.Code.Value);
        }
    }

    class TaxingPaymWithholdConSpec : PayrolexConceptSpec
    {
        public TaxingPaymWithholdConSpec(Int32 code) : base(code)
        {
            Path = ConceptSpec.ConstToPathArray(new List<Int32>() {
                (Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_TOTAL,
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
                new TaxingPaymWithholdTarget(month, con, pos, var, article, this.Code, 0),
            };
        }

        private IList<Result<ITermResult, ITermResultError>> ConceptEval(ITermTarget target, IArticleSpec spec, IPeriod period, IBundleProps ruleset, IList<Result<ITermResult, ITermResultError>> results)
        {
            var resTarget = GetTypedTarget<TaxingPaymWithholdTarget>(target, period);
            if (resTarget.IsFailure)
            {
                return BuildFailResults(resTarget.Error);
            }
            TaxingPaymWithholdTarget evalTarget = resTarget.Value;

            var resWithholdTotal = GetContractResult<TaxingWithholdTotalResult>(target, period, results,
                target.Contract, ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_TOTAL));

            if (resWithholdTotal.IsFailure)
            {
                return BuildFailResults(resWithholdTotal.Error);
            }

            var evalWithholdTotal = resWithholdTotal.Value;

            Int32 withholdPayment = evalWithholdTotal.ResultValue;

            ITermResult resultsValues = new TaxingPaymWithholdResult(target, spec, withholdPayment, 0);

            return BuildOkResults(resultsValues);
        }
    }
}
