using System;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace _2KotOleksiiHomeWorkADO.net
{
    class Program
    {
        static string connString;
        static SqlConnection connection = null;

        static void Main(string[] args)
        {
            connString = ConfigurationManager
                .ConnectionStrings["localDbCS"]
                .ConnectionString;

            connection = new SqlConnection(connString);

            try
            {
                connection.Open();

                dataInsertingFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (connection != null)
                    connection.Close();

                Console.WriteLine("BINGO! Your data is written to a SQL table");
                Console.ReadKey();
            }
        }

        static void dataInsertingFromFile()
        {           
            string fileName = "data.txt";
            string tableName = "users";
            string dataTitle = "email, password, nickname, birthday";
            string fileseparator = "|";
            string line = "";
            int counter = 0;

            StreamReader sr = new StreamReader(fileName);

            while ((line = sr.ReadLine()) != null)
            {
                if (counter > 0)
                {
                    string query = $@"INSERT into {tableName}({dataTitle}) 
                                        VALUES ('" + line.Replace(fileseparator, "','") + "') ";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                counter++;
            }

            sr.Close();
        }
    }
}