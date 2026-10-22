

using Android.OS;
using Java.Lang;
using Java.Lang.Reflect;

using qy;

//
namespace qy.Droid
{
    partial class qyFuncs_Droid
    {

        //
        static QY_DMITEM[] manufacturerTable = new QY_DMITEM[5]
{
            new QY_DMITEM(Consts.manufacturer_xiaoMi,"Xiaomi"),
            new QY_DMITEM(Consts.manufacturer_huaWei,"HUAWEI"),
            new QY_DMITEM(Consts.manufacturer_mbk,"MBK"),
            new QY_DMITEM(Consts.manufacturer_unione,"UniOne"),
            new QY_DMITEM(-1,""),


};

        static QY_DMITEM[] modelTable_xiaoMi = new QY_DMITEM[2]
        {
            new QY_DMITEM(Consts.model_xiaoMi_redMi10,"M2004J7AC"),
            new QY_DMITEM(-1,"M2004J7AC"),

        };

        static QY_DMITEM[] modelTable_huaWei = new QY_DMITEM[2]
        {
            new QY_DMITEM(Consts.model_huaWei_TAHAN00m,"TAH-AN00m"),
            new QY_DMITEM(-1,"M2004J7AC"),

        };

        static QY_DMITEM[] modelTable_mbk = new QY_DMITEM[2]
        {
            new QY_DMITEM(Consts.model_mbk_t2101,"MBK-T2101"),
            new QY_DMITEM(-1,"M2004J7AC"),
        };

        static QY_DMITEM[] modelTable_unione = new QY_DMITEM[2]
        {
            new QY_DMITEM(Consts.model_unione_ua100,"UA100"),
            new QY_DMITEM(-1,"M2004J7AC"),
        };




        //
        private static string HARMONY_OS = "harmony";




        //判断是否为鸿蒙
        public static bool isHarmonyOSa()
        {
            try
            {
                Class clz = Class.ForName("com.huawei.system.BuildEx");
                Method method = clz.GetMethod("getOsBrand");

                ClassLoader classLoader = clz.ClassLoader;

                qyFuncs.traceLog("classLoader: " + classLoader);

                //BootClassLoader的parent为null
                if (classLoader != null && classLoader.Parent == null)
                {


                    //return method.invoke(clz).toString();//返回获取的名称
                    string name = method.Invoke(clz).ToString();
                    bool b = HARMONY_OS.Equals(name);


                    //****返回是否是鸿蒙系统 自行注释并将方法返回值改为boolean
                    //return HARMONY_OS.Equals(method.Invoke(clz));
                    return b;
                }
            }
            catch (ClassNotFoundException e)
            {
            }
            catch (NoSuchMethodException e)
            {
            }
            catch (Java.Lang.Exception e)
            {
            }
            return false;

        }
        //
        public static unsafe void getSystemInfo(ref int iManufactor,ref int iModel, ref bool b_isHarmony)
        {
            string versionCode = "";

            string manufacturer = Build.Manufacturer;

            string model = Build.Model;

            //
            iManufactor = 0;
            iModel = 0;
            b_isHarmony = false;

            //
            b_isHarmony = qyFuncs_Droid.isHarmonyOSa();


            //LogUtils.i("Build.MANUFACTURER = " + manufacturer + " ,Build.MODEL = " + Build.MODEL);

            if (null != manufacturer)
            {
                int iVal;
                //
                char[] m1 = manufacturer.ToLower().ToCharArray();
                string ss = "mbk";
                char[] s1 = ss.ToLower().ToCharArray();
                int i;
                bool bErr = false;
                for  (  i  =  0;  i  <  ss.Length ; i++)
                {
                    if (s1[i] != m1[i])
                    {
                        bErr = true;
                        break;
                    }
                    continue;
                }
                if (!bErr)
                {
                    iManufactor = Consts.manufacturer_mbk;
                }
                else
                {
                    //
                    iVal = qyFuncs.qyGetTypeByDes(manufacturerTable, manufacturer);
                    if (iVal < 0) iVal = 0;
                    iManufactor = iVal;
                }
                //
                QY_DMITEM[] pModelTable = null;
                switch (iManufactor)
                {
                    case Consts.manufacturer_xiaoMi:
                        pModelTable = modelTable_xiaoMi;
                        break;
                    case Consts.manufacturer_huaWei:
                        pModelTable = modelTable_huaWei;
                        break;
                    case Consts.manufacturer_mbk:
                        pModelTable = modelTable_mbk;
                        break;
                    case Consts.manufacturer_unione:
                        pModelTable = modelTable_unione;
                        break;
                    default:
                        break;
                } 
                if (pModelTable != null)
                {
                    iVal = qyFuncs.qyGetTypeByDes(pModelTable, model);
                    if (iVal < 0) iVal = 0;
                    //
                    iModel = iVal;
                }

                

                //versionCode = SystemProperty("ro.miui.version.code_time");

            }

            return;

        }


        //
        public static int customSystem()
        {
            CCtxQyMc pQyMc = g.g_pQyMc;
            CCtxQmc pProcInfo = pQyMc.get_pProcInfo();

            switch(pQyMc.iManufacturer)
            {
                case Consts.manufacturer_xiaoMi:
                     switch (pQyMc.iModel)
                    {
                        case Consts.model_xiaoMi_redMi10:
                            pProcInfo.cfg.b_no_KeyLowLatency_a = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.manufacturer_mbk:
                     switch(pQyMc.iModel)
                    {
                        case Consts.model_mbk_t2101:
                            pProcInfo.cfg.b_no_KeyLowLatency_a = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case Consts.manufacturer_unione:
                    switch (pQyMc.iModel)
                    {
                        case Consts.model_unione_ua100:
                            pProcInfo.cfg.b_no_4k = true;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;

            }

            return 0;
        }

    }




}


