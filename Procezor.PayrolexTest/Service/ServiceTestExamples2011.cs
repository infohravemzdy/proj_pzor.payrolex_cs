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
using HraveMzdy.Legalios.Service;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Procezor.Service;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Procezor.Payrolex.Service;
using HraveMzdy.Procezor.Payrolex.Generator;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Payrolex.Registry.Providers;

namespace Procezor.PayrolexTest.Service
{
    public class ServiceTestExamples2011 : ServiceTestExampleTemplate
    {
        private static IPeriod TestPeriod = new Period(2011,1);
        private static Int32 TestPeriodCode = 201101;
        private static Int32 PrevPeriodCode = 201001;
        public static IEnumerable<object[]> GenTestData => GetGenTestDecData(_genTests, TestPeriod, TestPeriodCode, PrevPeriodCode);

        public ServiceTestExamples2011(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public void ServiceExamples_CreateImport()
        {
            ServiceExamplesCreateImport(_genTests, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_101_PPomMzdaDanPojSlevyZakladTest()
        {
            ExampleGenerator example = Example_101_PPomMzdaDanPojSlevyZaklad();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_105_PPomMzdaDanMaxBonusTest()
        {
            ExampleGenerator example = Example_105_PPomMzdaDanMaxBonus();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_108_PPomMzdaDanMaxZdravPrevTest()
        {
            ExampleGenerator example = Example_108_PPomMzdaDanMaxZdravPrev();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_109_PPomMzdaDanMaxZdravCurrTest()
        {
            ExampleGenerator example = Example_109_PPomMzdaDanMaxZdravCurr();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_193_PPomMzdaNepodPojZaporPlatTest()
        {
            ExampleGenerator example = Example_193_PPomMzdaNepodPojZaporPlat();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_201_PPomMzdaNepodPojPrevLoTest()
        {
            ExampleGenerator example = Example_201_PPomMzdaNepodPojPrevLo();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_301_PPomMzdaDanPojDan099Test()
        {
            ExampleGenerator example = Example_301_PPomMzdaDanPojDan099();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_501_PPomMzdaNeUcastZdravPrevTest()
        {
            ExampleGenerator example = Example_501_PPomMzdaNeUcastZdravPrev();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_502_PPomMzdaUcastZdravPrevTest()
        {
            ExampleGenerator example = Example_502_PPomMzdaUcastZdravPrev();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_601_PPomMzdaNeUcastNemocPrevTest()
        {
            ExampleGenerator example = Example_601_PPomMzdaNeUcastNemocPrev();
            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_701_PPomMzdaNeUcastNemocTest()
        {
            ExampleGenerator example = Example_701_PPomMzdaNeUcastNemoc();

            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_751_Mpom2_PPomMzdaMinZdravTest()
        {
            ExampleGenerator example = Example_751_Mpom2_PPomMzdaMinZdrav();

            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_753_XDpp2_PPomMzdaMinZdravTest()
        {
            ExampleGenerator example = Example_753_XDpp2_PPomMzdaMinZdrav();

            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_763_XDpp2_PPomMzdaMaxZdravTest()
        {
            ExampleGenerator example = Example_763_XDpp2_PPomMzdaMaxZdrav();

            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Fact]
        public void ServiceExamples_773_XDpp2_PPomMzdaMaxSocialTest()
        {
            ExampleGenerator example = Example_773_XDpp2_PPomMzdaMaxSocial();

            ServiceExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }

        [Theory]
        [MemberData(nameof(GenTestData))]
        public void ServiceExamplesTest(ExampleGenerator example)
        {
            ServiceTemplateExampleTest(example, TestPeriod, TestPeriodCode, PrevPeriodCode);
        }
    }
}
