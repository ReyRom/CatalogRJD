﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogRJD.Library.DB
{
    public class DAL
    {
        public string ConnectionString { get; set; }
        public SqlConnection SqlConnection { get; set; }

        public DAL(string connectionString) 
        {
            ConnectionString = connectionString;
            SqlConnection = new SqlConnection(ConnectionString);
        }
        
        public void Open()
        {
            SqlConnection.Open();
        }
        public void Close()
        {
            SqlConnection.Close();
        }

        public List<Product> GetProducts(int startIndex=0, int count=5)
        {
            List<Product> products = new List<Product>();
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();
            
            string query = "SELECT SKIP " + startIndex + " TOP " + count + " * FROM [MTR] LEFT JOIN [OKPD_2] ON [MTR].[ОКПД2] = [OKPD_2].[OKPD2] LEFT JOIN [ED_IZM] ON [MTR].[Базисная Единица измерения] = [ED_IZM].[Код ЕИ]";

            SqlCommand cmd = new SqlCommand(query, SqlConnection);

            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Product product = new Product();
                product.Id = (string)reader["код СКМТР"];
                product.Name = (string)reader["Наименование"];
                product.Mark = (string)reader["Маркировка"];
                //product.Reglaments = (string)reader["Регламенты (ГОСТ/ТУ)"];
                product.Parameters = (string)reader["Параметры"];
                product.MeasureUnit = (string)reader["ED_IZM.Наименование"];
                //product.MeasureUnitId = (string)reader["Базисная Единица измерения"];
                //product.Okpd2 = (string)reader["ОКПД2"];
                product.Okpd2Name = (string)reader["OKPD2_NAME"];
                products.Add(product);
            }
            return products;
        }

        public bool UpdateGroup(string productId, string group)
        {
            if (SqlConnection.State == System.Data.ConnectionState.Closed) SqlConnection.Open();

            string query = "UPDATE [MTR] SET [Группа] = " + group + " WHERE [код СКМТР] = " + productId; 

            SqlCommand command = new SqlCommand(query, SqlConnection);

            return command.ExecuteNonQuery() != 0;
        }
    }
}
