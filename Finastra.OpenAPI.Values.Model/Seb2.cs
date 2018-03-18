using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finastra.OpenAPI.Values.Model
{
    public class Seb2
    {
        public Seb2(int id)
        {
            Id = id;
        }
        public int Id { get; }
        public string FirstName { get; set; } = "Seb2";
        public string SecondName { get; set; } = "Bacquet";
    }
}
