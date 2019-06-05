using System.Configuration;
using System.Windows.Forms;
using Amcom.TesteWinForm.DataAccess;

namespace Amcom.TesteWinForm.Base
{
    public partial class BaseFormConfig : Form
    {
        protected static string Conn = ConfigurationManager.ConnectionStrings["SqLiteDatabaseConn"].ToString();
        protected ClienteContext ClienteContext;
        protected ProdutoContext ProdutoContext;
        protected VendasContext VendasContext;

        public BaseFormConfig()
        {
            ProdutoContext = new ProdutoContext(Conn);
            ClienteContext = new ClienteContext(Conn);
            VendasContext = new VendasContext(Conn);
            InitializeComponent();
        }

    }
}