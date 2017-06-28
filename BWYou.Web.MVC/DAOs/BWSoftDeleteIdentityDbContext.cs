using BWYou.Web.MVC.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BWYou.Web.MVC.Extensions;

namespace BWYou.Web.MVC.DAOs
{
    public class BWSoftDeleteIdentityDbContext<TUser> : BWIdentityDbContext<TUser> where TUser : IdentityUser
    {
        public BWSoftDeleteIdentityDbContext()
            : this("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public BWSoftDeleteIdentityDbContext(string nameOrConnectionString, bool throwIfV1Schema)
            : base(nameOrConnectionString, throwIfV1Schema)
        {

        }

        public override int SaveChanges()
        {
            ChangeDeleteToSoftDelete();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            ChangeDeleteToSoftDelete();

            return base.SaveChangesAsync();
        }

        private void ChangeDeleteToSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries().Where(p => p.State == EntityState.Deleted))
            {
                if (entry.Entity.IsSubclassOfRawGeneric(typeof(SoftDeleteModel<>)))
                {
                    SoftDelete(entry);
                }
            }
        }

        private void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();

            string tableName = GetTableName(entryEntityType);
            string primaryKeyName = GetPrimaryKeyName(entryEntityType);

            string sql =
                string.Format(
                    "UPDATE {0} SET IsDeleted = 1, UpdateDT = getdate() WHERE {1} = @id",
                        tableName, primaryKeyName);

            Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@id", entry.OriginalValues[primaryKeyName])); //MSSQL 전용 식이 되 버린.. ~_~;;

            // prevent hard delete            
            entry.State = EntityState.Detached;
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return string.Format("[{0}].[{1}]",
                es.MetadataProperties["Schema"].Value,
                es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return es.ElementType.KeyMembers[0].Name;
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace
                                .GetItemCollection(DataSpace.SSpace)
                                .GetItems<EntityContainer>()
                                .SelectMany(c => c.BaseEntitySets
                                                .Where(e => e.Name == typeName))
                                .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }
    }
}
