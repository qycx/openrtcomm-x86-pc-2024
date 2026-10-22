using System;
using System.Collections.Generic;
using System.Text;




//
namespace qy
{


	//
	public class CTX_qm_thread
	{		
		public	int				iType;						
		public	uint			dwTickCnt_lastGot;						
		public 	uint			dwThreadId;									
		public 	int				reserved;

		//
		public CTX_caller		ctxCaller;

		//
		public CTX_qm_thread()
        {
			this.ctxCaller = new CTX_caller();
        }

	}
	;

}
