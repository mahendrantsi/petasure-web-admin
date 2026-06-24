namespace Project.Models.Validations
{
    using Project.Models.AccountModel;
    using Project.Models.CommonModel;
    using ServiceStack.FluentValidation;

    public class LoginReqDTOValidator : ProjectValidator<LoginReqDTO>
    {
        public LoginReqDTOValidator()
        {
            this.RuleFor(vm => vm.Username).NotEmpty().WithMessage("Username cannot be empty");
            this.RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}
