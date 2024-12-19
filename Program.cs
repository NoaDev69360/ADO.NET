using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connectionString = "Server=localhost;Database=StoreDB;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=True;";
        DatabaseHelper dbHelper = new DatabaseHelper(connectionString);

        // Vérification et création des tables au démarrage
        CreateTablesCategorie(dbHelper);
        InsertDataCategorie(dbHelper);

        CreateTablesProduits(dbHelper);
        InsertDataProduits(dbHelper);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== GESTION DES ARTICLES ===");
            Console.WriteLine("1. Ajouter un article");
            Console.WriteLine("2. Supprimer un article");
            Console.WriteLine("3. Modifier un article");
            Console.WriteLine("4. Consulter les articles");
            Console.WriteLine("5. Quitter");
            Console.Write("Faites un choix : ");
            string choix = Console.ReadLine();

            switch (choix)
            {
                case "1":
                    AjouterArticle(dbHelper);
                    break;
                case "2":
                    SupprimerArticle(dbHelper);
                    break;
                case "3":
                    ModifierArticle(dbHelper);
                    break;
                case "4":
                    ConsulterArticles(dbHelper);
                    break;
                case "5":
                    Console.WriteLine("Au revoir !");
                    return;
                default:
                    Console.WriteLine("Choix invalide, veuillez réessayer.");
                    break;
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }

    static void AjouterArticle(DatabaseHelper dbHelper)
    {
        Console.Clear();
        Console.WriteLine("=== AJOUTER UN ARTICLE ===");

        Console.Write("Nom de l'article : ");
        string nom = Console.ReadLine();

        Console.Write("Prix de l'article : ");
        decimal prix;
        while (!decimal.TryParse(Console.ReadLine(), out prix))
        {
            Console.Write("Entrez un prix valide : ");
        }

        Console.Write("ID de la catégorie : ");
        int categorieId;
        while (!int.TryParse(Console.ReadLine(), out categorieId))
        {
            Console.Write("Entrez un ID de catégorie valide : ");
        }

        string insertQuery = $@"
            INSERT INTO Produits (Nom, Prix, CategorieId) 
            VALUES ('{nom}', {prix}, {categorieId})";
        dbHelper.ExecuteQuery(insertQuery);

        Console.WriteLine("Article ajouté avec succès !");
    }

    static void SupprimerArticle(DatabaseHelper dbHelper)
    {
        Console.Clear();
        Console.WriteLine("=== SUPPRIMER UN ARTICLE ===");

        Console.Write("ID de l'article à supprimer : ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Write("Entrez un ID valide : ");
        }

        string deleteQuery = $@"
            DELETE FROM Produits 
            WHERE Id = {id}";
        dbHelper.ExecuteQuery(deleteQuery);

        Console.WriteLine("Article supprimé avec succès !");
    }

    static void ModifierArticle(DatabaseHelper dbHelper)
    {
        Console.Clear();
        Console.WriteLine("=== MODIFIER UN ARTICLE ===");

        Console.Write("ID de l'article à modifier : ");
        int id;
        while (!int.TryParse(Console.ReadLine(), out id))
        {
            Console.Write("Entrez un ID valide : ");
        }

        Console.Write("Nouveau nom de l'article : ");
        string nom = Console.ReadLine();

        Console.Write("Nouveau prix de l'article : ");
        decimal prix;
        while (!decimal.TryParse(Console.ReadLine(), out prix))
        {
            Console.Write("Entrez un prix valide : ");
        }

        Console.Write("Nouvel ID de la catégorie : ");
        int categorieId;
        while (!int.TryParse(Console.ReadLine(), out categorieId))
        {
            Console.Write("Entrez un ID de catégorie valide : ");
        }

        string updateQuery = $@"
            UPDATE Produits 
            SET Nom = '{nom}', Prix = {prix}, CategorieId = {categorieId} 
            WHERE Id = {id}";
        dbHelper.ExecuteQuery(updateQuery);

        Console.WriteLine("Article modifié avec succès !");
    }

    static void ConsulterArticles(DatabaseHelper dbHelper)
    {
        Console.Clear();
        Console.WriteLine("=== CONSULTER LES ARTICLES ===");

        string selectQuery = @"
            SELECT p.Id, p.Nom, p.Prix, c.Nom_cat 
            FROM Produits p
            INNER JOIN Categorie c ON p.CategorieId = c.Id";
        
        using (SqlConnection connection = new SqlConnection(dbHelper.ConnectionString))
        {
            SqlCommand command = new SqlCommand(selectQuery, connection);
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                Console.WriteLine("ID\tNom\t\tPrix\t\tCatégorie");
                Console.WriteLine("----------------------------------------------------");
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]}\t{reader["Nom"]}\t{reader["Prix"]}\t{reader["Nom_cat"]}");
                }
            }
        }
    }

    static void CreateTablesCategorie(DatabaseHelper dbHelper)
    {
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Categorie (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Nom_cat NVARCHAR(100) NOT NULL
            )";
        dbHelper.ExecuteQuery(createTableQuery);
        Console.WriteLine("La table Categorie a été créée avec succès.");
    }

    static void InsertDataCategorie(DatabaseHelper dbHelper)
    {
        string insertDataQuery = @"
            INSERT INTO Categorie (Nom_cat) VALUES
            ('Électronique'),
            ('Vêtements'),
            ('Alimentation')";
        dbHelper.ExecuteQuery(insertDataQuery);
        Console.WriteLine("Les catégories ont été insérées avec succès.");
    }

    static void CreateTablesProduits(DatabaseHelper dbHelper)
    {
        string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Produits (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Nom NVARCHAR(100) NOT NULL,
                Prix DECIMAL(10, 2) NOT NULL,
                CategorieId INT NOT NULL,
                FOREIGN KEY (CategorieId) REFERENCES Categorie(Id)
            )";
        dbHelper.ExecuteQuery(createTableQuery);
        Console.WriteLine("La table Produits a été créée avec succès.");
    }

    static void InsertDataProduits(DatabaseHelper dbHelper)
    {
        string insertDataQuery = @"
            INSERT INTO Produits (Nom, Prix, CategorieId) VALUES
            ('Smartphone', 699.99, 1),
            ('Ordinateur portable', 999.99, 1),
            ('Tablette', 399.99, 1),
            ('Écouteurs', 49.99, 1),
            ('Télévision', 599.99, 1),
            ('T-shirt', 19.99, 2),
            ('Jean', 49.99, 2),
            ('Veste', 89.99, 2),
            ('Chaussures', 69.99, 2),
            ('Casquette', 14.99, 2),
            ('Pomme', 0.99, 3),
            ('Lait', 1.49, 3),
            ('Pain', 1.29, 3),
            ('Fromage', 4.99, 3),
            ('Café', 6.99, 3)";
        dbHelper.ExecuteQuery(insertDataQuery);
        Console.WriteLine("Les produits ont été insérés avec succès.");
    }
}
