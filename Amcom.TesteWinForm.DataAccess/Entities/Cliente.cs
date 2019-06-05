namespace Amcom.TesteWinForm.DataAccess.Entities
{
    public class Cliente : BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(Nome) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Telefone);
        }
    }
}