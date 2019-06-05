using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using Amcom.TesteWinForm.DataAccess.Entities;

namespace Amcom.TesteWinForm.DataAccess
{
    public class ClienteContext : BaseDbContext
    {
        public ClienteContext(string connection)
        {
            ConnString = connection;
        }

        public DataTable GetData()
        {
            var dtable = new DataTable();

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();
                var da = new SQLiteDataAdapter("SELECT Id,Nome,Email,Telefone FROM Clientes", db);
                da.Fill(dtable);
                db.Close();
            }

            return dtable;
        }

        public DataTable GetData(string nome)
        {
            var dtable = new DataTable();

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();
                var da = new SQLiteDataAdapter(
                    string.Format("SELECT Id,Nome,Email,Telefone FROM Clientes where Nome LIKE '%{0}%'", nome), db);
                da.Fill(dtable);
                db.Close();
            }

            return dtable;
        }

        public List<string> GetClientNames()
        {
            var list = new List<string>();

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();

                using (var cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "SELECT Nome from Clientes";
                    cmd.Connection = db;

                    var sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read()) list.Add(sqlReader.GetString(0));
                }

                db.Close();
            }

            return list;
        }

        public int GetClientId(string nome)
        {
            var clientId = 0;

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();

                using (var cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "SELECT Id from Clientes where Nome = '" + nome + "'";
                    cmd.Connection = db;

                    var sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                        clientId = sqlReader.GetInt32(0);
                }

                db.Close();
            }

            return clientId;
        }

        public bool Gravar(Cliente cliente)
        {
            try
            {
                var saveOk = false;

                using (var db = new SQLiteConnection(ConnString))
                {
                    db.Open();

                    using (var cmd = new SQLiteCommand())
                    {
                        cmd.CommandText = "INSERT INTO Clientes (Nome,Email,Telefone) VALUES (@Nome,@Email,@Telefone)";
                        cmd.Connection = db;
                        cmd.Prepare();
                        cmd.Parameters.AddWithValue("@Nome", cliente.Nome);
                        cmd.Parameters.AddWithValue("@Email", cliente.Email);
                        cmd.Parameters.AddWithValue("@Telefone", cliente.Telefone);
                        saveOk = cmd.ExecuteNonQuery() > 0;
                    }

                    db.Close();
                }

                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar cliente: " + ex.Message);
            }
        }
    }
}