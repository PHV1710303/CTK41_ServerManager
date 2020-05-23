using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerManagev1._0
{
    public class ProcessMSSQL
    {
        string connectionString = "";

        public ProcessMSSQL()
        {
            connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
        }

        public ProcessMSSQL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int CreateNewLogin(string username, string password)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            try
            {
                sqlCommand.CommandText = "CREATE LOGIN " + username + " WITH PASSWORD = '" + password + "';" +
                    "Create user " + username + " for login " + username;
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return -1;
            }

            catch (Exception)
            {
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            return 1;
        }

        public int RemoveLogin(string username, string password)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();

            try
            {
                sqlCommand.CommandText = "drop user " + username + ";" +
                    "drop login " + username + ";";
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return -1;
            }

            catch (Exception)
            {
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            return 1;
        }

        public List<string> LoadListServerRoles()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            SqlDataReader dataReader;
            List<string> listServerRoles = new List<string>();

            sqlCommand.CommandText = "select name from sys.server_principals where type = 'R'";

            try
            {
                sqlConnection.Open();
                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    string serverRoleName = dataReader["name"].ToString();
                    listServerRoles.Add(serverRoleName);
                }
            }
            catch (SqlException)
            {
                return null;
            }

            catch (Exception)
            {
                return null;
            }
            finally
            {
                sqlConnection.Close();
            }
            return listServerRoles;
        }

        public int GrantPermissionAccount(string username, string permissions)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ALTER SERVER ROLE " + permissions + " ADD MEMBER " + username + ";";

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return -1;
            }

            catch (Exception)
            {
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            return 1;
        }

        public int DenyPermissionAccount(string username, string permissions)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "ALTER SERVER ROLE " + permissions + " DROP MEMBER " + username + ";";

            try
            {
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                return -1;
            }

            catch (Exception)
            {
                return -1;
            }
            finally
            {
                sqlConnection.Close();
            }
            return 1;
        }
    }
}
