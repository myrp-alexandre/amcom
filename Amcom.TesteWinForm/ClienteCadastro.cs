using System;
using System.Windows.Forms;
using Amcom.TesteWinForm.Base;
using Amcom.TesteWinForm.DataAccess.Entities;

namespace Amcom.TesteWinForm
{
    public partial class ClienteCadastro : BaseFormConfig
    {
        public ClienteCadastro()
        {
            InitializeComponent();
        }

        private void CadastrarClienteClick(object sender, EventArgs e)
        {
            try
            {
                var cliente = new Cliente {Nome = txtNome.Text, Email = txtEmail.Text, Telefone = txtTelefone.Text};

                if (cliente.Validate())
                {
                    var verificaCliente = ClienteContext.GetClientId(cliente.Nome);

                    if (verificaCliente > 0)
                    {
                        MessageBox.Show("Já existe um cliente cadastrado com este nome");
                        txtNome.Focus();
                    }
                    else
                    {
                        var result = ClienteContext.Gravar(cliente);
                        MessageBox.Show("Cliente Cadastrado com Sucesso");
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Os campos não foram preenchidos corretamente");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro reportado : " + ex.Message);
            }
        }

        private void CloseFormClick(object sender, EventArgs e)
        {
            Close();
        }

        private void txtNome_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) txtEmail.Focus();
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) txtTelefone.Focus();
        }

        private void txtTelefone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btCadastrar.PerformClick();
        }
    }
}