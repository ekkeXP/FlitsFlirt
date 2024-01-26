using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirtdev.Services
{
    public interface IDBConnect
    {
        SqlConnectionStringBuilder builder { get; set; }

        private void ConnectAsync() { }
        Task<string> ReadQueryAsync(String query, string value1 = "", string value2 = "");
        Task<bool> InsertQueryAsync(string query, string value1 = "", string value2 = "");
        Task<bool> UpdateQueryAsync(string query, string value1 = "", string value2 = "");
        Task<bool> DeleteQueryAsync(string query, string value1 = "", string value2 = "");
        List<string> GetStringListFromTable(string query);
        List<int> GetIntListFromTable(string query);
        Task<bool> UserExists(string username);
        Task<bool> IsEqual(string enteredPass, string enteredConfirm);
        List<string> GetHobbies();
    }
}
