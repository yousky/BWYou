using BWYou.Web.MVC.Models;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public class BWIdentityDbContext<TUser> : IdentityDbContext<TUser> where TUser : IdentityUser
    {
        public ILog logger = LogManager.GetLogger(typeof(BWIdentityDbContext<TUser>));

        public BWIdentityDbContext()
            : this("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public BWIdentityDbContext(string nameOrConnectionString, bool throwIfV1Schema)
            : base(nameOrConnectionString, throwIfV1Schema)
        {

        }

        public override int SaveChanges()
        {
            ChangeCurrentDT();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            ChangeCurrentDT();

            return base.SaveChangesAsync();
        }

        protected void ChangeCurrentDT()
        {
            int cntAdded = 0;
            int cntDeleted = 0;
            int cntModified = 0;
            foreach (var entry in ChangeTracker.Entries().Where(p => (p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified)))
            {
                logger.Debug(string.Format("ChangeEntity : {0} {1}", entry.Entity.ToString(), entry.State.ToString()));
                if (entry.State == EntityState.Modified)
                {
                    cntModified++;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    cntDeleted++;
                }
                else
                {
                    cntAdded++;
                }

                if (typeof(ICUModel).IsAssignableFrom(entry.Entity.GetType()))
                {
                    Type t = typeof(int?);
                    DateTime dtCur = DateTime.Now;
                    if (entry.State == EntityState.Added)
                    {
                        ((ICUModel)entry.Entity).CreateDT = dtCur;
                    }
                    if (entry.State != EntityState.Deleted)
                    {
                        ((ICUModel)entry.Entity).UpdateDT = dtCur;
                    }
                }
            }

            logger.Debug(string.Format("ChangeEntity Count : Added={0}, Deleted={1}, Modified={2}", cntAdded, cntDeleted, cntModified));
        }

    }
}
