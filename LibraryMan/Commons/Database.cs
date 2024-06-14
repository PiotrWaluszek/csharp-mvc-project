using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.Sqlite;

namespace LibraryMan.Commons
{
    public static class Database
    {
        public static void CreateDatabase()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "./LibraryMan.Data.db"
            };
            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();
            InitializeDatabase(connection);
        }

        static void InitializeDatabase(SqliteConnection connection)
        {
            try
            {
                var command = connection.CreateCommand();
                var checkCommand = connection.CreateCommand();

                // Initialize UzytkownikModel
                checkCommand.CommandText = @"
                    SELECT COUNT(1)
                    FROM UzytkownikModel
                ";
                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                if (!exists)
                {
                    string adminEmail = "admin@libraryman.com";
                    string adminPassword = "admin";

                    var adminPasswordHash = LibraryMan.Commons.Hash.CalculateMD5Hash(adminPassword);
                    command.CommandText = $"INSERT INTO UzytkownikModel (UserID, Email, Password, IsAdmin, Token) VALUES (1, '{adminEmail}', '{adminPasswordHash}', 1, '{Commons.Tokens.GenerateToken()}' )";
                    command.ExecuteNonQuery();
                }

                // Initialize WydawnictwoModel
                checkCommand.CommandText = @"
                    SELECT COUNT(1)
                    FROM WydawnictwoModel
                ";
                exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                if (!exists)
                {
                    command.CommandText = @"
                    INSERT INTO WydawnictwoModel (PublisherName, City, Country, Founded, Description) VALUES
                    ('Wydawnictwo A', 'Warszawa', 'Polska', 1990, 'Opis wydawnictwa A.'),
                    ('Wydawnictwo B', 'Kraków', 'Polska', 1985, 'Opis wydawnictwa B.'),
                    ('Wydawnictwo C', 'Poznań', 'Polska', 2000, 'Opis wydawnictwa C.'),
                    ('Wydawnictwo D', 'Gdańsk', 'Polska', 1995, 'Opis wydawnictwa D.'),
                    ('Wydawnictwo E', 'Wrocław', 'Polska', 2010, 'Opis wydawnictwa E.');
                    ";
                    command.ExecuteNonQuery();
                }

                // Initialize KsiazkaModel
                checkCommand.CommandText = @"
                    SELECT COUNT(1)
                    FROM KsiazkaModel
                ";
                exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                if (!exists)
                {
                    command.CommandText = @"
                    INSERT INTO KsiazkaModel (BookName, PublisherName, Genre, AverageRating) VALUES
                    ('Ksiazka A', 'Wydawnictwo A', 'Fantastyka', 0.0),
                    ('Ksiazka B', 'Wydawnictwo B', 'Horror', 0.0),
                    ('Ksiazka C', 'Wydawnictwo C', 'Dramat', 0.0),
                    ('Ksiazka D', 'Wydawnictwo D', 'Kryminał', 0.0),
                    ('Ksiazka E', 'Wydawnictwo E', 'Komedia', 0.0);
                    ";
                    command.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }
    }
}
