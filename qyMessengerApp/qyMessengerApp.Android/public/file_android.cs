
using System;
using Android.Content;
using Java.IO;
using Java.Nio.Channels;
using File = Java.IO.File;
namespace qy.Droid
{
    public class Tmp_file
    {
        public bool bW;

        public File file;
        public RandomAccessFile raf;
        //public System.IO.Stream    outputStream;
        //public OutputStreamWriter  outputStreamWriter;

        public byte[] buf;


    };

    //
    partial class qyFuncs_Droid
    {


        public static object _tfopen_android(string filename, string mode)
        {
            int iErr = 0;
            Tmp_file tf = new Tmp_file();
            tf.bW = true;

            Context context = qyMessengerApp.Droid.MyApplication.Context;
            string real_mode = "rws";

            try
            {
                tf.file = new File(filename);
                //
                if ( mode.Equals("rb"))
                {
                    real_mode = "r";
                }

                //
                //tf.raf = new RandomAccessFile(tf.file, "rws");
                tf.raf = new RandomAccessFile(tf.file, real_mode);
                tf.buf = new byte[1024];
            }
            catch(Exception e)
            {
                goto errLabel;
            }

            iErr = 0; 

            errLabel:
            if (0!=iErr)
            {
                fclose_android(tf);

            }
            return iErr != 0 ?null:tf;
        }


        public static int fclose_android(object stream)
        {
            if (stream == null) return 0;
            Tmp_file tf = (Tmp_file)stream;
            try
            {
                if (tf.raf != null)
                {
                    tf.raf.Close();
                    tf.raf = null;
                }
                if (tf.file != null)
                {
                    tf.file = null;
                }
                tf.buf = null;
            }
            catch(Exception e)
            {

            }
        errLabel:
            return 0;
        }

        //
        public static int fseeki64_android(object stream, Int64 offset, int origin)
        {
            int iErr = -1;
            Tmp_file tf = (Tmp_file)stream;
            if (tf == null) return -1;

            try
            {
                tf.raf.Seek(offset);
            }
            catch(Exception e)
            {
                goto errLabel;
            }
            iErr = 0;
            errLabel:

            return iErr;
        }


        public static unsafe Int64 fread_android(void* buffer, int size, int count, object stream)
        {
            int iErr = -1;
            Tmp_file tf = (Tmp_file)stream;
            if (tf == null) return -1;
            int nRead = 0;
            int nCnt_read = 0;

            //
            if (count != 1) return -1;

            //
            try
            {
                Int64 nTotal = size * count;
                if (nTotal == 0) goto errLabel;
                if (tf.buf.Length < nTotal)
                {
                    tf.buf = new byte[nTotal];
                }
                nRead=tf.raf.Read(tf.buf, 0, (int)nTotal);
                if ( nRead>0)
                {
                    System.Runtime.InteropServices.Marshal.Copy(tf.buf, 0, (IntPtr)buffer, nRead);
                }
                nCnt_read = nRead / size;

            }
            catch(Exception e)
            {
                goto errLabel;
            }
            iErr = 0;
        errLabel:
            return iErr != 0 ? iErr : nCnt_read;


        }


            public static unsafe Int64 fwrite_android(void* buffer, Int64 size, Int64 count, object stream)
        {
            int iErr = -1;
            Tmp_file tf = (Tmp_file)stream;
            if (tf == null) return -1;
               
            try
            {
                Int64 nTotal = size * count;
                if (nTotal == 0) goto errLabel;
                if (tf.buf.Length < nTotal)
                {
                    tf.buf = new byte[nTotal];
                }
                System.Runtime.InteropServices.Marshal.Copy((IntPtr)buffer, tf.buf, 0, (int)nTotal);

                tf.raf.Write(tf.buf,0,(int)nTotal);

            }
            catch(Exception e)
            {
                goto errLabel;

            }
            iErr = 0;
        errLabel:
            return iErr != 0 ? iErr : count;

        }

        private static void copyFileUsingFileChannels(File source, File dest)
        {
            FileChannel inputChannel = null;
            FileChannel outputChannel = null;


		try {
                inputChannel = new FileInputStream(source).Channel;
                outputChannel = new FileOutputStream(dest).Channel;
                outputChannel.TransferFrom(inputChannel, 0, inputChannel.Size());
            }
            catch(Exception e)
            {

            }
            finally {
                inputChannel.Close();
                outputChannel.Close();
            }

            return;
        }

        //
        public static bool CopyFile_android(string lpExistingFileName, string lpNewFileName, bool bFailIfExists)
        {
            bool bRet = false;

            try
            {
                File file_in = new File(lpExistingFileName);
                File file_out = new File(lpNewFileName);

                //
                copyFileUsingFileChannels(file_in, file_out);
                

            }
            catch(Exception e)
            {
                goto errLabel;

            }
            
            bRet = true;
            errLabel:

            return bRet;
        }


        public static bool DeleteFile_android(string lpFileName)
        {
            bool bRet = false;
            try
            {
                File file = new File(lpFileName);
                file.Delete();
            }
            catch(Exception e)
            {
                goto errLabel;

            }
            bRet = true;
        errLabel:
            return bRet;

        }


    }




}