
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using File = Java.IO.File;
using Java.IO;
using Java.Lang;
using Java.Util.Concurrent;
using Java.Util;
using Android.Util;
using Java.Nio;
using Android.Content.Res;
using qyMessengerApp.Droid;
using Android.Renderscripts;
using static Android.Provider.Telephony.Mms;
using Thread = Java.Lang.Thread;
using Exception = Java.Lang.Exception;
using IOException = Java.IO.IOException;

//
namespace qy.Droid
{
    public class decObj_a_mediaCodec_async : Thread
    {

        static string LOG_TAG = "";
        private static bool VERBOSE = true;//false;           // lots of logging

        //
        public QOI_common m_qoi;

        public bool m_bEnd;

        bool m_bTest = false;

        public int m_old_lPktId;        //  用来防止getNextPkt出错，得到的是重复包

        //
        byte[] m_tmpDataBuf = new byte[Consts.bufSize_transferAudioData];


        private Context context;

        //private MediaFormat audioFormat;

        private File pcmFile;

        private FileOutputStream fos = null;

        private MediaCodec decodeCodec = null;



        private LinkedBlockingQueue mOutDataQueue = new Java.Util.Concurrent.LinkedBlockingQueue();
        //private Java.Util.IQueue<byte[]> mOutDataQueue = new LinkedBlockingQueue<>();

        private LinkedBlockingQueue mInputDataQueue = new LinkedBlockingQueue();
        //private Queue<Integer> mInputDataQueue = new LinkedBlockingQueue<>();


#if DEBUG
        public MediaExtractor m_test_extractor = null;
        public int m_test_trackIndex = 0;
#endif
        //
        MediaFormat m_dec_format = null;


        private Handler mHandler;



        public Tmp_outRunnable outRunnable = null;
        public Tmp_inputRunnable inputRunnable = null;

        //
        public TRANSFORM_AUDIO_procInfo_decInfo m_decInfo = new TRANSFORM_AUDIO_procInfo_decInfo();


        //
        public class Tmp_outRunnable : Java.Lang.Object, IRunnable
        {
            decObj_a_mediaCodec_async mOwner;

            public Tmp_outRunnable(decObj_a_mediaCodec_async owner)
            {
                mOwner = owner;
            }

            public void Run()
            {

                try
                {
                    //Log.Error(LOG_TAG, "outRunnable,当前线程： " + Thread.CurrentThread().Name);

                    byte[] pcmData = (byte[])mOwner.mOutDataQueue.Poll();
                    if (pcmData == null)
                    {
                        return;
                    }
                    Log.Error(LOG_TAG, "Handler回调收到,当前数据大小：" + pcmData.Length);
                    //装车
#if false
                    mOwner.fos.Write(pcmData);//数据写入文件中
                    mOwner.fos.Flush();
#endif
                    dec_putPcmData(mOwner, pcmData, pcmData.Length,  0);
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }

            }
        };


        public class Tmp_inputRunnable : Java.Lang.Object, IRunnable
        {
            decObj_a_mediaCodec_async mOwner;

            public Tmp_inputRunnable(decObj_a_mediaCodec_async owner)
            {
                mOwner = owner;
            }

            public void Run()
            {

                try
                {
                    //Log.Error(LOG_TAG, "inputRunnable,当前线程： " + Thread.CurrentThread().Name);

                    int index = (int)mOwner.mInputDataQueue.Poll();
                    if (index == null)
                    {
                        return;
                    }

                    ByteBuffer buffer;
                    if ((int)Build.VERSION.SdkInt >= 21)
                    {
                        buffer = mOwner.decodeCodec.GetInputBuffer(index);
                    }
                    else
                    {
                        buffer = mOwner.decodeCodec.GetInputBuffers()[index];
                    }
                    long sampleTime = 0;
                    int sampleSize = 0;
#if false
                    sampleSize = mOwner.m_test_extractor.ReadSampleData(buffer, 0);
                    sampleTime = mOwner.m_test_extractor.SampleTime;
#endif

                    sampleSize = dec_ReadSampleData(mOwner, buffer, 0, ref sampleTime);
                    //
                    if (sampleSize < 0)
                    {
                        Log.Info(LOG_TAG, "当前音频已经读取完了");
                        mOwner.decodeCodec.QueueInputBuffer(index, 0, 0, 0, MediaCodecBufferFlags.EndOfStream);
                    }
                    else
                    {
                        //Log.Info(LOG_TAG, "读取到了音频数据，当前音频数据的数据长度为：" + sampleSize);

                        //
                        mOwner.decodeCodec.QueueInputBuffer(index, 0, sampleSize, sampleTime, 0);

                        //
                        //mOwner.m_test_extractor.Advance();
                        dec_advance(mOwner);
                    }
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }

            }
        };

