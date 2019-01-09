using OpenCvSharp;
using OpenCvSharp.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JHoney_ImageConverter.Util
{
    class Test
    {
        

        public void TestApp()
        {
            //Cv2.GetDevice();
            
            int i = Cv2.GetCudaEnabledDeviceCount();
            OpenCvSharp.Cuda.DeviceInfo atest = new DeviceInfo(0);// = new DeviceInfo();
            
            GpuMat gMat = new GpuMat();
        }
    }
}
