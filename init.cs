using System;
using Microsoft.Data.SqlClient;

public class DatabaseHelper
{
    private string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void ExecuteQuery(string query)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                Console.WriteLine("Requête exécutée avec succès.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Erreur lors de l'exécution de la requête : " + ex.Message);
            }
        }
    }
}
