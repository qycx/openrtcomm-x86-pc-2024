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

//
using Com.Qycx.Gleslibrary;

//
using qyMessengerApp.Droid;
using Android.Opengl;
using Java.Nio;
using Android.Graphics;

//
namespace qy.Droid
{
    public class CTalkDraw : CQmcDrawTmpl
    {
        //
        public TALK_drawLayout m_drawLayout;
        public uint tn_modified_drawLayout;

        //
        //
        TalkTextureLoader mTextureLoader_me;
        TalkTextureLoader mTextureLoader_peer;

        //
        public static int me_texId;
        public static SurfaceTexture me_st;


        //
        public class Talk_tex
        {
            public int m_textureId_0;
            public int m_textureId_1;
            //
            public int m_cur_textureId;
        };
        public Talk_tex tex_me, tex_peer;

        //
#if DEBUG
        //public ExtractMpegFramesTest m_testDec;
#endif

        //
        public FloatBuffer mVertexBuffer_me;

        //
        public float[] VERTEX_me = {
                //1,1,0,
                //-3.27258f,15.18121f,-100.2942f,                
                1.105982f, -37.78366f, -85.60948f,
                //-1, 1, 0,
                //-52.47561f,15.18121f,-85.53316f,
                -15.27864f, -37.78366f, -80.69443f,
                //-1, -1, 0,
                //-45.90787f,-64.36166f,-63.64063f,
                -13.08971f, -64.29439f, -73.39761f,
                //1, -1, 0,
                //3.29516f,-64.36166f,-78.40168f,
                3.294913f, -64.29439f, -78.31304f,


        };


        //
        private T1_frm tf_me;
        private T1_frm tf_peer;
        private T1_frm centerSurfaceViewPortRect;
        //
        public class TalkTextureLoader : TextureLoader
        {
            //
            public TALK_drawMem m_drawMem;
            //
            public bool m_bMe;


            //
            public TalkTextureLoader(EglCore eglCore_shared, Context androidContext, CTalkDraw myDraw, bool bMe) : base(eglCore_shared, androidContext, myDraw)
            {
                this.m_bMe = bMe;
                return;
            }

            //
            public bool bDrawModified()
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                CTalkDraw mDraw = (CTalkDraw)this.mMyDraw;

                //
                //Fragment_dlgTalk_av fg = (Fragment_dlgTalk_av)mDraw.mOwner;
                Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)mDraw.mOwner.Activity;
                Activity_dlgTalk hTalk = (Activity_dlgTalk)hTalk_av.mOwner_hTalk;
                DLG_TALK_var m_var = hTalk.m_var;

                if (m_bMe)
                {
                    return (m_drawMem.tn_modified != m_var.m_drawLayout.param.me.tn_modified);
                }

                //                  
                return (m_drawMem.tn_modified != m_var.m_drawLayout.param.peer.tn_modified);
            }

            //
            public int updateDrawMem()
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                CTalkDraw mDraw = (CTalkDraw)this.mMyDraw;

