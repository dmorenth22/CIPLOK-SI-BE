using Microsoft.EntityFrameworkCore;

namespace CIPLOK_SI_BE.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
        : base(options)
        {
        }

        public DbSet<Models.Master.TBL_MSUSERS> TBL_MSUSERS { get; set; }

        public DbSet<Models.Master.TBL_MSROLES> TBL_MSROLES { get; set; }

        public DbSet<Models.Master.TBL_MSMAJELIS> TBL_MSMAJELIS { get; set; }

        public DbSet<Models.Master.TBL_MSROOM> TBL_MSROOM { get; set; }

        public DbSet<Models.Master.TBL_MSACTIVITY> TBL_MSACTIVITY { get; set; }

        public DbSet<Models.Master.TBL_MSCRITERIA> TBL_MSCRITERIA { get; set; }

        public DbSet<Models.Master.TBL_MSSUBCRITERIA> TBL_MSSUBCRITERIA { get; set; }

        public DbSet<Models.Master.TBL_SETTINGS> TBL_SETTINGS { get; set; }

        public DbSet<Models.Transaction.TBL_TR_HEADER_RESERVATION> TBL_TR_HEADER_RESERVATION { get; set; }

        public DbSet<Models.Transaction.TBL_TR_DETAIL_RESERVATION> TBL_TR_DETAIL_RESERVATION { get; set; }
    }
}
