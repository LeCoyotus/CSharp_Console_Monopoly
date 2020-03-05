using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class AbstractAction
    {
        protected AbstractAction() { }

        public virtual void Action(Joueur joueur, int valeur = 0) { }

    }
}
