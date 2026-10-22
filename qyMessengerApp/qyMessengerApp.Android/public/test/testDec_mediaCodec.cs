using Android.Animation;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    public struct TestDec
    {

        public TimeAnimator mTimeAnimator;// = new TimeAnimator();

        // A utility that wraps up the underlying input and output buffer processing operations
        // into an east to use API.
        public MediaCodecWrapper mCodecWrapper;
        public MediaExtractor mExtractor;// = new MediaExtractor();

        //
        public TMP_onFrameAvail m_onFrameAvail;

    };


    //
    public class TMP_onFrameAvail : Java.Lang.Object, SurfaceTexture.IOnFrameAvailableListener
    {
        public void OnFrameAvailable(SurfaceTexture? surfaceTexture)
        {
            //
            qyFuncs.traceLog("onFramAvail");

            //
            uint tid = qyFuncs.myGetCurrentThreadId();


            return;
        }

    }

    public class TMP_TimeListener : Java.Lang.Object, TimeAnimator.ITimeListener
    {
        TestDec m_td;

        public TMP_TimeListener(TestDec td)
        {
            m_td = td;
        }

        public void OnTimeUpdate(TimeAnimator animation, long totalTime, long deltaTime)
        {

            int ii = 0;
            string str;
  
            //
#if true

            bool isEos = (((int)m_td.mExtractor.SampleFlags & (int)MediaCodecBufferFlags.EndOfStream) == (int)MediaCodecBufferFlags.EndOfStream);

                        // BEGIN_INCLUDE(write_sample)
                        if (!isEos)
                        {
                            // Try to submit the sample to the codec and if successful advance the
                            // extractor to the next available sample to read.
                            bool result = m_td.mCodecWrapper.writeSample(m_td.mExtractor, false,
                                    m_td.mExtractor.SampleTime, (int)m_td.mExtractor.SampleFlags);

                            if (result)
                            {
                                // Advancing the extractor is a blocking operation and it MUST be
                                // executed outside the main thread in real applications.
                                m_td.mExtractor.Advance();
                            }
                        }
                        // END_INCLUDE(write_sample)

                        // Examine the sample at the head of the queue to see if its ready to be
                        // rendered and is not zero sized End-of-Stream record.
                        MediaCodec.BufferInfo out_bufferInfo = new MediaCodec.BufferInfo();
                        m_td.mCodecWrapper.peekSample(out_bufferInfo);

                        // BEGIN_INCLUDE(render_sample)
                        if (out_bufferInfo.Size <= 0 && isEos)
                        {
                            m_td.mTimeAnimator.End();
                            m_td.mCodecWrapper.stopAndRelease();
                            m_td.mExtractor.Release();
                        }
                        else if (out_bufferInfo.PresentationTimeUs / 1000 < totalTime)
                        {
                // Pop the sample off the queue and send it to {@link Surface}
                bool bRender = true;
                //
                //bRender = false;
                //
                            m_td.mCodecWrapper.popSample(bRender);
                        }
                        // END_INCLUDE(render_sample)
#endif

            return;
        }


    }


    partial class qyFuncs_Droid
    {
#if DEBUG




        //
        public static void testDec_startPlayback(ref TestDec td, TextureView mPlaybackView)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            Context context = mainActivity;

            //
#if true
            AssetManager assets = context.Assets;
            System.IO.Stream s = assets.Open("vid.mp4");
            s.Close();
            //
            AssetFileDescriptor afd = assets.OpenFd("vid.mp4");
#endif

            // Construct a URI that points to the video resource that we want to play
            //var file = new File(Android.Net.Uri.Parse("file:///android_asset/vid.mp4").ToString());

            //
#if false
            td.m_onFrameAvail = new TMP_onFrameAvail();
            mPlaybackView.SurfaceTexture.SetOnFrameAvailableListener(td.m_onFrameAvail);
#endif

            //
            try
            {

                // BEGIN_INCLUDE(initialize_extractor)
                td.mExtractor.SetDataSource(afd);
                int nTracks = td.mExtractor.TrackCount;

                // Begin by unselecting all of the tracks in the extractor, so we won't see
                // any tracks that we haven't explicitly selected.
                for (int i = 0; i < nTracks; ++i)
                {
                    td.mExtractor.UnselectTrack(i);
                }


                // Find the first video track in the stream. In a real-world application
                // it's possible that the stream would contain multiple tracks, but this
                // sample assumes that we just want to play the first one.
                for (int i = 0; i < nTracks; ++i)
                {
                    // Try to create a video codec for this track. This call will return null if the
                    // track is not a video track, or not a recognized video format. Once it returns
                    // a valid MediaCodecWrapper, we can break out of the loop.
                    td.mCodecWrapper = MediaCodecWrapper.fromVideoFormat(td.mExtractor.GetTrackFormat(i),
                            new Surface(mPlaybackView.SurfaceTexture));
                    if (td.mCodecWrapper != null)
                    {
                        td.mExtractor.SelectTrack(i);
                        break;
                    }
                }
                // END_INCLUDE(initialize_extractor)




                // By using a {@link TimeAnimator}, we can sync our media rendering commands with
                // the system display frame rendering. The animator ticks as the {@link Choreographer}
                // receives VSYNC events.

                td.mTimeAnimator.SetTimeListener(new TMP_TimeListener(td));
                

            // We're all set. Kick off the animator to process buffers and render video frames as
            // they become available
            td.mTimeAnimator.Start();
        } catch (Java.IO.IOException e) {
            e.PrintStackTrace();
        }
}
#endif


        }




}


