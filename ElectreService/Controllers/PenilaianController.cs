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
    public class PenilaianController : ApiController
    {
        string connStr = ConfigurationManager.ConnectionStrings["mySql"].ConnectionString;
        public IEnumerable<penilaian> GetAllPenilaian()
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM penilaian", _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(_command);
                DataTable results = new DataTable();

                adapter.Fill(results);
                _connection.Close();

                if (results == null)
                {
                    NotFound();
                }

                var srlJson = JsonConvert.SerializeObject(results);

                return JsonConvert.DeserializeObject<List<penilaian>>(srlJson);
            }

        }

        [HttpPost]
        public void SavePenilaian(penilaian atr)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "INSERT INTO penilaian (alternatif,id_kriteria, kriteria, pilihan, nilai) Values (@nama,@id, @kriteria, @pilihan, @nilai)";
                sqlCmd.Connection = _connection;

                sqlCmd.Parameters.AddWithValue("@nama", atr.alternatif);
                sqlCmd.Parameters.AddWithValue("@id", atr.id_kriteria);
                sqlCmd.Parameters.AddWithValue("@kriteria", atr.kriteria);
                sqlCmd.Parameters.AddWithValue("@pilihan", atr.pilihan);
                sqlCmd.Parameters.AddWithValue("@nilai", atr.nilai);

                _connection.Open();
                int rowInserted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        //[HttpPut]
        //public void UpdateAlternatif(alternatif atr)
        //{
        //    using (MySqlConnection _connection = new MySqlConnection(connStr))
        //    {
        //        MySqlCommand sqlCmd = new MySqlCommand();
        //        sqlCmd.CommandType = CommandType.Text;
        //        sqlCmd.CommandText = "UPDATE alternatif SET nama = @nama, alamat=@alamat WHERE Id= @id";
        //        sqlCmd.Connection = _connection;

        //        sqlCmd.Parameters.AddWithValue("@id", atr.Id);
        //        sqlCmd.Parameters.AddWithValue("@nama", atr.nama);
        //        sqlCmd.Parameters.AddWithValue("@alamat", atr.alamat);

        //        _connection.Open();
        //        int rowUpdatd = sqlCmd.ExecuteNonQuery();
        //        _connection.Close();
        //    }
        //}

        [HttpDelete]
        public void DeleteAlternatif(int Id)
        {
            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                MySqlCommand sqlCmd = new MySqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "delete from penilaian where Id = " + Id + "";
                sqlCmd.Connection = _connection;
                _connection.Open();
                int rowDeleted = sqlCmd.ExecuteNonQuery();
                _connection.Close();
            }
        }

        public string CheckKriteria(string alternatif,string kriteria)
        {
            string msg = "";

            using (MySqlConnection _connection = new MySqlConnection(connStr))
            {
                _connection.Open();
                MySqlCommand _command = new MySqlCommand("SELECT *FROM penilaian WHERE alternatif = '" + alternatif + "' AND kriteria='"+kriteria+"'", _connection);

                MySqlDataReader reader = _command.ExecuteReader();

                if (reader.Read()== true)
                {
                    msg = "Data Exist";
                }

                _connection.Close();

            }

            return msg;
        }
    }
}
