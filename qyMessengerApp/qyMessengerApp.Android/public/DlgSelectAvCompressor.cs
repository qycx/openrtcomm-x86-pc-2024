using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
	partial class qyFuncs_Droid
{
		public static bool bNoPrompt_selectAvCompressor(uint uiCapType, uint uiSubCapType)
        {
            return true;
        }

        public unsafe static int getTalkerPolicy(QY_MESSENGER_ID pIdInfo, out TALKER_policy pTALKER_policy)
        {
            fixed (TALKER_policy* p = &pTALKER_policy)
            {
                qyFuncs.mymemset((IntPtr)p, 0, sizeof(TALKER_policy));
            }
            return 0;
        }

}
}