

using Android.Media;
using Java.Lang;
using Java.Nio;
using Java.Nio.Channels;
using Java.Util;
using Exception = Java.Lang.Exception;

namespace qy.Droid
{

    partial class qyFuncs_Droid
    {
        public static int test()
        {
            //MediaFormat

            int i = 0;
            int j = 9;

            ref int kk = ref i;
            kk = ref j;


            return 0;
        }


        public static bool isSocketInvalid_android(ref QY_SOCK pSock)
        {
            //return ((s) == Consts.INVALID_SOCKET); //  2013/02/08
            if (pSock.socketChannel == null) return true;
            return false;
        }


        public static bool bQySockOpen_android(ref QY_SOCK pSock)
        {
            return pSock.socketChannel != null;
        }


        public static int qyConnect_android(string ipaddr, int port, ref SOCK_PARAM pSockParam, ref QY_SOCK pSock)
        {

            int iErr = -1;
            int tmp;



            /*
	 struct sockaddr_in server;
		uint arg;

*/
            //  if ( pAsrSock->sockFd >= 0 ) return -1;
            if (!isSocketInvalid_android(ref pSock)) return -1;   //  2014/02/08



            try
            {
                //
                pSock.socketChannel = SocketChannel.Open();
                if (pSock.socketChannel == null) goto errLabel;

                //
                SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;


                //
                socketChannel.Socket().Connect(new Java.Net.InetSocketAddress(ipaddr, port), pSockParam.to.iConnect);

                //
                //
                if (pSock.bb_buf_r != null) goto errLabel;
                if (pSock.bb_buf_w != null) goto errLabel;
                //	
                if (pSockParam.capacity_buf_r == 0)
                {
                    qyFuncs.traceLog("qyConntent_andorid: failed, capacity_buf_r is 0");
                    goto errLabel;
                }
                if (pSockParam.capacity_buf_w == 0)
                {
                    qyFuncs.traceLog("qyConntent_andorid: failed, capacity_buf_w is 0");
                    goto errLabel;
                }
                pSock.bb_buf_r = ByteBuffer.AllocateDirect(pSockParam.capacity_buf_r);
                if (pSock.bb_buf_r == null) goto errLabel;
                pSock.bb_buf_w = ByteBuffer.AllocateDirect(pSockParam.capacity_buf_w);
                if (pSock.bb_buf_w == null) goto errLabel;

                //
                Selector sel_r = Selector.Open();
                pSock.selector_r = sel_r;
                Selector sel_w = Selector.Open();
                pSock.selector_w = sel_w;
                //
                //
                socketChannel.ConfigureBlocking(false);

                //
                socketChannel.Register(sel_r, Operations.Read);
                socketChannel.Register(sel_w, Operations.Write);

                //

                //
                //pSock.socket.SendTimeout = to.iWrite;
                //pSock.socket.ReceiveTimeout = to.iRead;
            }
            catch (Exception e)
            {


                if (Consts.CONST_Status_Window_Function_Open == 0)
                {
                    qyFuncs.traceLog("qyConnect failed, " + e.ToString());

                }
                //Console.WriteLine("qyConnect failed, " + e.ToString());
                goto errLabel;
            }


            iErr = 0;

        errLabel:

            if (0 != iErr)
            {
                qyDisconnect_android(ref pSock);
            }

            return iErr;

        }



        public static unsafe void qyDisconnect_android(ref QY_SOCK pSock)
        {


            //  if ( pAsrSock->sockFd >= 0 ) 
            if (!isSocketInvalid_android(ref pSock))
            {
                SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;
                //closesocket(pAsrSock->sockFd ); pAsrSock->sockFd = -1;
                try
                {
                    socketChannel.Close();
                }
                catch (Exception e)
                {

                }
                //
                pSock.socketChannel = null;

            }
            if (pSock.selector_r != null)
            {
                Selector sel_r = (Selector)pSock.selector_r;
                sel_r.Close();
                pSock.selector_r = null;
            }
            if (pSock.selector_w != null)
            {
                Selector sel_w = (Selector)pSock.selector_w;
                sel_w.Close();
                pSock.selector_w = null;
            }
            if (pSock.bb_buf_r != null)
            {
                pSock.bb_buf_r = null;
            }
            if (pSock.bb_buf_w != null)
            {
                pSock.bb_buf_w = null;
            }

            return;
        }

