using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

partial class Consts
{
	public const uint INADDR_NONE = 0xffffffff;

}

namespace qy
{
   partial class qyFuncs
{
		public static unsafe void clearQySock(ref QY_SOCK pSock)
		{
#if false
			fixed (QY_SOCK* p = &pSock)
			{
				mymemset((IntPtr)p, 0, sizeof(QY_SOCK));
			}
			pSock.sockFd = -1;
#endif
		}

		public static unsafe bool bQySockOpen(ref QY_SOCK pSock)
		{
			//  return  (  pSock->sockFd  >=  0  );
			//return pSock.sockFd != -1;
			return pSock.socket == null ? false : true;
		}

		public static byte MACRO_byte0(uint l) {
			return (byte)((l & 0x000000ff));
		}
		public static byte MACRO_byte1(uint l) {
			return (byte)((l & 0x0000ff00) >> 8);
		}
		public static byte MACRO_byte2(uint l) {
			return (byte)((l & 0x00ff0000) >> 16);
		}
		public static byte MACRO_byte3(uint l) {
			return (byte)((l & 0xff000000) >> 24);
		}


		public static bool bulIpValid(uint ulIp)
		{
			ulIp = qyntohi(ulIp);
			if (MACRO_byte0(ulIp) == 0 || MACRO_byte0(ulIp) == 255) return false;
			if (MACRO_byte3(ulIp) == 0 || MACRO_byte3(ulIp) == 255) return false;
			return true;
		}


		public static bool bIpValid(string ip)
		{
			uint ulIp = 0;

			if (null == ip) return false;

			try { 
			IPAddress ipa = IPAddress.Parse(ip);
			ulIp = (uint)ipa.Address;
			}
			catch(Exception e)
            {
				Console.WriteLine("bIpValid failed");
				return false;
            }

			//if ((ulIp = inet_addr(ip)) == INADDR_NONE) return FALSE;
			return bulIpValid(ulIp);
		}


		public static unsafe uint myinet_addr( string ip)
        {
			uint ulIp = 0;

			if (null == ip) return (uint)Consts.INADDR_NONE;
			ulIp = (uint)IPAddress.Parse(ip).Address;
			return ulIp;


		}



		public static unsafe  byte* ulIp2Str(uint lVal, byte* ip, uint size)
		{
	 //struct in_addr  tmpInAddr;
	 byte* p;
	 
	 if  (  null==ip  ||  size<Consts.CONST_qyMaxIpLen  +  1  )  return  null;

	 //tmpInAddr.s_addr  =  lVal;
	 //if  ((p  =  inet_ntoa(tmpInAddr  )  )  )  safeStrnCpy(p, ip, size  );
		
			IPAddress ip1 = new IPAddress(lVal);
			if (ip1 == null) return null;
				safeStrnCpy(ip1.ToString(), ip, size);


			return  ip;
}


	//int getLocalIpBySocket(SOCKET s, char* ip)
	public static unsafe int getLocalIpBySocket(ref QY_SOCK  pSock, byte * ip)
		{
			string str_ip = "";
			/*
	 struct sockaddr_in  sockip;
	 char* ptr = null;
		int len = sizeof(sockip);

		sprintf(ip,  "0.0.0.0"  );
	 if  (getsockname(s, (  struct sockaddr  *  )&sockip,  &len  )  )  {		 
		 return  -1;
	 }

	 ptr  =  inet_ntoa(sockip.sin_addr  );
	 if  (  !ptr  )  return  -1;

	 sprintf(ip,  "%s", ptr  );
	 */

			safeStrnCpy(str_ip, ip, Consts.CONST_qyMaxIpLen + 1);

	 return  0;

		}


   //int getRemoteIpBySocket(SOCKET s, char* ip)
  public static unsafe int getRemoteIpBySocket(int s, byte * ip)
	{
			string str_ip = "";
			/*
	 struct sockaddr_in  sockip;
	 char* ptr = NULL;
	int len = sizeof(sockip);

	sprintf(ip,  "0.0.0.0"  );
	 if  (getpeername(s, (  struct sockaddr  *  )&sockip,  &len  )  )  {		 
		 return  -1;
	 }

ptr = inet_ntoa(sockip.sin_addr);
if (!ptr) return -1;

sprintf(ip, "%s", ptr);
			*/
			safeStrnCpy(str_ip, ip, Consts.CONST_qyMaxIpLen + 1);

return 0;

}

	}
}
