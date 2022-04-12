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
using System.IO;

namespace Procezor.PayrolexTest.Service
{
    public class ServiceTestExampleTemplate
    {
#if __MACOS__
        public const string PROTOKOL_TEST_FOLDER = "../../../test_import";
#else
        public const string PROTOKOL_TEST_FOLDER = "..\\..\\..\\test_import";
#endif
        public const string PROTOKOL_FOLDER_NAME = "test_import";
        public const string PARENT_PROTOKOL_FOLDER_NAME = "Procezor.PayrolexTest";

        protected const string TestFolder = PROTOKOL_TEST_FOLDER;

        public ServiceTestExampleTemplate(ITestOutputHelper output)
        {
            this.output = output;

            this._sut = new ServicePayrolex();
            this._leg = new ServiceLegalios();
        }
        public static IPeriod PrevYear(IPeriod period)
        {
            return new Period(Math.Max(2010, period.Year - 1), period.Month);
        }
        public static IBundleProps CurrYearBundle(IServiceLegalios legSvc, IPeriod period)
        {
            var legResult = legSvc.GetBundle(period);
            return legResult.Value;
        }
        public static IBundleProps PrevYearBundle(IServiceLegalios legSvc, IPeriod period)
        {
            var legResult = legSvc.GetBundle(PrevYear(period));
            return legResult.Value;
        }

        protected static StreamWriter CreateProtokolFile(string fileName)
        {
            string filePath = Path.GetFullPath(Path.Combine(TestFolder, fileName));

            string currPath = Path.GetFullPath(".");
            int nameCount = currPath.Split(Path.DirectorySeparatorChar).Length;

            while (!currPath.EndsWith(PARENT_PROTOKOL_FOLDER_NAME) && nameCount != 1)
            {
                currPath = Path.GetDirectoryName(currPath);
            }
            string basePath = Path.Combine(currPath, PROTOKOL_FOLDER_NAME);
            if (nameCount <= 1)
            {
                basePath = Path.Combine(Path.GetFullPath("."), PROTOKOL_FOLDER_NAME);
            }
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            filePath = Path.Combine(basePath, fileName);
            FileStream fileStream = new FileInfo(filePath).Create();
            return new StreamWriter(fileStream, System.Text.Encoding.GetEncoding("windows-1250"));
        }
        protected static StreamWriter OpenProtokolFile(string fileName)
        {
            string filePath = Path.GetFullPath(Path.Combine(TestFolder, fileName));

            string currPath = Path.GetFullPath(".");
            int nameCount = currPath.Split(Path.DirectorySeparatorChar).Length;

            while (!currPath.EndsWith(PARENT_PROTOKOL_FOLDER_NAME) && nameCount != 1)
            {
                currPath = Path.GetDirectoryName(currPath);
            }
            string basePath = Path.Combine(currPath, PROTOKOL_FOLDER_NAME);
            if (nameCount <= 1)
            {
                basePath = Path.Combine(Path.GetFullPath("."), PROTOKOL_FOLDER_NAME);
            }
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            filePath = Path.Combine(basePath, fileName);
            FileStream fileStream = new FileInfo(filePath).Open(FileMode.Append, FileAccess.Write);
            return new StreamWriter(fileStream, System.Text.Encoding.GetEncoding("windows-1250"));
        }
        protected static void ExportPropsStart(StreamWriter protokol)
        {
        }

        protected static void ExportPropsEnd(StreamWriter protokol)
        {
        }

        protected readonly ITestOutputHelper output;

        protected readonly IServiceProcezor _sut;
        protected readonly IServiceLegalios _leg;

        protected static readonly ExampleGenerator[] _genTests = new ExampleGenerator[] {
            ExampleGenerator.Spec(101, "PP-Mzda_DanPoj-SlevyZaklad",      "101").WithContracts(ContractGenerator.SpecEmp(1)),
            ExampleGenerator.Spec(102, "PP-Mzda_DanPoj-SlevyDite1",       "102").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(15600)).WithChild(ChildGenerator.SpecDisb1(1)), //, CZK 15600    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 1,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(103, "PP-Mzda_DanPoj-BonusDite1",       "103").WithContracts(ContractGenerator.SpecEmp(1)).WithChild(ChildGenerator.SpecDisb1(1)), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 1,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(104, "PP-Mzda_DanPoj-BonusDite2",       "104").WithContracts(ContractGenerator.SpecEmp(1)).WithChild(ChildGenerator.SpecDisb(1, 1, 0)), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 2,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(105, "PP-Mzda_DanPoj-MaxBonus",         "105").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MaxBonus())).WithChild(ChildGenerator.SpecDisb(1, 1, 5)), //, CZK 10000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 7,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(106, "PP-Mzda_DanPoj-MinZdravPrev",     "106").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MinZdrPrev(-200))), //, CZK 7800     ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(107, "PP-Mzda_DanPoj-MinZdravCurr",     "107").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MinZdr(-200))), //, CZK 7800     ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(108, "PP-Mzda_DanPoj-MaxZdravPrev",     "108").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MaxZdrPrev(100))), //, CZK 1809964  ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(109, "PP-Mzda_DanPoj-MaxZdravCurr",     "109").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MaxZdr(100))), //, CZK 1809964  ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(110, "PP-Mzda_DanPoj-MaxSocialPrev",    "110").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MaxSocPrev(100))), //, CZK 1206676  ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(111, "PP-Mzda_DanPoj-MaxSocialCurr",    "111").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(MaxSoc(100))), //, CZK 1242532  ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(112, "PP-Mzda_DanPoj-DuchSpor",         "112").WithContracts(ContractGenerator.SpecEmp(1)), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(113, "PP-Mzda_DanPoj-SlevyInv1",        "113").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(20000)).WithBenDisab1Val(1), //, CZK 20000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  YES, NO, NO              ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(114, "PP-Mzda_DanPoj-SlevyInv2",        "114").WithContracts(ContractGenerator.SpecEmp(1)).WithBenDisab2Val(1), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, YES, NO              ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(115, "PP-Mzda_DanPoj-SlevyInv3",        "115").WithContracts(ContractGenerator.SpecEmp(1)).WithBenDisab3Val(1), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, YES, NO              ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(120, "PP-Mzda_DanPoj-SlevyStud",        "120").WithContracts(ContractGenerator.SpecEmp(1)).WithBenStudyVal(1), //, CZK 15000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  YES                  ,  YES             ,  YES             , 
            ExampleGenerator.Spec(121, "PP-Mzda_DanPoj-SlevyZakl020",     "121").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(20000 )), //, CZK 20000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(122, "PP-Mzda_DanPoj-SlevyZakl025",     "122").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(25000 )), //, CZK 25000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(123, "PP-Mzda_DanPoj-SlevyZakl030",     "123").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(30000 )), //, CZK 30000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(124, "PP-Mzda_DanPoj-SlevyZakl035",     "124").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(35000 )), //, CZK 35000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(125, "PP-Mzda_DanPoj-SlevyZakl040",     "125").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(40000 )), //, CZK 40000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(126, "PP-Mzda_DanPoj-SlevyZakl045",     "126").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(45000 )), //, CZK 45000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(127, "PP-Mzda_DanPoj-SlevyZakl050",     "127").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(50000 )), //, CZK 50000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(128, "PP-Mzda_DanPoj-SlevyZakl055",     "128").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(55000 )), //, CZK 55000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(129, "PP-Mzda_DanPoj-SlevyZakl060",     "129").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(60000 )), //, CZK 60000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(130, "PP-Mzda_DanPoj-SlevyZakl065",     "130").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(65000 )), //, CZK 65000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(131, "PP-Mzda_DanPoj-SlevyZakl070",     "131").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(70000 )), //, CZK 70000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(132, "PP-Mzda_DanPoj-SlevyZakl075",     "132").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(75000 )), //, CZK 75000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(133, "PP-Mzda_DanPoj-SlevyZakl080",     "133").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(80000 )), //, CZK 80000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(134, "PP-Mzda_DanPoj-SlevyZakl085",     "134").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(85000 )), //, CZK 85000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(135, "PP-Mzda_DanPoj-SlevyZakl090",     "135").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(90000 )), //, CZK 90000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(136, "PP-Mzda_DanPoj-SlevyZakl095",     "136").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(95000 )), //, CZK 95000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(137, "PP-Mzda_DanPoj-SlevyZakl100",     "137").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(100000)), //, CZK 100000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(138, "PP-Mzda_DanPoj-SlevyZakl105",     "138").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(105000)), //, CZK 105000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(139, "PP-Mzda_DanPoj-SlevyZakl110",     "139").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(110000)), //, CZK 110000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(140, "PP-Mzda_DanPoj-TaxRate2DanPrevX", "140").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2TaxPrev(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(141, "PP-Mzda_DanPoj-TaxRate2DanPrev1", "141").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2TaxPrev(1000))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(142, "PP-Mzda_DanPoj-TaxRate2DanPrevY", "142").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2TaxPrev(1))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(143, "PP-Mzda_DanPoj-TaxRate2DanCurrX", "143").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2Tax(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(144, "PP-Mzda_DanPoj-TaxRate2DanCurr1", "144").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2Tax(1000))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(145, "PP-Mzda_DanPoj-TaxRate2DanCurrY", "145").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(Rate2Tax(1))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(150, "PP-Mzda_DanPoj-SolidarDanPrevX",  "150").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTaxPrev(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(151, "PP-Mzda_DanPoj-SolidarDanPrev1",  "151").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTaxPrev(1000))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(152, "PP-Mzda_DanPoj-SolidarDanPrevY",  "152").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTaxPrev(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(153, "PP-Mzda_DanPoj-SolidarDanCurrX",  "153").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTax(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(154, "PP-Mzda_DanPoj-SolidarDanCurr1",  "154").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTax(1000))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(155, "PP-Mzda_DanPoj-SolidarDanCurrY",  "155").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SolTax(0))), //, CZK 104536   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(190, "PP-Mzda_DanPoj-ZaporPlat",        "190").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(-10000)), //, CZK 110000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(191, "PP-Mzda_DanPoj-ZaporDohod",       "191").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(0).WithAgreemVal(-5000)), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(193, "PP-Mzda_NepodPoj-ZaporPlat",      "193").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(-10000)).WithTaxDeclVal(0), //, CZK 110000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(194, "PP-Mzda_NepodPoj-ZaporDohod",     "194").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(0).WithAgreemVal(-5000)).WithTaxDeclVal(0), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(201, "PP-Mzda_NepodPoj-PrevLo",         "201").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SrazNepPrev(0))).WithTaxDeclVal(0), //, CZK 5000     ,  YES       , NO,  YES          ,  YES          ,  YES          ,  NO            ,  NO                , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(202, "PP-Mzda_NepodPoj-PrevHi",         "202").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SrazNepPrev(1))).WithTaxDeclVal(0), //, CZK 5001     ,  YES       , NO,  YES          ,  YES          ,  YES          ,  NO            ,  NO                , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(203, "PP-Mzda_NepodPoj-CurrLo",         "203").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SrazNep(0))).WithTaxDeclVal(0), //, CZK 5000     ,  YES       , NO,  YES          ,  YES          ,  YES          ,  NO            ,  NO                , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(204, "PP-Mzda_NepodPoj-CurrHi",         "204").WithContracts(ContractGenerator.SpecEmp(1).WithSalary(SrazNep(1))).WithTaxDeclVal(0), //, CZK 5001     ,  YES       , NO,  YES          ,  YES          ,  YES          ,  NO            ,  NO                , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(301, "PP-Mzda_DanPoj-Dan099",           "301").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(0).WithAgreemVal(74).WithHealthMinimVal(0)), //, CZK 74       ,  YES          ,  YES          ,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(302, "PP-Mzda_DanPoj-Dan100",           "302").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(75).WithHealthMinimVal(0)), //, CZK 75       ,  YES          ,  YES          ,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(303, "PP-Mzda_DanPoj-Dan101",           "303").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(100).WithHealthMinimVal(0)), //, CZK 100      ,  YES          ,  YES          ,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(401, "PP-Mzda_DanPoj-Neodpr064",        "401").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(20000).WithAbsenceVal(46)), //, CZK 20000    ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(402, "PP-Mzda_DanPoj-Neodpr184",        "402").WithContracts(ContractGenerator.SpecEmp(1).WithSalaryVal(20000).WithHealthMinimVal(0).WithAbsenceVal(184)), //, CZK 20000    ,  YES          ,  YES          ,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(501, "DPC-Mzda_NeUcastZdrav-Prev",      "501").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdrPrev(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(502, "DPC-Mzda_UcastZdrav-Prev",        "502").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdrPrev(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(503, "DPC-Mzda_NeUcastNemoc-Prev",      "503").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastNemPrev(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(504, "DPC-Mzda_UcastNemoc-Prev",        "504").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastNemPrev(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(505, "DPP-Mzda_NeUcastZdrav-Prev",      "505").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdrEmpPrev(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(506, "DPP-Mzda_UcastZdrav-Prev",        "506").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdrEmpPrev(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(507, "DPC-Mzda_NeUcastZdrav-Curr",      "507").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdr(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(508, "DPC-Mzda_UcastZdrav-Curr",        "508").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastZdr(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(509, "DPC-Mzda_NeUcastNemoc-Curr",      "509").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastNem(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(510, "DPC-Mzda_UcastNemoc-Curr",        "510").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreem(UcastNem(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(511, "DPP-Mzda_NeUcastZdrav-Curr",      "511").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastZdrEmp(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(512, "DPP-Mzda_UcastZdrav-Curr",        "512").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastZdrEmp(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(551, "DPC-Mzda_DanPoj-ZaporDohod",      "551").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreemVal(-5000)), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(552, "DPP-Mzda_DanPoj-ZaporDohod",      "552").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreemVal(-5000)), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(553, "DPC-Mzda_NepodPoj-ZaporDohod",    "553").WithContracts(ContractGenerator.SpecDpc(1).WithSalaryVal(0).WithAgreemVal(-5000)).WithTaxDeclVal(0), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(554, "DPP-Mzda_NepodPoj-ZaporDohod",    "554").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreemVal(-5000)).WithTaxDeclVal(0), //, CZK -5000   ,  YES          ,  YES          ,  YES          ,  YES          ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(601, "DPP-Mzda_NeUcastNemoc-Prev",      "601").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastNemPrev(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(602, "DPP-Mzda_UcastNemoc-Prev",        "602").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastNemPrev(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(603, "DPP-Mzda_NeUcastNemoc-Curr",      "603").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastNem(-1)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 
            ExampleGenerator.Spec(604, "DPP-Mzda_UcastNemoc-Curr",        "604").WithContracts(ContractGenerator.SpecDpp(1).WithSalaryVal(0).WithAgreem(UcastNem(0)).WithHealthMinimVal(0)).WithTaxDeclVal(0), //,CZK 0,  YES       , NO,  YES          ,  NO           ,  YES          ,  NO            ,  YES               , 0,  NO, NO, NO               ,  NO                   ,  YES             ,  YES             , 

