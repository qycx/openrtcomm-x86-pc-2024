using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

		public static unsafe QY_PLAYER getPlayerByIndex(CCtxQmc pProcInfo, int index)
		{
			if (index < 0 || index >= pProcInfo.av.usCnt_players) return null;
			//  if  (  !index  )  return  &pProcInfo->av.localAv.player;	//  2009/06/02
			return pProcInfo.av.pPlayers[index];
		}

	}
}
