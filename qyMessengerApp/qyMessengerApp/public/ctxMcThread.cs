using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
	public unsafe class CTX_MC_thread:CTX_qm_thread
	{
		//
		public CMcSharedQ pProcessQ;

		public uint dwTickCnt_tryToResumeVideoConference; //  2009/10/17

		public CMIS_MSGU pMsgBuf_forInput;                     //  2009/12/11
                                                               //public MIS_MSGU* pMsgBuf;                              //  2009/12/11
        public CMIS_MSGU pMsgBuf;                              //  2009/12/11

        //
        public CTX_MC_thread()
        {
			return;
        }

	};
	public class CTX_MC_work:CTX_MC_thread
	{
		//public CTX_MC_thread common;

		//GENERIC_Q guestQ;
		//bool bGuestQInited;
		//void** ppGuests_sortedByDevIdStr;
		//

	}
	;


public class CTX_MC_scheduler:CTX_MC_thread
	{       //  2009/09/05				
		//public CTX_MC_thread common;

		//

	}
	;
	public struct CTX_mc_pretrans_status_errFlgs
	{
		public bool bErrExists_mallocfFailed;
	};
	public struct CTX_mc_pretrans_status
	{
		public CTX_mc_pretrans_status_errFlgs errFlgs;
		public byte ucMsgSkipped_ifMallocFailed;     //  2014/05/12
	};

	public class CTX_mc_pretrans:CTX_MC_thread
	{

		//  2014/05/12
	public CTX_mc_pretrans_status		status;

	//
	public IntPtr pMsgBuf;
				 //
}		 ;


}
