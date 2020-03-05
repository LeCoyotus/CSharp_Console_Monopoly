using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public abstract class AbstractTerrain : AbstractCase
    {
        public int Cost { get; }

        public Joueur Proprio { get; set; }

        protected AbstractTerrain(string nom, int cost) : base(nom)
        {
            Cost = cost;
            Proprio = null;
        }

        public abstract int PayerLoyer();
        
    }
}
