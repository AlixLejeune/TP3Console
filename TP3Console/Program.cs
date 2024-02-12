
using System;
using System.Linq;
using TP3Console.Models.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace TP3Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
            using (var ctx = new FilmsDBContext())
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
            }

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
            Console.ReadLine();
        }
    }
}