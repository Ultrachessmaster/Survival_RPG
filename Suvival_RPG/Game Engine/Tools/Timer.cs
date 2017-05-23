using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class Timer
    {
        Action action;
        float time;
        float timepassed;
        RefWrapper<bool> enabled;

        public static void AddTimer(Action act, float time, RefWrapper<bool> enabled = null)
        {
            Timer t = new Timer();
            t.action = act;
            t.time = time;
            t.enabled = enabled;
            Eng.Timers.Add(t);
        }

        public void CheckTime(float timepass)
        {
            timepassed += timepass;
            if (timepassed >= time)
            {
                if(enabled == null || enabled.Value)
                    action.Invoke();
                Eng.Timers.Remove(this);
            }
        }

    }
}
