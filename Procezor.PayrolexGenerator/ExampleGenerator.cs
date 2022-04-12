using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Procezor.Payrolex.Registry.Providers;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Service.Types;

namespace HraveMzdy.Procezor.Payrolex.Generator
{
    public class ExampleGenerator
    {
        public ExampleGenerator(int id, string name, string number)
        {
            Id = id;
            Name = name;
            Number = number;

            ContractList = Array.Empty<ContractGenerator>();
            ChildrenGen = new ChildGenerator();

            DefaultTaxDeclValue = 1;
            DefaultBenPayerValue = 1;
            DefaultBenDisab1Value = 0;
            DefaultBenDisab2Value = 0;
            DefaultBenDisab3Value = 0;
            DefaultBenStudyValue = 0;

            TaxDeclFunc = DefaultTaxDeclFunc;
            BenPayerFunc = DefaultBenPayerFunc;
            BenDisab1Func = DefaultBenDisab1Func;
            BenDisab2Func = DefaultBenDisab2Func;
            BenDisab3Func = DefaultBenDisab3Func;
            BenStudyFunc = DefaultBenStudyFunc;
        }

        private Int32 DefaultTaxDeclFunc(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultTaxDeclValue;
        }
        private Int32 DefaultBenPayerFunc(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultBenPayerValue;
        }
        private Int32 DefaultBenDisab1Func(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultBenDisab1Value;
        }
        private Int32 DefaultBenDisab2Func(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultBenDisab2Value;
        }
        private Int32 DefaultBenDisab3Func(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultBenDisab3Value;
        }
        private Int32 DefaultBenStudyFunc(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultBenStudyValue;
        }

        public int Id { get; }
        public string Name { get; }
        public string Number { get; }
        public ContractGenerator[] ContractList { get; private set; }
        public ChildGenerator ChildrenGen { get; private set; }
        public Int32 DefaultTaxDeclValue { get; private set; }
        public Int32 DefaultBenPayerValue { get; private set; }
        public Int32 DefaultBenDisab1Value { get; private set; }
        public Int32 DefaultBenDisab2Value { get; private set; }
        public Int32 DefaultBenDisab3Value { get; private set; }
        public Int32 DefaultBenStudyValue { get; private set; }

        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> TaxDeclFunc { get; private set; }
        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> BenPayerFunc { get; private set; }
        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> BenDisab1Func { get; private set; }
        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> BenDisab2Func { get; private set; }
        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> BenDisab3Func { get; private set; }
        public Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> BenStudyFunc { get; private set; }
        public bool ExportToOKmzdy { get; private set; }

        public static ExampleGenerator Spec(Int32 id, string name, string number)
        {
            return new ExampleGenerator(id, name, number);
        }

        public ExampleGenerator WithContracts(params ContractGenerator[] contracts)
        {
            ContractList = contracts.Select((x) => { x.Example = this; return x; }).ToArray();
            return this;
        }
        public ExampleGenerator WithChild(ChildGenerator children)
        {
            ChildrenGen = children;
            return this;
        }
        public ExampleGenerator WithTaxDeclVal(Int32 val)
        {
            DefaultTaxDeclValue = val;
            return this;
        }
        public ExampleGenerator WithBenPayerVal(Int32 val)
        {
            DefaultBenPayerValue = val;
            return this;
        }
        public ExampleGenerator WithBenDisab1Val(Int32 val)
        {
            DefaultBenDisab1Value = val;
            return this;
        }
        public ExampleGenerator WithBenDisab2Val(Int32 val)
        {
            DefaultBenDisab2Value = val;
            return this;
        }
        public ExampleGenerator WithBenDisab3Val(Int32 val)
        {
            DefaultBenDisab3Value = val;
            return this;
        }
        public ExampleGenerator WithBenStudyVal(Int32 val)
        {
            DefaultBenStudyValue = val;
            return this;
        }

        public ExampleGenerator WithTaxDecl(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            TaxDeclFunc = func;
            return this;
        }
        public ExampleGenerator WithBenPayer(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            BenPayerFunc = func;
            return this;
        }
        public ExampleGenerator WithBenDisab1(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            BenDisab1Func = func;
            return this;
        }
        public ExampleGenerator WithBenDisab2(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            BenDisab2Func = func;
            return this;
        }
        public ExampleGenerator WithBenDisab3(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            BenDisab3Func = func;
            return this;
        }
        public ExampleGenerator WithBenStudy(Func<ExampleGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            BenStudyFunc = func;
            return this;
        }

        private string TaxPodImp(ExampleGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            string imp = "0";
            if (TaxDeclFunc(gen, period, ruleset, prevset)==1)
            {
                if (BenPayerFunc(gen, period, ruleset, prevset) == 1)
                {
                    imp = "3";
                }
                else
                {
                    imp = "1";
                }
            }
            else
            {
                imp = "0";
            }
            return imp;
        }

