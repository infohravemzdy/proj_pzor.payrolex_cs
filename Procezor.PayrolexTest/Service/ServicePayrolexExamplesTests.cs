using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;
using HraveMzdy.Procezor.Payrolex.Service;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Providers;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Legalios.Service;

namespace Procezor.PayrolexTest.Service
{
    public class ServicePayrolexExamplesTests
    {
        private readonly ITestOutputHelper output;

        private readonly ServicePayrolex _sut;
        private readonly ServiceLegalios _leg;

        public ServicePayrolexExamplesTests(ITestOutputHelper output)
        {
            this.output = output;

            this._sut = new ServicePayrolex();
            this._leg = new ServiceLegalios();
        }
        static IEnumerable<ITermTarget> GetTargetsWithSalaryHomeOffice(IPeriod period)
        {
            const Int16 CONTRACT_CODE = 0;
            const Int16 POSITION_CODE = 0;

            var montCode = MonthCode.Get(period.Code);
            var contract = ContractCode.Get(CONTRACT_CODE);
            var position = PositionCode.Get(POSITION_CODE);
            var variant1 = VariantCode.Get(1);

            DateTime? dateTermFrom = new DateTime(2021, 1, 1);
            DateTime? dateTermStop = null;

            var targets = new TermTarget[] {
                new ContractWorkTermTarget(montCode, contract, position, variant1,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_CONTRACT_WORK_TERM), 
                    WorkContractTerms.WORKTERM_EMPLOYMENT_1, dateTermFrom, dateTermStop),
                new PositionWorkTermTarget(montCode, contract, position, variant1,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_TERM),
                    "position one", dateTermFrom, dateTermStop),
                new PositionWorkPlanTarget(montCode, contract, position, variant1,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_PLAN),
                    WorkScheduleType.SCHEDULE_NORMALY_WEEK, 5, 8, 8),
                new PaymentBasisTarget(montCode, contract, position, variant1,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_PAYMENT_SALARY),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_PAYMENT_BASIS),
                    10000),
            };
            return targets;
        }
        [Fact]
        public void ServiceTest()
        {
            var testVersion = _sut.Version;
            testVersion.Value.Should().Be(100);

            var testPeriod = new Period(2021, 1);
            testPeriod.Code.Should().Be(202101);

            var testLegalResult = _leg.GetBundle(testPeriod);
            testLegalResult.IsSuccess.Should().Be(true);

            IBundleProps testLegal = testLegalResult.Value;

            var initService = _sut.InitWithPeriod(testPeriod);
            initService.Should().BeTrue();

            var restTargets = GetTargetsWithSalaryHomeOffice(testPeriod);
            var restService = _sut.GetResults(testPeriod, testLegal, restTargets);
            restService.Count().Should().NotBe(0);

            foreach (var (result, index) in restService.Select((item, index) => (item, index)))
            {
                if (result.IsSuccess)
                {
                    var resultValue = result.Value as PayrolexTermResult;
                    var articleSymbol = resultValue.ArticleDescr();
                    var conceptSymbol = resultValue.ConceptDescr();
                    output.WriteLine("Index: {0}; ART: {1}; CON: {2}; Result: {3}", index, articleSymbol, conceptSymbol, resultValue.ResultMessage());
                }
                else if (result.IsFailure)
                {
                    var errorValue = result.Error;
                    var articleSymbol = errorValue.ArticleDescr();
                    var conceptSymbol = errorValue.ConceptDescr();
                    output.WriteLine("Index: {0}; ART: {1}; CON: {2}; Error: {3}", index, articleSymbol, conceptSymbol, errorValue.Description());
                }
            }
        }
    }
}
