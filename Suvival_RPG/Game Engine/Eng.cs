using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Engine {
    public static class Eng {
        public static int pxlsize = 3;
        public static int tilesize = 16;
        public static List<Timer> Timers = new List<Timer>();
        public static void Update(GameTime gt) {
            for(int i = Timers.Count - 1; i >= 0; i--) {
                Timers[i].CheckTime(gt.ElapsedGameTime.Milliseconds / 1000f);
            }
            Input.Update();
        }
    }
}
