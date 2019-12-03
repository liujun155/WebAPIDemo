using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntitysTS
{
    class HumanEnt
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int? Sex { get; set; }
        public string Phone { get; set; }
        public int? Education { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
