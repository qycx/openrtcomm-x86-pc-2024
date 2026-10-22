using System;
using System.Collections.Generic;
using System.Text;

partial class Consts {
    public const int MB_OK = (int)0x00000000L;
    /*
    public const int MB_OKCANCEL = 0x00000001L;
    public const int MB_ABORTRETRYIGNORE = 0x00000002L;
    public const int MB_YESNOCANCEL = 0x00000003L;
    public const int MB_YESNO = 0x00000004L;
    public const int MB_RETRYCANCEL = 0x00000005L;
    */
};


namespace qy
{
    partial class qyFuncs
{


public static unsafe int myMessageBox(object hWnd, string lpText, string lpCaption, uint uType)
        {
            return 0;
        }


}
}
