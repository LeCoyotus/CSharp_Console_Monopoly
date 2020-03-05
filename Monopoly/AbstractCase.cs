using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public abstract class AbstractCase : ElementNomme
    {
        protected AbstractCase(string nom) : base(nom) { }
    }
}