        //
        public static unsafe int qySend_android(ref QY_SOCK pSock, byte* buf, int len, int timeoutVal)
        {
            int iErr = -1;
            int nSent = -1;

            if (0 == len) return -1;

            SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;
            Selector selector = (Selector)pSock.selector_w;

            try
            {
#if false
				if (!pSock.b_w_selected)
				{
					Selector selector = (Selector)pSock.selector_w;

					int nReady = selector.Select(timeoutVal);
					if (nReady <= 0) goto errLabel;
					pSock.b_w_selected = true;
				}
#endif


                ByteBuffer bb_w = (ByteBuffer)pSock.bb_buf_w;
                if (len > bb_w.Capacity()) goto errLabel;
                System.IntPtr p = bb_w.GetDirectBufferAddress();
                qyFuncs.mymemcpy(p, (System.IntPtr)buf, (uint)len);
                bb_w.Position(0);
                bb_w.Limit(len);

                //
                int nWrite = 0;
                //
#if false
				while (bb_w.HasRemaining)
				{
					nWrite=socketChannel.Write(bb_w);
				}
#endif
                bool bDbg = false;
                //
                nWrite = Com.Qycx.Tmpjavalibrary.TmpSock.QySend_android(socketChannel, selector, bb_w, len, timeoutVal, bDbg);
                if (nWrite != len)
                {
                    goto errLabel;
                }

            }
            catch (Exception e)
            {
                qyFuncs.traceLog("qySend_android exception: " + e.ToString());
                goto errLabel;
            }

#if false
			//
			byte[] a_buf = new byte[len];
			Marshal.Copy((IntPtr)buf, a_buf, 0, len);
			try
			{
				nSent = pSock.socket.Send(a_buf);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				goto errLabel;
			}
			if (nSent != len) goto errLabel;
#endif


            iErr = 0;

        errLabel:

            return iErr;


        }



        public static unsafe int qyRecv_android(ref QY_SOCK pSock, byte* buf, int* pLen, int timeoutVal)
        {
            int iErr = -1;
            int nRead = -1;
            string str;

            if (0 == *pLen) return -1;

            //
            SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;
            Selector selector = (Selector)pSock.selector_r;

            try
            {

                ByteBuffer bb_r = (ByteBuffer)pSock.bb_buf_r;
#if false
				if (*pLen > bb_r.Capacity()) goto errLabel;
				bb_r.Position(0);
				bb_r.Limit(*pLen);
#endif

                //
                bool bDbg = false;
#if DEBUG
                //bDbg=true;
#endif
                //
                nRead = Com.Qycx.Tmpjavalibrary.TmpSock.QyRecv_android(socketChannel, selector, bb_r, *pLen, timeoutVal, bDbg);


                //nRead = socketChannel.Read(bb_r);
                //
                //pSock.b_r_selected = false;
                //
                if (nRead <= 0)
                {
                    goto errLabel;
                }
                System.IntPtr p = bb_r.GetDirectBufferAddress();
                qyFuncs.mymemcpy((System.IntPtr)buf, p, (uint)nRead);
                *pLen = nRead;
            }
            catch (Exception e)
            {
                goto errLabel;
            }

            //byte[] a_buf = new byte[*pLen];
#if false
				byte[] a_buf = pSock.buf_r;
			if (a_buf == null)
			{
				traceLog("qyRecv failed, buf_r is null");
				goto errLabel;
			}
			if (a_buf.Length < *pLen)
			{
				traceLog("qyRecv failed, buf_r is too small");
				goto errLabel;
			}
			try
			{
				nRead = pSock.socket.Receive(a_buf, *pLen, 0);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				goto errLabel;
			}
			if (nRead <= 0) goto errLabel;
			Marshal.Copy(a_buf, 0, (IntPtr)buf, nRead);

			*pLen = nRead;

#endif

            iErr = 0;

        errLabel:

            return iErr;

        }

        //
        // Use select() to determine when
        // data is waiting to be read
        //  返回值：>  0  为有数据
        //			== 0  为超时
        //			<  0  为失败
        public static int qyWaitForRead_android(ref QY_SOCK pSock, int timeoutInMs)
        {

            int iErr = -1;

            try
            {
                SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;

                Selector selector = (Selector)pSock.selector_r;

                bool bDbg = false;
#if DEBUG
                //bDbg = true;
#endif

                iErr = Com.Qycx.Tmpjavalibrary.TmpSock.QyWaitForRead_android(socketChannel, selector, timeoutInMs, bDbg);



            }
            catch (Exception e)
            {
                //Console.WriteLine(e.ToString());
                goto errLabel;
            }


        errLabel:
            return iErr;
        }


        public static int qyWaitForWrite_android(ref QY_SOCK pSock, int timeoutInMs)
        {
            int iErr = -1;

            try
            {
                SocketChannel socketChannel = (SocketChannel)pSock.socketChannel;
                Selector selector = (Selector)pSock.selector_w;

                bool bDbg = false;
#if DEBUG
                //bDbg = true;
#endif

                iErr = Com.Qycx.Tmpjavalibrary.TmpSock.QyWaitForWrite_android(socketChannel, selector, timeoutInMs, bDbg);


            }
            catch (Exception e)
            {
                qyFuncs.traceLog(e.ToString());
                goto errLabel;
            }
        errLabel:
            return iErr;
        }


    }
}