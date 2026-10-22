using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
	public static unsafe bool bCapAudioQuit(CAP_procInfo_audioCommon pCAP_procInfo_audioCommon, void* p1, void* p2)
	{

		//
		CAP_procInfo_audioCommon pCapProcInfo = (CAP_procInfo_audioCommon)pCAP_procInfo_audioCommon;
		if (null==pCapProcInfo) return false;


		return pCapProcInfo.thread.bQuit;
	}

}
}
