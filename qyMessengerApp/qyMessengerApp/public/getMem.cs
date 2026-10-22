using System;
using System.Collections.Generic;
using System.Text;

namespace qy
{
    partial class qyFuncs
    {
        public static unsafe myDRAW_VIDEO_DATA* getMem(myDRAW_VIDEO_DATA* pStart, int bufSize, int index)
        {
            int memSize = sizeof(myDRAW_VIDEO_DATA);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            myDRAW_VIDEO_DATA* p = (myDRAW_VIDEO_DATA*)(pStartAddr + index * memSize);
            return p;
        }

        public static unsafe TRANSFER_VIDEO_dataMemHead* getMem(TRANSFER_VIDEO_dataMemHead* pStart, int bufSize, int index)
        {
            int memSize = sizeof(TRANSFER_VIDEO_dataMemHead);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            TRANSFER_VIDEO_dataMemHead* p = (TRANSFER_VIDEO_dataMemHead*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe TRANSFER_AUDIO_dataMemHead* getMem(TRANSFER_AUDIO_dataMemHead* pStart, int bufSize, int index)
        {
            int memSize = sizeof(TRANSFER_AUDIO_dataMemHead);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            TRANSFER_AUDIO_dataMemHead* p = (TRANSFER_AUDIO_dataMemHead*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe IM_GRP_INFO* getMem(IM_GRP_INFO* pStart, int bufSize, int index)
        {
            int memSize = sizeof(IM_GRP_INFO);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            IM_GRP_INFO* p = (IM_GRP_INFO*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe IM_GRP_MEM* getMem(IM_GRP_MEM* pStart, int bufSize, int index)
        {
            int memSize = sizeof(IM_GRP_MEM);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            IM_GRP_MEM* p = (IM_GRP_MEM*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe RETRIEVE_contact_mem* getMem(RETRIEVE_contact_mem* pStart, int bufSize, int index)
        {
            int memSize = sizeof(RETRIEVE_contact_mem);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            RETRIEVE_contact_mem* p = (RETRIEVE_contact_mem*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe VIDEO_CONFERENCE_MEM* getMem(VIDEO_CONFERENCE_MEM* pStart, int bufSize, int index)
        {
            int memSize = sizeof(VIDEO_CONFERENCE_MEM);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            VIDEO_CONFERENCE_MEM* p = (VIDEO_CONFERENCE_MEM*)(pStartAddr + index * memSize);
            return p;

        }


        public static unsafe CONF_requesting_mem* getMem(CONF_requesting_mem* pStart, int bufSize, int index)
        {
            int memSize = sizeof(CONF_requesting_mem);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            CONF_requesting_mem* p = (CONF_requesting_mem*)(pStartAddr + index * memSize);
            return p;

        }

        public static unsafe IpcInfo* getMem(IpcInfo* pStart, int bufSize, int index)
        {
            int memSize = sizeof(IpcInfo);
            byte* pStartAddr = (byte*)pStart;
            if (index < 0) return null;
            if (index * memSize + memSize > bufSize) return null;
            IpcInfo* p = (IpcInfo*)(pStartAddr + index * memSize);
            return p;

        }




        //
    }
}
