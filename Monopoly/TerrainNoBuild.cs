using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class TerrainNoBuild : AbstractTerrain
    {
        public TerrainNoBuild(string nom, int cost) : base(nom, cost) { }

        public override int PayerLoyer()
        {
            return Cost / 20 * 10;
        }

        public override string ToString()
        {
            return Nom + "prix compagnie/gare : " + Cost;
        }
    }
}
