namespace Project.Models.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using ServiceStack.FluentValidation;

    public abstract class ProjectValidator<T> : AbstractValidator<T>
    {
        public ProjectValidator()
        {
            this.CascadeMode = CascadeMode.StopOnFirstFailure;
        }

        // / <summary>
        // / Is Valid Otp
        // / </summary>
        // / <param name="value">value</param>
        // / <returns></returns>
        public bool IsValidOtp(string value)
        {
            /*****************************************************
            * if this null check is removed, we will need to add *
            * it to all the validators that call this method     *
            *****************************************************/
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            string pattern = @"^[0-9]{6}?$";
            Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            return rx.IsMatch(value.ToString());
        }

        // / <summary>
        // /  Is Valid Mobile
        // / </summary>
        // / <param name="value"></param>
        // / <returns></returns>
        public bool IsValidMobile(string value)
        {
            string pattern = @"^[0-9]{3,15}$";
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            Regex rx = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            return rx.IsMatch(value.ToString());
        }
    }
}
