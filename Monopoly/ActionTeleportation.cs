using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    class ActionTeleportation : AbstractAction
    {
        public ActionTeleportation() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            joueur.Position = valeur;
        }
    }
}
