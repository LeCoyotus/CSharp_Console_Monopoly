using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class TerrainBuildAble : AbstractTerrain
    {
        public List<AbstractBatiment> ListePropriete;
        public TerrainBuildAble(string nom, int cost) : base(nom, cost)
        {
            ListePropriete = new List<AbstractBatiment>();
        }

        public override int PayerLoyer()
        {
            return Cost / 10 * ListePropriete.Count;
        }

        public override string ToString()
        {
            return Nom + " prix : " + Cost;
        }
    }
}