        //
        public unsafe int dec_init()
        {
            int iErr = -1;

            //

            //
            if (m_bTest)
            {
#if DEBUG
                Context context = qyMessengerApp.Droid.MyApplication.Context;

                //
                AssetManager assets = context.Assets;
                //
                AssetFileDescriptor afd = assets.OpenFd("dumpPcm.mp3");

                //
                m_test_extractor = new MediaExtractor();
                MediaExtractor extractor = m_test_extractor;
                extractor.SetDataSource(afd);
                int trackIndex = selectTrack(extractor);
                if (trackIndex < 0)
                {
                    //throw new RuntimeException("No video track found in " + inputFile);
                }
                extractor.SelectTrack(trackIndex);

                MediaFormat format = extractor.GetTrackFormat(trackIndex);
                if (VERBOSE)
                {
                    //Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }

                //
                m_test_trackIndex = trackIndex;

                //
                m_dec_format = format;

                //
                //m_iW = format.GetInteger(MediaFormat.KeyWidth);
                //m_iH = format.GetInteger(MediaFormat.KeyHeight);
#endif
            }
            else
            {
                //int width = this.m_iW;
                //int height = this.m_iH;
                MediaFormat format = MediaFormat.CreateAudioFormat(Consts.MIME_TYPE_mp3, 16000, 1);

#if false
                ByteBuffer bb_extraData;
                int sizeof_spspps = 1024;
                bb_extraData = ByteBuffer.AllocateDirect(sizeof_spspps);
                if (bb_extraData == null) goto errLabel;
                //
                uint len = (uint)bb_extraData.Capacity();
                //                            
                //if (0 != this.m_qoi.cfg.v.pf_transformGetSpsPps(this.m_qoi.cfg.v.uiTransformType_pParent, this.m_qoi.cfg.v.pParent_transform, this.m_qoi.cfg.v.iDecParam_index_pMems_from, (byte*)bb_extraData.GetDirectBufferAddress(), &len)) goto errLabel;

                //
                format.SetByteBuffer("csd-0", bb_extraData);

                if (VERBOSE)
                {
                    Android.Util.Log.Debug(TAG, "Video size is " + format.GetInteger(MediaFormat.KeyWidth) + "x" + format.GetInteger(MediaFormat.KeyHeight));
                }
#endif

                //
                m_dec_format = format;

                //
            }

            iErr = 0;
        errLabel:

            string str;
            str = string.Format("decObj_mediaCodec.dec_init leaves, iErr {0}", iErr);
            qyFuncs.showInfo_open(0, null, str);

            //
            return iErr;
        }

        public int dec_exit()
        {
            if (m_bTest)
            {
#if DEBUG
                MediaExtractor extractor = m_test_extractor;
                if (extractor != null)
                {
                    extractor.Release();
                    extractor = null;
                }
                m_test_extractor = extractor;
#endif
            }
            else
            {

            }

            //
            return 0;
        }



        //
        static unsafe int dec_ReadSampleData(decObj_a_mediaCodec_async dm, ByteBuffer inputBuf, int offset, ref long presentationTimeUs)
        {
            int chunkSize = -1;

            //
            bool bDbg = false;
            string str;

            //
#if DEBUG
            //bDbg = true;
#endif

            //
#if DEBUG
            if (bDbg)
            {
                str = string.Format("decA.dec_ReadSampleData enters");
                qyFuncs.showInfo_open(0, null, str);
            }
#endif


            //
            if (dm.m_bTest)
            {
#if DEBUG
                chunkSize = dm.m_test_extractor.ReadSampleData(inputBuf, offset);
                presentationTimeUs = dm.m_test_extractor.SampleTime;
#endif
            }
            else
            {
                uint uiToInMs = 0;

                uint nBytesRead = (uint)inputBuf.Capacity();
                uint uiSampleTimeInMs = 0;
#if true
                //fixed (int* pold_lPktId = &dm.m_old_lPktId)
                ref int pold_lPktId = ref dm.m_old_lPktId;
                {
                    nBytesRead = (uint)dm.m_tmpDataBuf.Length;
                    //
                    if (0 != dm.m_qoi.cfg.a.dec.pf_smplBitstreamReader_ReadNextFrame_func(ref dm.m_qoi, ref pold_lPktId, uiToInMs, dm.m_tmpDataBuf, ref nBytesRead, ref uiSampleTimeInMs,dm))
                    {
                        str = string.Format("decA.dec_ReadSmpleData：sampleBitstreamReader failed");
                        qyFuncs.showInfo_open(0, null, str);
                        //
                        goto errLabel;
                    }
                    if (nBytesRead == 0)
                    {
                        int ii = 0;
                    }
                    //inputBuf.Limit((int)nBytesRead);
                    inputBuf.Clear();
                    inputBuf.Put(dm.m_tmpDataBuf, 0, (int)nBytesRead);


                }
#endif
                //
                chunkSize = (int)nBytesRead;
                presentationTimeUs = uiSampleTimeInMs * 1000;

                //
#if DEBUG
#endif

            }

        errLabel:


#if DEBUG
            if (bDbg)
            {
                str = string.Format("decA.dec_ReadSampleData leaves. chunkSize {0}", chunkSize);
                qyFuncs.showInfo_open(0, null, str);
            }
#endif

            ///
            return chunkSize;
        }


        //
        static int dec_advance(decObj_a_mediaCodec_async dm)
        {
            if (dm.m_bTest)
            {
#if DEBUG
                dm.m_test_extractor.Advance();
#endif
            }
            else
            {

            }

            //
            return 0;
        }

        //
        static unsafe int dec_putPcmData(decObj_a_mediaCodec_async dm, byte[] pcm, int pcmLen,  int iSampleTimeInMs)
        {
            int iErr = -1;
            string str;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            myPLAY_AUDIO_DATA pkt;
            qyFuncs.mymemset((System.IntPtr)(byte*)&pkt, 0, sizeof(myPLAY_AUDIO_DATA));

            myPLAY_AUDIO_DATA* pPkt_dec = &pkt;
            //
            //mymemset((IntPtr)(byte*)&pPkt_dec->head, 0, sizeof(TRANSFER_AUDIO_dataMemHead));   //  2015/02/14
            QY_TRANSFORM pTransform = (QY_TRANSFORM)dm.m_qoi.cfg.a.pParent_transform;
#if false
            if (0 != pMem->idInfo.ui64Id)
            {
                pPkt_dec->head.idInfo.ui64Id = pMem->idInfo.ui64Id;
                if (pPkt_dec->head.idInfo.ui64Id == pMsgTask->data.route.idInfo_from.ui64Id) pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
                else pPkt_dec->head.tmpInternal.uiTranNo_org = 0;
            }
            else
            {
                pPkt_dec->head.idInfo.ui64Id = pMsgTask->data.route.idInfo_from.ui64Id;
                pPkt_dec->head.tmpInternal.uiTranNo_org = pReq->uiTranNo_openAvDev;
            }
            //  2017/10/13
            pPkt_dec->head.uiPts = pMem->uiPts;
#endif
            pPkt_dec->head.idInfo.ui64Id = pTransform.audio.idInfo.ui64Id;
            pPkt_dec->head.uiTranNo_openAvDev = pTransform.audio.ta.uiTranNo_openAvDev;

            //
            int nAvgBytes = 16000 * 2 * 1;
            if (nAvgBytes == 0) goto errLabel;
            pPkt_dec->head.uiSampleTimeInMs = (uint)(1000.0f * dm.m_qoi.var.decInfo.nBytes_deced / (float)nAvgBytes);
            dm.m_qoi.var.decInfo.nBytes_deced += pcmLen;// pcm.Length;



            //GuiShare.pf_dumpData(chunkPcm, 0, 0, true, false, "dec_pc.pcm");
            //
            //str = string.Format("doDec: get pcm {0} bytes, st {1}", pcm.Length, pPkt_dec->head.uiSampleTimeInMs);
            //qyFuncs.showInfo_open(0, null, str);

            //
            pPkt_dec->head.uiLen = (uint)pcmLen;// pcm.Length;

            //
            if (0 == qyFuncs.chkFromPlayerIndex_a(Consts.CONST_uiTransformType_dec, pTransform, pPkt_dec, null))
            {
                qyFuncs.playAudioData(pProcInfo, (int)pPkt_dec->head.uiSampleTimeInMs, pPkt_dec->head.uiPts, pcm, pPkt_dec->head.uiLen, pTransform.iIndex_player);
            }

            //
            iErr = 0;


        errLabel:
            return iErr;


        }





        //
        public decObj_a_mediaCodec_async()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;

            //
            context = mainActivity;

            //
            Log.Error(LOG_TAG, "outRunnable,当前线程： " + Thread.CurrentThread().Name);

            //
            outRunnable = new Tmp_outRunnable(this);
            inputRunnable = new Tmp_inputRunnable(this);


            //
#if false
            pcmFile = new File(context.GetExternalFilesDir(Environment.DirectoryMusic), "demo5a.pcm");
            try
            {
                pcmFile.CreateNewFile();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
#endif


        }

        public class Tmp_Handler : Handler
        {
            decObj_a_mediaCodec_async m_owner;

            public Tmp_Handler(decObj_a_mediaCodec_async mm, Looper looper) : base(looper)
            {
                m_owner = mm;
            }


            public override void HandleMessage(Message msg)
            {
                base.HandleMessage(msg);
                if (msg.What == 0)
                {
                    m_owner.destory();
                }
            }

        }


        public class Tmp_MediaCodec_Callback : MediaCodec.Callback

        {
            decObj_a_mediaCodec_async mOwner;

            public Tmp_MediaCodec_Callback(decObj_a_mediaCodec_async owner)
            {
                mOwner = owner;
            }


            public override void OnInputBufferAvailable(MediaCodec codec, int index)
            {
                //Log.Error(LOG_TAG, "OnInputBufferAvailable,当前线程： " + Thread.CurrentThread().Name);

                //
                //Log.Info(LOG_TAG, "异步回调，onInputBufferAvailable,当前index:" + index);

                //
                mOwner.mInputDataQueue.Offer(index);
                mOwner.mHandler.Post(mOwner.inputRunnable);

            }

            //
            byte[] m_pcmData = new byte[1024];

            public override void OnOutputBufferAvailable(MediaCodec codec, int index, MediaCodec.BufferInfo info)
            {
                //Log.Error(LOG_TAG, "OnOutputBufferAvailable,当前线程： " + Thread.CurrentThread().Name);

                //
                //Log.Info(LOG_TAG, "异步回调，onOutputBufferAvailable,当前index:" + index);
                //Log.Info(LOG_TAG, "获取到解码后的数据了，当前解析后的数据长度为：" + info.Size);
                //拿到当前装满火腿肠的筐
                ByteBuffer outputBuffer;
                if ((int)Build.VERSION.SdkInt >= 21)
                {
                    outputBuffer = codec.GetOutputBuffer(index);
                }
                else
                {
                    outputBuffer = codec.GetOutputBuffers()[index];
                }
                //将火腿肠放到新的容器里，便于后期装车运走
                //byte[] pcmData = new byte[info.Size];
                if  (m_pcmData.Length < info.Size)
                {
                    m_pcmData = new byte[info.Size];
                }
                byte[] pcmData = m_pcmData;
                
                //
                outputBuffer.Get(pcmData,0,info.Size);//写入到字节数组中
                outputBuffer.Clear();//清空当前筐
                                     //将装猪的数据放到队列里面，通过handler发送消息在子线程装入数据
#if false
                mOwner.mOutDataQueue.Offer(pcmData);
                mOwner.mHandler.Post(mOwner.outRunnable);
#endif
                int iSampleInMs = (int)(info.PresentationTimeUs / 1000.0f);

                dec_putPcmData(mOwner, pcmData, info.Size,  iSampleInMs);

                //把筐放回工厂里面
                codec.ReleaseOutputBuffer(index, false);

                if ((info.Flags & MediaCodecBufferFlags.EndOfStream) != 0)
                {
                    mOwner.mHandler.SendEmptyMessage(0);
                }
            }


            public override void OnError(MediaCodec codec, MediaCodec.CodecException e)
            {
                Log.Info(LOG_TAG, "异步回调，onError" + e.ToString());
            }


            public override void OnOutputFormatChanged(MediaCodec codec, MediaFormat format)
            {
                Log.Info(LOG_TAG, "异步回调，onOutputFormatChanged" + format.ToString());

            }
        }

        private int selectTrack(MediaExtractor extractor)
        {
            // Select the first video track we find, ignore the rest.
            int numTracks = extractor.TrackCount;
            for (int i = 0; i < numTracks; i++)
            {
                MediaFormat format = extractor.GetTrackFormat(i);
                string mime = format.GetString(MediaFormat.KeyMime);
                if (mime.StartsWith("audio/"))
                {
                    if (VERBOSE)
                    {
                        //Log.d(TAG, "Extractor selected track " + i + " (" + mime + "): " + format);
                    }
                    return i;
                }
            }

            return -1;
        }


        public override void Run()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();


            //base.Run();
            Looper.Prepare();
            mHandler = new Tmp_Handler(this, Looper.MyLooper());



            Log.Error(LOG_TAG, "Decode,当前线程： " + Thread.CurrentThread().Name);

            try
            {
#if false
                fos = new FileOutputStream(pcmFile.AbsoluteFile);
                //audioExtractor.SetDataSource(context.Resources.OpenRawResourceFd(Resource.Raw.dumppcm));
                AssetManager assets = context.Assets;
                this.m_test_extractor.SetDataSource(assets.OpenFd("dumpPcm.mp3"));
                int count = this.m_test_extractor.TrackCount;
                for (int i = 0; i < count; i++)
                {
                    audioFormat = this.m_test_extractor.GetTrackFormat(i);
                    if (audioFormat.GetString(MediaFormat.KeyMime).StartsWith("audio/"))
                    {
                        this.m_test_extractor.SelectTrack(i);
                        Log.Info(LOG_TAG, "aac 找到了通道" + i);
                        break;
                    }
                }
#endif
                dec_init();

                //
                string str1 = m_dec_format.GetString(MediaFormat.KeyMime);

                //初始化MiediaCodec
                decodeCodec = MediaCodec.CreateDecoderByType(this.m_dec_format.GetString(MediaFormat.KeyMime));
                //
                // 通过回调方式来进行数据的编码，比刚才手动调用方式更合理，回调运行在主线程，记得切换线程
                //
                decodeCodec.SetCallback(new Tmp_MediaCodec_Callback(this));

                //
                if (!pProcInfo.cfg.b_no_KeyLowLatency_a)
                {
                    this.m_dec_format.SetInteger(MediaFormat.KeyLowLatency, 1);
                }

                //
                /*
                m_dec_format.SetInteger(MediaFormat.KeyBitRate, 32000);
                m_dec_format.SetInteger(MediaFormat.KeySampleRate, 16000);
                m_dec_format.SetInteger(MediaFormat.KeyChannelCount, 1);

                //
                m_dec_format.SetInteger(MediaFormat.KeyMaxInputSize, 0);
                */


                //先配置callBack，再配置config；数据格式，surface用来渲染解析出来的数据;加密用的对象；标志 encode ：1 decode：0
                decodeCodec.Configure(this.m_dec_format, null, null, 0);

                //启动解码
                decodeCodec.Start();

            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }

            Looper.Loop();

            //
            //
            this.m_bEnd = true;

            //
            qyFuncs.mySetEvent((System.Threading.EventWaitHandle)this.m_qoi.hEvent_notifyToQuit);

            //
            string str;
            str = string.Format("a mediaCodec_async.Run ends.");
            qyFuncs.showInfo_open(0, null, str);


        }

        public void destory()
        {

            Log.Info(LOG_TAG, "销毁资源");
#if false
            if (this.m_test_extractor != null)
            {
                this.m_test_extractor.Release();
            }
#endif
            dec_exit();
            //
            if (decodeCodec != null)
            {
                decodeCodec.Stop();
                decodeCodec.Release();
            }
            if (fos != null)
            {
                try
                {
                    fos.Close();
                }
                catch (IOException e)
                {
                    e.PrintStackTrace();
                }
            }

            //
            Looper.MyLooper().QuitSafely();

        }


    }


}


