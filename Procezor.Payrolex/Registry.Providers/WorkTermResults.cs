using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // ContractWorkTerm		CONTRACT_WORK_TERM
    class ContractWorkTermResult : PayrolexTermResult
    {
        public WorkContractTerms TermType { get; private set; }
        public Byte TermDayFrom { get; private set; }
        public Byte TermDayStop { get; private set; }
        public ContractWorkTermResult(ITermTarget target, IArticleSpec spec, WorkContractTerms termType, Byte dayTermFrom, Byte dayTermStop) : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            TermType = termType;
            TermDayFrom = dayTermFrom;
            TermDayStop = dayTermStop;
        }
        public override string ResultMessage()
        {
            return $"{TermDayFrom} - {TermDayStop}";
        }
    }

    // PositionWorkTerm		POSITION_WORK_TERM
    class PositionWorkTermResult : PayrolexTermResult
    {
        public string TermName { get; private set; }
        public Byte TermDayFrom { get; private set; }
        public Byte TermDayStop { get; private set; }
        public PositionWorkTermResult(ITermTarget target, IArticleSpec spec, string termName, Byte dayFrom, Byte dayStop) : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            TermName = termName;
            TermDayFrom = dayFrom;
            TermDayStop = dayStop;
        }
        public override string ResultMessage()
        {
            return $"{TermDayFrom} - {TermDayStop}";
        }
    }

}
