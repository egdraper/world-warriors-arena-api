using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WWA.GrainInterfaces.Models
{
    public class GameUpdateModel
    {
        public string Name { get; set; }
        public string OwnedBy { get; set; }
        public IEnumerable<string> Players { get; set; }
    }
}
