using Inventory360DataModel;
using DAL.DataAccess.Select.Security;
using DAL.DataAccess.Update.Setup;
using DAL.Interface.Select.Security;
using DAL.Interface.Update.Setup;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BLL
{
    public class ManagerSecurity
    {
        private string _passwordEncryptDecryptKey = "LetsAmericaGreat";

        public object SelectLocationWiseSecurityUserAll(long companyId, long locationId, string query)
        {
            try
            {
                ISelectSecurityUser iSelectSecurityUser = new DSelectSecurityUser(companyId);

                return iSelectSecurityUser.SelectSecurityUserLocation()
                    .Where(x => x.LocationId == locationId
                        && x.Security_User.Active == "Y")
                    .Select(s => new CommonResultList
                    {
                        Item = s.Security_User.UserName + " # " + s.Security_User.FullName,
                        Value = s.SecurityUserId.ToString()
                    })
                    .OrderBy(o => o.Item)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonSecurityLoginCredential CheckSecurityUserAtLogin(long companyId, long locationId, string userName, string password)
        {
            try
            {
                CommonSecurityLoginCredential commonSecurityLoginCredential = new CommonSecurityLoginCredential();
                ISelectSecurityUser iSelectSecurityUser = new DSelectSecurityUser(companyId);

                var securityUserLocation = iSelectSecurityUser.SelectSecurityUserLocation()
                    .Where(x => x.LocationId == locationId
                        && x.Security_User.UserName == userName
                        && x.Security_User.Active == "Y")
                    .Select(s => new
                    {
                        Password = s.Security_User.Password,
                        LoginCompanyCode = s.Security_User.Setup_Company.Code,
                        LoginCompanyName = s.Security_User.Setup_Company.Name,
                        LoginLocationCode = s.Setup_Location.Code,
                        LoginLocationName = s.Setup_Location.Name,
                        DefaultCurrency = s.Setup_Location.DefaultCurrency,
                        LoginUserId = s.Security_User.SecurityUserId,
                        LoginUserName = s.Security_User.UserName,
                        LoginLevelId = s.Security_User.LevelId,
                        LoginUserLevel = s.Security_User.Security_Level.Name,
                        LoginRoleId = s.Security_User.RoleId,
                        LoginUserRole = s.Security_User.Security_Role.Name,
                        IsFirstLogin = s.Security_User.FirstLogin
                    })
                    .FirstOrDefault();

                if (securityUserLocation != null)
                {
                    //if (DecryptString(securityUserLocation.Security_User.Password, _passwordEncryptDecryptKey).Trim().Equals(password.Trim()))
                    if (string.Equals(DecryptString(securityUserLocation.Password, _passwordEncryptDecryptKey).Trim(), password, StringComparison.Ordinal))
                    {
                        commonSecurityLoginCredential.IsSuccess = true;
                        commonSecurityLoginCredential.Message = "Success";
                        commonSecurityLoginCredential.LoginCompanyId = companyId;
                        commonSecurityLoginCredential.LoginCompanyCode = securityUserLocation.LoginCompanyCode;
                        commonSecurityLoginCredential.LoginCompanyName = securityUserLocation.LoginCompanyName;
                        commonSecurityLoginCredential.LoginLocationId = locationId;
                        commonSecurityLoginCredential.LoginLocationCode = securityUserLocation.LoginLocationCode;
                        commonSecurityLoginCredential.LoginLocationName = securityUserLocation.LoginLocationName;
                        commonSecurityLoginCredential.DefaultCurrency = securityUserLocation.DefaultCurrency;
                        commonSecurityLoginCredential.LoginUserId = securityUserLocation.LoginUserId;
                        commonSecurityLoginCredential.LoginUserName = securityUserLocation.LoginUserName;
                        commonSecurityLoginCredential.LoginLevelId = securityUserLocation.LoginLevelId;
                        commonSecurityLoginCredential.LoginUserLevel = securityUserLocation.LoginUserLevel;
                        commonSecurityLoginCredential.LoginRoleId = securityUserLocation.LoginRoleId;
                        commonSecurityLoginCredential.LoginUserRole = securityUserLocation.LoginUserRole;
                        commonSecurityLoginCredential.IsFirstLogin = securityUserLocation.IsFirstLogin;
                    }
                    else
                    {
                        commonSecurityLoginCredential.IsSuccess = false;
                        commonSecurityLoginCredential.Message = "Invalid Password.";
                        commonSecurityLoginCredential.LoginCompanyId = companyId;
                        commonSecurityLoginCredential.LoginCompanyCode = "";
                        commonSecurityLoginCredential.LoginCompanyName = "";
                        commonSecurityLoginCredential.LoginLocationId = locationId;
                        commonSecurityLoginCredential.LoginLocationCode = "";
                        commonSecurityLoginCredential.LoginLocationName = "";
                        commonSecurityLoginCredential.LoginUserId = 0;
                        commonSecurityLoginCredential.LoginUserName = "";
                        commonSecurityLoginCredential.LoginLevelId = 0;
                        commonSecurityLoginCredential.LoginUserLevel = "";
                        commonSecurityLoginCredential.LoginRoleId = 0;
                        commonSecurityLoginCredential.LoginUserRole = "";
                        commonSecurityLoginCredential.IsFirstLogin = "";
                    }
                }
                else
                {
                    commonSecurityLoginCredential.IsSuccess = false;
                    commonSecurityLoginCredential.Message = "Invalid Login Credential. Please Try Again With Valid Information.";
                    commonSecurityLoginCredential.LoginCompanyId = companyId;
                    commonSecurityLoginCredential.LoginCompanyCode = "";
                    commonSecurityLoginCredential.LoginCompanyName = "";
                    commonSecurityLoginCredential.LoginLocationId = locationId;
                    commonSecurityLoginCredential.LoginLocationCode = "";
                    commonSecurityLoginCredential.LoginLocationName = "";
                    commonSecurityLoginCredential.LoginUserId = 0;
                    commonSecurityLoginCredential.LoginUserName = "";
                    commonSecurityLoginCredential.LoginLevelId = 0;
                    commonSecurityLoginCredential.LoginUserLevel = "";
                    commonSecurityLoginCredential.LoginRoleId = 0;
                    commonSecurityLoginCredential.LoginUserRole = "";
                    commonSecurityLoginCredential.IsFirstLogin = "";
                }

                return commonSecurityLoginCredential;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public CommonResult ChangeSecurityUserPassword(long companyId, long locationId, long userId, string oldPassword, string newPassword, string confirmPassword)
        {
            try
            {
                if (!string.Equals(newPassword, confirmPassword, StringComparison.Ordinal))
                {
                    return new CommonResult
                    {
                        IsSuccess = false,
                        Message = "New password and confirm password mismatch."
                    };
                }

                CommonResult result = new CommonResult();
                ISelectSecurityUser iSelectSecurityUser = new DSelectSecurityUser(companyId);

                var securityUserLocation = iSelectSecurityUser.SelectSecurityUserLocation()
                    .Where(x => x.LocationId == locationId
                        && x.Security_User.SecurityUserId == userId
                        && x.Security_User.Active == "Y")
                    .Select(s => new
                    {
                        Password = s.Security_User.Password
                    })
                    .FirstOrDefault();

                if (securityUserLocation != null)
                {
                    if (string.Equals(DecryptString(securityUserLocation.Password, _passwordEncryptDecryptKey).Trim(), oldPassword, StringComparison.Ordinal))
                    {
                        string changedPassword = EncryptString(newPassword, _passwordEncryptDecryptKey).Trim();

                        IUpdateSecurityUser iUpdateSecurityUser = new DUpdateSecurityUser(new CommonSecurityUser
                        {
                            SecurityUserId = userId,
                            Password = changedPassword
                        });

                        if (iUpdateSecurityUser.UpdateSecurityUser())
                        {
                            result.IsSuccess = true;
                            result.Message = "Password changed successfully.";
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Password changed unsuccessfull.";
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Password changed unsuccessfull.";
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string ddd()
        //{
        //    return EncryptString("1234567890", _passwordEncryptDecryptKey);
        //}

        //public string eee()
        //{
        //    return DecryptString("d+GRJkLqLbN2/mgTw9SRtnk0eHB9n05Yr2Y4hUYCPgqIGyp9xl3EczvLSm9vzYEJMz27mUhTrWvoOzwo8uS0Qw==", _passwordEncryptDecryptKey);
        //}

        #region Encrypt/Decrypt
        //https://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt
        private static string EncryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = AES_Encrypt(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        private static string DecryptString(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = AES_Decrypt(baText, baPwdHash);

            // Remove salt
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        private static int GetSaltLength()
        {
            return 48;
        }
        #endregion
    }
}