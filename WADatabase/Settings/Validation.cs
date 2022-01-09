using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WADatabase.Settings
{
    public static class Validation
    {
        public static bool IsMailValid(string emailaddress)
        {
            try
            {
                MailAddress mail = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static string PhoneValidation(string phone)
        {
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();

            try
            {
                PhoneNumber queryPhoneNumber = phoneUtil.Parse(phone, "UA");

                if (phoneUtil.IsValidNumber(queryPhoneNumber))
                {
                    return queryPhoneNumber.NationalNumber.ToString();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