                //
                Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)mDraw.mOwner.Activity;
                Activity_dlgTalk hTalk = hTalk_av.mOwner_hTalk;
                DLG_TALK_var m_var = hTalk.m_var;

                if (m_bMe)
                {
                    if (m_drawMem.tn_modified != m_var.m_drawLayout.param.me.tn_modified)
                    {
                        m_drawMem.tn_modified = m_var.m_drawLayout.param.me.tn_modified;
                        m_drawMem.param = m_var.m_drawLayout.param.me.param;
                    }
                }
                else
                {
                    if (m_drawMem.tn_modified != m_var.m_drawLayout.param.peer.tn_modified)
                    {
                        m_drawMem.tn_modified = m_var.m_drawLayout.param.peer.tn_modified;
                        m_drawMem.param = m_var.m_drawLayout.param.peer.param;
                    }
                }
                return 0;
            }

            //
            public override int fillTex_run()
            {
                CCtxQyMc pQyMc = g.g_pQyMc;
                CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
                CTalkDraw mDraw = (CTalkDraw)this.mMyDraw;

                bool bDbg = false;
                string str;

                //
                Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)mDraw.mOwner.Activity;
                Activity_dlgTalk hTalk = hTalk_av.mOwner_hTalk;
                DLG_TALK_var m_var = hTalk.m_var;

                //
                //mDraw.tn_modified_drawLayout = m_var.tn_modified_drawLayout;
                //mDraw.m_drawLayout = m_var.m_drawLayout;

                //
                str = string.Format("{0}.fillTex_run starts", this.m_tag);
                qyFuncs.showInfo_open(0, null, str);

                //
                if (!this.m_bMe)
                {
                    int ii = 0;

                }


                //
                for (; !mDraw.bQuit;)
                {
                    //
                    updateDrawMem();

                    //
                    if (m_drawMem.param.playerId.uiTranNo_player == 0)
                    {
                        qyFuncs.mySleep(200);
                        continue;
                    }
                    QY_PLAYER player = qyFuncs.getPlayerByIndex(pProcInfo, m_drawMem.param.playerId.index_player);
                    if (player == null)
                    {
                        qyFuncs.mySleep(200);
                        continue;
                    }
                    //
                    /*
                    fixed (BITMAPINFOHEADER* pBih = &player.video.vh.bih)
                    {
                        qyFuncs.makeBmpInfoHeader_i420(12, 640, 480, pBih);
                    }
                    */
                    if (player.video.vh.bih.biWidth == 0 || player.video.vh.bih.biHeight == 0)
                    {
                        //
                        str = string.Format("fillTex_run: {0}, bih err {1}x{2}", this.m_tag, player.video.vh.bih.biWidth, player.video.vh.bih.biHeight);
                        qyFuncs.traceLog(str);
                        //
                        qyFuncs.mySleep(200);
                        continue;
                    }
                    //
                    qyFuncs_Droid.ttl_readPkt(this);

                }


            errLabel:

                //
                if (m_bMe)
                {
                    int ii = 0;
                }

                //
                str = string.Format("{0}.fillTex_run ends", this.m_tag);
                qyFuncs.showInfo_open(0, null, str);


                //
                return 0;
            }

            //
            public int tmpFillTex(byte[] rgb, int w, int h)
            {
                return 0;
            }


            //
            public int tmpFillTex(int w, int h)
            {
                CTalkDraw myDraw = (CTalkDraw)this.mMyDraw;
                //byte[] rgb_array = (byte[])rgb;
                bool bDbg = false;
                if (!m_bMe)
                {
                    //bDbg = true;
                }

                //
                Talk_tex myTex = null;
                if (m_bMe)
                {
                    myTex = myDraw.tex_me;
                }
                else
                {
                    myTex = myDraw.tex_peer;
                }

                //                  
                if (myTex.m_textureId_0 == 0)
                {
                    int texId = loadTexture(w, h, m_texId_0);
                    if (texId != 0)
                    {
                        myTex.m_textureId_0 = texId;
                        //
                        if (bDbg)
                        {
                            qyFuncs.traceLog("tmpFillTex: tex0");
                        }
                    }
                }
                else if (myTex.m_textureId_1 == 0)
                {
                    int texId = loadTexture(w, h, m_texId_1);
                    if (texId != 0)
                    {
                        myTex.m_textureId_1 = texId;
                        //
                        if (bDbg)
                        {
                            qyFuncs.traceLog("tmpFillTex: tex1");
                        }
                    }
                }



                return 0;

            }


        }



        //
        public CTalkDraw(Activity_dlgTalk_av owner, int iFgType, WindowSurface windowSurface) : base(owner, iFgType, windowSurface)
        {
            //base(owner, windowSurface);

            int ii = 0;


        }

        //

        //


        //
        public override int init()
        {
            int iErr = -1;

            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            //
            if (0 != base.init()) return -1;
            MainActivity mainActivity =(MainActivity) pQyMc.gui.hMainWnd;
            //
            //
            mVertexBuffer_me = ByteBuffer.AllocateDirect(VERTEX_me.Length * 4)
                .Order(ByteOrder.NativeOrder())
                .AsFloatBuffer()
                .Put(VERTEX_me);
            mVertexBuffer_me.Position(0);


            //
            this.tf_me = new T1_frm();

            //            
            CAP_RECT capRect;
            capRect.iX = 0;
            capRect.iY = this.mWindowSurface.Height * 2 / 3;
            capRect.iW = (uint)this.mWindowSurface.Width / 3;
            capRect.iH = (uint)this.mWindowSurface.Height / 3;

           

            //
            if (capRect.iH == 0) goto errLabel;
            float fAspectRatio_img = capRect.iW / (float)capRect.iH;
            if (0 != tf_me.T1_frm_init(null, ref capRect, fAspectRatio_img))
            {
                goto errLabel;
            }

            //            
            Fragment_dlgTalk_av2 fg = (Fragment_dlgTalk_av2)this.mOwner;
            this.tf_peer = fg.tf_peer;

            //
            //            
            capRect.iX = 0;
            capRect.iY = 0;
            capRect.iW = (uint)this.mWindowSurface.Width;
            capRect.iH = (uint)this.mWindowSurface.Height;

         

            //
            if (capRect.iH == 0) goto errLabel;
            fAspectRatio_img = capRect.iW / (float)capRect.iH;
            if (0 != tf_peer.T1_frm_init(null, ref capRect, fAspectRatio_img))
            {
                goto errLabel;
            }




            //
            this.tex_me = new Talk_tex();
            this.tex_peer = new Talk_tex();

            //
            if (pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av)
            {
                //
                mTextureLoader_me = new TalkTextureLoader(this.mEglCore_shared, mOwner.Activity, this, true);
                if (mTextureLoader_me.init() != 0)
                {
                    goto errLabel;
                }
                mTextureLoader_me.iTestNo = 1;
                mTextureLoader_me.m_tag = "tex_me";
                mTextureLoader_me.Start();


                mTextureLoader_peer = new TalkTextureLoader(this.mEglCore_shared, mOwner.Activity, this, false);
                if (mTextureLoader_peer.init() != 0)
                {
                    goto errLabel;
                }
                mTextureLoader_peer.iTestNo = 2;
                mTextureLoader_peer.m_tag = "tex_peer";
                mTextureLoader_peer.Start();
            }

            //
            {
                int[] textureHandle = new int[1];
                GLES20.GlGenTextures(1, textureHandle, 0);
                //
                qyFuncs.showInfo_open(0, "", "talkDraw.init, glGenTex 1");
                //
                int tmpiRet = GLES20.GlGetError();
                if (textureHandle[0] == 0)
                {
                    qyFuncs.traceLog("TextureLoader.Run failed, GlGenTextures failed");
                    goto errLabel;
                }
                //
                me_texId = textureHandle[0];
            }
            me_st = new SurfaceTexture(me_texId);


            if (me_st == null) goto errLabel;

            //
            Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)mOwner.Activity;
            if (hTalk_av != null)
            {
                hTalk_av.save_dlgTalk_share_eglContext(this.mEglCore_shared.MEGLContext, "talkDraw.init");

                //
                switch (miFgType)
                {
                    case Consts.CONST_iFgType_dlgTalk_av2:
                        {
                            //Fragment_dlgTalk_av2 fg = (Fragment_dlgTalk_av2)mOwner;
                            hTalk_av.mOwner_hTalk.gui_PostMessage(Consts.CONST_qyWm_postComm, Consts.CONST_qyWmParam_chkTalkDraw, 0);
                        }

                        break;
                    default:
                        break;
                }
            }


            //
