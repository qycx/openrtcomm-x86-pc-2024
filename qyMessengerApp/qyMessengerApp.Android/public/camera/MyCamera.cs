using Android.App;
using Android.Content;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Lang;
using Java.Util;
using Java.Util.Concurrent;
using qyMessengerApp.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    public class MyCamera
    {
        //
        public string mCameraId;
        public CameraDevice mCameraDevice;
        public CameraCaptureSession mPreviewSession;
        public bool m_camera_isOpen;

        //
        public Size mPreviewSize;
        public Size mVideoSize;

        //
        public Java.Util.Concurrent.Semaphore mCameraOpenCloseLock = new Java.Util.Concurrent.Semaphore(1);

        //
        MyCameraStateCallback mStateCallback;

        //
        public int mSensorOrientation;
        CaptureRequest.Builder mPreviewBuilder;

        //
        Handler mBackgroundHandler;


        //
        public MyCamera(Handler mBackgroundHandler)
        {
            this.mBackgroundHandler = mBackgroundHandler;

            //
            this.mStateCallback = new MyCameraStateCallback(this);

        }

        //


        //
        static Size chooseOptimalSize(Size[] choices, int width1, int height1, Size aspectRatio1)
        {
            List<Size> bigEnough = new List<Size>();
            List<Size> notBigEnough = new List<Size>();
            int w = aspectRatio1.Width;
            int h = aspectRatio1.Height;

            //
            int width = height1;
            int height = width1;

            //
#if DEBUG
            qyFuncs_Droid.printChoices(choices);
#endif

            if (w != 0)
            {
                //
                for (int i = 0; i < choices.Length; i++)
                {
                    Size option = choices[i];
                    if (option.Height == option.Width * h / w
                        && option.Width >= width && option.Height >= height)
                    {
                        bigEnough.Add(option);
                    }
                }
            }

            //
            if (bigEnough.Count > 0)
            {
                return (Size)Collections.Min(bigEnough, new CompareSizesByArea());
            }
            else
            {
                return choices[0];
            }


        }



        //
        public string selectCamera(CameraManager manager, bool bFront)
        {
            string sel_id = null;

            string[] cameraIdList = manager.GetCameraIdList();
            if (cameraIdList == null) return null;

            foreach (string cameraId in cameraIdList)
            {
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                if (bFront)
                {
                    if (((Integer)(characteristics.Get(CameraCharacteristics.LensFacing))).IntValue() == (int)LensFacing.Front)
                    {
                        sel_id = cameraId;
                        break;
                    }
                }
                else
                {
                    if (((Integer)(characteristics.Get(CameraCharacteristics.LensFacing))).IntValue() == (int)LensFacing.Back)
                    {
                        sel_id = cameraId;
                        break;
                    }
                }
            }
            if (sel_id == null)
            {
                if (cameraIdList.Length > 0)
                {
                    sel_id = cameraIdList[0];
                }
            }

            return sel_id;
        }

        public void openCamera(int width, int height, bool bFront)
        {
            Context context = qyMessengerApp.Droid.MyApplication.Context;

            //
            CameraManager manager = (CameraManager)context.GetSystemService(Context.CameraService);
            try
            {
                if (!mCameraOpenCloseLock.TryAcquire(2500, TimeUnit.Milliseconds))
                {
                    goto errLabel;

                }
                string[] cameraIdList = manager.GetCameraIdList();
                //bool bFront = true;
                string cameraId = selectCamera(manager, bFront);// manager.GetCameraIdList()[0];
                //
                mCameraId = cameraId;
                //
                CameraCharacteristics characteristics = manager.GetCameraCharacteristics(cameraId);
                StreamConfigurationMap map = (StreamConfigurationMap)characteristics
                    .Get(CameraCharacteristics.ScalerStreamConfigurationMap);
                mSensorOrientation = (int)characteristics.Get(CameraCharacteristics.SensorOrientation);
                if (map == null) goto errLabel;

                //Size[] sizes = map.GetOutputSizes(Class.FromType(typeof(ImageReader)));
                //if (sizes.Length == 0) goto errLabel;

                //mVideoSize = new Size(640, 480);
                if (mSensorOrientation == 90 || mSensorOrientation == 270)
                {
                    mVideoSize = new Size(height, width);

                }
                else
                {
                    mVideoSize = new Size(width, height);
                }
                //mVideoSize = chooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(ImageReader))), mVideoSize.Width, mVideoSize.Height, mVideoSize);
                //
                //mImageReader = ImageReader.NewInstance(mVideoSize.Width, mVideoSize.Height, Android.Graphics.ImageFormatType.Yuv420888, 3);
                //mImageReader.SetOnImageAvailableListener(mOnImageAvailableListener, mBackgroundHandler);
                //

                mVideoSize = chooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(Android.Graphics.SurfaceTexture))), width, height, mVideoSize);
                /*
                mPreviewSize = chooseOptimalSize(map.GetOutputSizes(Class.FromType(typeof(SurfaceTexture))), width, height, mVideoSize);
                //
                int orientation = (int)Resources.Configuration.Orientation;
                if (orientation == (int)Android.Content.Res.Orientation.Landscape)
                {
                    mTextureView.SetAspectRatio(mPreviewSize.Width, mPreviewSize.Height);
                }
                else
                {
                    mTextureView.SetAspectRatio(mPreviewSize.Height, mPreviewSize.Width);
                }
                configureTransform(width, height);
                */
                //
                manager.OpenCamera(cameraId, mStateCallback, null);

                //
                this.m_camera_isOpen = true;

                //
            }
            catch (CameraAccessException e)
            {
                //  Toast.MakeText(context, "cannot access camera", ToastLength.Short).Show();
                goto errLabel;
            }
            catch (NullPointerException e)
            {
                // Toast.MakeText(context, "cannot support camera2", ToastLength.Short).Show();
                goto errLabel;

            }
            catch (InterruptedException e)
            {
                //   Toast.MakeText(context, "camera2, interrupted", ToastLength.Short).Show();
                goto errLabel;

            }
            catch (Java.Lang.Exception e)
            {
                //  Toast.MakeText(context, "camera2, exception", ToastLength.Short).Show();
                goto errLabel;
            }

        errLabel:

            return;
        }

        public void closeCamera()
        {
            try
            {
                mCameraOpenCloseLock.Acquire();
                //
                this.m_camera_isOpen = false;
                //
                closePreviewSession();
                if (null != mCameraDevice)
                {
                    mCameraDevice.Close();
                    mCameraDevice = null;
                }
                /*
                if (null != mImageReader)
                {
                    mImageReader.Close();
                    mImageReader = null;
                }
                */
            }
            catch (InterruptedException e)
            {

            }
            catch (Java.Lang.Exception e)
            {
                int ii = 0;
            }
            finally
            {
                mCameraOpenCloseLock.Release();
            }

            //
            qyFuncs.showInfo_open(0, null, "MyCamera.closeCamera");

        }

        //
        public void closePreviewSession()
        {
            if (mPreviewSession != null)
            {
                mPreviewSession.Close();
                mPreviewSession = null;
            }
        }


        public void startPreview(Android.Graphics.SurfaceTexture st)
        {
            if (null == mCameraDevice
                //|| null==mPreviewSize
                )
            {
                return;
            }
            try
            {
                closePreviewSession();
                /*
                SurfaceTexture texture = mTextureView.SurfaceTexture;
                //Assert(texture != null);
                texture.SetDefaultBufferSize(mPreviewSize.Width, mPreviewSize.Height);
                */
                mPreviewBuilder = mCameraDevice.CreateCaptureRequest(CameraTemplate.Preview);

                List<Surface> surfaces = new List<Surface>();

                //
#if false
                Surface previewSurface = new Surface(texture);
                surfaces.Add(previewSurface);
                mPreviewBuilder.AddTarget(previewSurface);
#endif

                //
#if false
                Surface readerSurface = mImageReader.Surface;
                surfaces.Add(readerSurface);
                mPreviewBuilder.AddTarget(readerSurface);
#endif

                //
#if DEBUG
#if false
                int saveWidth = 640;
                int saveHeight = 360;

                //
                m_outputSurface = new CameraOutputSurface(this, (EGLContext)mOwner_wnd.mOwner_hTalk.m_var.av.m_share_eglContext, 640, 360,null);
                //
                m_outputSurface.mTextureRender.createEnv(saveWidth, saveHeight);

                //
                surfaces.Add(m_outputSurface.getSurface());
                mPreviewBuilder.AddTarget(m_outputSurface.getSurface());
#endif
#endif

                //
                Surface surface = new Surface(st);
                //
                surfaces.Add(surface);
                mPreviewBuilder.AddTarget(surface);

                //
                mCameraDevice.CreateCaptureSession(surfaces, new PreviewCaptureStateCallback(this), mBackgroundHandler);



            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }

        }


        public void updatePreview()
        {
            if (null == mCameraDevice)
            {
                return;
            }
            try
            {
                setupCaptureRequestBuilder(mPreviewBuilder);
                //
                //HandlerThread thread = new HandlerThread("cameraPreview");
                //thread.Start();
                //
                mPreviewSession.SetRepeatingRequest(mPreviewBuilder.Build(), null, mBackgroundHandler);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }

        }

        //
        public Android.Util.Range getRange()
        {
            Context context = qyMessengerApp.Droid.MyApplication.Context;

            CameraManager manager = (CameraManager)context.GetSystemService(Context.CameraService);
            CameraCharacteristics chars = null;
            try
            {
                chars = manager.GetCameraCharacteristics(mCameraId);
            }
            catch (CameraAccessException e)
            {
                e.PrintStackTrace();
            }
            Android.Util.Range[] ranges = chars.Get(CameraCharacteristics.ControlAeAvailableTargetFpsRanges).ToArray<Android.Util.Range>();

            Android.Util.Range result = null;

            foreach (Android.Util.Range range in ranges)
            {
                int min_fps = 7;// 10;

                if ((int)range.Lower < min_fps)
                    continue;
                if (result == null) result = range;
                else if ((int)range.Lower <= 15 && (int)range.Upper - (int)range.Lower > (int)result.Upper - (int)result.Lower)
                    result = range;

                /*
                int upper = (int)range.Upper;
                if (upper >= 10)
                {
                    if ( result==null || upper < (int)result.Upper)
                    {
                        result = range;
                    }
                }
                */
                continue;
            }

        errLabel:
            return result;
        }


        //
        public void setupCaptureRequestBuilder(CaptureRequest.Builder builder)
        {
            //builder.Set(CaptureRequest.ControlMode, new Java.Lang.Integer((int)ControlMode.Auto));
            //
            builder.Set(CaptureRequest.ControlAfMode, new Java.Lang.Integer((int)ControlAFMode.ContinuousPicture));
            //
            Android.Util.Range range = getRange();
            if (range != null)
            {
                builder.Set(CaptureRequest.ControlAeTargetFpsRange, range);
                //
                string str;
                str = string.Format("setupCaptureRequestBuilder: range [{0},{1}]", range.Lower, range.Upper);
                qyFuncs.showInfo_open(0, null, str);
            }
            //
            builder.Set(CaptureRequest.ControlAfTrigger, new Java.Lang.Integer((int)ControlAFTrigger.Idle));
        }


        ///////////////////////////
        ///
        class MyCameraStateCallback : CameraDevice.StateCallback
        {
            private MyCamera owner;

            public MyCameraStateCallback(MyCamera owner)
            {
                if (owner == null)
                {
                    throw new System.ArgumentNullException("owner");
                }
                this.owner = owner;
            }

            public override void OnOpened(CameraDevice cameraDevice)
            {
                bool bDbg = false;
                string str;

#if DEBUG
                bDbg = true;
#endif
                //
                if (bDbg)
                {
                    str = string.Format("MyCamperaStateCallback.478.OnOpened");
                    qyFuncs.showInfo_open(0, null, str);
                }


                //throw new NotImplementedException();
                owner.mCameraDevice = cameraDevice;
                //owner.startPreview();
                owner.mCameraOpenCloseLock.Release();
#if false
                if (null != owner.mTextureView)
                {
                    owner.configureTransform(owner.mTextureView.Width, owner.mTextureView.Height);
                }
#endif
                //
                owner.mBackgroundHandler.SendEmptyMessage(Consts.CONST_msg_cameraOpen);

            }

            public override void OnDisconnected(CameraDevice camera)
            {
                //throw new NotImplementedException();
                owner.mCameraOpenCloseLock.Release();
                camera.Close();
                owner.mCameraDevice = null;
            }

            public override void OnError(CameraDevice camera, [GeneratedEnum] CameraError error)
            {
                //throw new NotImplementedException();
                owner.mCameraOpenCloseLock.Release();
                camera.Close();
                owner.mCameraDevice = null;
                //
                //Activity.
            }

        }

        //
        class PreviewCaptureStateCallback : CameraCaptureSession.StateCallback
        {
            MyCamera owner;

            public PreviewCaptureStateCallback(MyCamera owner)
            {
                this.owner = owner;
            }

            public override void OnConfigured(CameraCaptureSession session)
            {
                bool bDbg = false;
                string str;
#if DEBUG
                bDbg = true;
#endif
                //
                if (bDbg)
                {
                    str = string.Format("PreviewCaptrueStateCallback.OnConfigured");
                    qyFuncs.showInfo_open(0, null, str);
                }

                //
                owner.mPreviewSession = session;
                owner.updatePreview();
            }

            public override void OnConfigureFailed(CameraCaptureSession session)
            {
                Context context = qyMessengerApp.Droid.MyApplication.Context;
                {
                    Toast.MakeText(context, "Failed", ToastLength.Short).Show();
                }

            }

        }


    }



}