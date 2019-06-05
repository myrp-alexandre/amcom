using System;
using System.Data.SQLite;
using Amcom.TesteWinForm.DataAccess.Model;

namespace Amcom.TesteWinForm.DataAccess
{
    public class VendasContext : BaseDbContext
    {
        public VendasContext(string connection)
        {
            ConnString = connection;
        }


        public int GetNewId()
        {
            var maxid = 0;

            using (var db = new SQLiteConnection(ConnString))
            {
                db.Open();

                using (var cmd = new SQLiteCommand())
                {
                    cmd.CommandText = "SELECT max(id) FROM Vendas";
                    cmd.Connection = db;
                    var sqlReader = cmd.ExecuteReader();

                    while (sqlReader.Read())
                        maxid = sqlReader.GetInt16(0);
                }

                db.Close();
            }

            return maxid + 1;
        }


        public bool Gravar(VendaModel model)
        {
            try
            {
                var saveOk = false;
                var result = 0;

                using (var db = new SQLiteConnection(ConnString))
                {
                    db.Open();

                    using (var transaction = db.BeginTransaction())
                    {
                        var clienteId = new ClienteContext(ConnString).GetClientId(model.NomeCliente);

                        using (var cmd = new SQLiteCommand())
                        {
                            cmd.CommandText =
                                "INSERT INTO Vendas (Id,Data,ClienteId,Total) VALUES (@Id,@Data,@ClienteId,@Total)";
                            cmd.Connection = db;
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@Id", model.VendaId);
                            cmd.Parameters.AddWithValue("@Data", model.DataVenda);
                            cmd.Parameters.AddWithValue("@ClienteId", clienteId);
                            cmd.Parameters.AddWithValue("@Total", model.Total);
                            result = cmd.ExecuteNonQuery();
                        }


                        foreach (var prodItem in model.Produtos)
                            using (var cmd = new SQLiteCommand())
                            {
                                cmd.CommandText =
                                    "INSERT INTO VendaProdutos (VendaId,ProdutoId,Quantidade,Valor) VALUES (@VendaId,@ProdutoId,@Quantidade,@Valor)";
                                cmd.Connection = db;
                                cmd.Prepare();
                                cmd.Parameters.AddWithValue("@VendaId", prodItem.IdVenda);
                                cmd.Parameters.AddWithValue("@ProdutoId", prodItem.ProdutoId);
                                cmd.Parameters.AddWithValue("@Quantidade", prodItem.Quantidade);
                                cmd.Parameters.AddWithValue("@Valor", prodItem.Subtotal);
                                result = cmd.ExecuteNonQuery();
                            }

                        transaction.Commit();

                        saveOk = result > 0;
                    }

                    db.Close();
                }

                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gravar venda: " + ex.Message);
            }
        }
    }
}