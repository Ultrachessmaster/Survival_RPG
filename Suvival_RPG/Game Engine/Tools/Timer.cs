using System;
using Microsoft.Xna.Framework;

namespace Engine
{
    public class Timer
    {
        Action action;
        float time;
        float timepassed;
        Entity e;

        public static void AddTimer(Action act, float time, Entity e)
        {
            Timer t = new Timer();
            t.action = act;
            t.time = time;
            t.e = e;
            Eng.Timers.Add(t);
        }

        public void CheckTime(float timepass)
        {
            timepassed += timepass;
            if (timepassed >= time)
            {
                if(ERegistry.entities.Contains(e))
                    action.Invoke();
                Eng.Timers.Remove(this);
            }
        }

    }
}