            ExampleGenerator.Spec(701, "MPOM-PPOM-Mzda_NeUcastNemoc",     "701").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(UcastNem(-1))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(702, "MDPC-PPOM-Mzda_NeUcastNemoc",     "702").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastNem(-1))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(703, "XDPP-PPOM-Mzda_NeUcastNemoc",     "703").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastNem(-1))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(711, "MPOM-PPOM-Mzda_UcastNemoc",     "711").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(UcastNem(0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(712, "MDPC-PPOM-Mzda_UcastNemoc",     "712").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastNem(0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(713, "XDPP-PPOM-Mzda_UcastNemoc",     "713").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastNem(0))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),

             ExampleGenerator.Spec(731, "MPOM2-PPOM-Mzda_NeUcastNemoc",     "731").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Div(UcastNem(1), 2, -1))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(732, "MDPC2-PPOM-Mzda_NeUcastNemoc",     "732").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, -1))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(733, "XDPP2-PPOM-Mzda_NeUcastNemoc",     "733").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, -1))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(741, "MPOM2-PPOM-Mzda_UcastNemoc",     "741").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(742, "MDPC2-PPOM-Mzda_UcastNemoc",     "742").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(743, "XDPP2-PPOM-Mzda_UcastNemoc",     "743").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 0))
                    .WithHealthMinimVal(0),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(UcastNem(0)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(751, "MPOM2-PPOM-Mzda_MinZdrav",     "751").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearBW(2011, 2022), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MinZdr(0), UcastNem(0), -1000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(752, "MDPC2-PPOM-Mzda_MinZdrav",     "752").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearLE(2013), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MinZdr(0), UcastNem(0), -1000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(753, "XDPP2-PPOM-Mzda_MinZdrav",     "753").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearLE(2013), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MinZdr(0), UcastNem(0), -1000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(761, "MPOM2-PPOM-Mzda_MaxZdrav",     "761").WithContracts(
                ContractGenerator.SpecEmp(1).WithPriority(IIf(YearBW(2014, 2022), ConValue(1), ConValue(0)))
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2).WithPriority(IIf(YearBW(2014, 2022), ConValue(2), ConValue(0)))
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearBW(2014, 2022), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(762, "MDPC2-PPOM-Mzda_MaxZdrav",     "762").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(763, "XDPP2-PPOM-Mzda_MaxZdrav",     "763").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(765, "MPOM2-PPOM-Mzda_MaxZdrav",     "765").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(766, "MDPC2-PPOM-Mzda_MaxZdrav",     "766").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(3)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(767, "XDPP2-PPOM-Mzda_MaxZdrav",     "767").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(3)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(771, "MPOM2-PPOM-Mzda_MaxSocial",     "771").WithContracts(
                ContractGenerator.SpecEmp(1).WithPriority(IIf(YearBW(2014, 2022), ConValue(2), ConValue(0)))
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2).WithPriority(IIf(YearBW(2014, 2022), ConValue(1), ConValue(0)))
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearBW(2014, 2022), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(772, "MDPC2-PPOM-Mzda_MaxSocial",     "772").WithContracts(
                ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(773, "XDPP2-PPOM-Mzda_MaxSocial",     "773").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(775, "MPOM2-PPOM-Mzda_MaxSocial",     "775").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(776, "MDPC2-PPOM-Mzda_MaxSocial",     "776").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)),
                ContractGenerator.SpecDpc(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecDpc(3)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1))
             .WithTaxDeclVal(0),
            ExampleGenerator.Spec(777, "XDPP2-PPOM-Mzda_MaxSocial",     "777").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(3)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)))
             .WithTaxDeclVal(0),
        };

        public static IEnumerable<object[]> GetGenTestDecData(IEnumerable<ExampleGenerator> tests, IPeriod testPeriod, Int32 testPeriodCode, Int32 prevPeriodCode)
        {
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            return tests.Select((tt) => (new object[] { tt }));
        }

        public ExampleGenerator Example_101_PPomMzdaDanPojSlevyZaklad()
        {
            return ExampleGenerator.Spec(101, "PP-Mzda_DanPoj-SlevyZaklad", "101")
                .WithContracts(ContractGenerator.SpecEmp(1));
        }

        public ExampleGenerator Example_105_PPomMzdaDanMaxBonus()
        {
            return ExampleGenerator.Spec(105, "PP-Mzda_DanPoj-MaxBonus", "105")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalary(MaxBonus()))
                .WithChild(ChildGenerator.SpecDisb(1, 1, 5));
        }

        public ExampleGenerator Example_108_PPomMzdaDanMaxZdravPrev()
        {
            return ExampleGenerator.Spec(108, "PP-Mzda_DanPoj-MaxZdravPrev", "108")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalary(MaxZdrPrev(100)));
        }

        public ExampleGenerator Example_109_PPomMzdaDanMaxZdravCurr()
        {
            return ExampleGenerator.Spec(109, "PP-Mzda_DanPoj-MaxZdravCurr", "109")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalary(MaxZdr(100)));
        }

        public ExampleGenerator Example_193_PPomMzdaNepodPojZaporPlat()
        {
            return ExampleGenerator.Spec(193, "PP-Mzda_NepodPoj-ZaporPlat", "193")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalaryVal(-10000))
                .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_201_PPomMzdaNepodPojPrevLo()
        {
            return ExampleGenerator.Spec(201, "PP-Mzda_NepodPoj-PrevLo", "201")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalary(SrazNepPrev(0)))
                .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_301_PPomMzdaDanPojDan099()
        {
            return ExampleGenerator.Spec(301, "PP-Mzda_DanPoj-Dan099", "301")
                .WithContracts(ContractGenerator.SpecEmp(1)
                    .WithSalaryVal(0)
                    .WithAgreemVal(74)
                    .WithHealthMinimVal(0));
        }

        public ExampleGenerator Example_501_PPomMzdaNeUcastZdravPrev()
        {
            return ExampleGenerator.Spec(501, "DPC-Mzda_NeUcastZdrav-Prev", "501")
                .WithContracts(ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastZdrPrev(-1))
                    .WithHealthMinimVal(0))
                .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_502_PPomMzdaUcastZdravPrev()
        {
            return ExampleGenerator.Spec(502, "DPC-Mzda_UcastZdrav-Prev", "502")
                .WithContracts(ContractGenerator.SpecDpc(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastZdrPrev(0))
                    .WithHealthMinimVal(0))
                .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_601_PPomMzdaNeUcastNemocPrev()
        {
            return ExampleGenerator.Spec(601, "DPP-Mzda_NeUcastNemoc-Prev", "601")
                .WithContracts(ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(UcastNemPrev(-1))
                    .WithHealthMinimVal(0))
                .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_701_PPomMzdaNeUcastNemoc()
        {
            return ExampleGenerator.Spec(701, "MPOM-PPOM-Mzda_NeUcastNemoc", "701").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(UcastNem(-1))
                    .WithHealthMinimVal(0)
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalaryVal(2500))
             .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_751_Mpom2_PPomMzdaMinZdrav()
        {
            return ExampleGenerator.Spec(751, "MPOM2-PPOM-Mzda_MinZdrav", "751").WithContracts(
                ContractGenerator.SpecEmp(1)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(2)
                    .WithSalary(Div(UcastNem(1), 2, 1))
                    .WithSocialLoIncomeVal(1),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearLE(2014), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MinZdr(0), UcastNem(0), -1000)))
             .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_753_XDpp2_PPomMzdaMinZdrav()
        {
            return ExampleGenerator.Spec(753, "XDPP2-PPOM-Mzda_MinZdrav", "753").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3).WithPriority(IIf(YearLE(2013), ConValue(3), ConValue(0)))
                    .WithSalary(Sub(MinZdr(0), UcastNem(0), -1000)))
             .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_763_XDpp2_PPomMzdaMaxZdrav()
        {
            return ExampleGenerator.Spec(763, "XDPP2-PPOM-Mzda_MaxZdrav", "763").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxZdr(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0);
        }

        public ExampleGenerator Example_773_XDpp2_PPomMzdaMaxSocial()
        {
            return ExampleGenerator.Spec(773, "XDPP2-PPOM-Mzda_MaxSocial", "773").WithContracts(
                ContractGenerator.SpecDpp(1)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecDpp(2)
                    .WithSalaryVal(0)
                    .WithAgreem(Div(UcastNem(1), 2, 1)),
                ContractGenerator.SpecEmp(3)
                    .WithSalary(Sub(MaxSoc(0), UcastNem(0), 2000)))
             .WithTaxDeclVal(0);
        }

        protected void ServiceExamplesCreateImport(IEnumerable<ExampleGenerator> tests, IPeriod testPeriod, Int32 testPeriodCode, Int32 prevPeriodCode)
        {
            System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(ppp);

            try
            {
                testPeriod.Code.Should().Be(testPeriodCode);

                var prevPeriod = PrevYear(testPeriod);
                prevPeriod.Code.Should().Be(prevPeriodCode);

                var testLegalResult = _leg.GetBundle(testPeriod);
                testLegalResult.IsSuccess.Should().Be(true);

                var testRuleset = testLegalResult.Value;

                var prevLegalResult = _leg.GetBundle(prevPeriod);
                prevLegalResult.IsSuccess.Should().Be(true);

                var prevRuleset = prevLegalResult.Value;

                using (var testProtokol = CreateProtokolFile($"OKmzdyImport_{testPeriod.Year}.txt"))
                {
                    ExportPropsStart(testProtokol);

                    foreach (var example in tests)
                    {
                        foreach (var impLine in example.BuildImportString(testPeriod, testRuleset, prevRuleset))
                        {
                            testProtokol.WriteLine(impLine);
                        }
                    }
                    ExportPropsEnd(testProtokol);
                }
            }
            catch (Xunit.Sdk.XunitException e)
            {
                throw e;
            }
        }

        protected void ServiceExampleTest(ExampleGenerator example, IPeriod testPeriod, Int32 testPeriodCode, Int32 prevPeriodCode)
        {
#if __TEST_PRESCRIPTION__
            //name                             |101-PP-Mzda-DanPoj-SlevyZaklad
            //period                           |01 2011
            //schedule                         |40
            //absence                          |0
            //salary                           |CZK 15000
            //tax payer                        |DECLARE
            //health payer                     |YES
            //health minim                     |YES
            //social payer                     |YES
            //pension payer                    |NO
            //tax payer benefit                |YES
            //tax child benefit                |0
            //tax disability benefit           |NO:NO:NO
            //tax studying benefit             |NO
            //health employer                  |YES
            //social employer                  |YES
            //tax income                       |CZK 15000
            //premium insurance                |CZK 5100
            //tax base                         |CZK 20100
            //health base                      |CZK 15000
            //social base                      |CZK 15000
            //health ins                       |CZK 675
            //social ins                       |CZK 975
            //tax before                       |CZK 3015
            //payer relief                     |CZK 2070
            //tax after A relief               |CZK 945
            //child relief                     |CZK 0
            //tax after C relief               |CZK 945
            //tax advance                      |CZK 945
            //tax bonus                        |CZK 0
            //gross income                     |CZK 15000
            //netto income                     |CZK 12405
#endif
            output.WriteLine($"Test: {example.Name}, Number: {example.Number}");
            try
            {
                testPeriod.Code.Should().Be(testPeriodCode);

                var prevPeriod = PrevYear(testPeriod);
                prevPeriod.Code.Should().Be(prevPeriodCode);

                var testLegalResult = _leg.GetBundle(testPeriod);
                testLegalResult.IsSuccess.Should().Be(true);

                var testRuleset = testLegalResult.Value;

                var prevLegalResult = _leg.GetBundle(prevPeriod);
                prevLegalResult.IsSuccess.Should().Be(true);

                var prevRuleset = prevLegalResult.Value;

                var targets = example.BuildSpecTargets(testPeriod, testRuleset, prevRuleset);
                foreach (var (target, index) in targets.Select((item, index) => (item, index)))
                {
                    var articleSymbol = target.ArticleDescr();
                    var conceptSymbol = target.ConceptDescr();
                    output.WriteLine("Index: {0}; ART: {1}; CON: {2}; con: {3}; pos: {4}; var: {5}", index, articleSymbol, conceptSymbol, target.Contract.Value, target.Position.Value, target.Variant.Value);
                }

                var initService = _sut.InitWithPeriod(testPeriod);
                initService.Should().BeTrue();

                var restService = _sut.GetResults(testPeriod, testRuleset, targets);
                restService.Count().Should().NotBe(0);

                output.WriteLine($"Result Test: {example.Name}, Number: {example.Number}");

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
            catch (Xunit.Sdk.XunitException e)
            {
                throw e;
            }
        }

        protected void ServiceTemplateExampleTest(ExampleGenerator example, IPeriod testPeriod, Int32 testPeriodCode, Int32 prevPeriodCode)
        {
            if (example.Id == 101)
            {
                string[] strHeaderRadkaPRAC = new string[] {
                "EMPLOYEEID",
                "PERIOD",
                "TAXING_INCOME_SUBJECT",
                "TAXING_INCOME_HEALTH_RAW",
                "TAXING_INCOME_HEALTH_FIX",
                "TAXING_INCOME_SOCIAL_RAW",
                "TAXING_INCOME_SOCIAL_FIX",
                "TAXING_SIGNING",
                "TAXING_SIGNING_NONE",
                "TAXING_ADVANCES_INCOME",
                "TAXING_ADVANCES_HEALTH",
                "TAXING_ADVANCES_SOCIAL",
                "TAXING_ADVANCES_BASIS_RAW",
                "TAXING_ADVANCES_BASIS_RND",
                "TAXING_SOLIDARY_BASIS",
                "TAXING_ADVANCES",
                "TAXING_SOLIDARY",
                "TAXING_ADVANCES_TOTAL",
                "TAXING_ALLOWANCE_PAYER",
                "TAXING_ALLOWANCE_PAYER_SUM",
                "TAXING_ALLOWANCE_CHILD_ORD1",
                "TAXING_ALLOWANCE_CHILD_ORD2",
                "TAXING_ALLOWANCE_CHILD_ORD3",
                "TAXING_ALLOWANCE_CHILD_DIS1",
                "TAXING_ALLOWANCE_CHILD_DIS2",
                "TAXING_ALLOWANCE_CHILD_DIS3",
                "TAXING_ALLOWANCE_CHILD_SUM",
                "TAXING_ALLOWANCE_DISAB_DIS1",
                "TAXING_ALLOWANCE_DISAB_DIS2",
                "TAXING_ALLOWANCE_DISAB_DIS3",
                "TAXING_ALLOWANCE_DISAB_SUM",
                "TAXING_ALLOWANCE_STUDY",
                "TAXING_ALLOWANCE_STUDY_SUM",
                "TAXING_REBATE_PAYER",
                "TAXING_REBATE_CHILD",
                "TAXING_BONUS_CHILD_CAL",
                "TAXING_BONUS_CHILD_PAY",
                "TAXING_WITHHOLD_INCOME",
                "TAXING_WITHHOLD_HEALTH",
                "TAXING_WITHHOLD_SOCIAL",
                "TAXING_WITHHOLD_BASIS_RAW",
                "TAXING_WITHHOLD_BASIS_RND",
                "TAXING_WITHHOLD_TOTAL",
                "TAXING_PAYM_ADVANCES",
                "TAXING_PAYM_WITHHOLD",
                "INCOME_GROSS",
                "INCOME_NETTO",
                "EMPLOYER_COSTS",
            };
                string[] strHeaderRadkaPPOM = new string[] {
                "EMPLOYEEID",
                "CONTRACTID",
                "PERIOD",
                "CONTRACT_TYPE",
                "POSITION_WORK_PLAN",
                "CONTRACT_TIME_PLAN",
                "CONTRACT_TIME_WORK",
                "CONTRACT_TIME_ABSC",
                "PAYMENT_SALARY",
                "PAYMENT_FIXED",
                "HEALTH_DECLARE_SUB",
                "HEALTH_DECLARE_MIN",
                "HEALTH_DECLARE_FOR",
                "HEALTH_DECLARE_EHS",
                "HEALTH_DECLARE_PAR",
                "HEALTH_INCOME",
                "HEALTH_BASE",
                "HEALTH_BASE_ANNUALLY",
                "HEALTH_BASE_EMPLOYEE",
                "HEALTH_BASE_EMPLOYER",
                "HEALTH_BASE_MANDATE",
                "HEALTH_BASE_OVERCAP",
                "HEALTH_PAYM_EMPLOYEE",
                "HEALTH_PAYM_EMPLOYER",
                "SOCIAL_DECLARE",
                "SOCIAL_DECLARE_ZMR",
                "SOCIAL_DECLARE_KRZ",
                "SOCIAL_DECLARE_FOR",
                "SOCIAL_DECLARE_EHS",
                "SOCIAL_DECLARE_PAR",
                "SOCIAL_INCOME",
                "SOCIAL_BASE",
                "SOCIAL_BASE_ANNUALLY",
                "SOCIAL_BASE_EMPLOYEE",
                "SOCIAL_BASE_EMPLOYER",
                "SOCIAL_BASE_OVERCAP",
                "SOCIAL_PAYM_EMPLOYEE",
                "SOCIAL_PAYM_EMPLOYER",
                "TAXING_DECLARE",
                "TAXING_INCOME_SUBJECT",
                "TAXING_INCOME_HEALTH_RAW",
                "TAXING_INCOME_SOCIAL_RAW",
                };
                using (var testProtokol = CreateProtokolFile($"OKPRAC_TEST_{testPeriod.Year}_HRM_{testPeriod.Code}.CSV"))
                {
                    testProtokol.WriteLine(string.Join(";", strHeaderRadkaPRAC));
                }
                using (var testProtokol = CreateProtokolFile($"OKPPOM_TEST_{testPeriod.Year}_HRM_{testPeriod.Code}.CSV"))
                {
                    testProtokol.WriteLine(string.Join(";", strHeaderRadkaPPOM));
                }
            }
            output.WriteLine($"Test: {example.Name}, Number: {example.Number}");

            try
            {
                testPeriod.Code.Should().Be(testPeriodCode);

                var prevPeriod = PrevYear(testPeriod);
                prevPeriod.Code.Should().Be(prevPeriodCode);

                var testLegalResult = _leg.GetBundle(testPeriod);
                testLegalResult.IsSuccess.Should().Be(true);

                var testRuleset = testLegalResult.Value;

                var prevLegalResult = _leg.GetBundle(prevPeriod);
                prevLegalResult.IsSuccess.Should().Be(true);

                var prevRuleset = prevLegalResult.Value;

                var targets = example.BuildSpecTargets(testPeriod, testRuleset, prevRuleset);

                foreach (var (target, index) in targets.Select((item, index) => (item, index)))
                {
                    var articleSymbol = target.ArticleDescr();
                    var conceptSymbol = target.ConceptDescr();
                    output.WriteLine("Index: {0}; ART: {1}; CON: {2}; con: {3}; pos: {4}; var: {5}", index, articleSymbol, conceptSymbol, target.Contract.Value, target.Position.Value, target.Variant.Value);
                }

                var initService = _sut.InitWithPeriod(testPeriod);
                initService.Should().BeTrue();

                var restService = _sut.GetResults(testPeriod, testRuleset, targets);
                restService.Count().Should().NotBe(0);

                using (var testProtokol = OpenProtokolFile($"OKPRAC_TEST_{testPeriod.Year}_HRM_{testPeriod.Code}.CSV"))
                {
                    var testResults = GetExamplePracResultsLine(example, testPeriod, restService);
                    testProtokol.WriteLine(testResults);
                }
                using (var testProtokol = OpenProtokolFile($"OKPPOM_TEST_{testPeriod.Year}_HRM_{testPeriod.Code}.CSV"))
                {
                    var testResults = GetExamplePPomResultsLine(example, testPeriod, restService);
                    foreach (var ppomResult in testResults)
                    {
                        testProtokol.WriteLine(ppomResult);
                    }
                }

                output.WriteLine($"Result Test: {example.Name}, Number: {example.Number}");

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
            catch (Xunit.Sdk.XunitException e)
            {
                throw e;
            }
        }

        protected ITermResult GetResultArticle<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode)
            where T : class, ITermResult
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as T;
            return resultValue;
        }
        protected string GetResultValue(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode)
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as PayrolexTermResult;
            if (resultValue == null)
            {
                return "";
            }
            return resultValue.ResultValue.ToString();
        }
        protected string GetResultSelect<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as T;
            if (resultValue == null)
            {
                return "";
            }
            return selVal(resultValue).ToString();
        }
        protected string GetResultSelectSum<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            Int32 resultInit = default;
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.Select((c) => (c as T))
                .Aggregate(resultInit, (agr, x) => (agr + selVal(x)));
            return resultValue.ToString();
        }
        protected string GetResultSelect<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode, string prepText, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            Int32 resultSumValue = default;
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as T;
            if (resultValue != null)
            {
                resultSumValue += selVal(resultValue);
            }
            if (string.IsNullOrEmpty(prepText)==false)
            {
                return prepText + resultSumValue.ToString();
            }
            return resultSumValue.ToString();
        }
        protected string GetResultSelect<T1, T2>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode1, PayrolexArticleConst artCode2, Func<T1, Int32> selVal1, Func<T2, Int32> selVal2)
            where T1 : class, ITermResult
            where T2 : class, ITermResult
        {
            Int32 resultSumValue = default;
            var result1 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode1)).Select((x) => (x.Value)).ToList();
            var resultValue1 = result1.FirstOrDefault() as T1;
            if (resultValue1 != null)
            {
                resultSumValue += selVal1(resultValue1);
            }
            var result2 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode2)).Select((x) => (x.Value)).ToList();
            var resultValue2 = result2.FirstOrDefault() as T2;
            if (resultValue2 != null)
            {
                resultSumValue += selVal2(resultValue2);
            }
            return resultSumValue.ToString();
        }

        protected Int32 GetIntResultValue(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode)
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as PayrolexTermResult;
            if (resultValue == null)
            {
                return 0;
            }
            return resultValue.ResultValue;
        }
        protected Int32 GetIntResultSelect<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault() as T;
            if (resultValue == null)
            {
                return 0;
            }
            return selVal(resultValue);
        }
        protected Int32 GetIntResultSelectSum<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            Int32 resultInit = default;
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.Select((c) => (c as T))
                .Aggregate(resultInit, (agr, x) => (agr + selVal(x)));
            return resultValue;
        }
        protected Int32 GetIntResultSelect<T1, T2>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, PayrolexArticleConst artCode1, PayrolexArticleConst artCode2, Func<T1, Int32> selVal1, Func<T2, Int32> selVal2)
            where T1 : class, ITermResult
            where T2 : class, ITermResult
        {
            Int32 resultSumValue = default;
            var result1 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode1)).Select((x) => (x.Value)).ToList();
            var resultValue1 = result1.FirstOrDefault() as T1;
            if (resultValue1 != null)
            {
                resultSumValue += selVal1(resultValue1);
            }
            var result2 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode2)).Select((x) => (x.Value)).ToList();
            var resultValue2 = result2.FirstOrDefault() as T2;
            if (resultValue2 != null)
            {
                resultSumValue += selVal2(resultValue2);
            }
            return resultSumValue;
        }

        protected Int32 GetIntResultContractValue(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, Int32 contract, PayrolexArticleConst artCode)
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault((x) => (x.Contract.Value == contract)) as PayrolexTermResult;
            if (resultValue == null)
            {
                return 0;
            }
            return resultValue.ResultValue;
        }
        protected Int32 GetIntResultContractSelect<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, Int32 contract, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.FirstOrDefault((x) => (x.Contract.Value == contract)) as T;
            if (resultValue == null)
            {
                return 0;
            }
            return selVal(resultValue);
        }
        protected Int32 GetIntResultContractSelectSum<T>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, Int32 contract, PayrolexArticleConst artCode, Func<T, Int32> selVal)
            where T : class, ITermResult
        {
            Int32 resultInit = default;
            var result = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode)).Select((x) => (x.Value)).ToList();
            var resultValue = result.Where((x) => (x.Contract.Value == contract))
                .Select((c) => (c as T))
                .Aggregate(resultInit, (agr, x) => (agr + selVal(x)));
            return resultValue;
        }
        protected Int32 GetIntResultContractSelect<T1, T2>(IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> res, Int32 contract, PayrolexArticleConst artCode1, PayrolexArticleConst artCode2, Func<T1, Int32> selVal1, Func<T2, Int32> selVal2)
            where T1 : class, ITermResult
            where T2 : class, ITermResult
        {
            Int32 resultSumValue = default;
            var result1 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode1)).Select((x) => (x.Value)).ToList();
            var resultValue1 = result1.FirstOrDefault((x) => (x.Contract.Value == contract)) as T1;
            if (resultValue1 != null)
            {
                resultSumValue += selVal1(resultValue1);
            }
            var result2 = res.Where((e) => (e.IsSuccess && e.Value.Article.Value == (Int32)artCode2)).Select((x) => (x.Value)).ToList();
            var resultValue2 = result2.FirstOrDefault((x) => (x.Contract.Value == contract)) as T2;
            if (resultValue2 != null)
            {
                resultSumValue += selVal2(resultValue2);
            }
            return resultSumValue;
        }
