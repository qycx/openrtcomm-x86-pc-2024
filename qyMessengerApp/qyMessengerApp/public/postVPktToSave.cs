using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
{
        public static unsafe int postAPktToSave(uint uiTransformType, QY_TRANSFORM pQY_TRANSFORM, uint uiSampleTimeInMs, uint uiPts, byte* audio, int len)
        {
            //return postAvPktToSave(uiTransformType, pQY_TRANSFORM, index_pMems_from, CONST_vcfMediaType_a, uiSampleTimeInMs, uiPts, audio, len);
            return -1;
        }

        public static unsafe int postVPktToSave(uint uiTransformType, QY_TRANSFORM pQY_TRANSFORM, int index_pMems_from, uint uiSampleTimeInMs, uint uiPts, byte* video, int len)
        {
            //return postAvPktToSave(uiTransformType, pQY_TRANSFORM, index_pMems_from, CONST_vcfMediaType_v, uiSampleTimeInMs, uiPts, video, len);
            return -1;
        }

    }
}
