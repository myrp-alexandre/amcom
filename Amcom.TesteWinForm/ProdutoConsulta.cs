using System;
using System.Windows.Forms;
using Amcom.TesteWinForm.Base;

namespace Amcom.TesteWinForm
{
    public partial class ProdutoConsulta : BaseFormConfig
    {
        public ProdutoConsulta()
        {
            InitializeComponent();
            dataGridView1.DataSource = ProdutoContext.GetData();
        }

        private void btBusca_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = !string.IsNullOrEmpty(txtBusca.Text) ? ProdutoContext.GetData(txtBusca.Text) : ProdutoContext.GetData();

            dataGridView1.Refresh();
        }

        private void txtBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btBusca.PerformClick();
        }
    }
}