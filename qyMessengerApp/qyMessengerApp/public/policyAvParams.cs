using System;
using System.Collections.Generic;
using System.Text;

//
//
partial class Consts
{
	//
	//
	//
#if true
	public const int DEFAULT_conf_fourcc_confServ = CONST_fourcc_HEVC;  //h264
	public const int DEFAULT_conf_fourcc_others = CONST_fourcc_HEVC;    //h264
#else
	public const int DEFAULT_conf_fourcc_confServ = CONST_fourcc_h264;
	public const int DEFAULT_conf_fourcc_others = CONST_fourcc_h264;
#endif

}



//
namespace qy
{
	public class PolicyAvParams
	{

		//  2013/06/09
		AUDIO_PLAY_CFG CONST_audioPlayCfg_1s, CONST_audioPlayCfg_07s, CONST_audioPlayCfg_05s;

		//  ld
		AUDIO_PLAY_CFG audioPlayCfg_240p;               //  2014/04/23

		/*
		//  2011/08/01
		myVIDEO_init_cfg LD, LD_av;

		//  screen
		int SD_videoCompressors_screen;
		char* SD_videoFourccStr_screen;
		int SD_usMaxFps_screen;
		//
		int SD_videoCompressors_mediaFile;
		char* SD_videoFourccStr_mediaFile;
		//
		int SD_videoCompressors_mediaDevice;
		char* SD_videoFourccStr_mediaDevice;

		//  sd1
		AUDIO_PLAY_CFG audioPlayCfg_480p;               //  2014/04/23
		myVIDEO_init_cfg SD1, SD1_av;                   //  480p;


		//  sd3,  2012/03/16
		AUDIO_PLAY_CFG audioPlayCfg_576p;               //  2014/04/23
		myVIDEO_init_cfg SD3, SD3_av;

		//  hd2,  2012/01/28
		AUDIO_PLAY_CFG audioPlayCfg_720p;               //  2014/04/23
		myVIDEO_init_cfg HD, HD_av;                     //  2014/10/16
		myVIDEO_init_cfg HD_webcam;                     //  2015/02/09


		//  fullHD
		AUDIO_PLAY_CFG audioPlayCfg_1080p;              //  2014/04/23
		myVIDEO_init_cfg fullHD, fullHD_av;             //  
		myVIDEO_init_cfg fullHD_webcam;                 //  2015/02/09

		//
		int DEV2FPS_videoCompressors_screen;
		char* DEV2FPS_videoFourccStr_screen;
		//
		int DEV2FPS_videoCompressors_mediaFile;
		char* DEV2FPS_videoFourccStr_mediaFile;
		int DEV2FPS_videoCompressors_mediaDevice;
		char* DEV2FPS_videoFourccStr_mediaDevice;
		//  int  DEV2FPS_usMaxFps_mediaDevice;
		int DEV2FPS_usMaxFps;

		//
		int DEV5FPS_audioCompressors_mediaDevice;

		int DEV5FPS_videoCompressors_screen;
		char* DEV5FPS_videoFourccStr_screen;
		//  int  DEV5FPS_usMaxFps_screen;
		int DEV5FPS_usMaxFps;
		//  2012/10/29		

		myVIDEO_init_cfg SD_mosaic_video;                       //  2014/11/15
		int DEV5FPS_videoCompressors_mosaic_resource;
		char* DEV5FPS_videoFourccStr_mosaic_resource;
		int DEV5FPS_usMaxFps_mosaic_resource;

		//
		int DEV5FPS_350kb;
		int DEV5FPS_500kb;
		int DEV5FPS_700kb;

		//  2016/04/06
		myVIDEO_init_cfg LD_slave_video;
		myVIDEO_init_cfg SD_slave_video;


		//  webcam, 2012/06/12
		AUDIO_PLAY_CFG DEFAULT_audioPlayCfg_webcam;
		//
		//
		QY_DMITEM* pAudioCompressorsTable;                          //  2012/01/19
		QY_DMITEM* pVideoCompressorsTable;
		int iResId_policyAvLevelTable;
		int iResId_policyAvLevelTable_mosaic_video;                 //  2014/11/15
		int iResId_policyAvLevelTable_mosaic_resource;                  //  2014/11/15
																		//
		int iResId_policyAvLevelTable_slave_video;                      //  2016/04/06
																		//
		int iResId_policyAvLevelTable_screen;                           //  2012/02/16
		int iResId_policyAvLevelTable_mediaFile;
		int iResId_policyAvLevelTable_mediaDevice;
		int iResId_policyAvLevelTable_webcam;
		int iResId_policyAvLevelTable_unresizable;                      //  2015/03/11

		*/

	};

}
