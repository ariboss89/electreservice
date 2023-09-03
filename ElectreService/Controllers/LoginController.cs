using ElectreService.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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
    public class LoginController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;

        public IEnumerable<users> GetAllDataUser(string username)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM users WHERE username = '" + username + "'", _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(_command);
                DataTable results = new DataTable();

                adapter.Fill(results);
                _connection.Close();

                if (results == null)
                {
                    NotFound();
                }

                var srlJson = JsonConvert.SerializeObject(results);

                return JsonConvert.DeserializeObject<List<users>>(srlJson);
            }

        }

        [HttpPost]
        public string GetUserRoles(string username)
        {
            string roles = "";

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM users WHERE username = '" + username + "'", _connection);
                MySqlDataReader reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    roles = reader.GetString("role");
                }

                _connection.Close();


            }

            return roles;
        }

        [HttpPost]
        public Models.response UserLogin(users login)
        {
            string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand manise = new MySqlCommand("SELECT *FROM users WHERE username=@username AND password=@password", _connection);
                manise.Parameters.AddWithValue("@username", login.username);
                manise.Parameters.AddWithValue("@password", login.password);
                _connection.Open();
                var log = manise.ExecuteScalar();
                _connection.Close();

                if (log == null)
                {
                    return new Models.response { Status = "Invalid", Message = "Invalid User." };
                }
                else
                    return new Models.response { Status = "Success", Message = "Login Successfully" };
            }
        }

        [HttpPost]
        public void SaveUser(users log)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO users (username, password, role) Values (@username, @password, @role)";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@username", log.username);
                sqlCmd.Parameters.AddWithValue("@password", log.password);
                sqlCmd.Parameters.AddWithValue("@role", log.role);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        [HttpPut]
        public void UpdateUser(users login)
        {
            string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "UPDATE users SET password = @pasw WHERE username= @username";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@pasw", login.password);
                sqlCmd.Parameters.AddWithValue("@username", login.username);

                _connection.Open();
                int rowUpdatd = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        [HttpDelete]
        public void DeleteUser(string username)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete from users where username = " + username + "";
                sqlCmd.Connection = _connection;
                _connection.Open();
                int rowDeleted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public string CheckUsername(string username)
        {
            string user = "";
            string msg = "";

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM users WHERE username = '" + username + "'", _connection);

                MySqlDataReader reader = _command.ExecuteReader();

                if (reader.Read())
                {
                    user = reader.GetString("username");
                }

                if (username == user)
                {
                    msg = "Username exist";
                }
                else
                {
                    msg = "Username doesnt exist";
                }

                _connection.Close();

            }

            return msg;
        }
    }
}
