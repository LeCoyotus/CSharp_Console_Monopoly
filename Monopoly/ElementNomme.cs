using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public abstract class ElementNomme
    {
        public string Nom { get; } 

        protected ElementNomme(string nom)
        {
            Nom = nom;
        }
    }
}
