using System;
using System.Collections.Generic;
using System.Text;

namespace ObjectDetection.Common
{
    class NonMaxSuppression
    {
        /* To Do
         * 
         * The Original intention was to execute NMS after Outputs were 
         * returned from ProcessImage(). However, NMS could be optimized 
         * during the conversion (Convert() method) within the 
         * IInferenceProvider implementation. 
         * 
         * For example, rather than iterating through the Outputs twice, 
         * NMS can be executed and returned in the Convert() method. 
         * This would require the NMS be part of the implementation rather
         * than a generic NMS process. 
         * 
         * This will be evaluated soon...
         * 
         */
        
    }
}
