using Flitsflirtdev.Services;
using Microsoft.Data.SqlClient;
using Renci.SshNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirtdev.Services
{
    internal class DBConnect : IDBConnect
    {
        private static string RemoteServer = "145.44.235.179";
        private static uint RemotePort = 1433;
        private static string RemoteUsername = "student";
        private static string RemotePassword = "Flitsflirt123";
        private static ConnectionInfo connectionInfo = new ConnectionInfo(RemoteServer, RemoteUsername, new PasswordAuthenticationMethod(RemoteUsername, RemotePassword));
        private static SshTunnel sshTunnel = new SshTunnel(connectionInfo, RemotePort);

        public DBConnect()
        {
            ConnectAsync();
        }
        public SqlConnectionStringBuilder builder { get; set; }

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


        public async Task<bool> UpdateQueryAsync(string query, string value1 = "", string value2 = "")
        {
            bool result = await InsertQueryAsync(query);
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteQueryAsync(string query, string value1 = "", string value2 = "")
        {
            bool result = await InsertQueryAsync(query);
            return await Task.FromResult(result);
        }

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

        public async Task<bool> IsEqual(string str1, string str2)
        {
            return await Task.FromResult(string.Equals(str1, str2, StringComparison.Ordinal));
        }

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

        public List<string> GetHobbies()
        {
            return GetStringListFromTable("SELECT HobbyName FROM Hobby");
        }
    }

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
                    // Display error
                    client = new SshClient(connectionInfo);

                    // using 0 for the client port to dynamically allocate it
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

        public void Dispose()
        {
            if (port != null)
                port.Dispose();
            if (client != null)
                client.Dispose();
        }
    }
}