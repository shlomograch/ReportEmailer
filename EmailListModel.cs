namespace AutomationHelpers.Utilities.Emailer
{
    using System.Data.Entity;

    public partial class EmailListModel : DbContext
    {
        public EmailListModel()
            : base("name=EmailListModel")
        {
        }

        public virtual DbSet<EmailList> EmailLists { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
