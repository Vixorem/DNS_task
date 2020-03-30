using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    public class Position
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        public Position(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Position()
        { }
    }
}
