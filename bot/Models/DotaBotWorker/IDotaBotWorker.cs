using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bot
{
    public interface IDotaBotWorker
    {
        void Start();
        void Stop();
        void FrameCaptured(Bitmap frame);
        
    }
}
