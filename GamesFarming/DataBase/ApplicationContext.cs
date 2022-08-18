using System.Data.Entity;

namespace GamesFarming.DataBase
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public ApplicationContext() : base("DefaultConnection") { }

        public void AddAccount(Account account)
        {
            Accounts.Add(account);
            SaveChanges();
        }
    }
}
