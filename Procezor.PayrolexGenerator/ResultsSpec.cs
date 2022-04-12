using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HraveMzdy.Procezor.Payrolex.Generator
{
    class ResultsSpec
    {
        public Int32 HoursPlaned { get; set; }
        public Int32 HoursWorked { get; set; }
        public Int32 HoursAbsenc { get; set; }
        public Int32 NettoIncome { get; set; }
        public Int32 GrossIncome { get; set; }
        public Int32 CostsEmploy { get; set; }
        public Int32 SalaryPayms { get; set; }
        public Int32 HealthPayms { get; set; }
        public Int32 SocialPayms { get; set; }
        public Int32 HealthBasis { get; set; }
        public Int32 SocialBasis { get; set; }
        public Int32 TaxingBasis { get; set; }
        public Int32 TaxingAdvan { get; set; }
        public Int32 TaxingWithl { get; set; }
        public Int32 TaxingRedBa { get; set; }
        public Int32 TaxingRedCc { get; set; }
        public Int32 TaxingFinal { get; set; }
        public Int32 HealthCosts { get; set; }
        public Int32 SocialCosts { get; set; }

        public ResultsSpec()
        {
            HoursPlaned = 0;
            HoursWorked = 0;
            HoursAbsenc = 0;
            NettoIncome = 0;
            GrossIncome = 0;
            CostsEmploy = 0;
            SalaryPayms = 0;
            HealthPayms = 0;
            SocialPayms = 0;
            HealthBasis = 0;
            SocialBasis = 0;
            TaxingBasis = 0;
            TaxingAdvan = 0;
            TaxingWithl = 0;
            TaxingRedBa = 0;
            TaxingRedCc = 0;
            TaxingFinal = 0;
            HealthCosts = 0;
            SocialCosts = 0;
        }
    }
}
