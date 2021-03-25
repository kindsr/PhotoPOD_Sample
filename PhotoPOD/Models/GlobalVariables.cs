using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoPOD.Models
{
    public class GlobalVariables
    {
        private static object lockObj = new object();
        private static GlobalVariables singletonObj;

        public static GlobalVariables Instance
        {
            get
            {
                lock (lockObj)
                {
                    if (singletonObj == null)
                    {
                        singletonObj = new GlobalVariables();
                    }
                }
                return singletonObj;
            }
        }

        public string ScannedText { get; set; }

        public void Clear()
        {
            ScannedText = string.Empty;
        }
    }
}
