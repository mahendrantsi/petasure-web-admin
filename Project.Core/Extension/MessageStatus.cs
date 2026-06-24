namespace Project.Core.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class MessageStatus
    {
        public static string Success
        {
            get { return "Operation Successfully Completed."; }
            
        }

        public static string SecondaryUserAdded
        {
            get { return "Secondary User Added Successfully."; }

        }

        public static string Error
        {
            get { return "An Error Occurred !"; }
            
        }

        public static string Update
        {
            get { return "Data Updated successfully"; }
            
        }


        public static string NotificationNotFound
        {
            get { return "Notification not found"; }

        }

        public static string Delete
        {
            get { return "Data Deleted successfully"; }
            
        }

        public static string Fail
        {
            get { return "Operation Failed."; }
            
        }

        public static string Verified
        {
            get { return "Verified successfully"; }
            
        }

        public static string Expired
        {
            get { return "Verification Expired!"; }
            
        }

        public static string InActive
        {
            get { return "Data InActive!"; }
            
        }

        public static string NotExists
        {
            get { return "Not Exists!"; }
            
        }

        public static string NotFound
        {
            get { return "Data not found"; }

        }

        public static string AlreadyExists
        {
            get { return "Already Exists!"; }
            
        }

        public static string MobileNumberAlreadyExist
        {
            get { return "Mobile number already Exists!"; }
            
        }

        public static string SomethingWentWrong
        {
            get { return "Something went wrong, Please try again."; }
            
        }

        public static string KycNotCompleted
        {
            get { return "KYC is Not Completed."; }
            
        }
        public static string ProfileNotCompleted
        {
            get { return "Profile Not Completed."; }
            
        }
        public static string ProfileNotFound
        {
            get { return "Profile Not Found."; }
            
        }

        public static string ProfileUpdated
        {
            get { return "Profile Updated Successfully."; }

        }
        public static string NoInstitutionSelected
        {
            get { return "No Institution Selected "; }
            
        }
        public static string PayerNotFound
        {
            get { return "Payer Not Found"; }
        }
        public static string PayeeNotFound
        {
            get { return "Payee Not Found"; }
        }

        public static string MerchantIsInactive
        {
            get { return "Merchant is inactive."; }
        }

        public static string TransactionNotInitiated
        {
            get { return "Unable to create new transaction"; }
        }

        public static string PayeeBankNotfound
        {
            get { return "Unable to Get the Bank of Payee"; }
        }
        public static string PayerBankNotfound
        {
            get { return "Unable to Get the Bank of Payer"; }
        }
        public static string NoTransactions
        {
            get { return "No Transactions Found"; }
        }
        public static string RequiresTwoFactor
        {
            get { return "Requires Two Factor"; }
        }
        public static string EnableAuthenticator
        {
            get { return "Enable Authenticator"; }
        }
        public static string InactiveAccount
        {
            get { return "This account is inactive, please contact admin at admin@ssv.co.uk"; }
        }

        public static string AdminAccounts
        {
            get { return "Your role is not for App"; }
        }

        public static string UserNotValidForTransaction
        {
            get { return "User is not valid to initiate this transaction."; }
        }

        public static string UserNotFound {
            get { return "User Not Found !";  }
        }

        public static string YouCanNotPayYourself
        {
            get { return "It appears scanned QR belongs to you, You simply can not pay yourself."; }
        }

        public static string PaymentAuthenticationFailed
        {
            get { return "Payment authentication failed."; }
        }

        public static string PaymentFailed
        {
            get { return "Payment failed."; }
        }

        public static string PaymentCanceled
        {
            get { return "Payment Canceled."; }
        }

        public static string PaymentPendingFromBank
        {
            get { return "Payment is pending from bank end."; }
        }

  
        public static string PaymentSuccessFromBank
        {
            get { return "Payment success."; }
        }
        public static string BusinessAdded
        {
            get { return "Business Details Added successfully"; }
        }
        public static string BusinessUpdated
        {
            get { return "Business Details Updated successfully"; }
        }

        public static string PetDeleted
        {
            get { return "Pet deleted successfully"; }
        }

        public static string PetAdded
        {
            get { return "Pet added successfully"; }
        }

        public static string PetUpdated
        {
            get { return "Pet updated successfully"; }
        }

        public static string PetAlreadyExists
        {
            get { return "Pet Already Exists !"; }
        }

        public static string PetNotExists
        {
            get { return "Pet Not Exists !"; }
        }


        public static string PetDeleteError
        {
            get { return "Pet not found or deletion failed !"; }
        }

        public static string MissingReportAdded
        {
            get { return "Missing Report added successfully"; } 
        }

        public static string GuestUserFoundPetMessage
        {
            get { return "Thank you so much for your information, Our member will contact you soon !"; }
        }

        public static string FoundMessage
        {
            get { return "Enjoy with your pet !"; }
        }

        public static string ContactUsSuccessMessage
        {
            get { return "Your message has been submitted successfully. Our team will get back to you shortly."; }
        }
    }
}
