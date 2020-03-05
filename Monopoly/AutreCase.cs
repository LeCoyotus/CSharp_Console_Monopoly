using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class AutreCase : AbstractCase
    {
        public string Effet { get; }

        public AutreCase(string nom, string effet) : base(nom)
        {
            Effet = effet;
        }

        public override string ToString()
        {
            return Nom + " : " + Effet;
        }
    }
}