#region __GENERATOR_FUNC__
        protected string GetExamplePracResultsLine(ExampleGenerator example, IPeriod period, IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> results)
        {
            Int32 TAXING_INCOME_SUBJECT = GetIntResultSelectSum<TaxingIncomeSubjectResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT, (x) => (x.ResultValue));//TAXING_INCOME_SUBJECT,	15000
            Int32 TAXING_INCOME_HEALTH_RAW = GetIntResultSelectSum<TaxingIncomeHealthResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH, (x) => (x.ResultBasis));//TAXING_INCOME_HEALTH_RAW,	X
            Int32 TAXING_INCOME_HEALTH_FIX = GetIntResultSelectSum<TaxingIncomeHealthResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH, (x) => (x.ResultValue));//TAXING_INCOME_HEALTH_FIX,	X
            Int32 TAXING_INCOME_SOCIAL_RAW = GetIntResultSelectSum<TaxingIncomeSocialResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL, (x) => (x.ResultBasis));//TAXING_INCOME_SOCIAL_RAW,	X
            Int32 TAXING_INCOME_SOCIAL_FIX = GetIntResultSelectSum<TaxingIncomeSocialResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL, (x) => (x.ResultValue));//TAXING_INCOME_SOCIAL_FIX,	X
            Int32 TAXING_DECLARE_SIGNING = GetIntResultSelect<TaxingSigningResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_SIGNING, (x) => (x.DeclSignValue()));// TAXING_SIGNING,	1
            Int32 TAXING_DECLARE_NONSIGN = GetIntResultSelect<TaxingSigningResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_SIGNING, (x) => (x.NoneSignValue()));// TAXING_SIGNING_NONE,	0
            Int32 TAXING_ADVANCES_INCOME = GetIntResultSelect<TaxingAdvancesIncomeResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_INCOME, (x) => (x.ResultValue));// TAXING_ADVANCES_INCOME,	15000
            Int32 TAXING_ADVANCES_HEALTH = GetIntResultSelect<TaxingAdvancesHealthResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_HEALTH, (x) => (x.ResultValue));// TAXING_ADVANCES_HEALTH,	X
            Int32 TAXING_ADVANCES_SOCIAL = GetIntResultSelect<TaxingAdvancesSocialResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_SOCIAL, (x) => (x.ResultValue));// TAXING_ADVANCES_SOCIAL,	X
            Int32 TAXING_ADVANCES_BASIS_RAW = GetIntResultSelect<TaxingAdvancesBasisResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS, (x) => (x.ResultBasis));// TAXING_ADVANCES_BASIS_RAW,	21100
            Int32 TAXING_ADVANCES_BASIS_RND = GetIntResultSelect<TaxingAdvancesBasisResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_BASIS, (x) => (x.ResultValue));// TAXING_ADVANCES_BASIS_RND,	21100
            Int32 TAXING_SOLIDARY_BASIS = GetIntResultSelect<TaxingSolidaryBasisResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY_BASIS, (x) => (x.ResultValue));// TAXING_SOLIDARY_BASIS,	0
            Int32 TAXING_ADVANCES = GetIntResultSelect<TaxingAdvancesResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES, (x) => (x.ResultValue));// TAXING_ADVANCES,	X
            Int32 TAXING_SOLIDARY = GetIntResultSelect<TaxingSolidaryResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_SOLIDARY, (x) => (x.ResultValue));// TAXING_SOLIDARY,	0
            Int32 TAXING_ADVANCES_TOTAL = GetIntResultSelect<TaxingAdvancesTotalResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ADVANCES_TOTAL, (x) => (x.ResultValue));// TAXING_ADVANCES_TOTAL,	X
            Int32 TAXING_ALLOWANCE_PAYER = GetIntResultSelect<TaxingAllowancePayerResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_PAYER, (x) => (x.BenefitApplyResult()));// TAXING_ALLOWANCE_PAYER,	1, 2710
            Int32 TAXING_ALLOWANCE_PAYER_SUM = GetIntResultSelectSum<TaxingAllowancePayerResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_PAYER, (x) => (x.ResultValue));// TAXING_ALLOWANCE_PAYER_SUM,	1, 2710
            Int32 TAXING_ALLOWANCE_CHILD_ORD1 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyOrder1()));// TAXING_ALLOWANCE_CHILD_ORD1,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_ORD2 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyOrder2()));// TAXING_ALLOWANCE_CHILD_ORD2,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_ORD3 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyOrder3()));// TAXING_ALLOWANCE_CHILD_ORD3,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_DIS1 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyDisab1()));// TAXING_ALLOWANCE_CHILD_DIS1,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_DIS2 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyDisab2()));// TAXING_ALLOWANCE_CHILD_DIS2,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_DIS3 = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.BenefitApplyDisab3()));// TAXING_ALLOWANCE_CHILD_DIS3,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_CHILD_SUM = GetIntResultSelectSum<TaxingAllowanceChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD, (x) => (x.ResultValue));// TAXING_ALLOWANCE_CHILD_SUM,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
            Int32 TAXING_ALLOWANCE_DISAB_DIS1 = GetIntResultSelectSum<TaxingAllowanceDisabResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB, (x) => (x.BenefitApplyDisab1()));// TAXING_ALLOWANCE_DISAB_DIS1,	1, 0, 0, x
            Int32 TAXING_ALLOWANCE_DISAB_DIS2 = GetIntResultSelectSum<TaxingAllowanceDisabResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB, (x) => (x.BenefitApplyDisab2()));// TAXING_ALLOWANCE_DISAB_DIS2,	1, 0, 0, x
            Int32 TAXING_ALLOWANCE_DISAB_DIS3 = GetIntResultSelectSum<TaxingAllowanceDisabResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB, (x) => (x.BenefitApplyDisab3()));// TAXING_ALLOWANCE_DISAB_DIS3,	1, 0, 0, x
            Int32 TAXING_ALLOWANCE_DISAB_SUM = GetIntResultSelectSum<TaxingAllowanceDisabResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB, (x) => (x.ResultValue));// TAXING_ALLOWANCE_DISAB_SUM,	1, 0, 0, x
            Int32 TAXING_ALLOWANCE_STUDY = GetIntResultSelectSum<TaxingAllowanceStudyResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_STUDY, (x) => (x.BenefitApplyResult()));// TAXING_ALLOWANCE_STUDY,	1, x
            Int32 TAXING_ALLOWANCE_STUDY_SUM = GetIntResultSelect<TaxingAllowanceStudyResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_STUDY, (x) => (x.ResultValue));// TAXING_ALLOWANCE_STUDY_SUM, 1, x
            Int32 TAXING_REBATE_PAYER = GetIntResultSelectSum<TaxingRebatePayerResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_REBATE_PAYER, (x) => (x.ResultValue));// TAXING_REBATE_PAYER,	X
            Int32 TAXING_REBATE_CHILD = GetIntResultSelectSum<TaxingRebateChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_REBATE_CHILD, (x) => (x.ResultValue));// TAXING_REBATE_CHILD,	X
            Int32 TAXING_BONUS_CHILD_CAL = GetIntResultSelectSum<TaxingBonusChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_BONUS_CHILD, (x) => (x.ResultBasis));// TAXING_BONUS_CHILD_CAL,	X
            Int32 TAXING_BONUS_CHILD_PAY = GetIntResultSelectSum<TaxingBonusChildResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_BONUS_CHILD, (x) => (x.ResultValue));// TAXING_BONUS_CHILD_PAY,	X
            Int32 TAXING_WITHHOLD_INCOME = GetIntResultSelectSum<TaxingWithholdIncomeResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_INCOME, (x) => (x.ResultValue));// TAXING_WITHHOLD_INCOME,	0
            Int32 TAXING_WITHHOLD_HEALTH = GetIntResultSelectSum<TaxingWithholdHealthResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_HEALTH, (x) => (x.ResultValue));// TAXING_WITHHOLD_HEALTH,	0
            Int32 TAXING_WITHHOLD_SOCIAL = GetIntResultSelectSum<TaxingWithholdSocialResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_SOCIAL, (x) => (x.ResultValue));// TAXING_WITHHOLD_SOCIAL,	0
            Int32 TAXING_WITHHOLD_BASIS_RAW = GetIntResultSelectSum<TaxingWithholdBasisResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_BASIS, (x) => (x.ResultValue));// TAXING_WITHHOLD_BASIS_RAW,	0
            Int32 TAXING_WITHHOLD_BASIS_RND = GetIntResultSelectSum<TaxingWithholdBasisResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_BASIS, (x) => (x.ResultValue));// TAXING_WITHHOLD_BASIS_RND,	0
            Int32 TAXING_WITHHOLD_TOTAL = GetIntResultSelectSum<TaxingWithholdTotalResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_WITHHOLD_TOTAL, (x) => (x.ResultValue));// TAXING_WITHHOLD_TOTAL,	0
            Int32 TAXING_PAYM_ADVANCES = GetIntResultSelectSum<TaxingPaymAdvancesResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_PAYM_ADVANCES, (x) => (x.ResultValue));// TAXING_PAYM_ADVANCES,	X
            Int32 TAXING_PAYM_WITHHOLD = GetIntResultSelectSum<TaxingPaymWithholdResult>(results,
                    PayrolexArticleConst.ARTICLE_TAXING_PAYM_WITHHOLD, (x) => (x.ResultValue));// TAXING_PAYM_WITHHOLD,	X
            Int32 INCOME_GROSS = GetIntResultValue(results, PayrolexArticleConst.ARTICLE_INCOME_GROSS);// INCOME_GROSS,	X
            Int32 INCOME_NETTO = GetIntResultValue(results, PayrolexArticleConst.ARTICLE_INCOME_NETTO);// INCOME_NETTO,	X
            Int32 ELOYER_COSTS = GetIntResultValue(results, PayrolexArticleConst.ARTICLE_EMPLOYER_COSTS);// ELOYER_COSTS,	X

            string[] resultLine = new string[]
            {
                example.Number,
                period.Code.ToString(),
                TAXING_INCOME_SUBJECT.ToString(),//TAXING_INCOME_SUBJECT,	15000
                TAXING_INCOME_HEALTH_RAW.ToString(),//TAXING_INCOME_HEALTH_RAW,	X
                TAXING_INCOME_HEALTH_FIX.ToString(),//TAXING_INCOME_HEALTH_FIX,	X
                TAXING_INCOME_SOCIAL_RAW.ToString(),//TAXING_INCOME_SOCIAL_RAW,	X
                TAXING_INCOME_SOCIAL_FIX.ToString(),//TAXING_INCOME_SOCIAL_FIX,	X
                TAXING_DECLARE_SIGNING.ToString(),// TAXING_SIGNING,	1
                TAXING_DECLARE_NONSIGN.ToString(),// TAXING_SIGNING_NONE,	0
                TAXING_ADVANCES_INCOME.ToString(),// TAXING_ADVANCES_INCOME,	15000
                TAXING_ADVANCES_HEALTH.ToString(),// TAXING_ADVANCES_HEALTH,	X
                TAXING_ADVANCES_SOCIAL.ToString(),// TAXING_ADVANCES_SOCIAL,	X
                TAXING_ADVANCES_BASIS_RAW.ToString(),// TAXING_ADVANCES_BASIS_RAW,	21100
                TAXING_ADVANCES_BASIS_RND.ToString(),// TAXING_ADVANCES_BASIS_RND,	21100
                TAXING_SOLIDARY_BASIS.ToString(),// TAXING_SOLIDARY_BASIS,	0
                TAXING_ADVANCES.ToString(),// TAXING_ADVANCES,	X
                TAXING_SOLIDARY.ToString(),// TAXING_SOLIDARY,	0
                TAXING_ADVANCES_TOTAL.ToString(),// TAXING_ADVANCES_TOTAL,	X
                TAXING_ALLOWANCE_PAYER.ToString(),// TAXING_ALLOWANCE_PAYER,	1, 2710
                TAXING_ALLOWANCE_PAYER_SUM.ToString(),// TAXING_ALLOWANCE_PAYER_SUM,	1, 2710
                TAXING_ALLOWANCE_CHILD_ORD1.ToString(),// TAXING_ALLOWANCE_CHILD_ORD1,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_ORD2.ToString(),// TAXING_ALLOWANCE_CHILD_ORD2,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_ORD3.ToString(),// TAXING_ALLOWANCE_CHILD_ORD3,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_DIS1.ToString(),// TAXING_ALLOWANCE_CHILD_DIS1,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_DIS2.ToString(),// TAXING_ALLOWANCE_CHILD_DIS2,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_DIS3.ToString(),// TAXING_ALLOWANCE_CHILD_DIS3,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_CHILD_SUM.ToString(),// TAXING_ALLOWANCE_CHILD_SUM,	Počet 1, 2, 3, Počet ZTP 1, 2, 3, x
                TAXING_ALLOWANCE_DISAB_DIS1.ToString(),// TAXING_ALLOWANCE_DISAB_DIS1,	1, 0, 0, x
                TAXING_ALLOWANCE_DISAB_DIS2.ToString(),// TAXING_ALLOWANCE_DISAB_DIS2,	1, 0, 0, x
                TAXING_ALLOWANCE_DISAB_DIS3.ToString(),// TAXING_ALLOWANCE_DISAB_DIS3,	1, 0, 0, x
                TAXING_ALLOWANCE_DISAB_SUM.ToString(),// TAXING_ALLOWANCE_DISAB_SUM,	1, 0, 0, x
                TAXING_ALLOWANCE_STUDY.ToString(),// TAXING_ALLOWANCE_STUDY,	1, x
                TAXING_ALLOWANCE_STUDY_SUM.ToString(),// TAXING_ALLOWANCE_STUDY_SUM, 1, x
                TAXING_REBATE_PAYER.ToString(),// TAXING_REBATE_PAYER,	X
                TAXING_REBATE_CHILD.ToString(),// TAXING_REBATE_CHILD,	X
                TAXING_BONUS_CHILD_CAL.ToString(),// TAXING_BONUS_CHILD_CAL,	X
                TAXING_BONUS_CHILD_PAY.ToString(),// TAXING_BONUS_CHILD_PAY,	X
                TAXING_WITHHOLD_INCOME.ToString(),// TAXING_WITHHOLD_INCOME,	0
                TAXING_WITHHOLD_HEALTH.ToString(),// TAXING_WITHHOLD_HEALTH,	0
                TAXING_WITHHOLD_SOCIAL.ToString(),// TAXING_WITHHOLD_SOCIAL,	0
                TAXING_WITHHOLD_BASIS_RAW.ToString(),// TAXING_WITHHOLD_BASIS_RAW,	0
                TAXING_WITHHOLD_BASIS_RND.ToString(),// TAXING_WITHHOLD_BASIS_RND,	0
                TAXING_WITHHOLD_TOTAL.ToString(),// TAXING_WITHHOLD_TOTAL,	0
                TAXING_PAYM_ADVANCES.ToString(),// TAXING_PAYM_ADVANCES,	X
                TAXING_PAYM_WITHHOLD.ToString(),// TAXING_PAYM_WITHHOLD,	X
                INCOME_GROSS.ToString(),// INCOME_GROSS,	X
                INCOME_NETTO.ToString(),// INCOME_NETTO,	X
                ELOYER_COSTS.ToString(),// ELOYER_COSTS,	X
            };

            string resultOutput = string.Join(";", resultLine);

            return resultOutput;
        }

        protected string[] GetExamplePPomResultsLine(ExampleGenerator example, IPeriod period, IEnumerable<ResultMonad.Result<ITermResult, HraveMzdy.Procezor.Service.Errors.ITermResultError>> results)
        {
           List<string> resultOutput = new List<string>();

            foreach (var con in example.ContractList)
            {              
                Int32 POSITION_WORK_PLAN = GetIntResultContractSelect<PositionWorkPlanResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN, (x) => (x.TotalRealWeeks())); //POSITION_WORK_PLAN,	40
                Int32 CONTRACT_TIME_PLAN = GetIntResultContractSelect<ContractTimePlanResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_CONTRACT_TIME_PLAN, (x) => (x.TotalTimeMonth()));//CONTRACT_TIME_PLAN,	184
                Int32 CONTRACT_TIME_WORK = GetIntResultContractSelect<ContractTimeWorkResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_CONTRACT_TIME_WORK, (x) => (x.TotalTimeMonth()));//CONTRACT_TIME_WORK,	184
                Int32 CONTRACT_TIME_ABSC = GetIntResultContractSelect<ContractTimeAbscResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_CONTRACT_TIME_ABSC, (x) => (x.TotalTimeMonth()));//CONTRACT_TIME_ABSC,	0
                Int32 PAYMENT_SALARY = GetIntResultContractSelect<PaymentBasisResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_PAYMENT_SALARY, (x) => (x.ResultBasis));//PAYMENT_SALARY,	15000
                Int32 PAYMENT_WORKED = GetIntResultContractSelect<PaymentFixedResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_PAYMENT_WORKED, (x) => (x.ResultBasis));//PAYMENT_WORKED,	0
                Int32 HEALTH_DECLARE_SUB = GetIntResultContractSelect<HealthDeclareResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_DECLARE, (x) => (x.InterestCode));//HEALTH_DECLARE_SUB,	subject
                Int32 HEALTH_DECLARE_MIN = GetIntResultContractSelect<HealthDeclareResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_DECLARE, (x) => (x.MandatorBase));//HEALTH_DECLARE_MIN,	minimum
                Int32 HEALTH_DECLARE_FOR = 0;//HEALTH_DECLARE_FOR,	zahraniční
                Int32 HEALTH_DECLARE_EHS = 0;//HEALTH_DECLARE_EHS,	eu
                Int32 HEALTH_DECLARE_PAR = GetIntResultContractSelect<HealthIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_INCOME, (x) => (x.ParticyCode));//HEALTH_DECLARE_PAR,	účast
                Int32 HEALTH_INCOME = GetIntResultContractSelect<HealthIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_INCOME, (x) => (x.ResultValue));//HEALTH_INCOME,	15000
                Int32 HEALTH_BASE = GetIntResultContractSelect<HealthPaymEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYEE, (x) => (x.TotalBasic()));//HEALTH_BASE,	15000
                Int32 HEALTH_BASE_ANNUALLY = 0;//,	extra
                Int32 HEALTH_BASE_EMPLOYEE = GetIntResultContractSelect<HealthBaseEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYEE, (x) => (x.ResultValue));//HEALTH_BASE_EMPLOYEE,	extra
                Int32 HEALTH_BASE_EMPLOYER = GetIntResultContractSelect<HealthBaseEmployerResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_BASE_EMPLOYER, (x) => (x.ResultValue));//HEALTH_BASE_EMPLOYER,	extra
                Int32 HEALTH_BASE_MANDATE = GetIntResultContractSelect<HealthBaseMandateResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_BASE_MANDATE, (x) => (x.ResultValue));//HEALTH_BASE_MANDATE,	0
                Int32 HEALTH_BASE_OVERCAP = GetIntResultContractSelect<HealthBaseOvercapResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_BASE_OVERCAP, (x) => (x.ResultValue));//HEALTH_BASE_OVERCAP,	0
                Int32 HEALTH_PAYM_EMPLOYEE = GetIntResultContractSelect<HealthPaymEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYEE, (x) => (x.ResultValue));//HEALTH_PAYM_EMPLOYEE,	X
                Int32 HEALTH_PAYM_EMPLOYER = GetIntResultContractSelect<HealthPaymEmployerResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_HEALTH_PAYM_EMPLOYER, (x) => (x.ResultValue));//HEALTH_PAYM_EMPLOYER,	X
                Int32 SOCIAL_DECLARE_SUB = GetIntResultContractSelect<SocialDeclareResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE, (x) => (x.InterestCode));//SOCIAL_DECLARE_SUB,	1
                Int32 SOCIAL_DECLARE_ZMR = GetIntResultContractSelect<SocialIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_INCOME, (x) => (x.HasSubjectTermZMR()));//SOCIAL_DECLARE_ZMR,	Malý rozsah
                Int32 SOCIAL_DECLARE_KRZ = GetIntResultContractSelect<SocialIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_INCOME, (x) => (x.HasSubjectTermZKR()));//SOCIAL_DECLARE_KRZ,	Krátkodobé
                Int32 SOCIAL_DECLARE_FOR = 0;//SOCIAL_DECLARE_FOR,	zahraniční
                Int32 SOCIAL_DECLARE_EHS = 0;//SOCIAL_DECLARE_EHS,	eu
                Int32 SOCIAL_DECLARE_PAR = GetIntResultContractSelect<SocialIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_INCOME, (x) => (x.ParticyCode));//SOCIAL_DECLARE_PAR,	účast
                Int32 SOCIAL_INCOME = GetIntResultContractSelect<SocialIncomeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_INCOME, (x) => (x.ResultValue));//SOCIAL_INCOME,	15000
                Int32 SOCIAL_BASE  = GetIntResultContractSelect<SocialPaymEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYEE, (x) => (x.TotalBasic()));//SOCIAL_BASE,	15000
                Int32 SOCIAL_BASE_ANNUALLY = 0;//SOCIAL_BASE_ANNUALLY,	extra
                Int32 SOCIAL_BASE_EMPLOYEE = GetIntResultContractSelect<SocialBaseEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYEE, (x) => (x.ResultValue));//SOCIAL_BASE_EMPLOYEE,	extra
                Int32 SOCIAL_BASE_EMPLOYER = GetIntResultContractSelect<SocialBaseEmployerResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_BASE_EMPLOYER, (x) => (x.ResultValue));//SOCIAL_BASE_EMPLOYER,	extra
                Int32 SOCIAL_BASE_OVERCAP = GetIntResultContractSelect<SocialBaseOvercapResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_BASE_OVERCAP, (x) => (x.ResultValue));//SOCIAL_BASE_OVERCAP,	0
                Int32 SOCIAL_PAYM_EMPLOYEE = GetIntResultContractSelect<SocialPaymEmployeeResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYEE, (x) => (x.ResultValue));//SOCIAL_PAYM_EMPLOYEE,	X
                Int32 SOCIAL_PAYM_EMPLOYER = GetIntResultContractSelect<SocialPaymEmployerResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_SOCIAL_PAYM_EMPLOYER, (x) => (x.ResultValue));//SOCIAL_PAYM_EMPLOYER,	X
                Int32 TAXING_DECLARE_SUB = GetIntResultContractSelect<TaxingDeclareResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_TAXING_DECLARE, (x) => (x.InterestCode));//TAXING_DECLARE_SUB,	1
                Int32 TAXING_INCOME_SUBJECT = GetIntResultContractSelect<TaxingIncomeSubjectResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_SUBJECT, (x) => (x.ResultValue));//TAXING_INCOME_SUBJECT,	15000
                Int32 TAXING_INCOME_HEALTH_RAW = GetIntResultContractSelect<TaxingIncomeHealthResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_HEALTH, (x) => (x.ResultBasis));//TAXING_INCOME_HEALTH_RAW,	X
                Int32 TAXING_INCOME_SOCIAL_RAW = GetIntResultContractSelect<TaxingIncomeSocialResult>(results, con.Id,
                    PayrolexArticleConst.ARTICLE_TAXING_INCOME_SOCIAL, (x) => (x.ResultBasis));//TAXING_INCOME_SOCIAL_RAW,	X

                string[] resultLine = new string[]
                {
                    example.Number,
                    con.Number(example.Number),
                    period.Code.ToString(),
                    con.TermChar(),
                    POSITION_WORK_PLAN.ToString(),//POSITION_WORK_PLAN,	40
                    CONTRACT_TIME_PLAN.ToString(),//CONTRACT_TIME_PLAN,	184
                    CONTRACT_TIME_WORK.ToString(),//CONTRACT_TIME_WORK,	184
                    CONTRACT_TIME_ABSC.ToString(),//CONTRACT_TIME_ABSC,	0
                    PAYMENT_SALARY.ToString(),//PAYMENT_SALARY,	15000
                    PAYMENT_WORKED.ToString(),//PAYMENT_AGRFIX,	0
                    HEALTH_DECLARE_SUB.ToString(),//HEALTH_DECLARE_SUB,	subject
                    HEALTH_DECLARE_MIN.ToString(),//HEALTH_DECLARE_MIN,	minimum
                    HEALTH_DECLARE_FOR.ToString(),//HEALTH_DECLARE_FOR,	zahraniční
                    HEALTH_DECLARE_EHS.ToString(),//HEALTH_DECLARE_EHS,	eu
                    HEALTH_DECLARE_PAR.ToString(),//HEALTH_DECLARE_PAR,	účast
                    HEALTH_INCOME.ToString(),//HEALTH_INCOME,	15000
                    HEALTH_BASE.ToString(),//HEALTH_BASE,	15000
                    HEALTH_BASE_ANNUALLY.ToString(),//,	extra
                    HEALTH_BASE_EMPLOYEE.ToString(),//HEALTH_BASE_EMPLOYEE,	extra
                    HEALTH_BASE_EMPLOYER.ToString(),//HEALTH_BASE_EMPLOYER,	extra
                    HEALTH_BASE_MANDATE.ToString(),//HEALTH_BASE_MANDATE,	0
                    HEALTH_BASE_OVERCAP.ToString(),//HEALTH_BASE_OVERCAP,	0
                    HEALTH_PAYM_EMPLOYEE.ToString(),//HEALTH_PAYM_EMPLOYEE,	X
                    HEALTH_PAYM_EMPLOYER.ToString(),//HEALTH_PAYM_EMPLOYER,	X
                    SOCIAL_DECLARE_SUB.ToString(),//SOCIAL_DECLARE_SUB,	1
                    SOCIAL_DECLARE_ZMR.ToString(),//SOCIAL_DECLARE_ZMR,	Malý rozsah
                    SOCIAL_DECLARE_KRZ.ToString(),//SOCIAL_DECLARE_KRZ,	Krátkodobé
                    SOCIAL_DECLARE_FOR.ToString(),//SOCIAL_DECLARE_FOR,	zahraniční
                    SOCIAL_DECLARE_EHS.ToString(),//SOCIAL_DECLARE_EHS,	eu
                    SOCIAL_DECLARE_PAR.ToString(),//SOCIAL_DECLARE_PAR,	účast
                    SOCIAL_INCOME.ToString(),//SOCIAL_INCOME,	15000
                    SOCIAL_BASE.ToString(),//SOCIAL_BASE,	15000
                    SOCIAL_BASE_ANNUALLY.ToString(),//SOCIAL_BASE_ANNUALLY,	extra
                    SOCIAL_BASE_EMPLOYEE.ToString(),//SOCIAL_BASE_EMPLOYEE,	extra
                    SOCIAL_BASE_EMPLOYER.ToString(),//SOCIAL_BASE_EMPLOYER,	extra
                    SOCIAL_BASE_OVERCAP.ToString(),//SOCIAL_BASE_OVERCAP,	0
                    SOCIAL_PAYM_EMPLOYEE.ToString(),//SOCIAL_PAYM_EMPLOYEE,	X
                    SOCIAL_PAYM_EMPLOYER.ToString(),//SOCIAL_PAYM_EMPLOYER,	X
                    TAXING_DECLARE_SUB.ToString(),//TAXING_DECLARE_SUB,	1
                    TAXING_INCOME_SUBJECT.ToString(),//TAXING_INCOME_SUBJECT,	15000
                    TAXING_INCOME_HEALTH_RAW.ToString(),//TAXING_INCOME_HEALTH_RAW,	X
                    TAXING_INCOME_SOCIAL_RAW.ToString(),//TAXING_INCOME_SOCIAL_RAW,	X
                };

                resultOutput.Add(string.Join(";", resultLine));
            }
            return resultOutput.ToArray();
        }

        private static Int32 HealthParticyIncome(WorkContractTerms contractTerm, IBundleProps ruleset)
        {
            int marginIncome = 0;
            switch (contractTerm)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    marginIncome = ruleset.HealthProps.MarginIncomeEmp;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    marginIncome = ruleset.HealthProps.MarginIncomeEmp;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    marginIncome = ruleset.HealthProps.MarginIncomeAgr;
                    break;
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    marginIncome = ruleset.HealthProps.MarginIncomeEmp;
                    break;
            }
            return marginIncome;
        }
        private static Int32 SocialParticyIncome(WorkContractTerms contractTerm, IBundleProps ruleset)
        {
            int marginIncome = 0;
            switch (contractTerm)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    marginIncome = ruleset.SocialProps.MarginIncomeEmp;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    marginIncome = ruleset.SocialProps.MarginIncomeEmp;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    marginIncome = ruleset.SocialProps.MarginIncomeAgr;
                    break;
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    marginIncome = ruleset.SocialProps.MarginIncomeEmp;
                    break;
            }
            return marginIncome;
        }
        private static Int32 HealthDefaultIncome(WorkContractTerms contractTerm)
        {
            int marginIncome = 0;
            switch (contractTerm)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    marginIncome = 2000;
                    break;
                default:
                    marginIncome = 2000;
                    break;
            }
            return marginIncome;
        }
        private static Int32 SocialDefaultIncome(WorkContractTerms contractTerm)
        {
            int marginIncome = 0;
            switch (contractTerm)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    marginIncome = 2000;
                    break;
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    marginIncome = 2000;
                    break;
                default:
                    marginIncome = 2000;
                    break;
            }
            return marginIncome;
        }
        protected static Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> GenValue(Int32 val)
        {
            return (ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (val);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> ConValue(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (val);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> IsGenImport()
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (gen.Example.ExportToOKmzdy);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> YearBW(Int32 valB, Int32 valE)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (period.Year >= valB && period.Year <= valE);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> YearEQ(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (period.Year == val);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> YearLE(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (period.Year <= val);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> YearGE(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (period.Year >= val);
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> IIfVal(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> funcIff, Int32 valPos, Int32 valNeg)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 returnVal = 0;
                if (funcIff(gen, period, ruleset, prevset))
                {
                    returnVal = valPos;
                }
                else
                {
                    returnVal = valNeg;
                }
                return returnVal;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> IIf(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, bool> funcIff, Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> funcPos, Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> funcNeg)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 returnVal = 0;
                if (funcIff(gen, period, ruleset, prevset))
                {
                    returnVal = funcPos(gen, period, ruleset, prevset);
                }
                else
                {
                    returnVal = funcNeg(gen, period, ruleset, prevset);
                }
                return returnVal;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> Div(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func, Int32 div, Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 funcVal = func(gen, period, ruleset, prevset);
                return funcVal/div + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> Sub(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> funcBase, Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> funcSubs, Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 baseVal = funcBase(gen, period, ruleset, prevset);
                Int32 subsVal = funcSubs(gen, period, ruleset, prevset);
                return (baseVal - subsVal) + val;
            };
        }

        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MaxBonus()
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                return ruleset.SalaryProps.MinMonthlyWage + 2000;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MinZdrPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                return prevset.HealthProps.MinMonthlyBasis + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MinZdr(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                return ruleset.HealthProps.MinMonthlyBasis + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MaxZdrPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = prevset.HealthProps.MaxAnnualsBasis;
                if (marginIncome == 0)
                {
                    marginIncome = 2000000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MaxZdr(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = ruleset.HealthProps.MaxAnnualsBasis;
                if (marginIncome == 0)
                {
                    marginIncome = prevset.HealthProps.MaxAnnualsBasis; 
                }
                if (marginIncome == 0)
                {
                    marginIncome = 2000000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MaxSocPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = prevset.SocialProps.MaxAnnualsBasis;
                if (marginIncome == 0)
                {
                    marginIncome = 2000000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> MaxSoc(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = ruleset.SocialProps.MaxAnnualsBasis;
                if (marginIncome == 0)
                {
                    marginIncome = prevset.SocialProps.MaxAnnualsBasis;
                }
                if (marginIncome == 0)
                {
                    marginIncome = 2000000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> SolTaxPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = prevset.TaxingProps.MarginIncomeOfSolidary;
                if (marginIncome == 0)
                {
                    marginIncome = 150000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> SolTax(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = ruleset.TaxingProps.MarginIncomeOfSolidary;
                if (marginIncome == 0)
                {
                    marginIncome = prevset.TaxingProps.MarginIncomeOfSolidary;
                }
                if (marginIncome == 0)
                {
                    marginIncome = 150000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> Rate2TaxPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = prevset.TaxingProps.MarginIncomeOfTaxRate2;
                if (marginIncome == 0)
                {
                    marginIncome = 150000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> Rate2Tax(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = ruleset.TaxingProps.MarginIncomeOfTaxRate2;
                if (marginIncome == 0)
                {
                    marginIncome = prevset.TaxingProps.MarginIncomeOfTaxRate2;
                }
                if (marginIncome == 0)
                {
                    marginIncome = 150000;
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> SrazNepPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = prevset.TaxingProps.MarginIncomeOfWithhold;
                if (marginIncome == 0)
                {
                    marginIncome = 5000;
                }
                return (marginIncome + val);
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> SrazNep(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = ruleset.TaxingProps.MarginIncomeOfWithhold;
                if (marginIncome == 0)
                {
                    marginIncome = prevset.TaxingProps.MarginIncomeOfWithhold;
                }
                if (marginIncome == 0)
                {
                    marginIncome = 5000;
                }
                return (marginIncome + val);
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastZdrPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = HealthParticyIncome(gen.Term, prevset);
                if (marginIncome == 0)
                {
                    marginIncome = HealthDefaultIncome(gen.Term);
                }
                return (marginIncome + val);
            };
        }

        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastZdr(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = HealthParticyIncome(gen.Term, ruleset);
                if (marginIncome == 0)
                {
                    marginIncome = HealthParticyIncome(gen.Term, prevset);
                }
                if (marginIncome == 0)
                {
                    marginIncome = HealthDefaultIncome(gen.Term);
                }
                return (marginIncome + val);
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastNemPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = SocialParticyIncome(gen.Term, prevset);
                if (marginIncome == 0)
                {
                    marginIncome = SocialDefaultIncome(gen.Term);
                }
                return marginIncome + val;
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastNem(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = SocialParticyIncome(gen.Term, ruleset);
                if (marginIncome == 0)
                {
                    marginIncome = SocialParticyIncome(gen.Term, prevset);
                }
                if (marginIncome == 0)
                {
                    marginIncome = SocialDefaultIncome(gen.Term);
                }
                return (marginIncome + val);
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastZdrEmpPrev(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = HealthParticyIncome(gen.Term, prevset);
                if (marginIncome == 0)
                {
                    marginIncome = HealthDefaultIncome(gen.Term);
                }
                return (marginIncome + val);
            };
        }
        protected static Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> UcastZdrEmp(Int32 val)
        {
            return (ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset) => {
                Int32 marginIncome = HealthParticyIncome(gen.Term, ruleset);
                if (marginIncome == 0)
                {
                    marginIncome = HealthParticyIncome(gen.Term, prevset);
                }
                if (marginIncome == 0)
                {
                    marginIncome = HealthDefaultIncome(gen.Term);
                }
                return (marginIncome + val);
            };
        }
#endregion
    }
}
