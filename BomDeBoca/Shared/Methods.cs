using BlazorInputFile;
using System.Text.RegularExpressions;

namespace BomDeBoca.Shared
{
    public static class Methods
    {
        public static string ConvertImgBytesToDisplay(byte[] imgBytes) {
            if (imgBytes != null) {
                var base64 = Convert.ToBase64String(imgBytes); 
                return string.Format("data:img/jpg;base64,{0}", base64); ;
            } 
            return "/Imagens/noPhoto.jpg";
        }

        public static async Task<byte[]> ConvertDisplayToImgByte(IFileListEntry file) {
            var ms = new MemoryStream();
            await file.Data.CopyToAsync(ms);
            return ms.ToArray();
        }

        public static bool EmailIsValid(string email)
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex validEmailRegex = new Regex(validEmailPattern, RegexOptions.IgnoreCase);
            return validEmailRegex.IsMatch(email.Trim());
        }

        public static bool PasswordIsValid(string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Contains(' '))
                return false;
            return true;
        }
    }
}
