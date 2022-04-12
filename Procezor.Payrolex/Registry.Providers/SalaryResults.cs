using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // PaymentBasis		PAYMENT_BASIS
    public class PaymentBasisResult : PayrolexTermResult
    {
        public PaymentBasisResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // PaymentFixed		PAYMENT_FIXED
    public class PaymentFixedResult : PayrolexTermResult
    {
        public PaymentFixedResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }
}
