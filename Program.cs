using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialisation
            Dictionary<string, int> valeurCartes = new Dictionary<string, int>
            {
                { "A", 1 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "10", 10 },
                { "V", 10 },
                { "D", 10 },
                { "R", 10 },
            };

            List<string> joueurH = new List<string>();
            List<string> joueurO = new List<string>();
            int scoreH = 0;
            int scoreO = 0;
            Console.WriteLine("+--------------------------------+");
            Console.WriteLine("|           BlackJack            |");
            Console.WriteLine("|       par DIDIER Mathias       |");
            Console.WriteLine("+--------------------------------+");
            System.Threading.Thread.Sleep(1000);

            Console.WriteLine("\nQuel est votre prénom ?");
            string prenom = Console.ReadLine();

            // génération du paquet de cartes
            Console.WriteLine("Combien voulez-vous de paquet de cartes ?");
            int nbrPaquet = int.Parse(Console.ReadLine());

            List<string> paquet = new List<string>();
            foreach(string carte in valeurCartes.Keys)
            {
                for(int i = 0; i < 4 * nbrPaquet; i++)
                {
                    paquet.Add(carte);
                }
            }

            paquet = paquet.OrderBy(x => Guid.NewGuid()).ToList(); // on mélange les cartes

            // distribution des cartes
            for(int i = 0; i < 2; i++)
            {
                joueurH.Add(paquet.Last());
                paquet.RemoveAt(paquet.Count - 1);

                joueurO.Add(paquet.Last());
                paquet.RemoveAt(paquet.Count - 1);
            }

            // message début de parti
            Console.WriteLine("\n----------Initialisation de la parti----------");
            System.Threading.Thread.Sleep(500);

            // affichage du jeu
            Console.WriteLine("\n{0} : ", prenom);
            AffichageCartes(joueurH);
            scoreH = Score(joueurH, true);
            Console.WriteLine("{0} pts\n", scoreH);

            Console.WriteLine("Ordinateur :");
            List<string> vueJ = joueurO.Select((string i) => "?")
                                       .ToList(); // cette liste permet de cacher les carte de l'ordinateur non visibles par le joueur
            vueJ[1] = joueurO[1]; // on remplace la 2e carte car c'est la seule que l'on voit chez l'ordinateur
            AffichageCartes(vueJ);

            // déroulement d'un tour
            bool stopJoueur = false;
            bool stopOrdinateur = false;
            bool finPartie = false;

            while(!finPartie)
            {
                // décision du joueur
                if (!stopJoueur)
                {
                    Console.WriteLine("Voulez-vous piocher une nouvelle carte ?");
                    Console.WriteLine("o- Oui");
                    Console.WriteLine("n- Non");

                    string choixJoueur = Console.ReadLine();
                    if (choixJoueur == "o")
                    {
                        Console.WriteLine("{0} : Je pioche", prenom);
                        joueurH.Add(paquet.Last());
                        paquet.RemoveAt(paquet.Count - 1);
                    }
                    else
                    {
                        Console.WriteLine("{0} : Je m'arrête là.", prenom);
                        stopJoueur = true;
                    }
                }

                scoreO = Score(joueurO, false);
                // décision de l'ordinateur
                if (!stopOrdinateur)
                {
                    if (scoreO <= 15)
                    {
                        Console.WriteLine("Ordinateur : Je pioche.");
                        joueurO.Add(paquet.Last());
                        paquet.RemoveAt(paquet.Count - 1);
                    }
                    else
                    {
                        Console.WriteLine("Ordinateur : Je m'arrête là.");
                        stopOrdinateur = true;
                    }
                }

                // affichage du jeu
                Console.WriteLine("\n{0} : ", prenom);
                AffichageCartes(joueurH);
                scoreH = Score(joueurH, true);
                Console.WriteLine("{0} pts\n", scoreH);

                Console.WriteLine("Ordinateur :");
                vueJ = joueurO.Select((string i) => "?")
                              .ToList(); // cette liste permet de cacher les carte de l'ordinateur non visibles par le joueur
                vueJ[1] = joueurO[1]; // on remplace la 2e carte car c'est la seule que l'on voit chez l'ordinateur
                AffichageCartes(vueJ);
                Console.WriteLine();

                // fin de partie
                if (stopOrdinateur && stopJoueur)
                {
                    finPartie = true;
                }

                if (scoreH >= 21 || scoreO >= 21)
                {
                    finPartie = true;
                }
            }

            // messages de fin de partie
            int Jgagne = 0;

            if(scoreH == 21)
            {
                Console.WriteLine("Black Jack ! {0} a 21 points.", prenom);
                Jgagne = 1;
            }
            else if (scoreO == 21)
            {
                Console.WriteLine("Black Jack ! Ordinateur a 21 points.");
                Jgagne = 0;
            }
            else if(scoreH > 21)
            {
                Console.WriteLine("{0} a plus de 21 points.", prenom);
                Jgagne = 0;
            }
            else if(scoreO > 21)
            {
                Console.WriteLine("Ordinateur a plus de 21 points.");
                Jgagne = 1;
            }
            else // cas ou personne n'as 21 point ou plus
            {
                if(scoreH > scoreO)
                {
                    Console.WriteLine("{0} est le plus proche des 21 points", prenom);
                    Jgagne = 1;
                }
                else if (scoreH > scoreO)
                {
                    Console.WriteLine("Ordinateur est le plus proche des 21 points");
                    Jgagne = 0;
                }
                else
                {
                    Console.WriteLine("Les deux joueurs ont le même nombre de points.");
                    Jgagne = 2;
                }
            }


            if (Jgagne == 1)
            {
                Console.WriteLine("{0} gagne.", prenom);
                Console.WriteLine("Ordinateur perd.");
            }
            else if(Jgagne == 0)
            {
                Console.WriteLine("{0} perd.", prenom);
                Console.WriteLine("Ordinateur gagne.");
            }
            else
            {
                Console.WriteLine("Egalité");
            }
        }

        public static int Score(List<string> listeCartes, bool joueur)
        {
            Dictionary<string, int> valeurCartes = new Dictionary<string, int> // on ne défiit pas de valeur pour la carte "1" car elle est variable
            {
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "10", 10 },
                { "V", 10 },
                { "D", 10 },
                { "R", 10 },
            };

            int score = 0;
            foreach(string c in listeCartes)
            {
                if(c == "A" && joueur)
                {
                    Console.WriteLine("\nQuelle valeur voulez-vous pour cet As ? 1 ou 11 ?");
                    int choix = int.Parse(Console.ReadLine());
                    while(choix != 1 && choix != 11)
                    {
                        Console.WriteLine("Veuillez choisir une valeur valide. 1 ou 11");
                        choix = int.Parse(Console.ReadLine());
                    }
                    score += choix;
                }
                else if(c == "A")
                {
                    int choix = new Random().Next(0, 2);
                    if(choix == 0)
                    {
                        score += 1;
                    }
                    else
                    {
                        score += 11;
                    }
                }
                else
                {
                    score += valeurCartes[c];
                }
            }

            return score;
        }

        public static void AffichageCartes(List<string> listeCartes)
        {
            string l1 = "";
            string l2 = "";
            string l3 = "";
            string l4 = "";
            string l5 = "";

            foreach (string c in listeCartes)
            {
                l1 += "+-----+ ";
                l2 += "|     | ";
                if (c == "10")
                {
                    l3 += $"|  {c} | ";
                }
                else
                {
                    l3 += $"|  {c}  | ";
                }
                l4 += "|     | ";
                l5 += "+-----+ ";
            }
            Console.WriteLine($"{l1}\n{l2}\n{l3}\n{l4}\n{l5}");
        }
    }
}
