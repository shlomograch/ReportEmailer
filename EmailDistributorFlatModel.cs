namespace AutomationHelpers.Utilities.Emailer
{
    using System.Data.Entity;

    public partial class EmailDistributorFlatModel : DbContext
    {
        public EmailDistributorFlatModel()
            : base("name=EmailDistributorFlatModel")
        {
        }

        public virtual DbSet<EmailDistributorFlat> EmailDistributorFlats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmailDistributorFlat>()
                .Property(e => e.ApplicationName)
                .IsUnicode(false);
        }
    }
}
