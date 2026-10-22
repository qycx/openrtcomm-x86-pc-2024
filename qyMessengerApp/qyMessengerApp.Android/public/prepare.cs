

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Org.Xml.Sax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        //
        public static QisTrans[] g_transes = new QisTrans[3];
        public static DLG_TALK_videoConference[] m_tmp_videoConferences = new DLG_TALK_videoConference[Consts.DEFAULT_usMaxCnt_taskInfos];
        //public static QMC_taskData_conf[] m_tmp_taskData_confs = new QMC_taskData_conf[Consts.DEFAULT_usMaxCnt_taskInfos];

        public static int perpare_init()
        {
            int iErr = -1;

            do
            {
                int i;

                if (g_transes != null)
                {
                    for (i = 0; i < g_transes.Length; i++)
                    {
                        g_transes[i] = new QisTrans();

                        //
                        g_transes[i].usMaxCntLimit_transforms = Consts.MAX_activeMems_from;

                        //
#if DEBUG
                
#endif


                        //                    
                        g_transes[i].pTransforms = new QY_TRANSFORM[g_transes[i].usMaxCntLimit_transforms];
                        qyFuncs.traceLog("prepare_init, initTransforms.643");
                        int j;
                        for (j = 0; j < g_transes[i].usMaxCntLimit_transforms; j++)
                        {
                            bool bNoV = false;

                            switch  (  j)
                            {
                                case 0:
                                case 1:
                                case 2:
                                case 3:
                                    break;
                                default:
                                    bNoV = true;
                                    break;
                            }

                            qyFuncs.traceLog("initTransforms.647  " + j);

                            //
                            g_transes[i].pTransforms[j] = new QY_TRANSFORM(bNoV);
                        }

                        qyFuncs.traceLog("prepare_init, initTransforms.647");

                        //
                        continue;
                    }
                }

                //
                if (true)
                {
                    //
                    if (m_tmp_videoConferences != null)
                    {
                        for (i = 0; i < m_tmp_videoConferences.Length; i++)
                        {
                            m_tmp_videoConferences[i] = new DLG_TALK_videoConference();
                        }
                    }

                }

                //
                /*
                for (i = 0; i < m_tmp_taskData_confs.Length; i++)
                {
                    m_tmp_taskData_confs[i] = new QMC_taskData_conf(i);
                }
                */




                //
            } while (false);


            return iErr;
        }


        public static int  prepare_exit()
        {
            int iErr = -1;

            do
            {
                int i;
                for (i  =0; i< g_transes.Length; i++)
                {
                    g_transes[i] = null;
                }
                g_transes = null;

                //
                for  (  i=0;i<m_tmp_videoConferences.Length;i++)
                {
                    m_tmp_videoConferences[i] = null;

                }
                m_tmp_videoConferences = null;

                //
                /*
                for  (  i  =0;i<m_tmp_taskData_confs.Length;i++)
                {
                    m_tmp_taskData_confs[i] = null;

                }
                m_tmp_taskData_confs = null;
                */


                //
            } while (false);

            return iErr;
        }


        //  从1开始，放弃0
        public static int newIndex_qisTrans()
        {
            int iErr = -1;
            int index = -1;
            do
            {
                int i;
                for  (  i = 1; i < g_transes.Length; i++)
                {
                    if (g_transes[i].bUsed) continue;
                    break;
                }
                if (i == g_transes.Length) break;
                g_transes[i].bUsed = true;
                index = i;

                iErr = 0;
            } while (false);

            if (iErr != 0) return iErr;
            return index;

        }

        public static int freeIndex_qisTrans(int index)
        {
            if  (  index  >=  1  &&  index  < g_transes.Length)
            {
                if (!g_transes[index].bUsed) return 0;
                g_transes[index].bUsed = false;
                return 0;            
            }

            return -1;
        }

        public static QisTrans getQisTransByIndex(int index)
        {
            if (index >= 1 && index < g_transes.Length) return g_transes[index];
            return null;

        }

        //public static int 


    }


}

