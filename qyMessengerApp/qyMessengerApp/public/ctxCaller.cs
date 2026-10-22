

partial class Consts
{
	//
	public const int CONST_ctxCallerType_null = 0;
	public const int CONST_ctxCallerType_thread = 1;
	public const int CONST_ctxCallerType_task = 2;

	//
	public const int cntof_ctxCaller_tNameBuf = 16;
	public const int cntof_ctxCaller_tHintBuf = 24;


	//
}


//
namespace qy
{
	public unsafe struct TMP_ctxCaller_tNameBuf
	{
		public fixed char tNameBuf[Consts.cntof_ctxCaller_tNameBuf];
	}

	public unsafe struct  TMP_ctxCaller_tHintBuf
    {
		public fixed char tHintBuf[Consts.cntof_ctxCaller_tHintBuf];
    }




	//
	public class CTX_caller
	{
		//
		public int							type;

		// thread
		public uint							dwThreadId;
		public bool							bTmp;

		//
		public TMP_ctxCaller_tNameBuf		tNameBuf;
		public TMP_ctxCaller_tHintBuf		tHintBuf;


	};





}