using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // PaymentBasis		PAYMENT_BASIS
    public class PaymentBasisTarget : PayrolexTermTarget
    {
        public PaymentBasisTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 monthBasis) :
            base(monthCode, contract, position, variant, article, concept, monthBasis)
        {
        }
    }

    // PaymentFixed		PAYMENT_FIXED
    public class PaymentFixedTarget : PayrolexTermTarget
    {
        public PaymentFixedTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 fixedBasis) :
            base(monthCode, contract, position, variant, article, concept, fixedBasis)
        {
        }
    }
}
