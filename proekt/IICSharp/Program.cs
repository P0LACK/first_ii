using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using sn = SN_API;
using System.Web.UI.WebControls;

namespace IICSharp
{
    class Program
    {
        static bool loadImage(string imgPath, uint classCnt, List<List<string>> imgName, List<int> imgCntDir)
        {
            for (int i = 0; i< classCnt; ++i)
            {
                string dir = imgPath + i.ToString() + "/";
                if (!Directory.Exists(dir)) continue;

                imgName.Add(new List<string>());
                string[] files = Directory.GetFiles(dir);
                foreach (string s in files)
                {
                    imgName[i].Add(s);
                }
                imgCntDir.Add(files.Count());
            }
            bool ok = imgCntDir.Count == classCnt;
            foreach (int cnt in imgCntDir)
                if (cnt == 0) ok = false;

            return ok;
        }

        static void Main(string[] args)
        {
            sn.Net snet = new sn.Net();

            string ver = snet.versionLib();
            Console.WriteLine("Version snlib" + ver);

            snet.addNode("Input", new sn.Input (), "C1")
                .addNode("C1", new sn.Convolution(15, 0), "C2")
                .addNode("C2", new sn.Convolution(15, 0), "P1")
                .addNode("P1", new sn.Pooling(), "FC1")
                .addNode("FC1", new sn.FullyConnected(128), "FC2")
                .addNode("FC2", new sn.FullyConnected(10), "LS")
                .addNode("LS", new sn.LossFunction(sn.lossType.type.softMaxToCrossEntropy), "Output");
            string imgPath = "c://cpp//sunnet//example//mnist//images//";

            uint batchSz = 100, classCnt = 10, w = 28, h = 28; float lr = 0,001F;
            List<List<string>> imgName = new List<List<string>>();
            List<int> imgCntDir = new List<int>(10);
            Dictionary<string, Bitmap> image = new Dictionary<string, Bitmap>();

            if (!loadImage(imgPath,classCnt,imgName, imgCntDir))
            {
                Console.WriteLine("Error", `loadImage` path: " + imgPach");
                Console.ReadKey();
                return;
            }
            string wpath = "c:/cpp/w.dat";
            if (snet.loadAllWeightFromFile(wpath))
                Console.WriteLine("Load weight ok path: " + wpath);
            else
                Console.WriteLine("Load weight err path: " + wpath);


            sn.Tensor inLayer = new sn.Tensor(new sn.snLSize(w, h, 1, batchSz));
            sn.Tensor targetLayer = new sn.Tensor(new sn.snLSize(classCnt, 1, 1, batchSz));
            sn.Tensor outLayer = new sn.Tensor(new sn.snLSize(classCnt, 1, 1, batchSz));

        }
    }
}
