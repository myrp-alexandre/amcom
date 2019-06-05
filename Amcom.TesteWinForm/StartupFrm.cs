using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Forms;
using Amcom.TesteWinForm.Base;
using Amcom.TesteWinForm.Common.Extension;
using Amcom.TesteWinForm.DataAccess.Entities;

namespace Amcom.TesteWinForm
{
    public partial class StartupFrm : BaseFormConfig
    {
        public StartupFrm()
        {
            InitializeComponent();
        }

        private void consultaClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmClienteConsulta = new ClienteConsulta();
            frmClienteConsulta.Show();
        }

        private void importarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Selecionar arquivo CSV";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "CSV Files|*.CSV";
            openFileDialog1.CheckFileExists = false;
            openFileDialog1.CheckPathExists = false;
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;

            var dialogResult = openFileDialog1.ShowDialog();

            if (dialogResult == DialogResult.OK)
                foreach (var arquivo in openFileDialog1.FileNames)
                    try
                    {
                        var listaProduto = new List<Produto>();

                        var fileReader = new StreamReader(arquivo);
                        string line = null;
                        string[] prod = null;
                        var cabecalho = fileReader.ReadLine();

                        while ((line = fileReader.ReadLine()) != null)
                        {
                            prod = line.Split(';');

                            if (prod.Length < 3)
                                continue;

                            listaProduto.Add(new Produto
                                {Id = prod[0].ToInt(), Nome = prod[1], Preco = prod[2].ToDecimal()});
                        }

                        fileReader.Close();

                        var total = ProdutoContext.GravaProdutosImportados(listaProduto);

                        var textmessage = total > 1
                            ? " produtos importados com sucesso."
                            : " produto importado com sucesso.";

                        MessageBox.Show(string.Format("{0}{1}", total, textmessage));
                    }
                    catch (SecurityException ex)
                    {
                        MessageBox.Show(string.Format("Mensagem : {0}\n\n" + "Detalhes :\n\n{1}", ex.Message, ex.StackTrace));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format("Não foi possível carregar o arquivo: {0}\n\nErro reportado : {1}", arquivo.Substring(arquivo.LastIndexOf('\\')), ex.Message));
                    }
        }

        private void cadastroClienteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var formCadastroCliente = new ClienteCadastro();
            formCadastroCliente.Show();
        }

        private void consultaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmProdConsulta = new ProdutoConsulta();
            frmProdConsulta.Show();
        }

        private void cadastroVendaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmVendas = new VendasForm();
            frmVendas.Show();
        }
    }
}