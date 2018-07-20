namespace AutomationHelpers.Utilities.Emailer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("EmailDistributorFlat")]
    public partial class EmailDistributorFlat
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string ApplicationName { get; set; }

        [Column(Order = 0)]
        [StringLength(50)]
        public string Name { get; set; }

        [Column(Order = 1)]
        [StringLength(100)]
        public string Email { get; set; }
    }
}
