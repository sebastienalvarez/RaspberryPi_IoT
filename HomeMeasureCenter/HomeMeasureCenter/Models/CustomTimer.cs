using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace HomeMeasureCenter.Models
{
    public class CustomTimer
    {
        // PROPRIETES
        private DispatcherTimer timer = new DispatcherTimer();
        private int lastMinute = DateTime.Now.Minute;

        // EVENEMENTS
        public delegate void CustomTimerTick(object sender, DateTime instant);
        public event CustomTimerTick OnSecond;
        public event CustomTimerTick OnMinute;

        // CONSTRUCTEUR
        public CustomTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // METHODES
        private void Timer_Tick(object sender, object e)
        {
            DateTime now = DateTime.Now;
            OnSecond?.Invoke(this, now);
            if(lastMinute != now.Minute)
            {
                lastMinute = now.Minute;
                OnMinute?.Invoke(this, now);
            }
        }

    }
}
