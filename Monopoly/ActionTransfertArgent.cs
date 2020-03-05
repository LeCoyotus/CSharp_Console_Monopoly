using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class ActionTransfertArgent : AbstractAction
    {
        public ActionTransfertArgent() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            joueur.Solde -= valeur;
        }
    }
}
