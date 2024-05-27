using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace TakeCare.Application.Services
{
	public class JwtService
	{
		private readonly string secureKey;

		public JwtService(IConfiguration configuration)
		{
			secureKey = configuration!.GetSection("Jwt:Key").Value!;
		}

		public string Generate(int id, string role, string email)
		{
			// Encrypt the data before adding it to the JWT payload
			string encryptedId = EncryptString(id.ToString());
			string encryptedRole = EncryptString(role);
			string encryptedEmail = EncryptString(email);

			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
			var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
			var header = new JwtHeader(credentials);
			var expirationDate = DateTime.Today.AddDays(356);
			var exp = new DateTimeOffset(expirationDate).ToUnixTimeSeconds();
			var payload = new JwtPayload
			{
				{ "iss", encryptedId },
				{ "role", encryptedRole },
				{ "email", encryptedEmail },
				{ "exp", exp }
			};
			var securityToken = new JwtSecurityToken(header, payload);

			return new JwtSecurityTokenHandler().WriteToken(securityToken);
		}

		private string EncryptString(string text)
		{
			var aesAlg = Aes.Create();
			var keyBytes = Encoding.UTF8.GetBytes(secureKey);
			Array.Resize(ref keyBytes, aesAlg.KeySize / 8);
			aesAlg.Key = keyBytes;
			aesAlg.GenerateIV();
			var iv = aesAlg.IV;

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
			var msEncrypt = new MemoryStream();
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			using (var swEncrypt = new StreamWriter(csEncrypt))
			{
				swEncrypt.Write(text);
			}
			var encrypted = msEncrypt.ToArray();
			var result = new byte[iv.Length + encrypted.Length];
			Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
			Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);
			return Convert.ToBase64String(result);
		}

		public string DecryptString(string cipherText)
		{
			var fullCipher = Convert.FromBase64String(cipherText);
			var aesAlg = Aes.Create();
			var iv = new byte[aesAlg.BlockSize / 8];
			var cipher = new byte[fullCipher.Length - iv.Length];
			Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
			Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

			var keyBytes = Encoding.UTF8.GetBytes(secureKey);
			Array.Resize(ref keyBytes, aesAlg.KeySize / 8);
			aesAlg.Key = keyBytes;
			aesAlg.IV = iv;

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			var msDecrypt = new MemoryStream(cipher);
			var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
			var srDecrypt = new StreamReader(csDecrypt);
			return srDecrypt.ReadToEnd();
		}

		public JwtSecurityToken Verify(string jwt)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secureKey);

			tokenHandler.ValidateToken
			(
				jwt, 
				new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
				},
				out SecurityToken validatedToken
			);

			return (JwtSecurityToken) validatedToken;
		}
		
	}
}
