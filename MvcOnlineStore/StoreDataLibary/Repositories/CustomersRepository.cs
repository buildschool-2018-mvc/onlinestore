﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildSchool.MvcSolution.OnlineStore.Models.Repositories
{
    public class CustomersRepository
    {
        string serviceIP = "192.168.40.21";
        public void Create(Customer model)
        {

            SqlConnection connection = new SqlConnection(
                "Server=" + serviceIP + ";Database=Shopping;User Id=liouli;Password = 1226;");
            var sql = "INSERT INTO Customers VALUES(@CustomerID ,@CustomerAccountNumber,@CustomerPassword,@CustomerName,@Telephone,@Address,@CustomerMail)";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@customerID", model.CustomerID);
            command.Parameters.AddWithValue("@customerAccountNumber", model.CustomerAccountNumber);
            command.Parameters.AddWithValue("@customerPassword", model.CustomerPassword);
            command.Parameters.AddWithValue("@customerName", model.CustomerName);
            command.Parameters.AddWithValue("@telephone", model.Telephone);
            command.Parameters.AddWithValue("@address", model.Address);
            command.Parameters.AddWithValue("@customerID", model.CustomerID);

            connection.Open();//連線打開
            command.ExecuteNonQuery();//執行指令
            connection.Close();//關閉結束
        }
        public void Update(Customer model)
        {
            SqlConnection connection = new SqlConnection(
                "Server=" + serviceIP + ";Database=Shopping;User Id=liouli;Password = 1226;");
            var sql = "UPRATE Customers SET @CustomerAccountNumber,@CustomerPassword,@CustomerName,@Telephone,@Address,@CustomerMail WHERE CustomerID = @id";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", model.CustomerID);
            command.Parameters.AddWithValue("@customerAccountNumber", model.CustomerAccountNumber);
            command.Parameters.AddWithValue("@customerPassword", model.CustomerPassword);
            command.Parameters.AddWithValue("@customerName", model.CustomerName);
            command.Parameters.AddWithValue("@telephone", model.Telephone);
            command.Parameters.AddWithValue("@address", model.Address);
            command.Parameters.AddWithValue("@customerID", model.CustomerID);

            connection.Open();//連線打開
            command.ExecuteNonQuery();//執行指令
            connection.Close();//關閉結束
        }
        public void Delete(Customer model)
        {
            SqlConnection connection = new SqlConnection(
                "Server=" + serviceIP + ";Database=Shopping;User Id=liouli;Password = 1226;");
            var sql = "Delete From Customers WHERE CustomerID = @id";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", model.CustomerID);
            connection.Open();//連線打開
            command.ExecuteNonQuery();//執行指令
            connection.Close();//關閉結束
        }
        public Customer FindById(string customerId)
        //單筆資料查詢
        {
            SqlConnection connection = new SqlConnection(
                "Server=" + serviceIP + ";Database=Shopping;User Id=liouli;Password = 1226;");
            var sql = "SELECT * FROM Customers WHERE CustomerID = @id";

            SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", customerId);

            var result = command.ExecuteScalar();//純量值
            //如果查詢資料是NULL的話
            //if (result == DBNull.Value)
            connection.Open();

            var reader = command.ExecuteReader();
            var customer = new Customer();

            while (reader.Read())
            {
                customer.CustomerID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CustomerID")));//每個get都是欄位序號
                customer.CustomerAccountNumber = reader.GetValue(reader.GetOrdinal("CustomerAccountNumber")).ToString();
                customer.CustomerPassword = reader.GetValue(reader.GetOrdinal("CustomerPassword")).ToString();
                customer.CustomerName = reader.GetValue(reader.GetOrdinal("CustomerName")).ToString();
                customer.Telephone = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Telephone")));
                customer.Address = reader.GetValue(reader.GetOrdinal("Address")).ToString();
                customer.CustomerMail = reader.GetValue(reader.GetOrdinal("CustomerMail")).ToString();

            }

            reader.Close();

            return customer;
        }
        public IEnumerable<Customer> GetAll()
        {
            SqlConnection connection = new SqlConnection(
                "Server=" + serviceIP + ";Database=Shopping;User Id=liouli;Password = 1226;");

            var sql = "SELECT * FROM Customers";

            SqlCommand command = new SqlCommand(sql, connection);
            connection.Open();

            var reader = command.ExecuteReader();
            var customers = new List<Customer>();

            while (reader.Read())
            {
                var customer = new Customer();
                customer.CustomerID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CustomerID")));
                customer.CustomerAccountNumber = reader.GetValue(reader.GetOrdinal("CustomerAccountNumber")).ToString();
                customer.CustomerPassword = reader.GetValue(reader.GetOrdinal("CustomerPassword")).ToString();
                customer.CustomerName = reader.GetValue(reader.GetOrdinal("CustomerName")).ToString();
                customer.Telephone = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Telephone")));
                customer.Address = reader.GetValue(reader.GetOrdinal("Address")).ToString();
                customer.CustomerMail = reader.GetValue(reader.GetOrdinal("CustomerMail")).ToString();
            }

            reader.Close();

            return customers;
        }
    }
}