#if DEBUG



#endif


            //
            iErr = 0;

        errLabel:

            return iErr;
        }



        public override void exit()
        {
            this.bQuit = true;
            //
            if (mTextureLoader_me != null)
            {
                mTextureLoader_me.halt();
            }
            if (mTextureLoader_peer != null)
            {
                mTextureLoader_peer.halt();
            }
            //
            Activity_dlgTalk_av hTalk_av = (Activity_dlgTalk_av)mOwner.Activity;
            if (hTalk_av != null)
            {
                hTalk_av.save_dlgTalk_share_eglContext(null, "talkDraw.exit");
            }

            //
            if (me_st != null)
            {
                me_st.Release();
                me_st = null;
            }
            if (me_texId != 0)
            {
                int[] textureHandle = new int[1];
                textureHandle[0] = me_texId;


                GLES20.GlDeleteTextures(1, textureHandle, 0);
                //
                me_texId = 0;
            }

            //
            if (this.tf_me != null)
            {
                tf_me.T1_frm_exit(null);
                tf_me = null;
            }
            //
            if (this.tf_peer != null)
            {
                tf_peer.T1_frm_exit(null);
                tf_peer = null;
            }

            //
            base.exit();
            //
            return;
        }


        //
        public override int onSize(int w, int h)
        {
            int iErr = -1;

            base.onSize(w, h);



            //
            iErr = 0;
        errLabel:
            return iErr;
        }


        //
        int get_tti_peer(CCtxQmc pProcInfo, Activity_dlgTalk hTalk, ref TMP_tex_info tti)
        {
            int iErr = -1;
            DLG_TALK_var m_var = hTalk.get_pm_var();

            //
            ref TMP_texes texes = ref pProcInfo.av.texes;
            if (texes == null) return -1;

            TMP_task_texes ttt = texes.taskAvTexes;
            int i;
            ushort usCnt_activeMems = ttt.usCnt_activeMems;
            if (usCnt_activeMems > ttt.tti_peers.Length) goto errLabel;
            if (m_var.addr.uiObjType == Consts.CONST_objType_imGrp)
            {
                for (i = 0; i < usCnt_activeMems; i++)
                {
                    if (ttt.tti_peers[i].uiObjType == Consts.CONST_objType_mosaicStream_video
                        || ttt.tti_peers[i].uiObjType == Consts.CONST_objType_mosaicStream_resource)
                    {
                        tti = ttt.tti_peers[i];
                        break;
                    }
                }
            }
            else
            {
                // 暂时不处理屏幕共享
                for (i = 0; i < usCnt_activeMems; i++)
                {
                    if (ttt.tti_peers[i].idInfo.ui64Id == m_var.addr.idInfo.ui64Id
                        && ttt.tti_peers[i].uiObjType == 0)
                    {
                        tti = ttt.tti_peers[i];
                        break;
                    }
                }

            }

            iErr = 0;
        errLabel:

            return iErr;
        }


        //
        public void myPreRender(object pCtx)
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            //
            int width = this.mWindowSurface.Width;
            int height = this.mWindowSurface.Height;

            int img_w = 0;
            int img_h = 0;
            float fAspectRatio_img;

            //
            CAP_RECT capRect;

            capRect.iX = width * 2 / 3;
            capRect.iY = height * 2 / 3;
           
            capRect.iW = (uint)width / 3;
            capRect.iH = (uint)height / 3;
       

            //
            if (height == 0) goto errLabel;

            //
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)mOwner.Activity;
            if (dlgTalk_av == null) goto errLabel;
            Activity_dlgTalk dlgTalk = (Activity_dlgTalk)dlgTalk_av.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;
            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;

            if (m_var.av.ucbGot_iIndex_sharedObj_localAv)
            {
                QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                if (null == pSharedObj) goto errLabel;


                //
                // img_w=camera_RenderThread.m_iW_tex;
                // img_h=camera_RenderThread.m_iH_tex;
                img_w = pSharedObj.var.m_o_iW;
                img_h = pSharedObj.var.m_o_iH;
                //
                if (img_h != 0)
                {

                    //
                    fAspectRatio_img = img_w / (float)img_h;

                    //
                    if (this.tf_me.capRect.iX != capRect.iX
                        || this.tf_me.capRect.iY != capRect.iY
                        || this.tf_me.capRect.iW != capRect.iW
                        || this.tf_me.capRect.iH != capRect.iH
                        || Math.Abs(fAspectRatio_img - this.tf_me.fAspectRatio_img) > 0.001)
                    {
                       QY_timestamp ts = new QY_timestamp();
                        qyFuncs.traceLog("TAG", " new QY_timestamp()");
                        bool ucbAutoClip = false;// true;

                        float me_z_1 = Consts.CONST_fz_me;

                        if (0 != this.tf_me.T1_frm_set(pCtx, ref ts, ref capRect, fAspectRatio_img, ucbAutoClip, false, ref mMats, me_z_1, "", false))
                        {
#if DEBUG
                            qyFuncs.traceLog("myPreRender.644,  t1_frm_set failed");
#endif

                            goto errLabel;
                        }
                        //
#if DEBUG
                        qyFuncs.traceLog("myPreRender.647, after tf_me.t1_frm_set");
#endif
                       
                    }
                }
            }

            //////////
            ///
            capRect.iX = 0;
            capRect.iY = 0;
            capRect.iW = (uint)width;
            capRect.iH = (uint)height;
          



            if (height == 0) goto errLabel;

            //
