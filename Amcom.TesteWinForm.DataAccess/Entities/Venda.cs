using System;

namespace Amcom.TesteWinForm.DataAccess.Entities
{
    public class Venda : BaseEntity
    {
        public DateTime DataVenda { get; set; }
        public int ClienteId { get; set; }
        public decimal Total { get; set; }
    }
}