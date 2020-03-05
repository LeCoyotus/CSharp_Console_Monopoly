using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class Carte : ElementNomme
    {
        public string TextAction { get; set; }
        public int Value { get; set; }

        public AbstractAction Strategie;
        public Carte(string nom, string textAction, int value, AbstractAction strategie) : base(nom)
        {
            TextAction = textAction;
            Value = value;
            Strategie = strategie;
        }

        public override string ToString()
        {
            return Nom + " " +  Strategie.ToString();
        }

        public void AppliquerEffet(Joueur j)
        {
            Strategie.Action(j, Value);
        }
    }
}
