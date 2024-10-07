using System.Text.RegularExpressions;

namespace GloboClima.Infra.CrossCutting.Validators
{
    public static class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expressão regular para validar e-mails
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            // Valida o e-mail com Regex
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }
    }
}
