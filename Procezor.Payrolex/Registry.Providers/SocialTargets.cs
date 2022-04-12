using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HraveMzdy.Procezor.Service.Types;
using HraveMzdy.Procezor.Payrolex.Registry.Constants;
using HraveMzdy.Legalios.Service.Types;

namespace HraveMzdy.Procezor.Payrolex.Registry.Providers
{
    // SocialDeclare		SOCIAL_DECLARE
    public class SocialDeclareTarget : PayrolexTermTarget
    {
        public Int16 InterestCode { get; private set; }
        public WorkSocialTerms ContractType { get; private set; }

        public SocialDeclareTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept, 
            Int16 interestCode, WorkSocialTerms contractType) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            InterestCode = interestCode;
            ContractType = contractType;
        }
    }

    // SocialIncome		SOCIAL_INCOME
    public class SocialIncomeTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialIncomeTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // SocialBase		SOCIAL_BASE
    public class SocialBaseTarget : PayrolexTermTarget
    {
        public Int32 AnnuityBase { get; private set; }

        public SocialBaseTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 annuityBase) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            AnnuityBase = annuityBase;
        }
    }

    // SocialBaseEmployee		SOCIAL_BASE_EMPLOYEE
    public class SocialBaseEmployeeTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialBaseEmployeeTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // SocialBaseEmployer		SOCIAL_BASE_EMPLOYER
    public class SocialBaseEmployerTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialBaseEmployerTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // SocialBaseOvercap		SOCIAL_BASE_OVERCAP
    public class SocialBaseOvercapTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialBaseOvercapTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // SocialPaymEmployee		SOCIAL_PAYM_EMPLOYEE
    public class SocialPaymEmployeeTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialPaymEmployeeTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

    // SocialPaymEmployer		SOCIAL_PAYM_EMPLOYER
    public class SocialPaymEmployerTarget : PayrolexTermTarget
    {
        public Int32 TargetVals { get; private set; }

        public SocialPaymEmployerTarget(MonthCode monthCode, ContractCode contract, PositionCode position, VariantCode variant,
            ArticleCode article, ConceptCode concept,
            Int32 targetVals) :
            base(monthCode, contract, position, variant, article, concept, BASIS_ZERO)
        {
            TargetVals = targetVals;
        }
    }

}
