using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Data.DomainModels
{
    [Table("property")]
    public class Property
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public string Content { get; set; }

    }
}
