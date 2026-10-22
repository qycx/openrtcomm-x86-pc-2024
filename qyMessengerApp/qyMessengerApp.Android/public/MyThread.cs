using Android;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Thread = Java.Lang.Thread;
using Java.Lang;


namespace qy.Droid
{
	public class MyThread:Thread
{
        object m_p0;
        PF_threadProc m_pf;
        object m_param;

        public MyThread(object p0,PF_threadProc pf, object param)
        {
            m_p0 = p0;
            m_pf = pf;
            m_param = param;            
        }



        public override void Run()
        {
            //base.Run();
            if (m_pf != null)
            {
                m_pf(m_param);
            }

            return;
        }
    }
}