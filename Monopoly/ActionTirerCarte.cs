using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    class ActionTirerCarte : AbstractAction
    {
        public ActionTirerCarte() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            string nomCarte;

            nomCarte = (valeur == 1) ? "CC" : "Chance";

            Console.WriteLine(joueur.Nom + " tire une carte " + nomCarte);
        }
    }
}
