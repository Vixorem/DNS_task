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

        public void DeleteEmployee(int id)
        {
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("DeleteEmployee", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public int EmployeesCount()
        {
            int count = 0;
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("EmployeesCount", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
            }

            return count;
        }

        /*Returns updated row*/
        public Employee UpdateEmployee(Employee e)
        {
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("UpdateEmployee", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", (e.Id == null) ? ((object)DBNull.Value) : (e.Id));
                cmd.Parameters.AddWithValue("@Name", e.Name);
                cmd.Parameters.AddWithValue("@Secondname", e.Secondname);
                cmd.Parameters.AddWithValue("@Surname", e.Surname);
                cmd.Parameters.AddWithValue("@BossId", (e.BossId == null) ? ((object)DBNull.Value) : (e.BossId));
                cmd.Parameters.AddWithValue("@PosId", e.PositionId);
                cmd.Parameters.AddWithValue("@DepId", e.DepartmentId);
                cmd.Parameters.AddWithValue("@Rdate", e.RecruitDate);
                cmd.ExecuteNonQuery();

            }

            return e;
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
                var cmd = new SqlCommand("FetchEmployeeById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    e = new Employee()
                    {
                        Id = reader.GetInt32(0),
                        Name = (string)reader.GetString(1),
                        Secondname = reader.GetString(2),
                        Surname = reader.GetString(3),
                        BossId = (Convert.IsDBNull(reader.GetValue(4))) ? (null) : ((int?)reader.GetValue(4)),
                        Boss = new Employee
                        {
                            //Id = (int)reader.GetValue(4),
                            Surname = reader.GetString(5)
                        },
                        PositionId = reader.GetInt32(6),
                        Position = new Position
                        {
                            Id = reader.GetInt32(6),
                            Name = reader.GetString(7)
                        },
                        DepartmentId = reader.GetInt32(8),
                        Department = new Department
                        {
                            Id = reader.GetInt32(8),
                            Name = reader.GetString(9)
                        },
                        RecruitDate = reader.GetDateTime(10)
                    };
                }
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
                var cmd = new SqlCommand("FetchDepartmentById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    d = new Department()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }
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
                var cmd = new SqlCommand("FetchPositionById", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    p = new Position()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                    };
                }
            }
            return p;
        }

        public IList<Employee> FetchAllEmployees()
        {
            int cnt = 1;
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("EmployeesCount", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cnt = reader.GetInt32(0);
                }
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
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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
            if (fetchNum == 0)
            {
                return employees;
            }
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchEmployeesRange", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FetchNum", fetchNum);
                cmd.Parameters.AddWithValue("@PageNum", pageNum);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(
                        new Employee()
                        {
                            Id = reader.GetInt32(0),
                            Name = (string)reader.GetString(1),
                            Secondname = reader.GetString(2),
                            Surname = reader.GetString(3),
                            BossId = (Convert.IsDBNull(reader.GetValue(4))) ? (null) : ((int?)reader.GetValue(4)),
                            Boss = new Employee
                            {
                                //Id = (int)reader.GetValue(4),
                                Surname = reader.GetString(5)
                            },
                            PositionId = reader.GetInt32(6),
                            Position = new Position
                            {
                                Id = reader.GetInt32(6),
                                Name = reader.GetString(7)
                            },
                            DepartmentId = reader.GetInt32(8),
                            Department = new Department
                            {
                                Id = reader.GetInt32(8),
                                Name = reader.GetString(9)
                            },
                            RecruitDate = reader.GetDateTime(10)
                        });
                }
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
                    c.Parameters.AddWithValue("@Name", "Бухгалтерия");
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

        // Не очень хороший вариант,
        // т.к. если работать с бд будут много пользователей, то могут быть косяки
        public Employee FetchLastEmployee()
        {
            Employee e = null;

            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("FetchLastEmployee", connect)
                {
                    CommandType = CommandType.StoredProcedure
                };
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    e = new Employee()
                    {
                        Id = reader.GetInt32(0),
                        Name = (string)reader.GetString(1),
                        Secondname = reader.GetString(2),
                        Surname = reader.GetString(3),
                        BossId = (Convert.IsDBNull(reader.GetValue(4))) ? (null) : ((int?)reader.GetValue(4)),
                        Boss = new Employee
                        {
                            //Id = (int)reader.GetValue(4),
                            Surname = reader.GetString(5)
                        },
                        PositionId = reader.GetInt32(6),
                        Position = new Position
                        {
                            Id = reader.GetInt32(6),
                            Name = reader.GetString(7)
                        },
                        DepartmentId = reader.GetInt32(8),
                        Department = new Department
                        {
                            Id = reader.GetInt32(8),
                            Name = reader.GetString(9)
                        },
                        RecruitDate = reader.GetDateTime(10)
                    };
                }
            }

            return e;
        }

        public void AddEmployee(Employee e)
        {
            using (var connect = new SqlConnection(connectionStr))
            {
                connect.Open();
                var cmd = new SqlCommand("AddEmployee", connect) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Name", e.Name);
                cmd.Parameters.AddWithValue("@Secondname", e.Secondname);
                cmd.Parameters.AddWithValue("@Surname", e.Surname);
                cmd.Parameters.AddWithValue("@BossId", e.BossId ?? Convert.DBNull).IsNullable = true;
                cmd.Parameters.AddWithValue("@PosId", e.PositionId);
                cmd.Parameters.AddWithValue("@DepId", e.DepartmentId);
                cmd.Parameters.AddWithValue("@Rdate", e.RecruitDate);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