#if DEBUG
#if false
            img_w = decObj_mediaCodec.m_tex_w;
            img_h = decObj_mediaCodec.m_tex_h;
#endif
#endif
            //
            QMC_TASK_INFO pTaskInfo = null;
            if (!m_var.av.taskInfo.bTaskExists) goto errLabel;

            //
            //ref TMP_texes texes = ref pProcInfo.av.texes;
            TMP_tex_info tti = null;// texes.tti_peer;
            //
            //get_tti_peer(pProcInfo, dlgTalk, ref tti);
            //
         //  if (tti == null) goto errLabel;
            //
            if (mainActivity.textureId != 0)
            {
                img_w = mainActivity.textureIw;
                img_h = mainActivity.textureIh;
                
            }
            else
            {
                img_w = this.mTexName_default_iW;
                img_h = this.mTexName_default_iH;
            }

            //
            if (img_h != 0)
            {

                fAspectRatio_img = img_w / (float)img_h;

                //
                if (this.tf_peer.capRect.iX != capRect.iX
                    || this.tf_peer.capRect.iY != capRect.iY
                    || this.tf_peer.capRect.iW != capRect.iW
                    || this.tf_peer.capRect.iH != capRect.iH
                    || Math.Abs(fAspectRatio_img - this.tf_peer.fAspectRatio_img) > 0.001)
                {
                    QY_timestamp ts = new QY_timestamp();
                    bool ucbAutoClip = false;// true;

                    float me_z_1 = Consts.CONST_fz_peer;

                    if (0 != this.tf_peer.T1_frm_set(pCtx, ref ts, ref capRect, fAspectRatio_img, ucbAutoClip, false, ref mMats, me_z_1, "", true))
                    {
                        qyFuncs.traceLog("myPreRender.717, T1_frm_set failed");
                        goto errLabel;
                    }
                    m_var.av.hDc_av_rect = tf_peer.centerSurfaceViewPortRect;
#if DEBUG
                   qyFuncs.traceLog("myPreRender.722, after tf_peer.T1_frm_set");
#endif

                }

            }



        errLabel:

