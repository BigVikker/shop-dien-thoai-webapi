namespace Models.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ShopDienThoaiAPI : DbContext
    {
        public ShopDienThoaiAPI()
            : base("name=ShopDienThoaiAPI")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<ADMIN> ADMINs { get; set; }
        public virtual DbSet<BRAND> BRANDs { get; set; }
        public virtual DbSet<CONFIGURATION> CONFIGURATIONs { get; set; }
        public virtual DbSet<CUSTOMER> CUSTOMERs { get; set; }
        public virtual DbSet<ORDER> ORDERs { get; set; }
        public virtual DbSet<ORDERDETAIL> ORDERDETAILs { get; set; }
        public virtual DbSet<ORDERSTATU> ORDERSTATUS { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BRAND>()
                .HasMany(e => e.PRODUCTs)
                .WithRequired(e => e.BRAND)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ORDERSTATU>()
                .HasMany(e => e.ORDERs)
                .WithOptional(e => e.ORDERSTATU)
                .HasForeignKey(e => e.OrderStatusID);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.ProductPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.PromotionPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PRODUCT>()
                .HasMany(e => e.CONFIGURATIONs)
                .WithOptional(e => e.PRODUCT)
                .WillCascadeOnDelete();
        }

        public void FixEfProviderServicesProblem()
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
