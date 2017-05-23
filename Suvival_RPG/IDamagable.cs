using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suvival_RPG {
    interface IDamagable {
        float Health { get; }
        void Damage(float damage);
    }
}
