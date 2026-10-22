using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGLE
{
    internal class PerformanceTimer
    {
        private long lastTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        private long lastMeasurement = -1;
        private string lastDescription = null;
        private List<string> mutedDescriptions = new List<string>();

        public void Start(string description)
        {
            if (mutedDescriptions.Contains(description))
            {
                lastDescription = null;
                return;
            }
            if (lastDescription != description)
                lastMeasurement = -1;
            lastDescription = description;

            lastTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public void Stop()
        {
            if (lastDescription == null)
                return;

            long measurement = (System.Diagnostics.Stopwatch.GetTimestamp() - lastTimestamp) / 1000000;
            if (lastMeasurement == measurement)
                return;

         
            lastMeasurement = measurement;
            lastTimestamp = System.Diagnostics.Stopwatch.GetTimestamp();
        }

        public void Mute(string description)
        {
            mutedDescriptions.Add(description);
        }
    }
}
