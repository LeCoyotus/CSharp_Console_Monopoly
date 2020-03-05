using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    class ActionMouvement : AbstractAction
    {
        public ActionMouvement() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            joueur.Position += valeur;
        }
    }
}
