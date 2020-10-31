using System;

namespace LaplaceFilter.ProgressNotifier
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