        public string[] BuildImportString(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            this.ExportToOKmzdy = true;

            string[] importResult = Array.Empty<string>();

            string[] names = this.Name.Split('_');

            ImportData01 imp01 = new ImportData01()
            {
                IMP_OSC = this.Number,
                IMP01_PRIJ = $"X{this.Number}{names[1]}",
                IMP01_JMENO = $"{names[0]}",
                IMP01_RODPRIJ = $"{names[1]}{this.Number}",
                IMP01_RODCIS = $"{(period.Year-24)%100}08088{this.Number}",
                IMP_ADRESA_OBEC = "Praha",
                IMP_ADRESA_ULICE = "U Remízku",
                IMP_ADRESA_OCIS = "123/12",
                IMP_ADRESA_PSC = "11505",
                IMP01_MISTONAR = "Praha",
            };
            ImportData02 imp02 = new ImportData02()
            {
                IMP_OSC = this.Number,
                IMP02_ZPUSOB = "0",
            };
            ImportData05 imp05 = new ImportData05()
            {
                IMP_OSC = this.Number,
                IMP05_KODZDRAVPOJ = "111",
            };
            ImportData08 imp08 = new ImportData08()
            {
                IMP_OSC = this.Number,
                IMP08_PODEPSAL = this.TaxPodImp(this, period, ruleset, prevset),
                IMP08_INVALIDITA1 = OptionToImp(this.BenDisab1Func(this, period, ruleset, prevset)),
                IMP08_INVALIDITA2 = OptionToImp(this.BenDisab2Func(this, period, ruleset, prevset)),
                IMP08_INVALIDITA3 = OptionToImp(this.BenDisab3Func(this, period, ruleset, prevset)),
            };
            importResult = importResult.Concat(new string[] { imp01.Export(), imp02.Export(), imp05.Export(), imp08.Export() }).ToArray();
            if (this.BenStudyFunc(this, period, ruleset, prevset)==1)
            {
                ImportData10 imp10 = new ImportData10()
                {
                    IMP_OSC = this.Number,
                    IMP_ROK = period.Year.ToString(),
                    IMP10_KONUPLATROK = period.Year.ToString(),
                    IMP10_AKTUALNIOBD = OptionToImp(this.BenStudyFunc(this, period, ruleset, prevset)),
                };
                importResult = importResult.Concat(new string[] { imp10.Export() }).ToArray();
            }

            foreach (var con in ContractList)
            {
                ImportData17 imp17 = new ImportData17()
                {
                    IMP_OSC = this.Number,
                    IMP_POM = con.Number(this.Number),
                    IMP17_CINNOSTSPOJ = con.NemPojCin(),
                    IMP17_DATUMZAC = $"1.1.{period.Year-1}",
                    IMP17_DATUMKON = "",
                    IMP17_PLATCEDANPR = OptionToImp(con.TaxingPayerFunc(con, period, ruleset, prevset)),
                    IMP17_PLATCESPOJ = con.NemPojImp(con, period, ruleset, prevset),
                    IMP17_PLATCEZPOJ = con.ZdrPojImp(con, period, ruleset, prevset),
                    IMP17_MIN_ZP = con.ZdrPojMin(con, period, ruleset, prevset),
                    IMP17_PRIORITC = con.EmpPriorityFunc(con, period, ruleset, prevset),
                };
                Int32 weekValue = con.WeekFunc(con, period, ruleset, prevset) * 60;
                ImportData18 imp18 = new ImportData18()
                {
                    IMP_OSC = this.Number,
                    IMP_POM = con.Number(this.Number),
                    IMP18_ZPODMEN = "0",
                    IMP18_DRUHUVAZ = "0",
                    IMP18_PLNUVAZ = $"{weekValue}",
                    IMP18_SKUTUVAZ = $"{weekValue}",
                };
                ImportData19 imp19 = null;
                Int32 salaryValue = con.SalaryFunc(con, period, ruleset, prevset);
                if (salaryValue!=0)
                {
                    imp19 = new ImportData19()
                    {
                        IMP_OSC = this.Number,
                        IMP_POM = con.Number(this.Number),
                        IMP19_KODMZDA = "1000",
                        IMP19_SAZBAKC = $"{salaryValue}00",
                        IMP19_TRVALE = "A-1",
                    };
                }
                Int32 agreemValue = con.AgreemFunc(con, period, ruleset, prevset);
                if (agreemValue != 0)
                {
                    imp19 = new ImportData19()
                    {
                        IMP_OSC = this.Number,
                        IMP_POM = con.Number(this.Number),
                        IMP19_KODMZDA = "1809",
                        IMP19_SAZBAKC = $"{agreemValue}00",
                        IMP19_TRVALE = "A-1",
                    };
                }
                importResult = importResult.Concat(new string[] { imp17.Export(), imp18.Export() }).ToArray();
                if (imp19 != null)
                {
                    importResult = importResult.Concat(new string[] { imp19.Export() }).ToArray();
                }
            }
            if (TaxDeclFunc(this, period, ruleset, prevset)==1)
            {
                var childrenList = ChildrenGen.Build(period, ruleset, prevset);
                foreach (var child in childrenList)
                {
                    string[] cnames = child.Name.Split(' ');
                    ImportData09 imp09 = new ImportData09()
                    {
                        IMP_OSC = this.Number,
                        IMP_ROK = period.Year.ToString(),
                        IMP09_JMENO = cnames[1],
                        IMP09_PRIJ = cnames[0],
                        IMP09_DATUMNAR = "9.9.2009",
                        IMP09_RODCIS = $"09090990{child.Id}",
                        IMP09_AKTUALNIOBD = BoolToImp(child.TaxBenefitChild || child.TaxBenefitDisab),
                        IMP09_PLATNOSTOBD = "#",
                        IMP09_AKTUALNIPOR = child.TaxBenefitOrder.ToString(),
                        IMP09_PLATNOSTPOR = "#",
                        IMP09_ZTPP = BoolToImp(child.TaxBenefitDisab),
                    };
                    importResult = importResult.Concat(new string[] { imp09.Export() }).ToArray();
                }
            }
            return importResult;
        }

