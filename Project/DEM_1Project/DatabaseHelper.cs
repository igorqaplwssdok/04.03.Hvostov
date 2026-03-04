using System;
using System.Data.SqlClient;

public static class DatabaseHelper
{
    private static string connectionString = @"Server=localhost;Database=Dem_DB;Integrated Security=True;";

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }

    public static (bool success, string role, string fullName) CheckUser(string login, string password)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            string query = "SELECT Роль, ФИО FROM Пользователь WHERE Логин = @login AND Пароль = @password";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@login", login);
            cmd.Parameters.AddWithValue("@password", password);

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return (true, reader["Роль"].ToString(), reader["ФИО"].ToString());
                }
            }
        }
        return (false, null, null);
    }
}