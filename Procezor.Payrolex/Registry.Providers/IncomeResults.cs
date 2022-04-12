using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // IncomeGross		INCOME_GROSS
    public class IncomeGrossResult : PayrolexTermResult
    {
        public IncomeGrossResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // IncomeNetto		INCOME_NETTO
    public class IncomeNettoResult : PayrolexTermResult
    {
        public IncomeNettoResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }
    // EmployerCosts		EMPLOYER_COSTS
    public class EmployerCostsResult : PayrolexTermResult
    {
        public EmployerCostsResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }
}
