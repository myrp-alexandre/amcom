using System;
using System.Windows.Forms;
using Amcom.TesteWinForm.Base;

namespace Amcom.TesteWinForm
{
    public partial class ClienteConsulta : BaseFormConfig
    {
        public ClienteConsulta()
        {
            InitializeComponent();
            dataGridView1.DataSource = ClienteContext.GetData();
        }

        private void btBusca_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = !string.IsNullOrEmpty(txtBusca.Text) ? ClienteContext.GetData(txtBusca.Text) : ClienteContext.GetData();

            dataGridView1.Refresh();
        }

        private void txtBusca_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btBusca.PerformClick();
        }
    }
}