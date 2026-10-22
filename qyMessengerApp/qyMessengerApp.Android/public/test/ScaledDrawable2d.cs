using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Qycx.Gleslibrary;
using Java.Lang;
using Java.Nio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace qyMessengerApp.Droid
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
     * Tweaked version of Drawable2d that rescales the texture coordinates to provide a
     * "zoom" effect.
     */
    public class ScaledDrawable2d : Drawable2d
    {
    private static  string TAG = "ScaledDrawable2d";

    private static  int SIZEOF_FLOAT = 4;

    private FloatBuffer mTweakedTexCoordArray;
    private float mScale = 1.0f;
    private bool mRecalculate;


    /**
     * Trivial constructor.
     */
    public ScaledDrawable2d(Prefab shape):base(shape)
    {
        //super(shape);
        mRecalculate = true;
    }

    /**
     * Set the scale factor.
     */
    public void setScale(float scale)
    {
        if (scale < 0.0f || scale > 1.0f)
        {
            throw new RuntimeException("invalid scale " + scale);
        }
        mScale = scale;
        mRecalculate = true;
    }

    /**
     * Returns the array of texture coordinates.  The first time this is called, we generate
     * a modified version of the array from the parent class.
     * <p>
     * To avoid allocations, this returns internal state.  The caller must not modify it.
     */
    public override FloatBuffer TexCoordArray
    {
            get {

                if (mRecalculate)
                {
                    //Log.v(TAG, "Scaling to " + mScale);
                    FloatBuffer parentBuf = base.TexCoordArray;
                    int count = parentBuf.Capacity();

                    if (mTweakedTexCoordArray == null)
                    {
                        ByteBuffer bb = ByteBuffer.AllocateDirect(count * SIZEOF_FLOAT);
                        bb.Order(ByteOrder.NativeOrder());
                        mTweakedTexCoordArray = bb.AsFloatBuffer();
                    }

                    // Texture coordinates range from 0.0 to 1.0, inclusive.  We do a simple scale
                    // here, but we could get much fancier if we wanted to (say) zoom in and pan
                    // around.
                    FloatBuffer fb = mTweakedTexCoordArray;
                    float scale = mScale;
                    for (int i = 0; i < count; i++)
                    {
                        float fl = parentBuf.Get(i);
                        fl = ((fl - 0.5f) * scale) + 0.5f;
                        fb.Put(i, fl);
                    }

                    mRecalculate = false;
                }

                return mTweakedTexCoordArray;
            }
    }
}

}