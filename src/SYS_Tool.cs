using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using LingYunImageHost.Config;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Logging;

namespace LingYunImageHost
{
    public static class SYS_Tool
    {
        public static string GenerateToken(string id)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(sha256(ConfigEntity.sysConfig.WebName));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", id) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = "Admin",
                    Audience = id
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static string TokenDecrypt(string token, string uname)
        {
            string result = string.Empty;//加密内容
            try
            {
                ClaimsPrincipal principal = new ClaimsPrincipal();
                JwtSecurityToken jwt = null;
                var handler = new JwtSecurityTokenHandler();
                jwt = handler.ReadJwtToken(token);
                // 根据token是否正常解析
                if (jwt == null)
                {
                    return "";
                }
                // 验证token是否有效
                var TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "Admin",
                    ValidAudience = uname,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(sha256(ConfigEntity.sysConfig.WebName))),
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    /***********************************TokenValidationParameters的参数默认值***********************************/
                    // RequireSignedTokens = true,
                    // SaveSigninToken = false,
                    // ValidateActor = false,
                    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。    
                    // ValidateAudience = true,
                    // ValidateIssuer = true, 
                    // ValidateIssuerSigningKey = false,
                    // 是否要求Token的Claims中必须包含Expires
                    // RequireExpirationTime = true,
                    // 允许的服务器时间偏移量,默认300秒
                    // ClockSkew = TimeSpan.FromSeconds(300),
                    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                    //ValidateLifetime = true
                };
                SecurityToken sercutityToken = null;
                // 验证token是否有效，如果过期，报错SecurityTokenExpiredException
                // 报错信息：IDX10223 : Lifetime validation failed
                principal = handler.ValidateToken(token, TokenValidationParameters, out sercutityToken);
                if (principal != null && principal.Claims.Count() > 0)
                {
                    Claim claim = principal.Claims.First();
                    result = claim.Value;
                }
            }
            catch (SecurityTokenExpiredException ex)
            {
                Console.WriteLine(ex.Message);
                //可以在这写续期代码
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public static string sha256(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("X2"));
            }

            return builder.ToString();
        }
    }
}
