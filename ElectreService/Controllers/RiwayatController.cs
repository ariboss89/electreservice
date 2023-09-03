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
    public class RiwayatController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;

        public IEnumerable<riwayat> GetAllRiwayat()
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM riwayat", _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(_command);
                DataTable results = new DataTable();

                adapter.Fill(results);
                _connection.Close();

                if (results == null)
                {
                    NotFound();
                }

                var srlJson = JsonConvert.SerializeObject(results);

                return JsonConvert.DeserializeObject<List<riwayat>>(srlJson);
            }

        }
        public IEnumerable<dt_riwayat> GetAllDetailRiwayat(string idRiwayat)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM dt_riwayat WHERE id_riwayat='"+idRiwayat+"'", _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(_command);
                DataTable results = new DataTable();

                adapter.Fill(results);
                _connection.Close();

                if (results == null)
                {
                    NotFound();
                }

                var srlJson = JsonConvert.SerializeObject(results);

                return JsonConvert.DeserializeObject<List<dt_riwayat>>(srlJson);
            }

        }

        [HttpPost]
        public void SaveRiwayat(riwayat rwt)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO riwayat (Id, jumlah_data, alternatif_tertinggi) Values (@Id, @jumlah, @alt)";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@Id", rwt.Id);
                sqlCmd.Parameters.AddWithValue("@jumlah", rwt.jumlah_data);
                sqlCmd.Parameters.AddWithValue("@alt", rwt.alternatif_tertinggi);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }


        [HttpPost]
        public void SaveDetailRiwayat(dt_riwayat rwt)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO dt_riwayat (id_riwayat, alternatif, hasil, keterangan) Values (@Id, @alt, @hasil, @ket)";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@Id", rwt.id_riwayat);
                sqlCmd.Parameters.AddWithValue("@alt", rwt.alternatif);
                sqlCmd.Parameters.AddWithValue("@hasil", rwt.hasil);
                sqlCmd.Parameters.AddWithValue("@ket", rwt.keterangan);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }
    }
}
