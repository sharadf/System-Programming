//Домашнее задание 2.
//Надо создать программу которая читает, добавляет и изменяет Авторов из БД Library
//1. Get all
//2. Add Author
//3. Edit Author
//4. Exit

//P.S. Надо с помощью Thread.Sleep() симитировать долгую загрузку БД. При этом работа (анимация меню) не должна зависать.
//P.S.S. или симитировать загрузку БД процентами. Пока база данных загружается анимация процентов должно идти.

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System_Programming_HW2___Thread;

class Program
{
    static List<string> authors = new List<string>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Get all authors");
            Console.WriteLine("2. Add author");
            Console.WriteLine("3. Edit author");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    LoadDatabase();
                    GetAllAuthors();
                    break;
                case "2":
                    LoadDatabase();
                    AddAuthor();
                    break;
                case "3":
                    LoadDatabase();
                    EditAuthor();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }

    static void GetAllAuthors()
    {
        using (var db = new ApplicationContext())
        {

            Console.WriteLine("Authors in the library:");
            if (authors.Count == 0)
                Console.WriteLine("No authors found.");

            else
            {
                foreach (var author in authors)
                    Console.WriteLine(author);
            }
        }
    }

    static void AddAuthor()
    {
        using (var db = new ApplicationContext())
        {

            Console.Write("Enter author name: ");
            var name = Console.ReadLine();
            authors.Add(name);
            Console.WriteLine($"Author '{name}' added.");
        }
    }

    static void EditAuthor()
    {
        using (var db = new ApplicationContext())
        {

            Console.Write("Enter author name to edit: ");
            var oldName = Console.ReadLine();

            for (int i = 0; i < authors.Count; i++)
            {
                if (authors[i] == oldName)
                {
                    Console.Write("Enter new author name: ");
                    var newName = Console.ReadLine();
                    authors[authors.IndexOf(oldName)] = newName;
                    Console.WriteLine($"Author '{oldName}' updated to '{newName}'.");
                }
                else
                    Console.WriteLine($"Author '{oldName}' not found.");
            }
        }
    }

    static void LoadDatabase()
    {
        Console.WriteLine("Loading database...");
        for (int i = 0; i <= 100; i += 10)
        {
            Console.Write($"\rLoading... {i}%");
            Thread.Sleep(200);
        }
        Console.WriteLine("\nDatabase loaded.");
    }
}



public class ApplicationContext : DbContext
{
    public DbSet<Author> Authors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data source = library.Db");
    }
}

public class Author
{
    public int AuthorId { get; set; }
    public string Name { get; set; }
}