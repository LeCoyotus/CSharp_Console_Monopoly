using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class ActionPrison : AbstractAction
    {
        public ActionPrison() : base() { }

        public override void Action(Joueur joueur, int valeur = 0)
        {
            if (joueur.IsPrisonnier)
            {
                joueur.IsPrisonnier = false;
            } 
            else
            {
                joueur.IsPrisonnier = true;
            }
        }
    }
}
