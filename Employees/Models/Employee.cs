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
        public int? Id { get; set; }

        [Display(Name = "Имя")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(50)]
        public string Secondname { get; set; }

        [Display(Name = "Фамилия")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Display(Name = "Руководитель")]
        public int? BossId { get; set; }

        [Display(Name = "Должность")]
        public int? PositionId { get; set; }

        [Display(Name = "Отдел")]
        public int? DepartmentId { get; set; }

        [ForeignKey("BossId")]
        [Display(Name = "Руководитель")]
        public Employee Boss { get; set; }

        [ForeignKey("PositionId")]
        [Display(Name = "Должность")]
        public Position Position { get; set; }

        [ForeignKey("DepartmentId")]
        [Display(Name = "Отдел")]
        public Department Department { get; set; }

        [Display(Name = "Дата трудоустройства")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime RecruitDate { get; set; }
    }
}
