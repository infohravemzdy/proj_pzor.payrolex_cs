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
    // SocialDeclare		SOCIAL_DECLARE
    public class SocialDeclareResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkSocialTerms ContractType { get; private set; }
        public SocialDeclareResult(ITermTarget target, IArticleSpec spec,
            Int16 interestCode, WorkSocialTerms contractType) 
            : base(target, spec, VALUE_ZERO, BASIS_ZERO)
        {
            InterestCode = interestCode;
            ContractType = contractType;
        }
        public override string ResultMessage()
        {
            return $"Interrest: {this.InterestCode}; Type: {Enum.GetName<WorkSocialTerms>(this.ContractType)}";
        }
    }

    // SocialIncome		SOCIAL_INCOME
    public class SocialIncomeResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkSocialTerms SubjectType { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public SocialIncomeResult(ITermTarget target, ContractCode con, IArticleSpec spec,
            Int16 interestCode, WorkSocialTerms subjectType, Int16 particyCode, 
            Int32 value, Int32 basis) : base(target, con, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            ParticyCode = particyCode;
        }
        public override string ResultMessage()
        {
            return $"Interrest: {this.InterestCode}; Term: {Enum.GetName<WorkSocialTerms>(this.IncomeTerm())}; Particy: {this.ParticyCode}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int16 SetParticyCode(Int16 particyCode)
        {
            ParticyCode = particyCode;
            return ParticyCode;
        }
        public Int16 HasSubjectTermZMR()
        {
            switch (SubjectType)
            {
                case WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL:
                    return 1;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT:
                    return 0;
            }
            return 0;
        }
        public Int16 HasSubjectTermZKR()
        {
            switch (SubjectType)
            {
                case WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET:
                    return 1;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY:
                    return 1;
                case WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK:
                    return 0;
                case WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT:
                    return 0;
            }
            return 0;
        }
        public WorkSocialTerms IncomeTerm()
        {
            WorkSocialTerms resultTerm = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
            switch (SubjectType)
            {
                case WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT:
                    resultTerm = WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS;
                    break;
            }
            return resultTerm;
        }
        public Int32 TermScore()
        {
            Int32 resultType = 0;
            switch (SubjectType)
            {
                case WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS:
                    resultType = 9000;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL:
                    resultType = 5000;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET:
                    resultType = 4000;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY:
                    resultType = 0;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT:
                    resultType = 0;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK:
                    resultType = 0;
                    break;
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
            return resultType + interestRes + particyRes;
        }
        private class IncomeTermComparator : IComparer<SocialIncomeResult>
        {
            public IncomeTermComparator()
            {
            }

            public int Compare(SocialIncomeResult x, SocialIncomeResult y)
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
        public static IComparer<SocialIncomeResult> ResultComparator()
        {
            return new IncomeTermComparator();
        }
    }

    // SocialBase		SOCIAL_BASE
    public class SocialBaseResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkSocialTerms SubjectType { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public Int32 AnnuityBase { get; private set; }
        public SocialBaseResult(ITermTarget target, IArticleSpec spec,
            Int16 interestCode, WorkSocialTerms subjectType, Int16 particyCode, 
            Int32 annuityBase, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            ParticyCode = particyCode;
            AnnuityBase = annuityBase;
        }
        public override string ResultMessage()
        {
            return $"Annuity Base: {this.AnnuityBase}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // SocialBaseEmployee		SOCIAL_BASE_EMPLOYEE
    public class SocialBaseEmployeeResult : PayrolexTermResult
    {
        public SocialBaseEmployeeResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // SocialBaseEmployer		SOCIAL_BASE_EMPLOYER
    public class SocialBaseEmployerResult : PayrolexTermResult
    {
        public SocialBaseEmployerResult(ITermTarget target, IArticleSpec spec, Int32 value, Int32 basis) : base(target, spec, value, basis)
        {
        }
        public override string ResultMessage()
        {
            return $"Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
    }

    // SocialBaseOvercap		SOCIAL_BASE_OVERCAP
    public class SocialBaseOvercapResult : PayrolexTermResult
    {
        public Int16 InterestCode { get; private set; }
        public WorkSocialTerms SubjectType { get; private set; }
        public Int16 ParticyCode { get; private set; }
        public SocialBaseOvercapResult(ITermTarget target, ContractCode con, IArticleSpec spec,
            Int16 interestCode, WorkSocialTerms subjectType, Int16 particyCode, 
            Int32 value, Int32 basis) : base(target, con, spec, value, basis)
        {
            InterestCode = interestCode;
            SubjectType = subjectType;
            ParticyCode = particyCode;
        }
        public override string ResultMessage()
        {
            return $"Type: {Enum.GetName<WorkSocialTerms>(this.SubjectType)}; Value: {this.ResultValue}; Basis: {this.ResultBasis}";
        }
        public Int32 IncomeScore()
        {
            Int32 resultType = 0;
            switch (SubjectType)
            {
                case WorkSocialTerms.SOCIAL_TERM_EMPLOYMENTS:
                    resultType = 900;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SMALLS_EMPL:
                    resultType = 100;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_MEET:
                    resultType = 500;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_SHORTS_DENY:
                    resultType = 0;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_BY_CONTRACT:
                    resultType = 0;
                    break;
                case WorkSocialTerms.SOCIAL_TERM_AGREEM_TASK:
                    resultType = 0;
                    break;
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
            return resultType + interestRes + particyRes;
        }
        private class IncomeTermComparator : IComparer<SocialBaseOvercapResult>
        {
            public IncomeTermComparator()
            {
            }

            public int Compare(SocialBaseOvercapResult x, SocialBaseOvercapResult y)
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
        public static IComparer<SocialBaseOvercapResult> ResultComparator()
        {
            return new IncomeTermComparator();
        }
    }

    // SocialPaymEmployee		SOCIAL_PAYM_EMPLOYEE
    public class SocialPaymEmployeeResult : PayrolexTermResult
    {
        public Int32 EmployeeBasis { get; private set; }
        public Int32 GeneralsBasis { get; private set; }
        public SocialPaymEmployeeResult(ITermTarget target, IArticleSpec spec,
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

    // SocialPaymEmployer		SOCIAL_PAYM_EMPLOYER
    public class SocialPaymEmployerResult : PayrolexTermResult
    {
        public Int32 EmployerBasis { get; private set; }
        public Int32 GeneralsBasis { get; private set; }
        public SocialPaymEmployerResult(ITermTarget target, IArticleSpec spec,
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
