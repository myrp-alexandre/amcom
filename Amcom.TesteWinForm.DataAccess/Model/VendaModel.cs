using System;
using System.Collections.Generic;
using System.Globalization;

namespace Amcom.TesteWinForm.DataAccess.Model
{
    public class VendaModel
    {
        public VendaModel(int vendaId)
        {
            VendaId = vendaId;
            DataVenda = DateTime.Now;
            Produtos = new List<VendaProdutoModel>();
        }

        public int VendaId { get; set; }
        public DateTime DataVenda { get; set; }
        public string NomeCliente { get; set; }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var prod in Produtos)
                    total = total + prod.Subtotal;

                return total;
            }
        }

        public List<VendaProdutoModel> Produtos { get; set; }


        public string DataFormated()
        {
            return DataVenda.ToString("dd/MM/yyyy HH:mm", new CultureInfo("pt-BR"));
        }

        public override string ToString()
        {
            return VendaId.ToString();
        }
    }

    public class VendaProdutoModel
    {
        public VendaProdutoModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public int IdVenda { get; set; }
        public int ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Subtotal { get; set; }
    }
}