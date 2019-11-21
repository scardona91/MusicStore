﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public bool AddNewEmployee(Employee employee)
        {
            bool addSuccess = false;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_create_employee", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);

            try
            {
                conn.Open();

                addSuccess = (1 == cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return addSuccess;
        }

        public Employee AuthenticateUser(string username, string passwordHash)
        {
            Employee result = null;

            var conn = DBConnection.GetConnection();


            var cmd = new SqlCommand("sp_authenticate_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", username);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

            try
            {
                conn.Open();

                if (1 == Convert.ToInt32(cmd.ExecuteScalar()))
                {
                    result = getUserByEmail(username);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public bool DeactivateEmployee(int employeeID, string firstName, string lastName)
        {
            bool deactivateSuccess = false;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_deactivate_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);

            try
            {
                conn.Open();
                deactivateSuccess = (1 == cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            


            return deactivateSuccess;
        }

        public bool DeleteEmployee(int employeeID, string firstName, string lastName)
        {
            bool deactivateSuccess = false;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_delete_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);

            try
            {
                conn.Open();
                deactivateSuccess = (0 < cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return deactivateSuccess;
        }

        public List<Customer> GetCustomerByActive(bool active = true)
        {
            List<Customer> users = new List<Customer>();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_get_all_customers", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", active);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new Customer();

                        user.CustomerID = reader.GetInt32(0);
                        user.FirstName = reader.GetString(1);
                        user.LastName = reader.GetString(2);
                        user.PhoneNumber = reader.GetString(3);
                        user.Email = reader.GetString(4);

                        users.Add(user);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public List<Employee> GetEmployeesByActive(bool active = true)
        {
            List<Employee> users = new List<Employee>();
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_get_all_employees", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Active", active);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new Employee();

                        user.EmployeeID = reader.GetInt32(0);
                        user.FirstName = reader.GetString(1);
                        user.LastName = reader.GetString(2);
                        user.PhoneNumber = reader.GetString(3);
                        user.Email = reader.GetString(4);

                        users.Add(user);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public bool ReActivateEmployee(int employeeID, string firstName, string lastName)
        {
            bool reactivateSuccess = false;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_reactivate_employee", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            cmd.Parameters.AddWithValue("@FirstName", firstName);
            cmd.Parameters.AddWithValue("@LastName", lastName);

            try
            {
                conn.Open();
                reactivateSuccess = (1 == cmd.ExecuteNonQuery());
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return reactivateSuccess;
        }

        public bool UpdateEmployeeInfo(Employee oldEmployee, Employee updatedEmployee)
        {
            bool updateSuccess = false;

            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_update_employee_profile", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeID", oldEmployee.EmployeeID);

            cmd.Parameters.AddWithValue("@OldLastName", oldEmployee.LastName);
            cmd.Parameters.AddWithValue("@OldEmail", oldEmployee.Email);
            cmd.Parameters.AddWithValue("@OldPhoneNumber", oldEmployee.PhoneNumber);

            cmd.Parameters.AddWithValue("@NewLastName", updatedEmployee.LastName);
            cmd.Parameters.AddWithValue("@NewEmail", updatedEmployee.Email);
            cmd.Parameters.AddWithValue("@NewPhoneNumber", updatedEmployee.PhoneNumber);

            try
            {
                conn.Open();

                updateSuccess = (1 == cmd.ExecuteNonQuery());

            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return updateSuccess;
        }

        public bool UpdatePasswordHash(int employeeID, string oldPasswordHash, string newPasswordHash)
        {
            bool updateSuccess = false;
            var conn = DBConnection.GetConnection();
            var cmd = new SqlCommand("sp_update_employee_password", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@EmployeeID", employeeID);
            //cmd.Parameters.AddWithValue("@OldPasswordHash", oldPasswordHash);
            //cmd.Parameters.AddWithValue("@NewPasswordHash", newPasswordHash);

            cmd.Parameters.Add("@EmployeeID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPasswordHash", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPasswordHash", SqlDbType.NVarChar, 100);

            cmd.Parameters["@EmployeeID"].Value = employeeID;
            cmd.Parameters["@OldPasswordHash"].Value = oldPasswordHash;
            cmd.Parameters["@NewPasswordHash"].Value = newPasswordHash;

            try
            {
                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                updateSuccess = (rows == 1);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return updateSuccess;
        }

        private Employee getUserByEmail(string email)
        {
            Employee user = null;
            var conn = DBConnection.GetConnection();
            var cmd1 = new SqlCommand("sp_retrieve_employee_by_email", conn);
            var cmd2 = new SqlCommand("sp_get_all_roles_for_employeeID", conn);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@Email", email);
            cmd2.Parameters.Add("@EmployeeID", SqlDbType.Int);

            try
            {
                conn.Open();

                var reader1 = cmd1.ExecuteReader();

                if (reader1.Read())
                {
                    user = new Employee();
                    user.EmployeeID = reader1.GetInt32(0);
                    user.FirstName = reader1.GetString(1);
                    user.LastName = reader1.GetString(2);
                    user.PhoneNumber = reader1.GetString(3);
                    user.Email = email;
                }
                else
                {
                    throw new ApplicationException("User not found");
                }
                reader1.Close();
                cmd2.Parameters["@EmployeeID"].Value = user.EmployeeID;
                var reader2 = cmd2.ExecuteReader();
                List<string> roles = new List<string>();
                while (reader2.Read())
                {
                    string role = reader2.GetString(0);
                    roles.Add(role);
                }
                user.Roles = roles;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return user;
        }
    }
}
