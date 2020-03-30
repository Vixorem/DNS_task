using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Display(Name = "Имя")]
        [StringLength(50)]
        public string Name { get; set; }
        [Display(Name = "Отчество")]
        [StringLength(50)]
        public string Secondname { get; set; }
        [Display(Name = "Фамилия")]
        [StringLength(50)]
        public string Surname { get; set; }
        public int? BossId { get; set; }
        public int? PositionId { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("BossId")]
        [Display(Name = "Руководитель")]
        public Employee Boss { get; set; }
        [ForeignKey("PositionId")]
        [Display(Name = "Должность")]
        public Position Position { get; set; }
        [Display(Name = "Отдел")]
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public Employee()
        { }

        public Employee(int id, string name, string secondname, string surname, 
            Employee boss, Position pos, Department dep)
        {
            Id = id;
            Name = name;
            Secondname = secondname;
            Surname = surname;
            Boss = boss;
            Position = pos;
            Department = dep;
        }
    }
}
