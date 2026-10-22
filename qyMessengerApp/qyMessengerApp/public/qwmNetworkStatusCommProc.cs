

namespace qy
{
    
partial class qyFuncs
{

public static QY_DMITEM[] CONST_qyStatusTable_en =new QY_DMITEM[22]
{
	new QY_DMITEM(   Consts.CONST_qyStatus_ok,                                        ("OK")                ),
	//	
	new QY_DMITEM(   Consts.CONST_qyStatus_del,                                       "Del"                ),
	new QY_DMITEM(   Consts.CONST_qyStatus_update,                                    "Update"             ),
	//
	new QY_DMITEM(   Consts.CONST_qyStatus_err,                                       "Error"              ),		//  ???-1???Error?????
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_req,                                   "Req"                ),		//  2007/10/07
	new QY_DMITEM(   Consts.CONST_imTaskStatus_resp,                                  "Resp"               ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_applyToSend,                           "applyToSend"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_waitToSend,                            "waitToSend"                         ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_applyToRecv,                           "applyToRecv"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_waitToRecv,                            "waitToRecv"                         ),
    //
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sending,                               "sending"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_receiving,                             "receiving"                          ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sendBreak,                             "sendBreak"                          ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_recvBreak,                             "recvBreak"                          ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_recvFinished,                          "recvFinished"                           ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sendFinished,                          "sendFinished"                           ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_err_missingOrgReq,                     "err_missingOrgReq"                      ),
    //
	new QY_DMITEM(   Consts.CONST_imTaskStatus_canceledBySender,                      "canceledBySender"                       ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_canceledByReceiver,                    "canceledByReceiver"                         ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_deniedByReceiver,                      "deniedByReceiver"                       ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_autoCanceledByReceiver,                "autoCanceledByReceiver"                 ),		//  2014/07/30
	//
	//
	new QY_DMITEM(   -1,                                        ""                             ),
};


		//
		public static QY_DMITEM[] CONST_qyStatusTable_cn = new QY_DMITEM[22]
{
	new QY_DMITEM(   Consts.CONST_qyStatus_ok,                                        ("成功")                ),
	//	
	new QY_DMITEM(   Consts.CONST_qyStatus_del,                                       "删除"                ),
	new QY_DMITEM(   Consts.CONST_qyStatus_update,                                    "升级"             ),
	//
	new QY_DMITEM(   Consts.CONST_qyStatus_err,                                       "错误"              ),		//  ???-1???Error?????
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_req,                                   "请求"                ),		//  2007/10/07
	new QY_DMITEM(   Consts.CONST_imTaskStatus_resp,                                  "响应"               ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_applyToSend,                           "申请发送"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_waitToSend,                            "等待发送"                         ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_applyToRecv,                           "申请接收"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_waitToRecv,                            "等待接收"                         ),
    //
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sending,                               "发送中"                        ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_receiving,                             "接收中"                          ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sendBreak,                             "发送中断"                          ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_recvBreak,                             "接收中断"                          ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_recvFinished,                          "接收完成"                           ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_sendFinished,                          "发送完成"                           ),
	//
	new QY_DMITEM(   Consts.CONST_imTaskStatus_err_missingOrgReq,                     "丢失原始包"                      ),
    //
	new QY_DMITEM(   Consts.CONST_imTaskStatus_canceledBySender,                      "发送取消"                       ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_canceledByReceiver,                    "接收取消"                         ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_deniedByReceiver,                      "拒绝接收"                       ),
	new QY_DMITEM(   Consts.CONST_imTaskStatus_autoCanceledByReceiver,                "自动取消接收"                 ),		//  2014/07/30
	//
	//
	new QY_DMITEM(   -1,                                        ""                             ),
};


	}


}