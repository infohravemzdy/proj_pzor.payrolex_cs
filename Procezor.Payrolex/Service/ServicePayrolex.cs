using System;
using System.Collections.Generic;
using System.Linq;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Procezor.Service;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Procezor.Payrolex.Registry.Factories;
using HraveMzdy.Procezor.Payrolex.Registry.Providers;
using HraveMzdy.Legalios.Service.Types;

namespace HraveMzdy.Procezor.Payrolex.Service
{
    public class ServicePayrolex : ServiceProcezor
    {
        public const Int32 TEST_VERSION = 100;

        private static readonly IList<ArticleCode> TEST_FINAL_DEFS = new List<ArticleCode>() {
            ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_EMPLOYER_COSTS),
            ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_TIME_WORK),
            ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_TIME_ABSC),
            ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_INCOME_NETTO),
        };

        public ServicePayrolex() : base(TEST_VERSION, TEST_FINAL_DEFS)
        {
        }

        public override IEnumerable<IContractTerm> GetContractTerms(IPeriod period, IEnumerable<ITermTarget> targets)
        {
            return targets.Where((t) => (t.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM))
                .Select((x) => (x as ContractWorkTermTarget))
                .Where((x) => (x != null))
                .Select((x) => new ContractTerm()
                {
                    Contract = x.Contract,
                    DateFrom = x.DateFrom,
                    DateStop = x.DateStop,
                    TermDayFrom = OperationsPeriod.DateFromInPeriod(period, x.DateFrom),
                    TermDayStop = OperationsPeriod.DateStopInPeriod(period, x.DateStop),
                }).ToList();
        }

        public override IEnumerable<IPositionTerm> GetPositionTerms(IPeriod period, IEnumerable<IContractTerm> contracts, IEnumerable<ITermTarget> targets)
        {
            return targets.Where((t) => (t.Article.Value == (Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM))
                .Select((x) => (x as PositionWorkTermTarget))
                .Where((x) => (x != null))
                .Select((x) => new PositionTerm()
                {
                    BaseTerm = contracts.FirstOrDefault((c) => (c.Contract.Equals(x.Contract))),
                    Contract = x.Contract,
                    Position = x.Position,
                    DateFrom = x.DateFrom,
                    DateStop = x.DateStop,
                    TermDayFrom = OperationsPeriod.DateFromInPeriod(period, x.DateFrom),
                    TermDayStop = OperationsPeriod.DateStopInPeriod(period, x.DateStop),
                }).ToList();
        }

        protected override bool BuildArticleFactory()
        {
            ArticleFactory = new ServiceArticleFactory();

            return true;
        }

        protected override bool BuildConceptFactory()
        {
            ConceptFactory = new ServiceConceptFactory();

            return true;
        }
    }

}
