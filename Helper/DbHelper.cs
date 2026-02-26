using System.Data;
using Microsoft.Data.SqlClient;

namespace ERPAPP.Helper
{
    public class DbHelper
    {
        private readonly string _conStr;

        public DbHelper(IConfiguration configuration)
        {
            _conStr = configuration.GetConnectionString("DefaultConnection");
        }

        public DataTable GetDataTable(string spName, SqlParameter[]? parameters = null, bool isStoredProcedure = true)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_conStr))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public int ExecuteNonQuery(string spName, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = new SqlConnection(_conStr);
            using SqlCommand cmd = new SqlCommand(spName, con);

            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string spName, SqlParameter[]? parameters = null)
        {
            using (SqlConnection con = new SqlConnection(_conStr))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteScalar();
            }
        }


        public DataSet GetDataSet(string spName, SqlParameter[]? parameters = null)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(_conStr))
            using (SqlCommand cmd = new SqlCommand(spName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(ds);
                }
            }

            return ds;
        }
    }
}
