using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Data.Common;
using System.Linq.Expressions;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;

namespace EHSApps.API.Utils
{
    public static class DBGeneric
    {
        public static IEnumerable<T> ExecuteStoredProc<T>(EntityConnection conn, string procedure, string[] paramNames, object[] paramValues) where T : new()
        {
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
            var command = new EntityCommand
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = procedure,
                Connection = conn,
            };
            for (int i = 0; i < paramNames.Length; i++)
                command.Parameters.AddWithValue(paramNames[i], paramValues[i]);
            var list = new List<T>();
            using (var reader = command.ExecuteReader())
            {
                // get GUID values from the reader here,
                // and put them in the list
                list = reader.MapToList<T>();
                reader.Close();
            }
            return list;
        }
        public static Expression<Func<TElement, bool>> BuildContainsExpression<TElement, TValue>(
 Expression<Func<TElement, TValue>> valueSelector, IEnumerable<TValue> values)
        {
            if (null == valueSelector) { throw new ArgumentNullException("valueSelector"); }
            if (null == values) { throw new ArgumentNullException("values"); }
            ParameterExpression p = valueSelector.Parameters.Single();
            // p => valueSelector(p) == values[0] || valueSelector(p) == ...
            if (!values.Any())
            {
                return e => false;
            }
            var equals = values.Select(value => (Expression)Expression.Equal(valueSelector.Body, Expression.Constant(value, typeof(TValue))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<TElement, bool>>(body, p);
        }
        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);
                var entities = new List<T>();
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);

                while (dr.Read())
                {
                    T newObject = new T();
                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDict[dr.GetName(index).ToUpper()];
                            if ((info != null) && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    entities.Add(newObject);
                }
                return entities;
            }
            return null;
        }
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                List<T> list = new List<T>();

                foreach (DataRow row in table.Rows)
                {
                    T obj = new T();

                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        //Traditional way to  call stored procedure
        public static IEnumerable<T> ExecStoredProcedure<T>(string conString, string procedure, string[] paramNames, object[] paramValues) where T : new()
        {
            var data = new List<T>();

            using (var conn = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;

                com.CommandText = procedure;
                for (int i = 0; i < paramNames.Length; i++)
                    com.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                var dataset = new DataSet();
                adapt.Fill(dataset);
                DataTableToList<T>(data, dataset);
                return data;
            }

        }


        //Traditional way to  call insert/update stored procedure
        public static int ExecInsertUpdateStoredProcedure(string conString, string procedure, string[] paramNames, object[] paramValues)
        {
            using (var conn = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;

                com.CommandText = procedure;
                for (int i = 0; i < paramNames.Length; i++)
                    com.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                conn.Open();
                com.ExecuteNonQuery();
                conn.Close();

                return 1;
            }
        }

        //Traditional way to  call stored procedure
        public static IEnumerable<T> ExecStoredProcedureWithSqlParms<T>(string conString, string procedure, SqlParameter[] paramArrays) where T : new()
        {
            var data = new List<T>();

            using (var conn = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;

                com.CommandText = procedure;
                for (int i = 0; i < paramArrays.Length; i++)
                    com.Parameters.Add(paramArrays[i]);
                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                var dataset = new DataSet();
                adapt.Fill(dataset);
                DataTableToList<T>(data, dataset);
                return data;
            }

        }

        //Traditional way to  call stored procedure with SQLParameter for Data Type allow
        public static int ExecStoredProcedureWithSqlParms(string conString, string procedure, SqlParameter[] paramArrays)
        {
            var data = 0;
            try
            {
                using (var conn = new SqlConnection(conString))
                {
                    var com = new SqlCommand();
                    com.Connection = conn;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = procedure;
                    for (int i = 0; i < paramArrays.Length; i++)
                        com.Parameters.Add(paramArrays[i]);
                    var adapt = new SqlDataAdapter();
                    adapt.SelectCommand = com;
                    var dataset = new DataSet();
                    adapt.Fill(dataset);
                    if(dataset != null)
                    {
                        if (dataset.Tables.Count > 0)
                        {
                            if (dataset.Tables[0].Rows.Count > 0)
                            {
                                data = Convert.ToInt32(dataset.Tables[0].Rows[0].ItemArray[0]);
                            }
                        }
                    }
                    return data;
                }
            }
            catch
            { throw; }
        }


        //Traditional way to  call insert/update stored procedure with return dataset
        public static IEnumerable<T> ExecInsertUpdateStoredProcedure<T>(string conString, string procedure, string[] paramNames, object[] paramValues) where T : new()
        {
            var data = new List<T>();

            using (var conn = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;

                com.CommandText = procedure;
                for (int i = 0; i < paramNames.Length; i++)
                    com.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                var dataset = new DataSet();
                adapt.Fill(dataset);
                DataTableToList<T>(data, dataset);
                return data;
            }

        }


        private static void DataTableToList<T>(List<T> data, DataSet dataset) where T : new()
        {
            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                T obj = new T();

                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }

                data.Add(obj);
            }
        }

        public static DataSet ExecStoredProcedure(string conString, string procedure, string[] paramNames, object[] paramValues)
        {
            using (var conn = new SqlConnection(conString))
            {
                var com = new SqlCommand();
                com.Connection = conn;
                com.CommandType = CommandType.StoredProcedure;

                com.CommandText = procedure;
                for (int i = 0; i < paramNames.Length; i++)
                    com.Parameters.AddWithValue(paramNames[i], paramValues[i]);
                var adapt = new SqlDataAdapter();
                adapt.SelectCommand = com;
                var dataset = new DataSet();
                adapt.Fill(dataset);
                return dataset;
            }
        }
        /* Load an entity in Entity framework by Key and value*/
        public static T LoadByKey<T>(this ObjectContext context, String propertyName, Object keyValue)
        {
            IEnumerable<KeyValuePair<string, object>> entityKeyValues =
               new KeyValuePair<string, object>[] { 
           new KeyValuePair<string, object>(propertyName, keyValue) };

            // Create the  key for a specific SalesOrderHeader object. 
            EntityKey key = new EntityKey(context.GetType().Name + "." + typeof(T).Name, entityKeyValues);
            return (T)context.GetObjectByKey(key);
        }
    }
}
