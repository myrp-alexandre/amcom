using System;
using System.Windows.Forms;
using Amcom.TesteWinForm.Base;
using Amcom.TesteWinForm.DataAccess.Entities;
using Amcom.TesteWinForm.DataAccess.Model;

namespace Amcom.TesteWinForm
{
    public partial class VendasForm : BaseFormConfig
    {
        public VendasForm()
        {
            InitializeComponent();

            SetInterfaceVenda();
        }

        private Produto ProdutoItemWork { get; set; }

        private VendaModel VendaModel { get; set; }

        private void SetInterfaceVenda()
        {
            VendaModel = new VendaModel(VendasContext.GetNewId());

            clientComboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            clientComboBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            var sourceClientNames = new AutoCompleteStringCollection();

            foreach (var clienteName in ClienteContext.GetClientNames())
                sourceClientNames.Add(clienteName);

            clientComboBox.AutoCompleteCustomSource = sourceClientNames;

            txtIdVenda.Text = VendaModel.ToString();
            lblDataVenda.Text = VendaModel.DataFormated();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                try
                {
                    var id = 0;
                    int.TryParse(txtCodProduto.Text, out id);

                    if (id > 0)
                    {
                        ProdutoItemWork = ProdutoContext.GetById(id);

                        if (ProdutoItemWork != null)
                            txtNomeProduto.Text = ProdutoItemWork.Nome;
                        else
                            throw new Exception("Produto não encontrado");
                    }
                    else
                    {
                        throw new Exception("Código do Produto inválido");
                    }

                    txtQuantidade.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void btAdicionar_Click(object sender, EventArgs e)
        {
            var produtoItem = new VendaProdutoModel();
            var qtd = 0;

            int.TryParse(txtQuantidade.Text, out qtd);

            if (qtd > 0)
            {
                produtoItem.IdVenda = VendaModel.VendaId;
                produtoItem.ProdutoId = ProdutoItemWork.Id;
                produtoItem.Quantidade = qtd;
                produtoItem.Nome = ProdutoItemWork.Nome;
                produtoItem.Subtotal = qtd * ProdutoItemWork.Preco;
                VendaModel.Produtos.Add(produtoItem);

                UpdateFieldsAndRefreshDataGrid();
            }
        }

        private void UpdateFieldsAndRefreshDataGrid()
        {
            txtCodProduto.ResetText();
            txtNomeProduto.ResetText();
            txtQuantidade.ResetText();
            ProdutoItemWork = null;


            var bs = new BindingSource {DataSource = VendaModel.Produtos};
            dataGridView1.DataSource = bs;

            dataGridView1.Refresh();

            bs.ResetBindings(false);

            lblTotal.Text = VendaModel.Total.ToString("C");

            txtCodProduto.Focus();
        }

        private void btFinalizar_Click(object sender, EventArgs e)
        {
            try
            {
                if (VendasContext.Gravar(VendaModel))
                {
                    MessageBox.Show("Venda finalizada com sucesso!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Erro ao gravar venda.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gravar venda.\n\nErro reportado : " + ex.Message);
            }
        }

        private void clientComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                VendaModel.NomeCliente = clientComboBox.SelectedText;
                txtCodProduto.Focus();
            }
        }

        private void txtQuantidade_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) btAdicionar.PerformClick();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1 != null && e.ColumnIndex == dataGridView1.Columns["Remove"].Index)
            {
                var idItem = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                var item = VendaModel.Produtos.Find(a => a.Id == idItem);
                VendaModel.Produtos.Remove(item);

                UpdateFieldsAndRefreshDataGrid();
            }
        }
    }
}