using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaussFilter.ProgressNotifier
{
    public class ProgressNotifierEventArgs : EventArgs
    {
        public ProgressNotifierEventArgs(float percentage) : base()
        {
            Percentage = percentage;
        }
        public float Percentage { get; set; }
    }
}
