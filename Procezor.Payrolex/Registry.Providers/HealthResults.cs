using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Service.Interfaces;
using HraveMzdy.Legalios.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Procezor.Service.Types;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // HealthDeclare		HEALTH_DECLARE
    public class HealthDeclareResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkHealthTerms ContractType { get; private set; }
        public Int16 MandatorBase { get; private set; }

        public HealthDeclareResult(ITermTarget target, IArticleSpec spec, 
            Int16 interestCode, WorkHealthTerms contractType, Int16 mandatorBase) 
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            InterestCode = interestCode;
            ContractType = contractType;
            MandatorBase = mandatorBase;
        }
        public override string ResultMessage()
        {
            return $"Interrest: {this.InterestCode}; Type: {Enum.GetName<WorkHealthTerms>(this.ContractType)}; Mandatory: {this.MandatorBase}";
        }
    }

    // HealthIncome		HEALTH_INCOME
    public class HealthIncomeResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkHealthTerms SubjectType { get; private set; }
        public Int16 MandatorBase { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public HealthIncomeResult(ITermTarget target, ContractCode con, IArticleSpec spec,
            Int16 interestCode, WorkHealthTerms subjectType, Int16 mandatorBase, Int16 particyCode, 
            Int32 value, Int32 basis) : base(target, con, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            MandatorBase = mandatorBase;
            ParticyCode = particyCode;
        }
        public override string ResultMessage()
        {
            return $"Interrest: {this.InterestCode}; Term: {Enum.GetName<WorkHealthTerms>(this.IncomeTerm())}; Mandatory: {this.MandatorBase}; Particy: {this.ParticyCode}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int16 SetParticyCode(Int16 particyCode)
        {
            ParticyCode = particyCode;
            return ParticyCode;
        }
        public WorkHealthTerms IncomeTerm()
        {
            WorkHealthTerms resultTerm = WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS;
            switch (SubjectType)
            {
                case WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS:
                    resultTerm = WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_WORK:
                    resultTerm = WorkHealthTerms.HEALTH_TERM_AGREEM_WORK;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_TASK:
                    resultTerm = WorkHealthTerms.HEALTH_TERM_AGREEM_TASK;
                    break;
                case WorkHealthTerms.HEALTH_TERM_BY_CONTRACT:
                    resultTerm = WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS;
                    break;
            }
            return resultTerm;
        }
        public Int32 TermScore()
        {
            Int32 resultType = 0;
            switch (SubjectType)
            {
                case WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS:
                    resultType = 9000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_WORK:
                    resultType = 5000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_TASK:
                    resultType = 4000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_BY_CONTRACT:
                    resultType = 0;
                    break;
            }
            Int32 mandatorRes = 0;
            if (MandatorBase == 1)
            {
                mandatorRes = 100;
            }
            Int32 interestRes = 0;
            if (InterestCode == 1)
            {
                interestRes = 10000;
            }
            Int32 particyRes = 0;
            if (ParticyCode == 1)
            {
                particyRes = 100000;
            }
            return resultType + mandatorRes + interestRes + particyRes;
        }
        private class IncomeTermComparator : IComparer<HealthIncomeResult>
        {
            public IncomeTermComparator()
            {
            }

            public int Compare(HealthIncomeResult x, HealthIncomeResult y)
            {
                Int32 xScore = x.TermScore();
                Int32 yScore = y.TermScore();

                if (xScore.CompareTo(yScore) == 0)
                {
                    return x.Contract.CompareTo(y.Contract);
                }
                return yScore.CompareTo(xScore);
            }
        }
        public static IComparer<HealthIncomeResult> ResultComparator()
        {
            return new IncomeTermComparator();
        }
    }
    // HealthBase		HEALTH_BASE
    public class HealthBaseResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkHealthTerms SubjectType { get; private set; }
        public Int16 MandatorBase { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public Int32 AnnuityBase { get; private set; }

        public HealthBaseResult(ITermTarget target, IArticleSpec spec,
            Int16 interestCode, WorkHealthTerms subjectType, Int16 mandatorBase, Int16 particyCode, 
            Int32 annuityBase, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            MandatorBase = mandatorBase;
            ParticyCode = particyCode;
            AnnuityBase = annuityBase;
        }
        public override string ResultMessage()
        {
            return $"Annuity Base: {this.AnnuityBase}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // HealthBaseEmployee		HEALTH_BASE_EMPLOYEE
    public class HealthBaseEmployeeResult : PayrolexTermResult
    {
        public HealthBaseEmployeeResult(ITermTarget target, IArticleSpec spec, 
            Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // HealthBaseEmployer		HEALTH_BASE_EMPLOYER
    public class HealthBaseEmployerResult : PayrolexTermResult
    {
        public HealthBaseEmployerResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // HealthBaseMandate		HEALTH_BASE_MANDATE
    public class HealthBaseMandateResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkHealthTerms SubjectType { get; private set; }
        public Int16 MandatorBase { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public HealthBaseMandateResult(ITermTarget target, ContractCode con, IArticleSpec spec,
            Int16 interestCode, WorkHealthTerms subjectType, Int16 mandatorBase, Int16 particyCode, 
            Int32 value, Int32 basis) : base(target, con, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            MandatorBase = mandatorBase;
            ParticyCode = particyCode;
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int32 IncomeScore()
        {
            Int32 resultType = 0;
            switch (SubjectType)
            {
                case WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS:
                    resultType = 9000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_WORK:
                    resultType = 5000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_TASK:
                    resultType = 4000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_BY_CONTRACT:
                    resultType = 0;
                    break;
            }
            Int32 mandatorRes = 0;
            if (MandatorBase == 1)
            {
                mandatorRes = 100;
            }
            Int32 interestRes = 0;
            if (InterestCode == 1)
            {
                interestRes = 10000;
            }
            Int32 particyRes = 0;
            if (ParticyCode == 1)
            {
                particyRes = 100000;
            }
            return resultType + mandatorRes + interestRes + particyRes;
        }
        private class IncomeTermComparator : IComparer<HealthBaseMandateResult>
        {
            public IncomeTermComparator()
            {
            }

            public int Compare(HealthBaseMandateResult x, HealthBaseMandateResult y)
            {
                Int32 xIncomeScore = x.IncomeScore();
                Int32 yIncomeScore = y.IncomeScore();

                if (xIncomeScore.CompareTo(yIncomeScore) == 0)
                {
                    if (x.ResultBasis.CompareTo(y.ResultBasis)==0)
                    {
                        return x.Contract.CompareTo(y.Contract);
                    }
                    return y.ResultBasis.CompareTo(x.ResultBasis);
                }
                return yIncomeScore.CompareTo(xIncomeScore);
            }
        }
        public static IComparer<HealthBaseMandateResult> ResultComparator()
        {
            return new IncomeTermComparator();
        }
    }

    // HealthBaseOvercap		HEALTH_BASE_OVERCAP
    public class HealthBaseOvercapResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkHealthTerms SubjectType { get; private set; }
        public Int16 MandatorBase { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public HealthBaseOvercapResult(ITermTarget target, ContractCode con, IArticleSpec spec,
            Int16 interestCode, WorkHealthTerms subjectType, Int16 mandatorBase, Int16 particyCode, 
            Int32 value, Int32 basis) : base(target, con, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            MandatorBase = mandatorBase;
            ParticyCode = particyCode;
        }
        public override string ResultMessage()
        {
            return $"Type: {Enum.GetName<WorkHealthTerms>(this.SubjectType)}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int32 IncomeScore()
        {
            Int32 resultType = 0;
            switch (SubjectType)
            {
                case WorkHealthTerms.HEALTH_TERM_EMPLOYMENTS:
                    resultType = 9000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_WORK:
                    resultType = 5000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_AGREEM_TASK:
                    resultType = 4000;
                    break;
                case WorkHealthTerms.HEALTH_TERM_BY_CONTRACT:
                    resultType = 0;
                    break;
            }
            Int32 mandatorRes = 0;
            if (MandatorBase == 1)
            {
                mandatorRes = 100;
            }
            Int32 interestRes = 0;
            if (InterestCode == 1)
            {
                interestRes = 10000;
            }
            Int32 particyRes = 0;
            if (ParticyCode == 1)
            {
                particyRes = 100000;
            }
            return resultType + mandatorRes + interestRes + particyRes;
        }
        private class IncomeTermComparator : IComparer<HealthBaseOvercapResult>
        {
            public IncomeTermComparator()
            {
            }

            public int Compare(HealthBaseOvercapResult x, HealthBaseOvercapResult y)
            {
                Int32 xIncomeScore = x.IncomeScore();
                Int32 yIncomeScore = y.IncomeScore();

                if (xIncomeScore.CompareTo(yIncomeScore) == 0)
                {
                    return x.Contract.CompareTo(y.Contract);
                }
                return yIncomeScore.CompareTo(xIncomeScore);
            }
        }
        public static IComparer<HealthBaseOvercapResult> ResultComparator()
        {
            return new IncomeTermComparator();
        }
    }

    // HealthPaymEmployee		HEALTH_PAYM_EMPLOYEE
    public class HealthPaymEmployeeResult : PayrolexTermResult
    {
        public Int32 EmployeeBasis { get; private set; }
        public Int32 GeneralsBasis { get; private set; }
        public HealthPaymEmployeeResult(ITermTarget target, IArticleSpec spec,
            Int32 employeeBasis, Int32 generalsBasis, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
            EmployeeBasis = employeeBasis;
            GeneralsBasis = generalsBasis;
        }
        public override string ResultMessage()
        {
            return $"Employee: {this.EmployeeBasis}; Generals: {this.GeneralsBasis}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int32 TotalBasic()
        {
            return (EmployeeBasis + GeneralsBasis);
        }
    }

    // HealthPaymEmployer		HEALTH_PAYM_EMPLOYER
    public class HealthPaymEmployerResult : PayrolexTermResult
    {
        public Int32 EmployerBasis { get; private set; }
        public Int32 GeneralsBasis { get; private set; }
        public HealthPaymEmployerResult(ITermTarget target, IArticleSpec spec, 
            Int32 employerBasis, Int32 generalsBasis, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
            EmployerBasis = employerBasis;
            GeneralsBasis = generalsBasis;
        }
        public override string ResultMessage()
        {
            return $"Employer: {this.EmployerBasis}; Generals: {this.GeneralsBasis}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int32 TotalBasic()
        {
            return (EmployerBasis + GeneralsBasis);
        }
    }
}
