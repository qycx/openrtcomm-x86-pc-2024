using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

        public static unsafe bool bQyGetStrMd5_x(string  str, byte* digest, uint* pLen)
        {
			// Use input string to calculate MD5 hash
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(str);
				byte[] hashBytes = md5.ComputeHash(inputBytes);

				// Convert the byte array to hexadecimal string
				StringBuilder sb = new StringBuilder();
				for (int i = 0; i < hashBytes.Length; i++)
				{
					sb.Append(hashBytes[i].ToString("x2"));
				}
				string str_digest = sb.ToString();
				if (*pLen < hashBytes.Length * 2 + 1) return false;
				safeStrnCpy(str_digest, digest, *pLen);
				*pLen = (uint)hashBytes.Length * 2;
				return true;
			}

		}

	}
}
