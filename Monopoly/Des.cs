using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class Des
    {
        private Random random;
        public int NbFace { get; }


        public Des(int nbFace)
        {
            NbFace = nbFace;
            random = new Random();
        }

        public int Roll()
        {
            return random.Next(1, NbFace + 1);
        }

        public int Roll(int n)
        {
            return random.Next(1, n);
        }
    }
}
