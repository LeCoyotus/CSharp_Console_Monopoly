using System;
using System.Collections.Generic;
using System.IO;

namespace Monopoly
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine jeu = new Engine();

            jeu.Init(4);

            Console.WriteLine("Nombre de case : " + jeu.Plateau.Count);
            Console.WriteLine("Nombre de Participant : " + jeu.Participants.Count);
            Console.WriteLine("---------------------");


            Console.WriteLine("Nombre de carte Chance : " + jeu.PileChance.Count);
            Console.WriteLine("Nombre de carte de Communaute : " + jeu.PileCommunaute.Count);
            Console.WriteLine("---------------------");

            do
            {
                jeu.Tour();

                Console.WriteLine("---------------------");
                foreach (Joueur j in jeu.Participants)
                {
                    Console.WriteLine(j);
                }
                Console.WriteLine("---------------------");

            } while (!jeu.Fini());

            Console.WriteLine(jeu.Participants[0].Nom + " a gagné !");

            int numero = 0;
            foreach (AbstractCase elt in jeu.Plateau)
            {
                Console.WriteLine("Case n° " + numero + " : " + elt);
                numero++;
            }
        }
    }
}
