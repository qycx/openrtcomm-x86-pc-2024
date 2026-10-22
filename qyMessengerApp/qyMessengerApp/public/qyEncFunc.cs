using System;
using System.Collections.Generic;
using System.Text;

partial class Consts
{
    public const int CONST_qyEncType_null = 0;
    public const int CONST_qyEncType_des = 1;
    public const int CONST_qyEncType_qnm = 2;       //  不用
    public const int CONST_qyEncType_qwm = 3;       //  2004/01/02
    public const int CONST_qyEncType_rsa = 4;		//  2008/06/09

    //
    public const int CONST_qyEncType_aes256 = 100;

}

namespace qy
{
    public class QY_ENC_CTX
    {
        public struct QY_ENC_CTX_var
        {
            public uint type;
        };

        public QY_ENC_CTX_var common;

        /*
        union  {
					 des_ctx des;
    }    u;
        */

};


partial class qyFuncs
{
}
}
