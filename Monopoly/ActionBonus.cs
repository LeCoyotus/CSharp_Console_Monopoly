using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class ActionBonus : AbstractAction
    {
        public ActionBonus() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            joueur.Solde += 0;
        }
    }
}
