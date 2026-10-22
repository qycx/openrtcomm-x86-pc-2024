using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{

        public static bool bUseSplitter(byte ucCompressors, uint dwFourcc)
        {
            return ((ucCompressors) == Consts.CONST_videoCompressors_stream && (dwFourcc) != Consts.CONST_fourcc_h264);
        }

}
}
