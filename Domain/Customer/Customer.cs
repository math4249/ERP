﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Customer : DB.Database
    {
        //Property
        public int CustomerID{ get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerTelephone { get; set; }
        public string CustomerZip { get; set; }
        public string CustomerTown{ get; set; }

        public Customer() { }

        public Customer(int customerID, string companyName, string contactPerson, string customerAddress, string customerTelephone, string customerZip, string customerTown)
        {
            this.CustomerID = customerID;
            this.CompanyName = companyName;
            this.ContactPerson = contactPerson;
            this.CustomerAddress = customerAddress;
            this.CustomerTelephone = customerTelephone;
            this.CustomerZip = customerZip;
            this.CustomerTown = customerTown;
        }

        public void AddCustomer(string companyName, string contactPerson, string customerAddress, string customerTelephone, string customerZip, string customerTown)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("AddCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@ContactPerson", contactPerson);
                command.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                command.Parameters.AddWithValue("@CustomerTelephone", customerTelephone);
                command.Parameters.AddWithValue("@CustomerZip", customerZip);
                command.Parameters.AddWithValue("@CustomerTown", customerTown);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void EditCustomer(int customerID, string companyName, string contactPerson, string customerAddress, string customerTelephone, string customerZip, string customerTown)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("EditCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerID);
                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@ContactPerson", contactPerson);
                command.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                command.Parameters.AddWithValue("@CustomerTelephone", customerTelephone);
                command.Parameters.AddWithValue("@CustomerZip", customerZip);
                command.Parameters.AddWithValue("@CustomerTown", customerTown);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void DeleteCustomer(int customerID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("DeleteCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerID", customerID);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("ShowCustomers", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int customerID = int.Parse(reader["CustomerID"].ToString());
                        string companyName = reader["CompanyName"].ToString();
                        string contactPerson = reader["ContactPerson"].ToString();
                        string customerAddress = reader["CustomerAddress"].ToString();
                        string customerTelephone = reader["CustomerTelephone"].ToString();
                        string customerZip = reader["CustomerZip"].ToString();
                        string customerTown = reader["CustomerTown"].ToString();
                        customers.Add(new Customer(customerID, companyName, contactPerson, customerAddress, customerTelephone, customerZip, customerTown));
                    }
                }
            }

            return customers;
        }

        public List<Customer> GetSpecificCustomers(string companyName, string contactPerson, string customerAddress, string customerTelephone, string customerZip, string customerTown)
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("ShowSpecificCustomers", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CompanyName", companyName);
                command.Parameters.AddWithValue("@ContactPerson", contactPerson);
                command.Parameters.AddWithValue("@CustomerAddress", customerAddress);
                command.Parameters.AddWithValue("@CustomerTelephone", customerTelephone);
                command.Parameters.AddWithValue("@CustomerZip", customerZip);
                command.Parameters.AddWithValue("@CustomerTown", customerTown);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int _customerID = int.Parse(reader["CustomerID"].ToString());
                        string _companyName = reader["CompanyName"].ToString();
                        string _contactPerson = reader["contactPerson"].ToString();
                        string _customerAddress = reader["CustomerAddress"].ToString();
                        string _customerTelephone = reader["CustomerTelephone"].ToString();
                        string _customerZip = reader["CustomerZip"].ToString();
                        string _customerTown = reader["CustomerTown"].ToString();
                        customers.Add(new Customer(_customerID, _companyName, _contactPerson, _customerAddress, _customerTelephone, _customerZip, _customerTown));
                    }
                }
            }

            return customers;
        }

        public Customer GetCustomer(int customerID)
        {
            Customer customer = new Customer();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("ShowCustomer", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@customerID", customerID);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int _customerID = int.Parse(reader["CustomerID"].ToString());
                        string _companyName = reader["CompanyName"].ToString();
                        string _contactPerson = reader["contactPerson"].ToString();
                        string _customerAddress = reader["CustomerAddress"].ToString();
                        string _customerTelephone = reader["CustomerTelephone"].ToString();
                        string _customerZip = reader["CustomerZip"].ToString();
                        string _customerTown = reader["CustomerTown"].ToString();
                        customer = new Customer(_customerID, _companyName, _contactPerson, _customerAddress, _customerTelephone, _customerZip, _customerTown);
                    }
                }
            }

            return customer;
        }
    }
}

