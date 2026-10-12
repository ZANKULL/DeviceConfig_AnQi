using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;

namespace DeviceConfig
{

    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnectionStringBuilder builder =
            new SqlConnectionStringBuilder(GetConnectionString());

            var connectionString = builder.ConnectionString;

            if (args.Length > 0)
            {
                if (!int.TryParse(args[0], out int deviceId))
                {
                    Console.WriteLine("Invalid Device ID.");
                    return;
                }
                else
                {
                    var sql = $"select * from dbo.DeviceConfig where DeviceId = {args[0]};";
                    DataTable dt = QueryTable(connectionString, sql);

                    if (dt.Rows.Count > 0)
                    {
                        List<int> maxColLengthList = GetColLengths(dt);
                        PrintTable(maxColLengthList, sql, connectionString);
                    }
                    else
                    {
                        Console.WriteLine("No such Device ID found.");
                    }
                }
                
            }
            else
            {
                var sql = "select * from dbo.DeviceConfig;";
                DataTable dt = QueryTable(connectionString, sql);
                List<int> maxColLengthList = GetColLengths(dt);
                PrintTable(maxColLengthList, sql, connectionString);
            }
        }

        private static string GetConnectionString()
        {
            // To avoid storing the connection string in your code,
            // you can retrieve it from a configuration file. 
            return "data source=aq\\SQLEXPRESS;TrustServerCertificate=True;initial catalog=DeviceConfigDB;trusted_connection=true";
        }

        private static DataTable QueryTable(string connectionString, string sql)
        {
            DataTable dt = new DataTable("TestTable");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dt);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return dt;

        }

        private static List<int> GetColLengths(DataTable dt)
        {
            List<int> maxColLengthList = new List<int>();
            foreach (DataColumn col in dt.Columns)
            {
                int maxLength = dt
                .AsEnumerable()
                .Select(row => row[col.ColumnName].ToString().Length)
                .DefaultIfEmpty(0).Max();
                if (col.ColumnName.Length >= maxLength)
                {
                    maxColLengthList.Add(col.ColumnName.Length);
                }
                else
                {
                    maxColLengthList.Add(maxLength);
                }
            }
            return maxColLengthList;
        }

        private static void PrintTable(List<int> maxColLengthList, string sql, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        // print header
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string s = "";
                            s += reader.GetName(i) + string.Concat(Enumerable.Repeat(" ", maxColLengthList[i] - reader.GetName(i).Length)) + " ";

                            Console.Write(s);
                        }

                        Console.WriteLine();

                        // print rows
                        while (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string s = "";
                                string value = reader.IsDBNull(i) ? "NULL" : reader[i].ToString()!;

                                s += value + string.Concat(
                                    Enumerable.Repeat(" ", Math.Max(0, maxColLengthList[i] - value.Length))
                                ) + " ";

                                Console.Write(s);
                            }
                        }
                    }
                }
            }
        }
    }
}