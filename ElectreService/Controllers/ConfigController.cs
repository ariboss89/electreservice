using ElectreService.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElectreService.Controllers
{
    public class ConfigController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;

        public string IdKeputusan()
        {
            int id = 0;
            string idPenjualan = "";

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM config WHERE config_key = 'keputusan'", _connection);
                MySqlDataReader reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    id = reader.GetInt32("config_value");
                }

                id++;

                _connection.Close();

                var date = DateTime.Now.ToString("ddMMyyyy");

                idPenjualan = $"KEP{date}-{id.ToString("D5")}";

            }

            return idPenjualan;
        }

        [HttpPost]
        public void UpdateConfigValue(config con)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "UPDATE config SET config_value = @value WHERE config_key= @key";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@value", con.config_value);
                sqlCmd.Parameters.AddWithValue("@key", con.config_key);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
