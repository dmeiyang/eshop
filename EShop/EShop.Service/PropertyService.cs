using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service
{
    public class PropertyService : BasicService
    {
        public Data.DomainModels.Property QueryDetail(string id)
        {
            return db.Propertys.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Data.DomainModels.Property> QueryList(string key, int top = 10)
        {
            var entitys = db.Propertys.AsQueryable();

            if (!string.IsNullOrEmpty(key))
            {
                entitys = entitys.Where(x => x.DisplayName.Contains(key));
            }

            if (top > 0)
            {
                return entitys.OrderBy(x => x.DisplayName).Take(top);
            }
            else
            {
                return entitys.OrderBy(x => x.DisplayName);
            }
        }

        public PageList<Data.DomainModels.Property> QueryPageList(string key, int page, int size)
        {
            var entitys = db.Propertys.AsQueryable();

            if (!string.IsNullOrEmpty(key))
            {
                entitys = entitys.Where(x => x.DisplayName.Contains(key));
            }

            return entitys.OrderBy(x => x.DisplayName).ToPageList(page, size);
        }

        public bool Save(Data.DomainModels.Property model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                model.Id = IDHelper.Id32;

                db.Propertys.Add(model);

                return db.SaveChanges() > 0;
            }
            else
            {
                var entity = db.Propertys.FirstOrDefault(x => x.Id == model.Id);

                entity.DisplayName = model.DisplayName;
                entity.Name = model.Name;
                entity.Type = model.Type;
                entity.Content = model.Content;

                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;

                return db.SaveChanges() > 0;
            }
        }

        public bool Delete(string id)
        {
            db.Propertys.Remove(new Data.DomainModels.Property() { Id = id });

            return db.SaveChanges() > 0;
        }
  
    }
}
