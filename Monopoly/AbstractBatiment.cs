using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public abstract class AbstractBatiment
    {
        public int Prix { get; }

        protected AbstractBatiment(int prix)
        {
            Prix = prix;
        }
    }
}