#if DEBUG
            //qyFuncs.traceLog("myPreRender leaves");
#endif

            return;
        }


        //
        //

        public override void draw()
        {
            bool bDbg = false;
            string str;
            CCtxQyMc pQyMc = g.g_pQyMc;
            if (pQyMc == null) return;
            MainActivity mainActivity = (MainActivity)pQyMc.gui.hMainWnd;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            if (pProcInfo == null) return;

            //
            //
            Activity_dlgTalk_av dlgTalk_av = (Activity_dlgTalk_av)this.mOwner.Activity;
            if (dlgTalk_av == null) return;
            Activity_dlgTalk dlgTalk = dlgTalk_av.mOwner_hTalk;
            DLG_TALK_var m_var = dlgTalk.m_var;

            //
            if (pProcInfo.cfg.ucb_USE_specialDbg)
            {
                return;
            }

            //
            myPreRender(null);

            //
            GLES20.GlClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            GLES20.GlClear(GLES20.GlColorBufferBit);
            //
            GLES20.GlUniformMatrix4fv(mMatrixHandle, 1, false, mMMatrix, 0);
            GLES20.GlUniformMatrix4fv(vMatrixHandle, 1, false, mVMatrix, 0);
            GLES20.GlUniformMatrix4fv(pMatrixHandle, 1, false, mPMatrix, 0);

            //
            GLES20.GlEnableVertexAttribArray(mPositionHandle);
            GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer);

            //
            GLES20.GlActiveTexture(GLES20.GlTexture0);

            //
            Talk_tex myTex;


            bool bShow_peer = true;
            if (bShow_peer)
            {

                
                    GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, this.tf_peer.mVertexBuffer);

                    GLES20.GlVertexAttribPointer(mTexCoordHandle, 2, GLES20.GlFloat, false, 0, this.tf_peer.mTexVertexBuffer);
                
                /*
                GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, this.tf_me.mVertexBuffer);

                GLES20.GlVertexAttribPointer(mTexCoordHandle, 2, GLES20.GlFloat, false, 0, this.tf_me.mTexVertexBuffer);
                */


                //
                int t1_texId = 0;


                    if (m_var.av.taskInfo.bTaskExists)
                    {
                        TMP_tex_info tti = null;// ref texes.tti_peer;
                                                //
                                                // get_tti_peer(pProcInfo, dlgTalk, ref tti);
                                                //
                                                //if (tti != null)
                                                //{
                                                //    t1_texId = tti.texId;
                                                //}
                    t1_texId = mainActivity.textureId;
                    }

                    //
                    if (t1_texId != 0)
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, t1_texId);
                    }
                    else
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName_default);
                    }

                    GLES20.GlUniform1i(mTexSmaplerHandle, 0);

                    //
                    GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                        GLES20.GlUnsignedShort, mVertexIndexBuffer);
                
            }

            ////////////////////////////////
            bool bShow_me = false;
            if (dlgTalk.m_var.av.taskInfo.bTaskExists)
            {
                if (dlgTalk.m_var.av.taskInfo.bShow_localVideo) bShow_me = true;
            }
            if (bShow_me)
            {
                if (pProcInfo.cfg.ucb_USE_Fragment_dlgTalk_av)
                {
                    //
                    GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, mVertexBuffer_me);

                    //
                    myTex = this.tex_me;
                    //
                    if (myTex.m_textureId_0 != 0 || myTex.m_textureId_1 != 0)
                    {
                        if (myTex.m_cur_textureId == 0)
                        {
                            if (myTex.m_textureId_0 != 0)
                            {
                                myTex.m_cur_textureId = myTex.m_textureId_0;
                            }
                            else
                            {
                                myTex.m_cur_textureId = myTex.m_textureId_1;
                            }
                        }
                        else
                        {
                            if (myTex.m_cur_textureId == myTex.m_textureId_0
                                && myTex.m_textureId_1 != 0)
                            {
                                myTex.m_cur_textureId = myTex.m_textureId_1;
                                myTex.m_textureId_0 = 0;
                                //
                                if (bDbg)
                                {
                                    qyFuncs.traceLog("myDraw: draw, free tex0");
                                }

                            }
                            else if (myTex.m_cur_textureId == myTex.m_textureId_1
                                && myTex.m_textureId_0 != 0)
                            {
                                myTex.m_cur_textureId = myTex.m_textureId_0;
                                myTex.m_textureId_1 = 0;
                                //
                                if (bDbg)
                                {
                                    qyFuncs.traceLog("myDraw: draw, free tex1");
                                }

                            }
                        }
                        //
                    }
                    //

                    //
                    if (myTex.m_cur_textureId != 0)
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, myTex.m_cur_textureId);
                    }
                    else
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName_default);
                    }
                    //
                    if (myTex.m_cur_textureId != 0)
                    {
                    }



                    //
                    GLES20.GlUniform1i(mTexSmaplerHandle, 0);

                    //
                    GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                        GLES20.GlUnsignedShort, mVertexIndexBuffer);

                }
                else
                {

                    //
                    GLES20.GlVertexAttribPointer(mPositionHandle, 3, GLES20.GlFloat, false, 12, this.tf_me.mVertexBuffer);

                    //
                    GLES20.GlVertexAttribPointer(mTexCoordHandle, 2, GLES20.GlFloat, false, 0, this.tf_me.mTexVertexBuffer);



                    //
                    int t1_texId = 0;

                    //

                    //
                    if (m_var.av.taskInfo.bTaskExists)
                    {
                        if (m_var.av.ucbGot_iIndex_sharedObj_localAv)
                        {
                            QY_SHARED_OBJ pSharedObj = qyFuncs.getSharedObjByIndex(pProcInfo, m_var.av.iIndex_sharedObj_localAv);
                            if (null == pSharedObj) goto errLabel;
                            //
                            t1_texId = pSharedObj.var.m_o_texId;
                        }
                    }

                    //
                    if (t1_texId != 0)
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, t1_texId);
                    }
                    else
                    {
                        GLES20.GlBindTexture(GLES20.GlTexture2d, mTexName_default);
                    }



                    //
                    GLES20.GlUniform1i(mTexSmaplerHandle, 0);

                    //
                    GLES20.GlDrawElements(GLES20.GlTriangles, VERTEX_INDEX.Length,
                        GLES20.GlUnsignedShort, mVertexIndexBuffer);

                }
            }

            //
            this.m_loop_ctrl++;
            int curTickCnt = (int)qyFuncs.myGetTickCount(null);
            int iElapseInMs = Java.Lang.Math.Abs((int)(this.m_lastFps_tickCnt - curTickCnt));
            if (iElapseInMs >= 1000)
            {
                if (bDbg)
                {
                    float fps = (this.m_loop_ctrl - m_lastFps_loop_ctrl) * 1000.0f / (float)iElapseInMs;
                    str = string.Format("myDraw: fps {0}", fps);
                    qyFuncs.traceLog(str);
                }
                //
                this.m_lastFps_loop_ctrl = m_loop_ctrl;
                this.m_lastFps_tickCnt = curTickCnt;
            }


        errLabel:

            return;
        }

        //
        int test_draw()
        {
            //
            //Bitmap bmp = qyFuncs_Droid.texture2Bmp(tex,w,h);
            //GuiShare.pf_dumpBmp(bmp, "hahakk");

            SimpleTexture mSampleBase = null;
            Context context = qyMessengerApp.Droid.MyApplication.Context;

            //
            int w = this.mWindowSurface.Width;
            int h = this.mWindowSurface.Height;

            //
            mSampleBase = new SimpleTexture(context, this.mTexName_default, true, "talkDraw.test_draw");


            //mSampleBase.resize(w, h);

            //
            mSampleBase.test_draw(w, h, 0);


            //
            mSampleBase.destroy();


            return 0;
        }



        //
    }
}