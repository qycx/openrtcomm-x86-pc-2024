using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{

    public unsafe struct QY_LOGICAL_SERVER
    {
        public fixed byte ip[Consts.CONST_qyMaxIpLen + 1];
        public ushort port;
    }
    ;

    public unsafe struct QWM_MACS_INFO
    {
        //unsigned char macs[CONST_qnmMaxMacs][CONST_qyMacLen  +  1];
				 public byte nMacs;
    }
    ;


}
