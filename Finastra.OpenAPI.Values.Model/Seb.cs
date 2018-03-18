using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finastra.OpenAPI.Values.Model
{
    public class Seb
    {
        public Seb(int id)
        {
            Id = id;
        }
        public int Id { get; }
        public string FirstName { get; set; } = "Seb";
        public string SecondName { get; set; } = "Bacquet";
    }
}
