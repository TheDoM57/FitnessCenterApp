using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FitnessCenterApp.Pages
{
    static class PasswordManagement
    {
        private const string staticSalt = "$N,-1tB<GoaD|\\-D";
        public static byte[] getPasswordHashWithSalt(string password, byte[] salt)
        {
            byte[] hash = new byte[24];
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(staticSalt + password, salt, 10000))
            {
                hash = pbkdf2.GetBytes(24);
            }
            byte[] result = new byte[40];
            Array.Copy(hash, 0, result, 0, 24);
            Array.Copy(salt, 0, result, 24, 16);
            return result;
        }
    }
}
