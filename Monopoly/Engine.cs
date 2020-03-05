using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monopoly
{
    public class Engine
    {
        static string[] ListingNom = { "Francis", "Gerrard", "Denis", "Bernard", "Patrick", "Didier", "Jean", "Gilbert", "Grace" , "Louis", "Jon", "Franck", "Patrice" , "Patricia"};

        static ActionTransfertArgent STransfertArgent = new ActionTransfertArgent();
        static ActionMouvement SMouvement = new ActionMouvement();
        static ActionTeleportation STeleportation = new ActionTeleportation();
        static ActionBonus SBonus = new ActionBonus();
        static ActionPrison SPrison = new ActionPrison();
        static ActionTirerCarte SPioche = new ActionTirerCarte();

        public Des PremierDe;
        public Des SecondDe;

        public List<AbstractCase> Plateau;

        public List<Carte> PileChance;
        public List<Carte> PileCommunaute;

        public List<Joueur> Participants;    

        public Engine()
        {
            PremierDe = new Des(6);
            SecondDe = new Des(6);
            Participants = new List<Joueur>();
        }

        public void AddJoueur(string nom)
        {
            if (Participants.Count < 4) Participants.Add(new Joueur(nom));
        }
        public void Init(int nbJoueurs)
        {
            while(Participants.Count < nbJoueurs)
            {
                AddJoueur(ListingNom[PremierDe.Roll(14)]);
            }
            Plateau = CreatePlateau();
            PileChance = CreatePackCarteChance();
            PileCommunaute = CreatePackCarteCommu();
        }

        public void Tour()
        {
            if (Participants.Count > 0 && PremierDe != null && SecondDe != null)
            {

                for (int i = 0; i < Participants.Count; i++)
                {
                    Joueur joueur = Participants[i];

                    if (!(joueur.IsPrisonnier) && joueur.Solde > 0)
                    {
                        TourJoueur(joueur);
                    }
                    else if (joueur.IsPrisonnier)
                    {
                        if(Evasion())
                        {
                            SPrison.Action(joueur);
                            TourJoueur(joueur);
                        }
                    }
                    else
                    {
                        FinDePartie(joueur);
                    }
                }
            }
        }

        public bool Fini()
        {
            if(Participants.Count > 1)
            {
               return false;
            }
            else
            {
                return true;                
            }
        }

        public void FinDePartie(Joueur joueur)
        {
            foreach(AbstractTerrain terrain in joueur.Portefeuille)
            {
                terrain.Proprio = null;
            }

            joueur.Portefeuille.Clear();

            Participants.Remove(joueur);
            Console.WriteLine(joueur.Nom + " est dans la dèche et a donc perdu la partie. \r\n\t");
            Console.WriteLine("Participants restant : " + Participants.Count + "\r\n");
        }

        public void TourJoueur(Joueur joueur)
        {
            bool bonCoup;
            int nbBonCoup = 0;

            
            int avancePosition;
            int premierLance;
            int secondLance;

            do
            {
                avancePosition = 0;
                bonCoup = false;
                premierLance = PremierDe.Roll();
                secondLance = SecondDe.Roll();

                avancePosition += premierLance + secondLance;

                if (premierLance == secondLance)
                {
                    bonCoup = true;
                    nbBonCoup++;
                    if (nbBonCoup == 3) SPrison.Action(joueur);
                }

                if (!joueur.IsPrisonnier)
                {
                    joueur.Position += avancePosition;

                    if(joueur.Position >= 40)
                    {
                        joueur.Position %= 40;
                        joueur.Solde += 2000;
                    }

                    Object o = Plateau[joueur.Position];

                    switch (o)
                    {
                        case null:
                            break;
                        case AutreCase A:
                            PhaseAutre(A, joueur);
                            break;
                        case TerrainBuildAble T:
                            PhaseTerrainB(joueur, T);
                            break;
                        case TerrainNoBuild N:
                            PhaseTerrainN(joueur, N);
                            break;
                    }
                }

            } while (bonCoup && !joueur.IsPrisonnier);


        }
        public bool Evasion()
        {
            int nbEssaiRestant = 3;
            int resultPremierDe;
            int resultSecondDe;

            while(nbEssaiRestant > 0)
            {
                resultPremierDe = PremierDe.Roll();
                resultSecondDe = SecondDe.Roll();

                if(resultPremierDe == resultSecondDe)
                {
                    return true;
                }
            }

            return false;

        }

        public int PayerLoyer(AbstractTerrain T)
        {
            return T.PayerLoyer();
        }

        public bool IsPossede(AbstractTerrain T)
        {
            if (T.Proprio != null)
            {
                return true;
            }
            return false;
        }

        public void Achat(Joueur j, AbstractTerrain T)
        {
            if(T.Proprio == null && j.Solde >= T.Cost + 1)
            {
                j.Solde -= T.Cost;
                T.Proprio = j;
                j.Portefeuille.Add(T);

                Console.WriteLine(j.Nom + " achète " + T.Nom + " pour " + T.Cost);
            }
        }

        public void PhaseTerrainB(Joueur j, TerrainBuildAble T)
        {
            if (IsPossede(T) && T.Proprio != j)
            {
                int loyer = PayerLoyer(T);
                STransfertArgent.Action(j, -loyer);
                STransfertArgent.Action(T.Proprio, loyer);

                Console.WriteLine(j.Nom + " paye un loyer de " + PayerLoyer(T) + " à " + T.Proprio.Nom);
            }
            else if(!IsPossede(T))
            {
                Achat(j, T);
            }
            else
            {
                ACHETETESPUTAINSDEMAISONS(j, T);
            }
        }

        public void PhaseTerrainN(Joueur j, TerrainNoBuild T)
        {
            if (IsPossede(T) && T.Proprio != j)
            {
                int loyer = PayerLoyer(T);
                STransfertArgent.Action(j, -loyer);
                STransfertArgent.Action(T.Proprio, loyer);
                Console.WriteLine(j.Nom + " paye un loyer de " + PayerLoyer(T) + " à " + T.Proprio.Nom);
            }
            else
            {
                Achat(j, T);
            }
        }

        public void ACHETETESPUTAINSDEMAISONS(Joueur j, TerrainBuildAble T)
        {
            int prixMaison = 2000;
            int prixHotel = 20000;
            if (T.ListePropriete.Count < 4 )
            {
                T.ListePropriete.Add(new Maison(prixMaison));
                STransfertArgent.Action(j, -prixMaison);
            }
            else
            {
                T.ListePropriete.Clear();
                T.ListePropriete.Add(new Hotel(prixHotel));
                STransfertArgent.Action(j, -prixHotel);
            }
        }
        public void PhaseAutre(AutreCase c, Joueur j)
        {
            AbstractAction strategieAFaire;
            Carte cartePioche;

            int valeur;
            int aleatoire = PremierDe.Roll(14) - 1;

            if(c.Effet.Length > 0)
            {
                switch (c.Effet)
                {
                    case "carte,CC":
                        SPioche.Action(j, 1);
                        cartePioche = PileCommunaute[aleatoire];
                        strategieAFaire = cartePioche.Strategie;
                        strategieAFaire.Action(j, cartePioche.Value);
                        break;
                    case "carte,chance":
                        SPioche.Action(j);
                        cartePioche = PileChance[aleatoire];
                        strategieAFaire = cartePioche.Strategie;
                        strategieAFaire.Action(j, cartePioche.Value);
                        break;
                    case "prison":
                        SPrison.Action(j);
                        break;
                    default:
                        valeur = (c.Nom == "Taxe de luxe") ? 10000 : 20000;
                        STransfertArgent.Action(j, valeur);
                        break;
                }
            }
        }

        public List<AbstractCase> CreatePlateau()
        {
            List<AbstractCase> Plateau = new List<AbstractCase>();

            using var reader = new StreamReader(@"Plateau.csv");
            List<string> NomsCase = new List<string>();
            List<string> TypeCase = new List<string>();
            List<string> IndexTerrain = new List<string>();
            List<string> CoutTerrain = new List<string>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(';');

                NomsCase.Add(values[1]);
                TypeCase.Add(values[2]);
                IndexTerrain.Add(values[3]);
                CoutTerrain.Add(values[5]);
            }

            reader.Close();

            for (int i = 0; i <= 39; i++)
            {
                string choix = IndexTerrain[i];

                switch (choix)
                {
                    case "terrain":
                        Plateau.Add(new TerrainBuildAble(NomsCase[i], Int32.Parse(CoutTerrain[i])));
                        break;
                    case "compagnie":
                        Plateau.Add(new TerrainNoBuild(NomsCase[i], Int32.Parse(CoutTerrain[i])));
                        break;
                    case "monopole":
                        Plateau.Add(new TerrainNoBuild(NomsCase[i], Int32.Parse(CoutTerrain[i])));
                        break;
                    case "":
                        Plateau.Add(new AutreCase(NomsCase[i], TypeCase[i]));
                        break;
                    default:
                        break;
                }
            }
            return Plateau;
        }
        public List<Carte> CreatePackCarteChance()
        {
            List<Carte> PileChance = new List<Carte>();

            using var reader = new StreamReader(@"Monopoly.csv");

            List<string> TypeCase = new List<string>();
            List<string> TextAction = new List<string>();
            List<string> Evenement = new List<string>();
            List<int> Valeurs = new List<int>();

            while (!reader.EndOfStream)
            {

                var line = reader.ReadLine();
                var values = line.Split(';');


                TypeCase.Add(values[1]);
                TextAction.Add(values[2]);
                Evenement.Add(values[3]);

                try
                {
                    Valeurs.Add(Int32.Parse(values[4]));
                }
                catch
                {
                    char[] charAEnlever = { ',', ':' };
                    string[] n = values[4].Split(charAEnlever);
                    if (n.Length > 1)
                    {
                        Valeurs.Add(Int32.Parse(n[1]));
                    }
                    else
                    {
                        Valeurs.Add(-71);
                    }
                }
            }

            reader.Close();

            TypeCase.RemoveAt(0);
            TextAction.RemoveAt(0);
            Evenement.RemoveAt(0);
            Valeurs.RemoveAt(0);

            for(int i = 0; i <= 14; i++)
            {
                string choix = Evenement[i];

                switch (choix)
                {
                    case "dépense":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], STransfertArgent));
                        break;
                    case "recette":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], STransfertArgent));
                        break;
                    case "aller":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], STeleportation));
                        break;
                    case "frais immo":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], STransfertArgent));
                        break;
                    case "bonus":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], SBonus));
                        break;
                    case "prison":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], SPrison));
                        break;
                    case "déplacement relatif":
                        PileChance.Add(new Carte("Chance", TextAction[i], Valeurs[i], SMouvement));
                        break;
                    default:
                        break;
                }
            }

            return PileChance;
           
        }

        public List<Carte> CreatePackCarteCommu()
        {
            List<Carte> PileCommunaute = new List<Carte>();
            using var reader = new StreamReader(@"Monopoly.csv");

            List<string> TypeCase = new List<string>();
            List<string> TextAction = new List<string>();
            List<string> Evenement = new List<string>();
            List<int> Valeurs = new List<int>();

            while (!reader.EndOfStream)
            {

                var line = reader.ReadLine();
                var values = line.Split(';');


                TypeCase.Add(values[1]);
                TextAction.Add(values[2]);
                Evenement.Add(values[3]);

                try
                {
                    Valeurs.Add(Int32.Parse(values[4]));
                }
                catch
                {
                    char[] charAEnlever = { ',', ':' };
                    string[] n = values[4].Split(charAEnlever);
                    if (n.Length > 1)
                    {
                        Valeurs.Add(Int32.Parse(n[1]));
                    } 
                    else
                    {
                        Valeurs.Add(-71);
                    }
                }
            }

            reader.Close();

            TypeCase.RemoveAt(0);
            TextAction.RemoveAt(0);
            Evenement.RemoveAt(0);
            Valeurs.RemoveAt(0);

            for (int i = 15; i <= 29; i++)
            {
                string choix = Evenement[i];

                switch (choix)
                {
                    case "dépense":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], STransfertArgent));
                        break;
                    case "cadeau":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], SBonus));
                        break;
                    case "recette":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], STransfertArgent));
                        break;
                    case "aller":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], SMouvement));
                        break;
                    case "revenir":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], STeleportation));
                        break;
                    case "prison":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], SPrison));
                        break;
                    case "bonus":
                        PileCommunaute.Add(new Carte("Caisse de Communauté", TextAction[i], Valeurs[i], SBonus));
                        break;
                    default:
                        break;
                }
            }

            return PileCommunaute;
        }
    }
}
