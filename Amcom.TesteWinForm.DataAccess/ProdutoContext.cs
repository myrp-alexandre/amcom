using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using Amcom.TesteWinForm.DataAccess.Entities;

namespace Amcom.TesteWinForm.DataAccess
{
    public class ProdutoContext : BaseDbContext
    {
        public ProdutoContext(string connection)
        {
            ConnString = connection;
        }

        public DataTable GetData()
        {
            var dtable = new DataTable();

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();
                var da = new SQLiteDataAdapter("SELECT Id,Nome,Preco FROM Produtos", db);
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
                    string.Format("SELECT Id,Nome,Preco FROM Produtos where Nome LIKE '%{0}%'", nome), db);
                da.Fill(dtable);
                db.Close();
            }

            return dtable;
        }

        public int GravaProdutosImportados(List<Produto> lista)
        {
            var total = 0;

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();

                foreach (var produto in lista)
                {
                    var selectCommand =
                        new SQLiteCommand(
                            string.Format("INSERT INTO Produtos (Nome,Preco)VALUES ('{0}',{1});", produto.Nome,
                                produto.Preco.ToString(CultureInfo.InvariantCulture.NumberFormat)), db);
                    total += selectCommand.ExecuteNonQuery();
                }

                db.Close();
            }

            return total;
        }


        public Produto GetById(int id)
        {
            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();

                using (var cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "SELECT Id, Nome, Preco from Produtos where Id = " + id;
                    cmd.Connection = db;

                    var sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                        return new Produto
                        {
                            Id = sqlReader.GetInt16(0), Nome = sqlReader.GetString(1), Preco = sqlReader.GetDecimal(2)
                        };
                }

                db.Close();
            }

            return null;
        }
    }
}