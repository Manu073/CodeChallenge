using Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DAO;
using System.Configuration;
using System.Data;

namespace BusinessController
{
    public class BusinessController
    {
        public bool InsertWikidump(List<Entity> list)
        {
            bool result = true;
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnx"].ConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                Dao dao = new Dao(connection, transaction);
                foreach (var entity in list)
                {
                    if (!dao.Insert(entity))
                        result = false;
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public bool InsertWikidumpTab(DataTable dt)
        {
            bool result = true;
            SqlConnection connection = null;
            SqlTransaction transaction = null;

            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnx"].ConnectionString);
                connection.Open();
                transaction = connection.BeginTransaction();

                Dao dao = new Dao(connection, transaction);
                dao.InsertFromTable(dt);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }

            return result;
        }

        public List<Entity> GetLanguageDomainTop(DateTime dateFrom, DateTime dateTo)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnx"].ConnectionString);
                connection.Open();

                Dao dao = new Dao(connection);
                return dao.GetLanguageDomainTop(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error geting the language/domain list");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }

        public List<Entity> GetPageTop(DateTime dateFrom, DateTime dateTo)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(ConfigurationManager.ConnectionStrings["cnx"].ConnectionString);
                connection.Open();

                Dao dao = new Dao(connection);
                return dao.GetPageTop(dateFrom, dateTo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error geting the page list");
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }
}