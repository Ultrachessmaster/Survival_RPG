using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survival {
    interface IDamagable {
        float Health { get; }
        void Damage(float damage);
    }
}
