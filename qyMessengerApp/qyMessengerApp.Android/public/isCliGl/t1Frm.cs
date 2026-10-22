using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{

    public class T1_frm
    {
        public uint n_components;

        //
        //DWORD										dwModifiedTickCnt_layout;	//  
        public QY_timestamp ts_layout;                 //  2017/10/17  
                                                       //
        public CAP_RECT capRect;
        public float fAspectRatio_img;         //  
                                               //
        public int imgW_org, imgH_org;     //  2016/02/13
                                           //
        public bool bUseOrtho;                 //  2013/11/12

        //
        public CUSTOMVERTEX_org[] points;//[4];

        //  2016/01/05

        public uint[] indices;//[6];


        //  2016/01/02
#if false
		struct                                         {
					 MY_MATRIX transMat;
		unsigned char ucbValid_transMat;
		//
		int new_capRect_iX, new_capRect_iY;
	}
	trans;
#endif

        //				 
#if __USE_dx11__
				 ID3D11Buffer* g_pVertexBuffer11;           // = NULL;					
	ID3D11Buffer* g_pIndexBuffer11;         // = NULL;
#endif






        //
        public float[] mVERTEX;
        //
        public FloatBuffer mVertexBuffer;

        //
        public ShortBuffer mVertexIndexBuffer;

        //
        public float[] mTEX_VERTEX;
        //
        public FloatBuffer mTexVertexBuffer;


        //  2016/02/14
        public bool bInited;
        public Rect centerSurfaceViewPortRect = new Rect(0, 0, 0, 0);

        /// <summary>
        /// //////////
        /// </summary>
        /// <param name="pCtx"></param>
        /// <param name="pCapRect_screen"></param>
        /// <param name="fAspectRatio_img"></param>
        /// <param name="pFrm"></param>
        /// <returns></returns>
        public float[] VERTEX_t1_r =  {
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


        public float[] VERTEX_t1_l =        {

#if false
			-43.63957f, -37.79223f, -72.17892f,
			-30.19244f, -37.79223f, -76.21281f,
			-28.00447f, -64.29143f, -68.91977f,
			-41.45151f, -64.29143f, -64.88551f,
#endif
			-30.19244f, -37.79223f, -76.21281f,
            -43.63957f, -37.79223f, -72.17892f,
            -41.45151f, -64.29143f, -64.88551f,
            -28.00447f, -64.29143f, -68.91977f,


        };

        //
        //
        public float[] TEX_VERTEX ={
                1,0,
                0,0,
                0,1,
                1,1,
        };

        //
        public unsafe int T1_frm_init(object pCtx, ref CAP_RECT pCapRect_screen, float fAspectRatio_img)
        {
            int iErr = -1;
            //HRESULT hr;

            T1_frm pFrm = this;

            //  2016/02/14
            if (pFrm.bInited)
            {
                qyFuncs.showInfo_open(0, null, ("T1_frm_init11 failed, bInited is true already"));
                return -1;
            }

            //
            int n_components = 1;           //  撓跺醱

            pFrm.n_components = (uint)n_components;

            //		
            // Create vertex buffer
#if false
			SimpleVertex vertices[] =
		{
		{ XMFLOAT3( -1.0f, 1.0f, 1.0f ),    XMFLOAT2( 0.0f, 0.0f ) },
		{ XMFLOAT3(  1.0f, 1.0f, 1.0f ),    XMFLOAT2( 1.0f, 0.0f ) },
		{ XMFLOAT3(  1.0f, -1.0f, 1.0f ),   XMFLOAT2( 1.0f, 1.0f ) },
		{ XMFLOAT3( -1.0f, -1.0f, 1.0f ),   XMFLOAT2( 0.0f, 1.0f ) },
	};


			//
			auto context = DXUTGetD3D11DeviceContext();

			//
			D3D11_BUFFER_DESC bd;
			bd.Usage = D3D11_USAGE_DYNAMIC;
			bd.ByteWidth = sizeof(SimpleVertex) * 4;
			bd.BindFlags = D3D11_BIND_VERTEX_BUFFER;
			bd.CPUAccessFlags = D3D11_CPU_ACCESS_WRITE;
			bd.MiscFlags = 0;
			D3D11_SUBRESOURCE_DATA InitData;
			InitData.pSysMem = vertices;
			hr = g_pd3dDevice->CreateBuffer(&bd, &InitData, &pFrm->g_pVertexBuffer11);
			if (FAILED(hr)) goto errLabel;

			// Set vertex buffer
			UINT stride = sizeof(SimpleVertex);
			UINT offset = 0;
			context->IASetVertexBuffers(0, 1, &pFrm->g_pVertexBuffer11, &stride, &offset);
#endif
            //
            //qyFuncs_Droid.matrix2MY_MATRIX(VERTEX_t11, )
            //
#if true
            pFrm.mVERTEX = VERTEX_t1_r;
#else
			pFrm.mVERTEX = VERTEX_t1_l;
#endif

            //
#if true
            pFrm.mVertexBuffer = ByteBuffer.AllocateDirect(pFrm.mVERTEX.Length * 4)
                .Order(ByteOrder.NativeOrder())
                .AsFloatBuffer()
                .Put(pFrm.mVERTEX);
            pFrm.mVertexBuffer.Position(0);
#endif

            //
            pFrm.mTEX_VERTEX = TEX_VERTEX;

            //
            mTexVertexBuffer = ByteBuffer.AllocateDirect(mTEX_VERTEX.Length * 4)
                .Order(ByteOrder.NativeOrder())
                .AsFloatBuffer()
                .Put(mTEX_VERTEX);
            mTexVertexBuffer.Position(0);



            //
#if DEBUG
            //
#if false
			qyFuncs.showInfo_open(0, null, "For test. set t1.mVertexBuffer to t1_l");
			//
			pFrm.mVERTEX = VERTEX_t1_l;
			pFrm.mVertexBuffer.Put(pFrm.mVERTEX);
			pFrm.mVertexBuffer.Position(0);
#endif
            //
#endif



            //
            pFrm.points = new CUSTOMVERTEX_org[4];

            // Create index buffer
            // Create vertex buffer
            pFrm.indices = new uint[]
            {
        0,1, 2,
        0,2, 3,
    };


#if false
			//
			bd.Usage = D3D11_USAGE_DEFAULT;
			bd.ByteWidth = sizeof(DWORD) * 6;
			bd.BindFlags = D3D11_BIND_INDEX_BUFFER;
			bd.CPUAccessFlags = 0;
			bd.MiscFlags = 0;
			InitData.pSysMem = indices;
			hr = (g_pd3dDevice->CreateBuffer(&bd, &InitData, &pFrm->g_pIndexBuffer11));
			if (FAILED(hr)) goto errLabel;

			// Set index buffer
			context->IASetIndexBuffer(pFrm->g_pIndexBuffer11, DXGI_FORMAT_R32_UINT, 0);

			// Set primitive topology
			context->IASetPrimitiveTopology(D3D11_PRIMITIVE_TOPOLOGY_TRIANGLELIST);
#endif







            //  2014/10/18
            bool ucbAutoClip = true;

            //
            float z_1 = Consts.CONST_fz_z1;// 1;
                                           //
            //QY_timestamp ts_tmp;
            //qyFuncs.mymemset((IntPtr)(byte*)&ts_tmp, 0, sizeof(QY_timestamp));
            //
            //if (0!=T1_frm_set(pCtx, ref ts_tmp, ref pCapRect_screen, fAspectRatio_img, ucbAutoClip, false, ref this.mats, z_1, ("T1_frm_init11"))) goto errLabel;

            //  2016/02/14
            pFrm.bInited = true;

            //
            iErr = 0;
        errLabel:
            if (0 != iErr)
            {
                T1_frm_exit(pCtx);
            }

            return iErr;
        }
        public unsafe void T1_frm_exit(object pCtx)
        {
            T1_frm pFrm = this;

            //
            pFrm.mVertexBuffer = null;
            pFrm.mVERTEX = null;

            //
            pFrm.mTexVertexBuffer = null;
            pFrm.mTEX_VERTEX = null;


            //
            pFrm.bInited = false;


            return;
        }


        public unsafe int T1_frm_set(object pCtx, ref QY_timestamp pTs_layout, ref CAP_RECT pCapRect_screen, float fAspectRatio_img, bool ucbAutoClip, bool bUseOrtho, ref QIS_mats pMats, float z_1, string hint, bool isCenterViewPort)
        {
            int iErr = -1;

            bool bDbg = false;
            string str;

            T1_frm pFrm = this;

            //
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();
            //
            if (null == hint) hint = ("");


            //  2014/11/23
            //if (pProcInfo.cfg.debugStatusInfo.ucbPrint_d3d)
            if (true)
            {
                str = string.Format("T1_frm_set called, fAspectRatio_img %f. %s", fAspectRatio_img, hint);
                qyFuncs.showInfo_open(0, null, str);
            }

            //
            //auto context = DXUTGetD3D11DeviceContext();

            //
            if (null == pFrm.mVertexBuffer)
            {
                //
                str = string.Format("T1_frm_set11 failed, pVertexBuffer10 is null");
                //
#if __DEBUG__
				traceLog(_T("%s"), tBuf);
#endif
                qyFuncs.showInfo_open(0, null, str);
                //
                return -1;
            }
            if (fAspectRatio_img < 0.000001) goto errLabel;

            float ix = pCapRect_screen.iX;
            float iy = pCapRect_screen.iY;
            float iw = pCapRect_screen.iW;
            float ih = pCapRect_screen.iH;


            //
            float fu = 1.0f;
            float fv = 1.0f;
            float fEdge_l = 0.0f;
            float fEdge_t = 0.0f;

            //
            if (ucbAutoClip && !bUseOrtho)
            {
                float tmp_w = ih * fAspectRatio_img;
                if (iw > tmp_w)
                {
                    fv = ih / (iw / fAspectRatio_img);
                }
                else
                {
                    fu = iw / (ih * fAspectRatio_img);
                }
            }
            else
            {
                float tmp_w_org = ih * fAspectRatio_img;
                float tmp_h_org = ih;
                //
                uint iW_i, iH_i;
                int iX_i, iY_i;
                uint iW_dst, iH_dst;
                int iX_dst, iY_dst;
                //				
                if (0 != qyFuncs.new_myGetImageInfo(false, false, true, (int)tmp_w_org, (int)tmp_h_org, (int)ix, (int)iy, (uint)iw, (uint)ih, &iW_i, &iH_i, &iX_i, &iY_i, &iX_dst, &iY_dst, &iW_dst, &iH_dst))
                {
                    goto errLabel;
                }
                //
                ix = iX_dst;
                iy = iY_dst;
                iw = iW_dst;
                ih = iH_dst;
            }
            //
            fEdge_l = (1.0f - fu) / 2.0f;
            fEdge_t = (1.0f - fv) / 2.0f;
            if (fEdge_l < 0.0f) fEdge_l = 0;
            if (fEdge_t < 0.0f) fEdge_t = 0;

            //
            myPOINT realPoint_lt, realPoint_rt, realPoint_lb, realPoint_rb;

            //
            realPoint_lt.x = (int)ix;
            realPoint_lt.y = (int)iy;

            realPoint_rt.x = (int)(ix + iw);
            realPoint_rt.y = (int)iy;

            realPoint_lb.x = (int)ix;
            realPoint_lb.y = (int)(iy + ih);

            realPoint_rb.x = (int)(ix + iw);
            realPoint_rb.y = (int)(iy + ih);

            if (isCenterViewPort)
            {

                centerSurfaceViewPortRect.Left = realPoint_lt.x;
                centerSurfaceViewPortRect.Top = realPoint_lt.y;
                centerSurfaceViewPortRect.Right = realPoint_rb.x;
                centerSurfaceViewPortRect.Bottom = realPoint_rb.y;

                qyFuncs.traceLog("zzstar视窗坐标: " + centerSurfaceViewPortRect.Left + " " + centerSurfaceViewPortRect.Top + " " + centerSurfaceViewPortRect.Right + " " + centerSurfaceViewPortRect.Bottom);


            }
#if true

            System.Numerics.Vector4 pt_far;

            int index = 0;
            qyFuncs.traceLog("zzstar A");
            //  get_pt_far(  pCtx,  realPoint_rt.x,  realPoint_rt.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_rt.x, realPoint_rt.y, ref pMats, z_1, out pt_far);


            pFrm.points[0].x = qyFuncs_Droid.XMVectorGetX(pt_far);
            pFrm.points[0].y = qyFuncs_Droid.XMVectorGetY(pt_far);
            pFrm.points[0].z = qyFuncs_Droid.XMVectorGetZ(pt_far);
            pFrm.points[index].u = 1.0f - fEdge_l;  //  1.0f;
            pFrm.points[index].v = fEdge_t;           //  0.0f;
            qyFuncs.traceLog("zzstar B");
            index++;
            //  get_pt_far(  pCtx,  realPoint_lt.x,  realPoint_lt.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_lt.x, realPoint_lt.y, ref pMats, z_1, out pt_far);

            pFrm.points[1].x = qyFuncs_Droid.XMVectorGetX(pt_far);
            pFrm.points[1].y = qyFuncs_Droid.XMVectorGetY(pt_far);
            pFrm.points[1].z = qyFuncs_Droid.XMVectorGetZ(pt_far);
            qyFuncs.traceLog("zzstar 453");

            pFrm.points[index].u = fEdge_l; //  0.0f;
            pFrm.points[index].v = fEdge_t;   //  0.0f;

            index++;
            //  get_pt_far(  pCtx,  realPoint_lb.x,  realPoint_lb.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_lb.x, realPoint_lb.y, ref pMats, z_1, out pt_far);

            qyFuncs.traceLog("zzstar 462");


            pFrm.points[2].x = qyFuncs_Droid.XMVectorGetX(pt_far);
            pFrm.points[2].y = qyFuncs_Droid.XMVectorGetY(pt_far);
            pFrm.points[2].z = qyFuncs_Droid.XMVectorGetZ(pt_far);

            pFrm.points[index].u = fEdge_l;         //  0.0f;
            pFrm.points[index].v = 1.0f - fEdge_t;  //  1.0f;
            qyFuncs.traceLog("zzstar 471");

            //
            index++;
            //  get_pt_far(  pCtx,  realPoint_rb.x,  realPoint_rb.y,  &pt_far  );
            qyFuncs_Droid.get_pt_far_mats(pCtx, realPoint_rb.x, realPoint_rb.y, ref pMats, z_1, out pt_far);

            pFrm.points[3].x = qyFuncs_Droid.XMVectorGetX(pt_far);
            pFrm.points[3].y = qyFuncs_Droid.XMVectorGetY(pt_far);
            pFrm.points[3].z = qyFuncs_Droid.XMVectorGetZ(pt_far);

            pFrm.points[index].u = 1.0f - fEdge_l;  //  1.0f;
            pFrm.points[index].v = 1.0f - fEdge_t;  //  1.0f;

#endif

            int i;

            //
#if false
			//  SimpleVertex	sv[4];
			SimpleVertex* pSV = NULL;
			//  pSV  =  sv;
			HRESULT hr;


			//  hr  =  pFrm->g_pVertexBuffer11->Map(  D3D11_MAP_WRITE_DISCARD,  NULL,  (  void  **  )&pSV  );
			D3D11_MAPPED_SUBRESOURCE resource;
			hr = context->Map(pFrm->g_pVertexBuffer11, 0, D3D11_MAP_WRITE_DISCARD, 0, &resource);
			if (SUCCEEDED(hr))
			{
				pSV = (SimpleVertex*)resource.pData;
				for (i = 0; i < 4; i++)
				{
					pSV[i].Pos = XMFLOAT3(pFrm->points[i].x, pFrm->points[i].y, pFrm->points[i].z);
					pSV[i].Tex = XMFLOAT2(pFrm->points[i].u, pFrm->points[i].v);
				}
				//  resource.pData  =  pSV;

				//  pFrm->g_pVertexBuffer11->Unmap(  );
				context->Unmap(pFrm->g_pVertexBuffer11, 0);
			}
#endif
            for (i = 0; i < 4; i++)
            {
                pFrm.mVERTEX[i * 3 + 0] = pFrm.points[i].x;
                pFrm.mVERTEX[i * 3 + 1] = pFrm.points[i].y;
                pFrm.mVERTEX[i * 3 + 2] = pFrm.points[i].z;
                //
                pFrm.mTEX_VERTEX[i * 2 + 0] = pFrm.points[i].u;
                pFrm.mTEX_VERTEX[i * 2 + 1] = pFrm.points[i].v;
            }
            qyFuncs.traceLog("zzstar 523");

            pFrm.mVertexBuffer.Put(pFrm.mVERTEX);
            pFrm.mVertexBuffer.Position(0);
            //
            pFrm.mTexVertexBuffer.Put(pFrm.mTEX_VERTEX);
            pFrm.mTexVertexBuffer.Position(0);
            qyFuncs.traceLog("zzstar 530");


            //  2016/01/03
            //pFrm->dwModifiedTickCnt_layout  =  dwModifiedTickCnt_layout;
            pFrm.ts_layout = pTs_layout;
            //
            //memcpy(&pFrm->capRect, pCapRect_screen, sizeof(pFrm->capRect)  );
            pFrm.capRect = pCapRect_screen;
            pFrm.fAspectRatio_img = fAspectRatio_img;
            pFrm.bUseOrtho = bUseOrtho;    //  2013/12/25

            //
            //  2016/01/02. force to setTrans
            //memset(&pFrm->trans, 0, sizeof(pFrm->trans)  );
            qyFuncs.traceLog("zzstar 545");

            //
            iErr = 0;
        errLabel:

            return iErr;
        }



        //
#if false
	int T1_frm_setTrans(D3D_context* pCtx, CAP_RECT* pCapRect_screen, QIS_mats* pMats, float z_1, LPCTSTR hint, T1_frm* pFrm);
	int T1_frm_renderScene(D3D_context* pCtx, TEX_infos_param* pParam, QIS_textureInfo* pTextureInfo, QIS_mats* pMats, PARAM_wm_render* pPwr, T1_frm* pFrm);
#endif

    }

    //

}