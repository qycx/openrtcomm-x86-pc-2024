
namespace qy.Droid
{

#if false
    public class DecodeAACAsyn : Thread
    {

        static string LOG_TAG = "";

        
        private Context context;
        
    private MediaFormat audioFormat;
        
    private File pcmFile;
        
    private static FileOutputStream fos = null;
        
    private static MediaCodec decodeCodec = null;



    private static LinkedBlockingQueue  mOutDataQueue = new Java.Util.Concurrent.LinkedBlockingQueue();
        //private Java.Util.IQueue<byte[]> mOutDataQueue = new LinkedBlockingQueue<>();

        private static LinkedBlockingQueue mInputDataQueue = new LinkedBlockingQueue();
    //private Queue<Integer> mInputDataQueue = new LinkedBlockingQueue<>();

    private static MediaExtractor audioExtractor = new MediaExtractor();

    private static Handler mHandler;



        public static Tmp_outRunnable outRunnable = new Tmp_outRunnable();

        public class Tmp_outRunnable : Java.Lang.Object, IRunnable
        {
            public void Run()
            {
                
            try
            {
                Log.Error(LOG_TAG, "outRunnable,当前线程： " + Thread.CurrentThread().Name);
                byte[] pcmData = (byte[])mOutDataQueue.Poll();
                if (pcmData == null)
                {
                    return;
                }
                Log.Error(LOG_TAG, "Handler回调收到,当前数据大小：" + pcmData.Length);
                //装车
                fos.Write(pcmData);//数据写入文件中
                fos.Flush();
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
                
            }
        };

        static Tmp_inputRunnable inputRunnable = new Tmp_inputRunnable();

        private class Tmp_inputRunnable : Java.Lang.Object, IRunnable
        {
            public void Run()
            {
                
                try
                {
                    Log.Error(LOG_TAG, "inputRunnable,当前线程： " + Thread.CurrentThread().Name);

                    int index = (int)mInputDataQueue.Poll();
                    if (index == null)
                    {
                        return;
                    }

                    ByteBuffer buffer;
                    if ((int)Build.VERSION.SdkInt >= 21)
                    {
                        buffer = decodeCodec.GetInputBuffer(index);
                    }
                    else
                    {
                        buffer = decodeCodec.GetInputBuffers()[index];
                    }
                    int sampleSize = audioExtractor.ReadSampleData(buffer, 0);
                    if (sampleSize < 0)
                    {
                        Log.Info(LOG_TAG, "当前音频已经读取完了");
                        decodeCodec.QueueInputBuffer(index, 0, 0, 0, MediaCodecBufferFlags.EndOfStream);
                    }
                    else
                    {
                        Log.Info(LOG_TAG, "读取到了音频数据，当前音频数据的数据长度为：" + sampleSize);
                        long sampleTime = audioExtractor.SampleTime;
                        decodeCodec.QueueInputBuffer(index, 0, sampleSize, sampleTime, 0);
                        audioExtractor.Advance();
                    }
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }
                
            }
        };

