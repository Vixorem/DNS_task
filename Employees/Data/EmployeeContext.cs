using System.Data;
using System.Data.SqlClient;

using Employees.Models;
using System.Collections.Generic;
using System;

namespace Employees.Data
{

    public class EmployeeContext
    {
        private string connectionStr = "Server='.';Database=empdb;Trusted_Connection=True;MultipleActiveResultSets=true";
        public EmployeeContext()
        {
            FirstInit();
        }

        public Employee FetchEmployeeById(int? id)
        {
            Employee e = null;
            if (id == null)
            {
                return null;
            }

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchEmployeeById", connect);
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                e = new Employee()
                {
                    Id = (int)reader.GetValue(0),
                    Name = (string)reader.GetValue(1),
                    Secondname = (string)reader.GetValue(2),
                    Surname = (string)reader.GetValue(3),
                    BossId = (int)reader.GetValue(4),
                    PositionId = (int)reader.GetValue(5),
                    DepartmentId = (int)reader.GetValue(6)
                };
            }
            return e;
        }

        public Department FetchDepartmentById(int? id)
        {
            Department d = null;
            if (id == null)
            {
                return null;
            }

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchDepartmentById", connect);
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                d = new Department()
                {
                    Id = (int)reader.GetValue(0),
                    Name = (string)reader.GetValue(1),
                };
            }
            return d;
        }

        public Position FetchPositionById(int? id)
        {
            Position p = null;
            if (id == null)
            {
                return null;
            }

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchDepartmentById", connect);
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                p = new Position()
                {
                    Id = (int)reader.GetValue(0),
                    Name = (string)reader.GetValue(1),
                };
            }
            return p;
        }

        public IList<Employee> FetchAllEmployees()
        {
            int cnt = 0;
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("EmployeesCount", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                reader.Read();
                cnt = (int)reader.GetValue(0);
            }

            return FetchEmployeesRange(cnt, 1);
        }

        public IList<Department> FetchAllDepartments()
        {
            List<Department> departments = new List<Department>();

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchAllDepartments", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(
                        new Department()
                        {
                            Id = (int)reader.GetValue(0),
                            Name = (string)reader.GetValue(1)
                        });
                }
            }

            return departments;
        }

        public IList<Position> FetchAllPositions()
        {
            List<Position> positions = new List<Position>();

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchAllPositions", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    positions.Add(
                        new Position()
                        {
                            Id = (int)reader.GetValue(0),
                            Name = (string)reader.GetValue(1)
                        });
                }
            }

            return positions;
        }

        public bool EmployeeExists(int id)
        {
            return (FetchEmployeeById(id) == null) ? (false) : (true);
        }

        public IList<Employee> FetchEmployeesRange(int fetchNum, int pageNum)
        {
            IList<Employee> employees = new List<Employee>();
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var empcmd = new SqlCommand("FetchEmployeesRange", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                empcmd.Parameters.AddWithValue("@FetchNum", fetchNum);
                empcmd.Parameters.AddWithValue("@PageNum", pageNum);
                var reader = empcmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(
                        new Employee()
                        {
                            Id = (int)reader.GetValue(0),
                            Name = (string)reader.GetValue(1),
                            Secondname = (string)reader.GetValue(2),
                            Surname = (string)reader.GetValue(3),
                            BossId = (int)reader.GetValue(4),
                            PositionId = (int)reader.GetValue(5),
                            DepartmentId = (int)reader.GetValue(6)
                        });
                }
            }

            foreach (var e in employees)
            {
                e.Boss = FetchEmployeeById(e.BossId);
                e.Position = FetchPositionById(e.PositionId);
                e.Department = FetchDepartmentById(e.DepartmentId);
            }

            return employees;
        }

        private void FirstInit()
        {
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                // returns count(*) only
                var cmd = new SqlCommand("DepartmentsCount", connect) { CommandType = CommandType.StoredProcedure };
                var reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.GetValue(0).ToString() == "0")
                {
                    var c = new SqlCommand("AddDepartment", connect) { CommandType = CommandType.StoredProcedure };
                    c.Parameters.AddWithValue("@Name", "Продажа");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Разработка");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Техническое обслуживание");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Кадры");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Маркетинг");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Логистика");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Закупка");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    //------
                    c = new SqlCommand("AddPosition", connect) { CommandType = CommandType.StoredProcedure };
                    c.Parameters.AddWithValue("@Name", "Младший программист");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Программист");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Старший программист");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Руководитель отдела");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Менеджер");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Старший менеджер");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Младший менеджер");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Специалист по кадрам");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Директор");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Бухгалтер");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Главный бухгалтер");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Помощник бухгалтера");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Финансовый директор");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                    c.Parameters.AddWithValue("@Name", "Специалист по закупке");
                    c.ExecuteNonQuery();
                    c.Parameters.Clear();
                }
            }
        }

        public void AddEmployee(Employee e)
        {
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("AddEmployee") { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Name", e.Name);
                cmd.Parameters.AddWithValue("@Secondname", e.Secondname);
                cmd.Parameters.AddWithValue("@Surname", e.Surname);
                cmd.Parameters.AddWithValue("@BossId", e.Boss.Id);
                cmd.Parameters.AddWithValue("@PosId", e.Position.Id);
                cmd.Parameters.AddWithValue("@DepId", e.Department.Id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
