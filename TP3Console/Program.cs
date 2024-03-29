﻿
using System;
using System.Linq;
using TP3Console.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace TP3Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region chargements
            /*using( FilmsDBContext ctx = new FilmsDBContext())
            {

                //Désactivation du tacking => pas de modif de la base
                //ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                Film titanic = ctx.Films.First(f => f.Nom.Contains("Titanic"));
                titanic.Description = "un bateau échoué. Date : " + new DateTime(1912, 4, 15);

                int nbchanges = ctx.SaveChanges();
                Console.WriteLine("nombre d'enregistrement modif ou ajout : " + nbchanges);
            }*/

            //chargement manuel de la categorie, puis des films qui appartiennent à cette catégorie
            /*using (var ctx = new FilmsDBContext())
            {
                //Chargement de la catégorie Action
                Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                Console.WriteLine("Films : ");
                //Chargement des films de la catégorie Action.
                foreach (var film in ctx.Films.Where(f => f.CategorieNavigation.Nom ==
                categorieAction.Nom).ToList())
                {
                    Console.WriteLine(film.Nom);
                }
            }*/

            //chargement explicite
            /*using (var ctx = new FilmsDBContext())
            {
                Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                //Chargement des films dans categorieAction
                ctx.Entry(categorieAction).Collection(c => c.Films).Load();
                Console.WriteLine("Films : ");
                foreach (var film in categorieAction.Films)
                {
                    Console.WriteLine(film.Nom);
                }
            }*/

            //chargement hâtif
            /*using (var ctx = new FilmsDBContext())
            {
                //Chargement de la catégorie Action et des films de cette catégorie
                Categorie categorieAction = ctx.Categories
                .Include(c => c.Films)
                .First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                Console.WriteLine("Films : ");
                foreach (var film in categorieAction.Films)
                {
                    Console.WriteLine(film.Nom);
                }
            }*/

            //chargement delayed / lazy
            /*using (var ctx = new FilmsDBContext())
            {
                //Chargement de la catégorie Action
                Categorie categorieAction = ctx.Categories.First(c => c.Nom == "Action");
                Console.WriteLine("Categorie : " + categorieAction.Nom);
                Console.WriteLine("Films : ");
                //Chargement des films de la catégorie Action.
                foreach (var film in categorieAction.Films) // lazy loading initiated
                {
                    Console.WriteLine(film.Nom);
                }
            }*/
            #endregion
            var ctx = new FilmsDBContext();
            Exo3Q5();

            Console.WriteLine();

        }

        #region Exo2
        //affiche tous les films
        public static void Exo2Q1()
        {
            var ctx = new FilmsDBContext();
            foreach (var film in ctx.Films)
            {
                Console.WriteLine(film.ToString());
            }
        }

        //affiche les mails de tous les utilisateurs
        public static void Exo2Q2()
        {
            var ctx = new FilmsDBContext();
            foreach(Utilisateur usr in ctx.Utilisateurs)
            {
                Console.WriteLine(usr.Email);
            }
        }

        //affiche tous les logins triés par login croissant
        public static void Exo2Q3()
        {
            var ctx = new FilmsDBContext();
            var users = 
                from usr in ctx.Utilisateurs
                orderby usr.Login
                select usr;

            foreach (Utilisateur user in users)
            {
                Console.WriteLine(user.Login);
            }
        }

        //affiche "Id : titre" pour tous les films d'action
        public static void Exo2Q4()
        {
            var ctx = new FilmsDBContext();
            var actions = from film in ctx.Films
                          where film.CategorieNavigation.Nom == "Action"
                          select film;
            foreach (Film f in actions)
            {
                Console.WriteLine(f.Id + " : " + f);
            }
        }

        //affiche le nombre de categories
        public static void Exo2Q5()
        {
            var ctx = new FilmsDBContext();
            Console.WriteLine("Nombre de catégories : " + ctx.Categories.Count());
        }

        //affiche la note la plus basse de tous les films
        public static void Exo2Q6()
        {
            var ctx = new FilmsDBContext();
            Console.WriteLine("La pire note : " + (from avis in ctx.Avis orderby avis.Note select avis).First().Note);
        }

        //affiche tous les films commençant par "le"
        public static void Exo2Q7()
        {
            var ctx = new FilmsDBContext();
            var films = from film in ctx.Films
                        where Regex.IsMatch(film.Nom.ToLower(),"^[l][e]")
                        select film;
            foreach(var film in films)
            {
                Console.WriteLine(film);
            }
        }

        //affiche la moyenne des notes pour le film "Pulp Fiction"
        public static void Exo2Q8()
        {
            var ctx = new FilmsDBContext();
            decimal avg = 0;
            var lesavis = from avis in ctx.Avis
                          where avis.FilmNavigation.Nom.ToLower() == "pulp fiction"
                          select avis;
            foreach(Avi avi in lesavis)
            {
                avg += avi.Note;
            }
            avg /= lesavis.Count();
            Console.WriteLine("Note moyenne de Pulp Fiction : " + avg.ToString());
        }

        //affiche l'utilisateur ayant mis la plus haute note dans la base
        public static void Exo2Q9()
        {
            var ctx = new FilmsDBContext();
            var usr = from avis in ctx.Avis
                          orderby avis.Note descending
                          select avis.UtilisateurNavigation;
            Console.WriteLine(usr.First());
        }
        #endregion

        #region Exo3

        public static int AddUser(string mail, string login, string password)
        {
            Utilisateur usr = new Utilisateur()
            { Login = login,
                Email = mail,
                Pwd = HashCode.Combine(password).ToString()
            };
            FilmsDBContext ctx = new FilmsDBContext();
            ctx.Utilisateurs.Add(usr);
            return ctx.SaveChanges();
        }

        public static void Exo3Q1()
        {
            int nb = AddUser("JMLP@fn.fr", "Jean-marie Le Pen", "pwd");
            Console.WriteLine("Nombre de lignes ajoutées : " + nb);
        }

        public static void Exo3Q2()
        {
            var ctx = new FilmsDBContext();
            ctx.Films.First(x => x.Nom == "L'armee des douze singes").Description = "*Monkey noises*";
            ctx.Films.First(x => x.Nom == "L'armee des douze singes").Categorie = ctx.Categories.First(x => x.Nom == "Drame").Id;

            int nb = ctx.SaveChanges();
            Console.WriteLine("Lignes modifiées : " + nb);
        }

        public static void Exo3Q3()
        {
            var ctx = new FilmsDBContext();
            ctx.Avis.RemoveRange(ctx.Avis.Where(x => x.FilmNavigation.Nom == "L'armee des douze singes"));
            ctx.Films.Remove(ctx.Films.FirstOrDefault(x =>  x.Nom == "L'armee des douze singes"));

            int nb = ctx.SaveChanges();
            Console.WriteLine("Lignes suppr : " + nb);
        }

        public static void Exo3Q4()
        {
            var ctx = new FilmsDBContext();
            AddUser("JMLP@fn.fr", "Jean-marie Le Pen", "pwd");
            ctx.Avis.Add(new Avi() { Utilisateur = 1, Film = 45});
            ctx.SaveChanges();
            Avi avis = ctx.Avis.First(x => x.Film == 45 && x.Utilisateur == 1);
            avis.Avis = "Jeanne ! Oskour !";
            avis.Note = (decimal)10;

            int nb = ctx.SaveChanges();
            Console.WriteLine("Lignes suppr : " + nb);
        }

        public static void Exo3Q5()
        {
            var ctx = new FilmsDBContext();
            ctx.Films.AddRange(new List<Film>() { new Film() { Categorie = 5, Nom = "Your Name" }, new Film() { Categorie = 5, Nom = "A Silent Voice" } });
            ctx.SaveChanges();
        }

        #endregion


    }
}