        public DecodeAACAsyn(Activity demo5Activity)
        {
            context = demo5Activity;

            //
            Log.Error(LOG_TAG, "outRunnable,当前线程： " + Thread.CurrentThread().Name);

            //
            pcmFile = new File(context.GetExternalFilesDir(Environment.DirectoryMusic), "demo5a.pcm");
            try
            {
                pcmFile.CreateNewFile();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
            

        }

        public class Tmp_Handler:Handler
        {
            DecodeAACAsyn m_mm;

            public Tmp_Handler(DecodeAACAsyn mm, Looper looper) : base(looper)
            {
                m_mm = mm;
            }

             
            public override void HandleMessage(Message msg)
            {
                base.HandleMessage(msg);
                if (msg.What == 0)
                {
                    m_mm.destory();
                }
            }

        }

    
        public class Tmp_MediaCodec_Callback:MediaCodec.Callback
            
        {
            
           
            public override void OnInputBufferAvailable( MediaCodec codec, int index)
            {
                Log.Error(LOG_TAG, "OnInputBufferAvailable,当前线程： " + Thread.CurrentThread().Name);

                //
                Log.Info(LOG_TAG, "异步回调，onInputBufferAvailable,当前index:" + index);
                mInputDataQueue.Offer(index);
                mHandler.Post(inputRunnable);

            }

            public override void OnOutputBufferAvailable( MediaCodec codec, int index, MediaCodec.BufferInfo info)
            {
                Log.Error(LOG_TAG, "OnOutputBufferAvailable,当前线程： " + Thread.CurrentThread().Name);

                //
                Log.Info(LOG_TAG, "异步回调，onOutputBufferAvailable,当前index:" + index);
                Log.Info(LOG_TAG, "获取到解码后的数据了，当前解析后的数据长度为：" + info.Size);
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
                byte[] pcmData = new byte[info.Size];
                outputBuffer.Get(pcmData);//写入到字节数组中
                outputBuffer.Clear();//清空当前筐
                                     //将装猪的数据放到队列里面，通过handler发送消息在子线程装入数据
                mOutDataQueue.Offer(pcmData);
                mHandler.Post(outRunnable);
                //把筐放回工厂里面
                codec.ReleaseOutputBuffer(index, false);

                if ((info.Flags & MediaCodecBufferFlags.EndOfStream) != 0)
                {
                    mHandler.SendEmptyMessage(0);
                }
            }


            public override void OnError( MediaCodec codec, MediaCodec.CodecException e)
            {
                Log.Info(LOG_TAG, "异步回调，onError" + e.ToString());
            }


            public override void OnOutputFormatChanged(MediaCodec codec, MediaFormat format)
            {
                Log.Info(LOG_TAG, "异步回调，onOutputFormatChanged" + format.ToString());

            }
        }


        public override void Run()
        {
            
        //base.Run();
        Looper.Prepare();
            mHandler = new Tmp_Handler(this,Looper.MyLooper());

    
    
    Log.Error(LOG_TAG, "Decode,当前线程： " + Thread.CurrentThread().Name);

    try
    {
        fos = new FileOutputStream(pcmFile.AbsoluteFile);
                //audioExtractor.SetDataSource(context.Resources.OpenRawResourceFd(Resource.Raw.dumppcm));
                AssetManager assets = context.Assets;
                audioExtractor.SetDataSource(assets.OpenFd("dumpPcm.mp3"));
                int count = audioExtractor.TrackCount;
        for (int i = 0; i < count; i++)
        {
            audioFormat = audioExtractor.GetTrackFormat(i);
            if (audioFormat.GetString(MediaFormat.KeyMime).StartsWith("audio/"))
            {
                audioExtractor.SelectTrack(i);
                Log.Info(LOG_TAG, "aac 找到了通道" + i);
                break;
            }
        }

        //初始化MiediaCodec
        decodeCodec = MediaCodec.CreateDecoderByType(audioFormat.GetString(MediaFormat.KeyMime));
        //
        // 通过回调方式来进行数据的编码，比刚才手动调用方式更合理，回调运行在主线程，记得切换线程
        //
        decodeCodec.SetCallback(new Tmp_MediaCodec_Callback());

    //先配置callBack，再配置config；数据格式，surface用来渲染解析出来的数据;加密用的对象；标志 encode ：1 decode：0
    decodeCodec.Configure(audioFormat, null, null, 0);

    //启动解码
    decodeCodec.Start();

            } catch (IOException e)
    {
        e.PrintStackTrace();
    }

    Looper.Loop();
            

        }

        public void destory()
        {

            Log.Info(LOG_TAG, "销毁资源");
            if (audioExtractor != null)
            {
                audioExtractor.Release();
            }
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
        }
        

    }
#endif

}
