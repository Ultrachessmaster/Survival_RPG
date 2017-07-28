using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival {
    interface IWeapon {
        string Name { get; }
        float Attack { get; }
    }

    public class SwordWeapon : IWeapon {
        public float Attack { get { return 5f; } }
        public string Name { get { return "Sword"; } }
    }
}