        private string BoolToImp(bool option)
        {
            if (option)
            {
                return "1";
            }
            return "0";
        }

        private Int16 BoolToNumber(bool option)
        {
            if (option)
            {
                return 1;
            }
            return 0;
        }
        public TaxDeclBenfOption TaxBenef(bool option)
        {
            return option ? TaxDeclBenfOption.DECL_TAX_BENEF1 : TaxDeclBenfOption.DECL_TAX_BENEF0;
        }

        private string OptionToImp(Int32 option)
        {
            if (option == 1)
            {
                return "1";
            }
            return "0";
        }

        public IEnumerable<ITermTarget> BuildSpecTargets(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            var targets = Array.Empty<ITermTarget>();

            var montCode = MonthCode.Get(period.Code);

            Int16 CONTRACT_NULL = 0;
            Int16 POSITION_NULL = 0;

            var contractEmp = ContractCode.Get(CONTRACT_NULL);
            var positionEmp = PositionCode.Get(POSITION_NULL);
            var variant1Emp = VariantCode.Get(1);

            foreach (var con in ContractList)
            {
                Int16 CONTRACT_CODE = (Int16)con.Id;
                Int16 POSITION_CODE = 1;

                DateTime? dateTermFrom = new DateTime(period.Year-1, 1, 1);
                DateTime? dateTermStop = null;

                var contractCon = ContractCode.Get(CONTRACT_CODE);
                var positionCon = PositionCode.Get(POSITION_CODE);
                var variant1Con = VariantCode.Get(1);

                var targetCon = new ContractWorkTermTarget(montCode, contractCon, positionEmp, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_CONTRACT_WORK_TERM),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_CONTRACT_WORK_TERM),
                    con.Term, dateTermFrom, dateTermStop);
                var targetPos = new PositionWorkTermTarget(montCode, contractCon, positionCon, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_TERM),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_TERM),
                    con.Number(this.Number), dateTermFrom, dateTermStop);
                var targetPWP = new PositionWorkPlanTarget(montCode, contractCon, positionCon, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_POSITION_WORK_PLAN),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_POSITION_WORK_PLAN),
                    WorkScheduleType.SCHEDULE_NORMALY_WEEK, 5, 8, 8);
                Int32 salaryValue = con.SalaryFunc(con, period, ruleset, prevset);
                var targetSAL = new PaymentBasisTarget(montCode, contractCon, positionCon, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_PAYMENT_SALARY),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_PAYMENT_BASIS),
                    salaryValue);
                Int32 agreemValue = con.AgreemFunc(con, period, ruleset, prevset);
                var targetAGR = new PaymentFixedTarget(montCode, contractCon, positionCon, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_PAYMENT_WORKED),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_PAYMENT_FIXED),
                    agreemValue);
                Int32 healthPayer = con.HealthPayerFunc(con, period, ruleset, prevset);
                Int32 healthMinim = con.HealthMinimFunc(con, period, ruleset, prevset);
                var targetHth = new HealthDeclareTarget(montCode, contractCon, positionEmp, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_HEALTH_DECLARE),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_HEALTH_DECLARE),
                    (Int16)healthPayer, WorkHealthTerms.HEALTH_TERM_BY_CONTRACT, (Int16)healthMinim);
                Int32 socialPayer = con.SocialPayerFunc(con, period, ruleset, prevset);
                Int32 socialLoInc = con.SocialLoIncomeFunc(con, period, ruleset, prevset);
                WorkSocialTerms socialTerms = WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT;
                if (socialLoInc == 1)
                {
                    socialTerms = WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL;
                }
                var targetSoc = new SocialDeclareTarget(montCode, contractCon, positionEmp, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_SOCIAL_DECLARE),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_SOCIAL_DECLARE),
                    (Int16)socialPayer, socialTerms);
                Int32 taxingPayer = con.TaxingPayerFunc(con, period, ruleset, prevset);
                var targetTax = new TaxingDeclareTarget(montCode, contractCon, positionEmp, variant1Con,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_DECLARE),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_DECLARE),
                    (Int16)taxingPayer, WorkTaxingTerms.TAXING_TERM_BY_CONTRACT);

                targets = targets.Concat(new ITermTarget[] { targetCon, targetPos, targetPWP, targetHth, targetSoc, targetTax }).ToArray();
                if (salaryValue!=0)
                {
                    targets = targets.Concat(new ITermTarget[] { targetSAL }).ToArray();
                }
                if (agreemValue!=0)
                {
                    targets = targets.Concat(new ITermTarget[] { targetAGR }).ToArray();
                }
            }
            var targetSgn = new TaxingSigningTarget(montCode, contractEmp, positionEmp, variant1Emp,
                ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_SIGNING),
                ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_SIGNING),
                TaxSigning(period, ruleset, prevset), TaxSignNon(period, ruleset, prevset));

            targets = targets.Concat(new ITermTarget[] { targetSgn }).ToArray();

            Int32 taxDecPayer = TaxDeclFunc(this, period, ruleset, prevset);
            Int32 taxBenPayer = BenPayerFunc(this, period, ruleset, prevset);
            if (taxDecPayer == 1 && taxBenPayer==1)
            {
                var targetAlw = new TaxingAllowancePayerTarget(montCode, contractEmp, positionEmp, variant1Emp,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_PAYER),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_PAYER),
                    TaxBenPayer(period, ruleset, prevset));
                targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
            }
            Int32 taxBenDisb1 = BenDisab1Func(this, period, ruleset, prevset);
            if (taxDecPayer == 1 && taxBenDisb1==1)
            {
                var variant1Dis = VariantCode.Get(1);
                var targetAlw = new TaxingAllowanceDisabTarget(montCode, contractEmp, positionEmp, variant1Dis,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_DISAB),
                    TaxDisab1(period, ruleset, prevset));
                targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
            }
            Int32 taxBenDisb2 = BenDisab2Func(this, period, ruleset, prevset);
            if (taxDecPayer == 1 && taxBenDisb2==1)
            {
                var variant1Dis = VariantCode.Get(2);
                var targetAlw = new TaxingAllowanceDisabTarget(montCode, contractEmp, positionEmp, variant1Dis,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_DISAB),
                    TaxDisab2(period, ruleset, prevset));
                targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
            }
            Int32 taxBenDisb3 = BenDisab3Func(this, period, ruleset, prevset);
            if (taxDecPayer == 1 && taxBenDisb3==1)
            {
                var variant1Dis = VariantCode.Get(3);
                var targetAlw = new TaxingAllowanceDisabTarget(montCode, contractEmp, positionEmp, variant1Dis,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_DISAB),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_DISAB),
                    TaxDisab3(period, ruleset, prevset));
                targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
            }
            Int32 taxBenStudy = BenStudyFunc(this, period, ruleset, prevset);
            if (taxDecPayer == 1 && taxBenStudy==1)
            {
                var targetAlw = new TaxingAllowanceStudyTarget(montCode, contractEmp, positionEmp, variant1Emp,
                    ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_STUDY),
                    ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_STUDY),
                    TaxBenStudy(period, ruleset, prevset));
                targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
            }

            if (taxDecPayer == 1)
            {
                var childrenList = ChildrenGen.Build(period, ruleset, prevset);
                foreach (var child in childrenList)
                {
                    var variantChld = VariantCode.Get(child.Id);
                    var targetAlw = new TaxingAllowanceChildTarget(montCode, contractEmp, positionEmp, variantChld,
                        ArticleCode.Get((Int32)PayrolexArticleConst.ARTICLE_TAXING_ALLOWANCE_CHILD),
                        ConceptCode.Get((Int32)PayrolexConceptConst.CONCEPT_TAXING_ALLOWANCE_CHILD),
                        TaxBenef(child.TaxBenefitChild || child.TaxBenefitDisab), BoolToNumber(child.TaxBenefitDisab), child.TaxBenefitOrder);
                    targets = targets.Concat(new ITermTarget[] { targetAlw }).ToArray();
                }
            }

            return targets;
        }

        private TaxDeclSignOption TaxSigning(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = TaxDeclFunc(this, period, ruleset, prevset);
            return val==1 ? TaxDeclSignOption.DECL_TAX_DO_SIGNED : TaxDeclSignOption.DECL_TAX_NO_SIGNED;
        }
        private TaxNoneSignOption TaxSignNon(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = TaxDeclFunc(this, period, ruleset, prevset);
            return val==1 ? TaxNoneSignOption.NOSIGN_TAX_ADVANCES : TaxNoneSignOption.NOSIGN_TAX_WITHHOLD;
        }
        public TaxDeclBenfOption TaxBenPayer(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = BenPayerFunc(this, period, ruleset, prevset);
            return val==1 ? TaxDeclBenfOption.DECL_TAX_BENEF1 : TaxDeclBenfOption.DECL_TAX_BENEF0;
        }
        public TaxDeclBenfOption TaxBenStudy(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = BenPayerFunc(this, period, ruleset, prevset);
            return val==1 ? TaxDeclBenfOption.DECL_TAX_BENEF1 : TaxDeclBenfOption.DECL_TAX_BENEF0;
        }
        public TaxDeclDisabOption TaxDisab1(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = BenDisab1Func(this, period, ruleset, prevset);
            return val==1 ? TaxDeclDisabOption.DECL_TAX_DISAB1 : TaxDeclDisabOption.DECL_TAX_BENEF0;
        }
        public TaxDeclDisabOption TaxDisab2(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = BenDisab2Func(this, period, ruleset, prevset);
            return val==1 ? TaxDeclDisabOption.DECL_TAX_DISAB2 : TaxDeclDisabOption.DECL_TAX_BENEF0;
        }
        public TaxDeclDisabOption TaxDisab3(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 val = BenDisab3Func(this, period, ruleset, prevset);
            return val==1 ? TaxDeclDisabOption.DECL_TAX_DISAB3 : TaxDeclDisabOption.DECL_TAX_BENEF0;
        }
    }

    public class ContractGenerator
    {
        public ExampleGenerator Example { get; set; }
        public int Id { get; }
        public string Number(string number)
        {
            return $"{number}-{Id}";
        }
        public WorkContractTerms Term { get; set; }

        public Int32 DefaultEmpPriorityValue { get; private set; }
        public Int32 DefaultWeekValue { get; private set; }
        public Int32 DefaultAbsenceValue { get; private set; }
        public Int32 DefaultSalaryValue { get; private set; }
        public Int32 DefaultAgreemValue { get; private set; }
        public Int32 DefaultHealthPayerValue { get; private set; }
        public Int32 DefaultHealthMinimValue { get; private set; }
        public Int32 DefaultHealthForeignValue { get; private set; }
        public Int32 DefaultHealthForeignEhsValue { get; private set; }
        public Int32 DefaultHealthEmperValue { get; private set; }
        public Int32 DefaultSocialPayerValue { get; private set; }
        public Int32 DefaultSocialLoIncomeValue { get; private set; }
        public Int32 DefaultSocialForeignValue { get; private set; }
        public Int32 DefaultSocialForeignEhsValue { get; private set; }
        public Int32 DefaultSocialEmperValue { get; private set; }
        public Int32 DefaultPenzisPayerValue { get; private set; }
        public Int32 DefaultTaxingPayerValue { get; private set; }


        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> EmpPriorityFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> WeekFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> AbsenceFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SalaryFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> AgreemFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> HealthPayerFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> HealthMinimFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> HealthForeignFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> HealthForeignEhsFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> HealthEmperFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SocialPayerFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SocialLoIncomeFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SocialForeignFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SocialForeignEhsFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> SocialEmperFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> PenzisPayerFunc { get; private set; }
        public Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, int> TaxingPayerFunc { get; private set; }

        public ContractGenerator(int id, WorkContractTerms term)
        {
            Id = id;
            Term = term;
            DefaultEmpPriorityValue = 0;
            DefaultWeekValue = 40;
            DefaultAbsenceValue = 0;
            DefaultSalaryValue = 15000;
            DefaultAgreemValue = 0;
            DefaultHealthPayerValue = 1;
            DefaultHealthEmperValue = 1;
            DefaultHealthMinimValue = 1;
            DefaultHealthForeignValue = 0;
            DefaultHealthForeignEhsValue = 0;
            DefaultSocialPayerValue = 1;
            DefaultSocialLoIncomeValue = 0;
            DefaultSocialForeignValue = 0;
            DefaultSocialForeignEhsValue = 0;
            DefaultSocialEmperValue = 1;
            DefaultPenzisPayerValue = 0;
            DefaultTaxingPayerValue = 1;

            EmpPriorityFunc = DefaultEmpPriorityFunc;
            WeekFunc = DefaultWeekFunc;
            AbsenceFunc = DefaultAbsenceFunc;
            SalaryFunc = DefaultSalaryFunc;
            AgreemFunc = DefaultAgreemFunc;
            HealthPayerFunc = DefaultHealthPayerFunc;
            HealthMinimFunc = DefaultHealthMinimFunc;
            HealthForeignFunc = DefaultHealthForeignFunc;
            HealthForeignEhsFunc = DefaultHealthForeignEhsFunc;
            HealthEmperFunc = DefaultHealthEmperFunc;
            SocialPayerFunc = DefaultSocialPayerFunc;
            SocialLoIncomeFunc = DefaultSocialLoIncomeFunc;
            SocialForeignFunc = DefaultSocialForeignFunc;
            SocialForeignEhsFunc = DefaultSocialForeignEhsFunc;
            SocialEmperFunc = DefaultSocialEmperFunc;
            PenzisPayerFunc = DefaultPenzisPayerFunc;
            TaxingPayerFunc = DefaultTaxingPayerFunc;

        }
        public string TermChar()
        {
            switch (Term)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    return "1";
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    return "A";
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    return "T";
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    return "Q";
            }
            return "0";
        }
        public string NemPojCin()
        {
            string imp = "0";
            switch (this.Term)
            {
                case WorkContractTerms.WORKTERM_EMPLOYMENT_1:
                    imp = "1";
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_A:
                    imp = "10";
                    break;
                case WorkContractTerms.WORKTERM_CONTRACTER_T:
                    imp = "30";
                    break;
                case WorkContractTerms.WORKTERM_PARTNER_STAT:
                    imp = "26";
                    break;
            }
            return imp;
        }
        public string NemPojImp(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            string imp = "0";
            if (SocialPayerFunc(gen, period, ruleset, prevset)==1)
            {
                if (SocialLoIncomeFunc(gen, period, ruleset, prevset)==1)
                {
                    //const int ZAMESTNANI09_ZAMESTPP = 0;
                    //const int ZAMESTNANI09_MALEROZS = 1;
                    //const int ZAMESTNANI09_KRATKE01 = 2;
                    //const int ZAMESTNANI09_KRATKE00 = 3;
                    //const int ZAMESTNANI09_KRATKE02 = 4;
                    if (SocialForeignFunc(gen, period, ruleset, prevset)==1)
                    {
                        imp = "12";
                    }
                    else if (SocialForeignEhsFunc(gen, period, ruleset, prevset)==1)
                    {
                        imp = "19";
                    }
                    else
                    {
                        imp = "11";
                    }
                }
                else
                {
                    if (SocialForeignFunc(gen, period, ruleset, prevset)==1)
                    {
                        imp = "2";
                    }
                    else if (SocialForeignEhsFunc(gen, period, ruleset, prevset)==1)
                    {
                        imp = "9";
                    }
                    else
                    {
                        imp = "1";
                    }
                }
            }
            return imp;
        }
        public string ZdrPojImp(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            string imp = "0";
            if (HealthPayerFunc(gen, period, ruleset, prevset)==1)
            {
                if (HealthForeignFunc(gen, period, ruleset, prevset)==1)
                {
                    imp = "2";
                }
                else
                if (HealthForeignEhsFunc(gen, period, ruleset, prevset)==1)
                {
                    imp = "9";
                }
                else
                {
                    imp = "1";
                }
            }
            return imp;
        }
        public string ZdrPojMin(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            string imp = "0";
            if (HealthPayerFunc(gen, period, ruleset, prevset)==1)
            {
                if (HealthMinimFunc(gen, period, ruleset, prevset)==1)
                {
                    imp = "1";
                }
            }
            return imp;
        }

        private Int32 DefaultEmpPriorityFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultEmpPriorityValue;
        }
        private Int32 DefaultWeekFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultWeekValue;
        }
        private Int32 DefaultAbsenceFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultAbsenceValue;
        }
        private Int32 DefaultSalaryFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSalaryValue;
        }
        private Int32 DefaultAgreemFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultAgreemValue;
        }
        private Int32 DefaultHealthPayerFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultHealthPayerValue;
        }
        private Int32 DefaultHealthEmperFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultHealthEmperValue;
        }
        private Int32 DefaultHealthMinimFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            if (HealthPayerFunc(gen, period, ruleset, prevset)==1)
            {
                return DefaultHealthMinimValue;
            }
            return 0;
        }
        private Int32 DefaultHealthForeignFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultHealthForeignValue;
        }
        private Int32 DefaultHealthForeignEhsFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultHealthForeignEhsValue;
        }
        private Int32 DefaultSocialPayerFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSocialPayerValue;
        }
        private Int32 DefaultSocialLoIncomeFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSocialLoIncomeValue;
        }
        private Int32 DefaultSocialForeignFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSocialForeignValue;
        }
        private Int32 DefaultSocialForeignEhsFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSocialForeignEhsValue;
        }
        private Int32 DefaultSocialEmperFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultSocialEmperValue;
        }
        private Int32 DefaultPenzisPayerFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultPenzisPayerValue;
        }
        private Int32 DefaultTaxingPayerFunc(ContractGenerator gen, IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultTaxingPayerValue;
        }

        public static ContractGenerator SpecEmp(Int32 id)
        {
            return new ContractGenerator(id, WorkContractTerms.WORKTERM_EMPLOYMENT_1);
        }
        public static ContractGenerator SpecDpc(Int32 id)
        {
            return new ContractGenerator(id, WorkContractTerms.WORKTERM_CONTRACTER_A);
        }
        public static ContractGenerator SpecDpp(Int32 id)
        {
            return new ContractGenerator(id, WorkContractTerms.WORKTERM_CONTRACTER_T);
        }
        public static ContractGenerator SpecStat(Int32 id)
        {
            return new ContractGenerator(id, WorkContractTerms.WORKTERM_PARTNER_STAT);
        }
        public ContractGenerator WithPriorityVal(Int32 val)
        {
            DefaultEmpPriorityValue = val;
            return this;
        }
        public ContractGenerator WithWeekVal(Int32 val)
        {
            DefaultWeekValue = val;
            return this;
        }
        public ContractGenerator WithAbsenceVal(Int32 val)
        {
            DefaultAbsenceValue = val;
            return this;
        }
        public ContractGenerator WithSalaryVal(Int32 val)
        {
            DefaultSalaryValue = val;
            return this;
        }
        public ContractGenerator WithAgreemVal(Int32 val)
        {
            DefaultAgreemValue = val;
            return this;
        }
        public ContractGenerator WithHealthPayerVal(Int32 val)
        {
            DefaultHealthPayerValue = val;
            return this;
        }
        public ContractGenerator WithHealthMinimVal(Int32 val)
        {
            DefaultHealthMinimValue = val;
            return this;
        }
        public ContractGenerator WithHealthEmperVal(Int32 val)
        {
            DefaultHealthEmperValue = val;
            return this;
        }
        public ContractGenerator WithSocialPayerVal(Int32 val)
        {
            DefaultSocialPayerValue = val;
            return this;
        }
        public ContractGenerator WithSocialLoIncomeVal(Int32 val)
        {
            DefaultSocialLoIncomeValue = val;
            return this;
        }
        public ContractGenerator WithSocialEmperVal(Int32 val)
        {
            DefaultSocialEmperValue = val;
            return this;
        }
        public ContractGenerator WithPenzisPayerVal(Int32 val)
        {
            DefaultPenzisPayerValue = val;
            return this;
        }
        public ContractGenerator WithTaxingPayerVal(Int32 val)
        {
            DefaultTaxingPayerValue = val;
            return this;
        }


        public ContractGenerator WithPriority(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            EmpPriorityFunc = func;
            return this;
        }
        public ContractGenerator WithWeek(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            WeekFunc = func;
            return this;
        }
        public ContractGenerator WithAbsence(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            AbsenceFunc = func;
            return this;
        }
        public ContractGenerator WithSalary(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            SalaryFunc = func;
            return this;
        }
        public ContractGenerator WithAgreem(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            AgreemFunc = func;
            return this;
        }
        public ContractGenerator WithHealthPayer(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            HealthPayerFunc = func;
            return this;
        }
        public ContractGenerator WithHealthMinim(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            HealthMinimFunc = func;
            return this;
        }
        public ContractGenerator WithHealthEmper(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            HealthEmperFunc = func;
            return this;
        }
        public ContractGenerator WithSocialPayer(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            SocialPayerFunc = func;
            return this;
        }
        public ContractGenerator WithSocialLoIncome(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            SocialLoIncomeFunc = func;
            return this;
        }
        public ContractGenerator WithSocialEmper(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            SocialEmperFunc = func;
            return this;
        }
        public ContractGenerator WithPenzisPayer(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            PenzisPayerFunc = func;
            return this;
        }
        public ContractGenerator WithTaxingPayer(Func<ContractGenerator, IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            TaxingPayerFunc = func;
            return this;
        }
    }
    public class ChildData
    {
        public ChildData()
        {
            Id = 0;
            Name = "";
            TaxBenefitChild = false;
            TaxBenefitDisab = false;
            TaxBenefitOrder = 0;
        }

        public Int16 Id { get; set; }
        public string Name { get; set; }
        public bool TaxBenefitChild { get; set; }
        public bool TaxBenefitDisab { get; set; }
        public Int16 TaxBenefitOrder { get; set; }
    }
    public class ChildGenerator
    {
        public Int32 DefaultNorm1Value { get; private set; }
        public Int32 DefaultNorm2Value { get; private set; }
        public Int32 DefaultNorm3Value { get; private set; }
        public Int32 DefaultDisb1Value { get; private set; }
        public Int32 DefaultDisb2Value { get; private set; }
        public Int32 DefaultDisb3Value { get; private set; }

        public Func<IPeriod, IBundleProps, IBundleProps, int> Norm1Func { get; private set; }
        public Func<IPeriod, IBundleProps, IBundleProps, int> Norm2Func { get; private set; }
        public Func<IPeriod, IBundleProps, IBundleProps, int> Norm3Func { get; private set; }
        public Func<IPeriod, IBundleProps, IBundleProps, int> Disb1Func { get; private set; }
        public Func<IPeriod, IBundleProps, IBundleProps, int> Disb2Func { get; private set; }
        public Func<IPeriod, IBundleProps, IBundleProps, int> Disb3Func { get; private set; }
        public ChildGenerator()
        {
            DefaultNorm1Value = 0;
            DefaultNorm2Value = 0;
            DefaultNorm3Value = 0;
            DefaultDisb1Value = 0;
            DefaultDisb2Value = 0;
            DefaultDisb3Value = 0;

            Norm1Func = DefaultNorm1Func;
            Norm2Func = DefaultNorm2Func;
            Norm3Func = DefaultNorm3Func;
            Disb1Func = DefaultDisb1Func;
            Disb2Func = DefaultDisb2Func;
            Disb3Func = DefaultDisb3Func;
        }

        private Int32 DefaultNorm1Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultNorm1Value;
        }
        private Int32 DefaultNorm2Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultNorm2Value;
        }
        private Int32 DefaultNorm3Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultNorm3Value;
        }
        private Int32 DefaultDisb1Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultDisb1Value;
        }
        private Int32 DefaultDisb2Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultDisb2Value;
        }
        private Int32 DefaultDisb3Func(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            return DefaultDisb3Value;
        }

        private static Func<IPeriod, IBundleProps, IBundleProps, Int32> IntValue(Int32 val)
        {
            return (IPeriod period, IBundleProps ruleset, IBundleProps prevset) => (val);
        }
        public static ChildGenerator Spec()
        {
            return new ChildGenerator();
        }
        public static ChildGenerator SpecNorm(Int32 val1, Int32 val2, Int32 val3)
        {
            return new ChildGenerator().WithNorm1Val(val1).WithNorm2Val(val2).WithNorm3Val(val3);
        }
        public static ChildGenerator SpecNorm1(Int32 val)
        {
            return new ChildGenerator().WithNorm1Val(val);
        }
        public static ChildGenerator SpecNorm2(Int32 val)
        {
            return new ChildGenerator().WithNorm2Val(val);
        }
        public static ChildGenerator SpecNorm3(Int32 val)
        {
            return new ChildGenerator().WithNorm3Val(val);
        }
        public static ChildGenerator SpecDisb(Int32 val1, Int32 val2, Int32 val3)
        {
            return new ChildGenerator().WithDisb1Val(val1).WithDisb2Val(val2).WithDisb3Val(val3);
        }
        public static ChildGenerator SpecDisb1(Int32 val)
        {
            return new ChildGenerator().WithDisb1Val(val);
        }
        public static ChildGenerator SpecDisb2(Int32 val)
        {
            return new ChildGenerator().WithDisb2Val(val);
        }
        public static ChildGenerator SpecDisb3(Int32 val)
        {
            return new ChildGenerator().WithDisb3Val(val);
        }
        public ChildGenerator WithNorm1Val(Int32 val)
        {
            DefaultNorm1Value = val;
            return this;
        }
        public ChildGenerator WithNorm2Val(Int32 val)
        {
            DefaultNorm2Value = val;
            return this;
        }
        public ChildGenerator WithNorm3Val(Int32 val)
        {
            DefaultNorm3Value = val;
            return this;
        }
        public ChildGenerator WithDisb1Val(Int32 val)
        {
            DefaultDisb1Value = val;
            return this;
        }
        public ChildGenerator WithDisb2Val(Int32 val)
        {
            DefaultDisb2Value = val;
            return this;
        }
        public ChildGenerator WithDisb3Val(Int32 val)
        {
            DefaultDisb3Value = val;
            return this;
        }

        public ChildGenerator WithNorm1(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Norm1Func = func;
            return this;
        }
        public ChildGenerator WithNorm2(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Norm2Func = func;
            return this;
        }
        public ChildGenerator WithNorm3(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Norm3Func = func;
            return this;
        }
        public ChildGenerator WithDisb1(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Disb1Func = func;
            return this;
        }
        public ChildGenerator WithDisb2(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Disb2Func = func;
            return this;
        }
        public ChildGenerator WithDisb3(Func<IPeriod, IBundleProps, IBundleProps, Int32> func)
        {
            Disb3Func = func;
            return this;
        }

        public ChildData[] Build(IPeriod period, IBundleProps ruleset, IBundleProps prevset)
        {
            Int32 taxChildPor1 = Norm1Func(period, ruleset, prevset);
            Int32 taxChildPor2 = Norm2Func(period, ruleset, prevset);
            Int32 taxChildPor3 = Norm3Func(period, ruleset, prevset);

            Int32 disChildPor1 = Disb1Func(period, ruleset, prevset);
            Int32 disChildPor2 = Disb2Func(period, ruleset, prevset);
            Int32 disChildPor3 = Disb3Func(period, ruleset, prevset);

            ChildData[] children = Array.Empty<ChildData>();
            for (int por1 = 0; por1 < taxChildPor1; por1++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por1 + 1 + 10)),
                    Name = $"Poradi1{por1 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = false,
                    TaxBenefitOrder = 0,
                }).ToArray();
            }
            for (int por2 = 0; por2 < taxChildPor2; por2++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por2 + 1 + 20)),
                    Name = $"Poradi2{por2 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = false,
                    TaxBenefitOrder = 1,
                }).ToArray();
            }
            for (int por3 = 0; por3 < taxChildPor3; por3++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por3 + 1 + 30)),
                    Name = $"Poradi3{por3 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = false,
                    TaxBenefitOrder = 2,
                }).ToArray();
            }
            for (int por1 = 0; por1 < disChildPor1; por1++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por1 + 1 + 40)),
                    Name = $"ZtpPoradi1{por1 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = true,
                    TaxBenefitOrder = 0,
                }).ToArray();
            }
            for (int por2 = 0; por2 < disChildPor2; por2++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por2 + 1 + 50)),
                    Name = $"ZtpPoradi2{por2 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = true,
                    TaxBenefitOrder = 1,
                }).ToArray();
            }
            for (int por3 = 0; por3 < disChildPor3; por3++)
            {
                children = children.Append(new ChildData
                {
                    Id = ((Int16)(por3 + 1 + 60)),
                    Name = $"ZtpPoradi3{por3 + 1} Dite",
                    TaxBenefitChild = true,
                    TaxBenefitDisab = true,
                    TaxBenefitOrder = 2,
                }).ToArray();
            }
            return children;
        }
    }
}
