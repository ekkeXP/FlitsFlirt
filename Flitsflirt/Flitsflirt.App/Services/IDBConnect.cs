using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flitsflirt.App.Services
{
    public interface IDBConnect
    {
        SqlConnectionStringBuilder builder { get; set; }

        void ConnectAsync() { }
        Task<string> ReadQueryAsync(String query, string value1 = "", string value2 = "");
        Task<bool> InsertQueryAsync(string query, string value1 = "", string value2 = "");
        Task<bool> UpdateQueryAsync(string query, string value1 = "", string value2 = "");
        Task<bool> DeleteQueryAsync(string query, string value1 = "", string value2 = "");
        List<string> GetStringListFromTable(string query);
        List<string> GetHobbies();
        Task<string> GetDescription();
        Task<bool> UserExists(string username);
        Task<bool> IsEqual(string str1, string str2);
        List<int> GetIntListFromTable(string query);

        void MakeToast(string text);
    }
}