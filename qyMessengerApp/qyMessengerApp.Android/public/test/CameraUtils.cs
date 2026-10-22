using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qy.Droid
{

    /*
 * Copyright 2014 Google Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

    /**
     * Camera-related utility functions.
     */
    public class CameraUtils
    {
        private static  string TAG = "cameraUtils";

    /**
     * Attempts to find a preview size that matches the provided width and height (which
     * specify the dimensions of the encoded video).  If it fails to find a match it just
     * uses the default preview size for video.
     * <p>
     * TODO: should do a best-fit match, e.g.
     * https://github.com/commonsguy/cwac-camera/blob/master/camera/src/com/commonsware/cwac/camera/CameraUtils.java
     */
    public static void choosePreviewSize(Camera.Parameters parms, int width, int height)
        {
            // We should make sure that the requested MPEG size is less than the preferred
            // size, and has the same aspect ratio.
            Camera.Size ppsfv = parms.PreferredPreviewSizeForVideo;
            if (ppsfv != null)
            {
                Log.Debug(TAG, "Camera preferred preview size for video is " +
                        ppsfv.Width + "x" + ppsfv.Height);
            }

            //for (Camera.Size size : parms.getSupportedPreviewSizes()) {
            //    Log.d(TAG, "supported: " + size.width + "x" + size.height);
            //}

            foreach (Camera.Size size in parms.SupportedPreviewSizes)
            {
                if (size.Width == width && size.Height == height)
                {
                    parms.SetPreviewSize(width, height);
                    return;
                }
            }

            Log.Warn(TAG, "Unable to set preview size to " + width + "x" + height);
            if (ppsfv != null)
            {
                parms.SetPreviewSize(ppsfv.Width, ppsfv.Height);
            }
            // else use whatever the default size is
        }

        /**
         * Attempts to find a fixed preview frame rate that matches the desired frame rate.
         * <p>
         * It doesn't seem like there's a great deal of flexibility here.
         * <p>
         * TODO: follow the recipe from http://stackoverflow.com/questions/22639336/#22645327
         *
         * @return The expected frame rate, in thousands of frames per second.
         */
        public static int chooseFixedPreviewFps(Camera.Parameters parms, int desiredThousandFps)
        {
            IList<int[]> supported = parms.SupportedPreviewFpsRange;

            foreach (int[] entry in supported)
            {
                //Log.d(TAG, "entry: " + entry[0] + " - " + entry[1]);
                if ((entry[0] == entry[1]) && (entry[0] == desiredThousandFps))
                {
                    parms.SetPreviewFpsRange(entry[0], entry[1]);
                    return entry[0];
                }
            }

            int[] tmp = new int[2];
            parms.GetPreviewFpsRange(tmp);
            int guess;
            if (tmp[0] == tmp[1])
            {
                guess = tmp[0];
            }
            else
            {
                guess = tmp[1] / 2;     // shrug
            }

            Log.Debug(TAG, "Couldn't find match for " + desiredThousandFps + ", using " + guess);
            return guess;
        }
    }



}