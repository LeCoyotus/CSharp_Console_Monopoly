using System;
using System.Collections.Generic;
using System.Text;

namespace Monopoly
{
    public class Joueur : ElementNomme
    {
        public int Solde { get; set; }
        public int Position { get; set; }

        public bool IsPrisonnier { get; set; }

        public List<AbstractTerrain> Portefeuille;
        public Joueur(string nom, int solde = 200000, int position = 0, bool isPrisonnier = false) : base(nom)
        {
            Solde = solde;
            Position = position;
            IsPrisonnier = isPrisonnier;
            Portefeuille = new List<AbstractTerrain>();
        }

        public override string ToString()
        {
            return "Nom : " + Nom + " | Solde : " + Solde + " | Position : " + Position;
        }
    }
}
