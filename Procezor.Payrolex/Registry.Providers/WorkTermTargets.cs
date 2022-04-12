using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // ContractWorkTerm		CONTRACT_WORK_TERM
    public class ContractWorkTermTarget : PayrolexTermTarget
    {
        public WorkContractTerms TermType { get; private set; }
        public DateTime? DateFrom { get; private set; }
        public DateTime? DateStop { get; private set; }

        public ContractWorkTermTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            WorkContractTerms termType, DateTime? dateFrom, DateTime? dateStop) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TermType = termType;
            DateFrom = dateFrom;
            DateStop = dateStop;
        }
    }

    // PositionWorkTerm		POSITION_WORK_TERM
    public class PositionWorkTermTarget : PayrolexTermTarget
    {
        public DateTime? DateFrom { get; private set; }
        public DateTime? DateStop { get; private set; }
        public string TermName { get; private set; }
        public PositionWorkTermTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            string termName, DateTime? dateFrom, DateTime? dateStop) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TermName = termName;
            DateFrom = dateFrom;
            DateStop = dateStop;
        }
    }
}
