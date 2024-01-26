using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Microsoft.Data.SqlClient;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Flitsflirt.App.Services
{
    internal class DBConnect : IDBConnect
    {
        //Het ip adres van de PFSense
        private static string RemoteServer = "145.44.235.179";
        //De port die gebruikt wordt door de ubuntu server
        private static uint RemotePort = 1433;
        //De gebruikersnaam die gebruikt wordt voor de database
        private static string RemoteUsername = "student";
        //Het wachtwoord voor de database EN ubuntu server
        private static string RemotePassword = "Flitsflirt123";
        //Initialiseer de connectie informatie
        private static ConnectionInfo connectionInfo = new ConnectionInfo(RemoteServer, RemoteUsername, new PasswordAuthenticationMethod(RemoteUsername, RemotePassword));
        //Maak een SSHtunnel om een directe verbinding te maken met de database
        private static SshTunnel sshTunnel = new SshTunnel(connectionInfo, RemotePort);
        //Annulerings token voor de Toast meldingen
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        //Zodra de DBConnect wordt aangemaakt maken we verbinding met de database
        public DBConnect()
        {
            ConnectAsync();
        }
        public SqlConnectionStringBuilder builder { get; set; }
        
        //Functie die verbinding maakt met de database
        private void ConnectAsync()
        {
            try
            {
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = "127.0.0.1";
                builder.UserID = "SA";
                builder.Password = "Flitsflirt123";
                builder.InitialCatalog = "FlitsflirtDB";
                builder.TrustServerCertificate = true;
                builder.Encrypt = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //Funtie voor het uitlezen van een SQL-Query
        public async Task<string> ReadQueryAsync(string query, string value1 = "", string value2 = "")
        {
            string output = "";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value1", value1);
                    command.Parameters.AddWithValue("@Value2", value2);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                output += reader[i].ToString();
                                output += " ";
                            }
                            if (reader.FieldCount >= 2)
                            {
                                output += "\n";
                            }

                        }

                    }
                    connection.Close();
                }
            }
            return await Task.FromResult(output);
        }

        //Funtie voor het uitvoeren van een INSERT-Query
        public async Task<bool> InsertQueryAsync(string query, string value1 = "", string value2 = "")
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value1", value1);
                    command.Parameters.AddWithValue("@Value2", value2);
                    connection.Open();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    connection.Close();

                    if (rowsAffected > 0)
                    {
                        // Data insertion was successful
                        return await Task.FromResult(true);
                    }
                    else
                    {
                        // No rows were affected, indicating an error or no data was inserted
                        return await Task.FromResult(false);
                    }
                }
            }
        }

        //Functie voor het uivoeren van een UPDATE-Query
        public async Task<bool> UpdateQueryAsync(string query, string value1 = "", string value2 = "")
        {
            bool result = await InsertQueryAsync(query, value1, value2);
            return await Task.FromResult(result);
        }
        //Functie voor het uitvoeren van een DELETE-Query
        public async Task<bool> DeleteQueryAsync(string query, string value1 = "", string value2 = "")
        {
            bool result = await InsertQueryAsync(query, value1, value2);
            return await Task.FromResult(result);
        }
        //Functie om te chechen of een gebruikersnaam al bestaat in de database
        public async Task<bool> UserExists(string username)
        {
            string query = $"SELECT COUNT(*) FROM Account WHERE Username = @Value1";
            int output = 0;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value1", username);
                    connection.Open();
                    output = (int)command.ExecuteScalar();
                    connection.Close();
                }
            }
            if (output >= 1)
            {
                return await Task.FromResult(true);
            }
            else { return await Task.FromResult(false); }
        }
        //Functie om te checken of 2 strings daadwerkelijk hetzelfde zijn.
        public async Task<bool> IsEqual(string str1, string str2)
        {
            return await Task.FromResult(string.Equals(str1, str2, StringComparison.Ordinal));
        }
        //Functie voor het ophalen van een List met string objects
        public List<string> GetStringListFromTable(string query)
        {
            List<string> stringList = new List<string>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string value = reader.GetString(0); // Assuming the column index is 0
                            stringList.Add(value);
                        }
                    }
                }
            }

            return stringList;
        }
        //Functie voor het ophalen van een List met de hobbynamen als strings
        public List<string> GetHobbies()
        {
            return GetStringListFromTable("SELECT HobbyName FROM Hobby");
        }
        //Functie voor het ophalen van de ingevulde beschrijving van de huidige gebruiker
        public async Task <string> GetDescription()
        {
            int User = Preferences.Default.Get("AccountID", -1);
            return await ReadQueryAsync($"SELECT Description FROM Account_Data WHERE AccountID = '{User}'");
        }
        //Functie voor het ophalen van een List met int objects
        public List<int> GetIntListFromTable(string query)
        {
            List<int> intList = new List<int>();

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int value = reader.GetInt32(0); // Assuming the column index is 0
                            intList.Add(value);
                        }
                    }
                }
            }

            return intList;
        }
        //Functie voor het maken van een toast melding
        public async void MakeToast(string text)
        {
            ToastDuration duration = ToastDuration.Short;
            double fontSize = 14;
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }

    }
    //Interne klasse voor het aanmaken van een SSH tunnel
    internal class SshTunnel : IDisposable
    {
        private SshClient client;
        private ForwardedPortLocal port;
        private int localPort;

        public SshTunnel(ConnectionInfo connectionInfo, uint remotePort)
        {
            try
            {
                if (client == null || !client.IsConnected)
                {
                    
                    client = new SshClient(connectionInfo);
                    
                    port = new ForwardedPortLocal("127.0.0.1", 1433, "127.0.0.1", remotePort);

                    client.Connect();
                    client.AddForwardedPort(port);
                    port.Start();
                    while (!port.IsStarted)
                    {
                        Thread.Sleep(100);
                    }
                    localPort = (int)port.BoundPort;
                }

            }
            catch
            {
                Dispose();
                throw;
            }
        }

        public int LocalPort { get { return localPort; } }
        //Dispose de SSH tunnel
        public void Dispose()
        {
            if (port != null)
                port.Dispose();
            if (client != null)
                client.Dispose();
        }

    }
}
