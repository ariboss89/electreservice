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
    public class SubkriteriaController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;
        public IEnumerable<subkriteria> GetAllSubkriteria()
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM subkriteria", _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(_command);
                DataTable results = new DataTable();

                adapter.Fill(results);
                _connection.Close();

                if (results == null)
                {
                    NotFound();
                }

                var srlJson = JsonConvert.SerializeObject(results);

                return JsonConvert.DeserializeObject<List<subkriteria>>(srlJson);
            }

        }

        [HttpPost]
        public void SaveSubkriteria(subkriteria atr)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO subkriteria (id_kriteria, nama_kriteria, pilihan, nilai) Values (@id, @nama, @pilihan, @nilai)";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@id", atr.id_kriteria);
                sqlCmd.Parameters.AddWithValue("@nama", atr.nama_kriteria);
                sqlCmd.Parameters.AddWithValue("@pilihan", atr.pilihan);
                sqlCmd.Parameters.AddWithValue("@nilai", atr.nilai);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        [HttpPut]
        public void UpdateSubkriteria(subkriteria atr)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "UPDATE subkriteria SET id_kriteria = @idKriteria, nama_kriteria=@nama, pilihan=@pilihan, nilai = @nilai WHERE Id= @id";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@id", atr.Id);
                sqlCmd.Parameters.AddWithValue("@idKriteria", atr.Id);
                sqlCmd.Parameters.AddWithValue("@nama", atr.nama_kriteria);
                sqlCmd.Parameters.AddWithValue("@pilihan", atr.pilihan);
                sqlCmd.Parameters.AddWithValue("@nilai", atr.nilai);

                _connection.Open();
                int rowUpdatd = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        [HttpDelete]
        public void DeleteSubkriteria(int Id)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete from kriteria where Id = " + Id + "";
                sqlCmd.Connection = _connection;
                _connection.Open();
                int rowDeleted = sqlCmd.ExecuteNonQuery();
                _connection.Close(); 
            }
        }
    }
}
