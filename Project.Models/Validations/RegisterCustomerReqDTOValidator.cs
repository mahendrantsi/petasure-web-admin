//using FluentValidation;
//using Project.Models.AccountModel;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Project.Models.Validations
//{
//    public class RegisterCustomerReqDTOValidator: ProjectValidator<RegisterCustomerReqDTO>
//    {
//        public RegisterCustomerReqDTOValidator()
//        {
//            RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty").EmailAddress().WithMessage("Email address is not valid");
//            RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
//            RuleFor(vm => vm.ConfirmPassword).NotEmpty().WithMessage("Confirm Password cannot be empty");
//            //RuleFor(vm => vm.FirstName).NotEmpty().WithMessage("First Name cannot be empty");
//            //RuleFor(vm => vm.LastName).NotEmpty().WithMessage("Last Name cannot be empty");
//            //RuleFor(vm => vm.UserName).NotEmpty().WithMessage("UserName cannot be empty").MinimumLength(5)
//            //    .Matches("@").WithMessage("@ is not allowed in UserName.");
//            //RuleFor(vm => vm.PhoneNumber).NotEmpty().WithMessage("Phone Number cannot be empty")
//               // .Must(x => IsValidMobile(x)).WithMessage("Phone Number is not valid");
//        }
//    }
//}
