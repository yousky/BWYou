using BWYou.Web.MVC.Models;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BWYou.Web.MVC.DAOs
{
    public class BWIdentityDbContext<TUser> : IdentityDbContext<TUser> where TUser : Microsoft.AspNet.Identity.EntityFramework.IdentityUser
    {
        public ILog logger = LogManager.GetLogger(typeof(BWIdentityDbContext<TUser>));

        public BWIdentityDbContext()
            : this("DefaultConnection", throwIfV1Schema: false)
        {
#if DEBUG
            this.Database.Log += m => System.Diagnostics.Debug.WriteLine(m);
#endif
        }

        public BWIdentityDbContext(string nameOrConnectionString, bool throwIfV1Schema)
            : base(nameOrConnectionString, throwIfV1Schema)
        {
#if DEBUG
            this.Database.Log += m => System.Diagnostics.Debug.WriteLine(m);
#endif
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

        private void ChangeCurrentDT()
        {
            int cntAdded = 0;
            int cntDeleted = 0;
            int cntModified = 0;
            foreach (var entry in ChangeTracker.Entries().Where(p => (p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified)))
            {
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

                if (typeof(BWModel).IsAssignableFrom(entry.Entity.GetType()))
                {
                    DateTime dtCur = DateTime.Now;
                    if (entry.State == EntityState.Added)
                    {
                        ((BWModel)entry.Entity).CreateDT = dtCur;
                    }
                    ((BWModel)entry.Entity).UpdateDT = dtCur;
                }
            }

            logger.Info(string.Format("ChangeEntity Count : Added={0}, Deleted={1}, Modified={2}", cntAdded, cntDeleted, cntModified));
        }

    }
}
