using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
		public static unsafe  int resetChromaKey()
		{
# if  __DEBUG__
			siCtrl_chromaKey = 0;
			traceLog(_T("resetChromaKey: to reinit chromaKey"));
#endif

			//  startToParse(  );

			return 0;
		}

	}
}
