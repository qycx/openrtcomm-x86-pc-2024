

using System;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {

		public static string displayShortTime(Int64 t)
		{
#if false
			char timeBuf_t[CONST_qyTimeLen + 1] = "";
			char curTimeBuf[CONST_qyTimeLen + 1] = "";
			char displayBuf[128 + 1] = "";

			getCurTime(curTimeBuf);
			getTimeStr(t, timeBuf_t, mycountof(timeBuf_t));

			qyDisplayTime(timeBuf_t, displayBuf, mycountof(displayBuf));

			if (_strnicmp(timeBuf_t, curTimeBuf, 4)) safeStrnCpy(displayBuf, timeBuf, min(len, 4 + 1));
			else if (_strnicmp(timeBuf_t + 4, curTimeBuf + 4, 4)) safeStrnCpy(displayBuf + 5, timeBuf, min(len, 5 + 1));
			else safeStrnCpy(displayBuf + 11, timeBuf, min(len, 8 + 1));

#endif
			string str = "";
			DateTime dt0 = new DateTime(1970, 1, 1);
			DateTime dt1 = dt0.AddSeconds(t);
			DateTime dt2 = dt1.ToLocalTime();
			DateTime dtNow = DateTime.Now;
			DateTime dtNow_local = dtNow.ToLocalTime();
			//
			if (dt2.Year != dtNow_local.Year)
            {
				str += dt2.Year;
            }
			else if (dt2.Month != dtNow_local.Month || dt2.Day != dtNow_local.Day)
            {
				str += dt2.Month + "-" + dt2.Day;
            }
			else
            {
				str += string.Format("{0:d2}:{1:d2}:{2:d2}",dt2.Hour,dt2.Minute,dt2.Second);
            }


			return str;
		}






		}




}