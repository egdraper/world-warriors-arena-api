﻿using System.Collections.Generic;
using WWA.Grains.Entities;

namespace WWA.Grains.Games.Entities
{
    public class Game : TrackedEntity
    {
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string OwnedBy { get; set; }
        public IEnumerable<string> Players { get; set; }
    }
}
