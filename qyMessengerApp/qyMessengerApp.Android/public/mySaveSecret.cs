
#if DEBUG
#define DBG_release
#endif


//
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileNotFoundException = Java.IO.FileNotFoundException;
using IOException = Java.IO.IOException;



namespace qy.Droid
{
    partial class qyFuncs_Droid
    {
        public static void writeToFile(String message, string cfgFileName)
        {
            int iErr = -1;
            Context context = qyMessengerApp.Droid.MyApplication.Context;


            try
            {
                var outputStream = context.OpenFileOutput(cfgFileName,
                     FileCreationMode.Private);
                if (outputStream == null) goto errLabel;

                OutputStreamWriter outputStreamWriter = new OutputStreamWriter(outputStream);
                //
                outputStreamWriter.Write(message);
                outputStreamWriter.Close();

                //
                iErr = 0;

            }
            catch (FileNotFoundException e)
            {
                e.PrintStackTrace();
            }
            catch (IOException e)
            {
                e.PrintStackTrace();
            }
        errLabel:
            return;
        }

        public static string readFromFile(string cfgFileName)
        {
            String result = "";

            Context context = qyMessengerApp.Droid.MyApplication.Context;


            try
            {
                var inputStream = context.OpenFileInput(name: cfgFileName);
                if (inputStream != null)
                {
                    InputStreamReader inputStreamReader = new InputStreamReader(inputStream);
                    BufferedReader bufferedReader = new BufferedReader(inputStreamReader);
                    String temp = "";
                    StringBuilder stringBuilder = new StringBuilder();

                    while ((temp = bufferedReader.ReadLine()) != null)
                    {
                        stringBuilder.Append(temp);
                        stringBuilder.Append("\n");
                    }

                    inputStream.Close();
                    result = stringBuilder.ToString();
                }
            }
            catch (Exception e)
            {
                goto errLabel;
            }

        //
        errLabel:


            return result;
        }


        public static int saveLoginInfo(string m_server, string m_name, string m_passwd)
        {
            int iErr = -1;
            //string dir = Consts.CONST_subDir_isCli;

            Context context = qyMessengerApp.Droid.MyApplication.Context;
            //string directoryPath = context.FilesDir + File.Separator + dir;
#if false
			File file = new File(directoryPath);
            if (!file.Exists())
            {
				file.Mkdirs();
            }
#endif
            //
            string str_login = "";
            str_login += Consts.CONST_cfgName_server + "=" + m_server + "\n";
            str_login += Consts.CONST_cfgName_usr + "=" + m_name + "\n";
            str_login += Consts.CONST_cfgName_passwd + "=" + m_passwd + "\n";

            //
            writeToFile(str_login, Consts.CONST_cfgFile_login_isCli);


            //
            iErr = 0;
        errLabel:

            return iErr;
        }



        public static string getCfgValByName(string cfgInfo, string cfgName)
        {
            string str = "";
            TMP_tBuf1024 tBuf1024;
            char[] tmp_tBuf = new char[1024];

            int i;
            int nStart = 0;
            int nEnd = 0;
            for (i = 0; i < cfgInfo.Length; i++)
            {
                if (cfgInfo[i] == '\n')
                {
                    nEnd = i;
                    //
                    // nStart->nEnd;
                    int nSep = 0;
                    for (nSep = nStart; nSep < nEnd; nSep++)
                    {
                        if (cfgInfo[nSep] == '=')
                        {
                            if (nSep == nStart) break;
                            if (nSep - nStart + 1 > tmp_tBuf.Length) goto errLabel;
                            cfgInfo.CopyTo(nStart, tmp_tBuf, 0, nSep - nStart);
                            tmp_tBuf[nSep - nStart] = '\0';

                            StringBuilder sb = new StringBuilder();
                            foreach (char c in tmp_tBuf)
                            {
                                if (c == '\0') break;
                                sb.Append(c);
                            }
                            string tmp_str = sb.ToString();
                            //
                            int len = tmp_str.Length;
                            //
                            if (0 != qyFuncs.mytcsicmp(tmp_str, cfgName))
                            {
                                break;
                            }
                            //
                            if (nEnd - nSep > tmp_tBuf.Length) goto errLabel;
                            cfgInfo.CopyTo(nSep + 1, tmp_tBuf, 0, nEnd - nSep - 1);
                            tmp_tBuf[nEnd - nSep - 1] = '\0';
                            //
                            sb.Clear();
                            foreach (char c in tmp_tBuf)
                            {
                                if (c == '\0') break;
                                sb.Append(c);
                            }
                            str = sb.ToString(); goto errLabel;
                        }
                    }

                    //
                    nStart = i + 1;
                    continue;

                }

            }


        errLabel:

            return str;
        }

        public static int readLoginInfo(ref string m_server, ref string m_name, ref string m_passwd)
        {
            int iErr = -1;

            m_server = "";
            m_name = "";
            m_passwd = "";

            string cfgInfo = readFromFile(Consts.CONST_cfgFile_login_isCli);

            m_server = getCfgValByName(cfgInfo, Consts.CONST_cfgName_server);
            m_name = getCfgValByName(cfgInfo, Consts.CONST_cfgName_usr);
            m_passwd = getCfgValByName(cfgInfo, Consts.CONST_cfgName_passwd);

            //
#if DBG_release
            m_server = "222.77.96.31";
            m_server = "bbs.qycx.com";
            m_server = "218.240.128.210";
            //
            //m_server = "10.0.1.110";
            //m_server = "192.168.133.47";

            //
            qyFuncs.traceLog("For test. set m_server to " + m_server);
            //m_name = "user1";

            m_name = "zs1";
            //m_name = "wqh";
            m_passwd = "123";

#endif


            iErr = 0;
        errLabel:
            return iErr;
        }

        //
        public static int saveDbgCfg(string m_statusServer)
        {
            int iErr = -1;
            //
            string str_cfg = "";
            str_cfg += Consts.CONST_cfgName_statusServer + "=" + m_statusServer + "\n";

            //
            writeToFile(str_cfg, Consts.CONST_cfgFile_dbg_isCli);


            //
            iErr = 0;
        errLabel:

            return iErr;
        }



        //
        public static int readDbgCfg(ref string m_statusServer)
        {
            int iErr = -1;

            m_statusServer = "";

            string cfgInfo = readFromFile(Consts.CONST_cfgFile_dbg_isCli);

            m_statusServer = getCfgValByName(cfgInfo, Consts.CONST_cfgName_statusServer);

            //
#if DBG_release
            //
            m_statusServer = "192.168.1.21";
            m_statusServer = "10.0.1.102";
            //m_statusServer = "192.168.133.47";

            //
            qyFuncs.traceLog("For test, set statusServer");


#endif


            iErr = 0;
        errLabel:
            return iErr;

        }

    }
}


