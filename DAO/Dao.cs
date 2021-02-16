using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DAO
{
    public class Dao
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        public Dao(SqlConnection connection)
        {
            this.connection = connection;
        }

        public Dao(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        public bool Insert(Entity entity)
        {
            bool ret = true;
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("USP_InsertWikidump", connection, transaction);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Period", SqlDbType.DateTime).Value = entity.Period;
                cmd.Parameters.Add("@Lang", SqlDbType.VarChar).Value = entity.Language;
                cmd.Parameters.Add("@Domain", SqlDbType.VarChar).Value = entity.Domain;
                cmd.Parameters.Add("@PageTitle", SqlDbType.VarChar).Value = entity.Language;
                cmd.Parameters.Add("@ViewCount", SqlDbType.Int).Value = entity.ViewCount;
                cmd.Parameters.Add("@ResponseSize", SqlDbType.Int).Value = entity.ResponseSize;

                cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                ret = false;
                Console.Write(ex.Message);
                throw;
            }

            return ret;
        }

        public void InsertFromTable(DataTable dt)
        {
            SqlCommand cmd = null;

            try
            {
                cmd = new SqlCommand("USP_InsertWikidump", connection, transaction);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.UpdatedRowSource = UpdateRowSource.None;

                cmd.Parameters.Add("@Period", SqlDbType.DateTime, 10, dt.Columns[0].ColumnName);
                cmd.Parameters.Add("@Lang", SqlDbType.VarChar, 20, dt.Columns[1].ColumnName);
                cmd.Parameters.Add("@Domain", SqlDbType.VarChar, 20, dt.Columns[2].ColumnName);
                cmd.Parameters.Add("@PageTitle", SqlDbType.VarChar, 150, dt.Columns[3].ColumnName);
                cmd.Parameters.Add("@ViewCount", SqlDbType.Int, 4, dt.Columns[4].ColumnName);
                cmd.Parameters.Add("@ResponseSize", SqlDbType.Int, 4, dt.Columns[5].ColumnName);

                SqlDataAdapter adp = new SqlDataAdapter();
                adp.InsertCommand = cmd;
                adp.UpdateBatchSize = dt.Rows.Count;
                int records = adp.Update(dt);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
        }

        public List<Entity> GetLanguageDomainTop(DateTime dateFrom, DateTime dateTo)
        {
            List<Entity> list = new List<Entity>();
            Entity entity = null;

            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                cmd = new SqlCommand("USP_LanDomTopViewPerDay", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = dateTo;
                cmd.CommandTimeout = 120;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entity = new Entity
                    {
                        Period = (Useful.ValidateNullFromDb(dr["Period"]) != null) ? Convert.ToDateTime(dr["Period"]) : default(DateTime),
                        Language = (Useful.ValidateNullFromDb(dr["Lang"]) != null) ? Convert.ToString(dr["Lang"]) : default(string),
                        Domain = (Useful.ValidateNullFromDb(dr["Domain"]) != null) ? Convert.ToString(dr["Domain"]) : default(string),
                        ViewCount = (Useful.ValidateNullFromDb(dr["ViewCount"]) != null) ? Convert.ToInt32(dr["ViewCount"]) : default(int)
                    };

                    list.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                if (dr != null) dr.Close();
            }

            return list;
        }

        public List<Entity> GetPageTop(DateTime dateFrom, DateTime dateTo)
        {
            List<Entity> list = new List<Entity>();
            Entity entity = null;

            SqlCommand cmd = null;
            SqlDataReader dr = null;

            try
            {
                cmd = new SqlCommand("USP_PageTopViewPerDay", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@DateFrom", System.Data.SqlDbType.DateTime).Value = dateFrom;
                cmd.Parameters.Add("@DateTo", System.Data.SqlDbType.DateTime).Value = dateTo;
                cmd.CommandTimeout = 120;

                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    entity = new Entity
                    {
                        Period = (Useful.ValidateNullFromDb(dr["Period"]) != null) ? Convert.ToDateTime(dr["Period"]) : default(DateTime),
                        PageTitle = (Useful.ValidateNullFromDb(dr["PageTitle"]) != null) ? Convert.ToString(dr["PageTitle"]) : default(string),
                        ViewCount = (Useful.ValidateNullFromDb(dr["ViewCount"]) != null) ? Convert.ToInt32(dr["ViewCount"]) : default(int)
                    };

                    list.Add(entity);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
            finally
            {
                if (dr != null) dr.Close();
            }

            return list;
        }
    }
}