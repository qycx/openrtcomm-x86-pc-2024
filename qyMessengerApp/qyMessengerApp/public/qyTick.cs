using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    public class CQyTick
{
}


    partial class qyFuncs {

        public static uint M_GetTickCount_av(CQyTick tickObj) {
            return myGetTickCount(null);
        }

        public static uint M_timeGetTime(CQyTick tickObj) {
            return myGetTickCount(null);
        }

        public static uint M_GetTickCount_audio(CQyTick tickObj) {
            return myGetTickCount(null);
        }

        public static uint M_GetTgt_video(CQyTick tickObj)
        {
            return myGetTickCount(null);
        }


    }

}
