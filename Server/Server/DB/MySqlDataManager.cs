
using System.Data;
using MySql.Data.MySqlClient;

public class MySqlDataManager : IDisposable
{
    private  string _connectionString;
    private MySqlConnection _connection;
    private MySqlTransaction _transaction;
    
    public MySqlDataManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    #region 连接管理

    public void OpenConnection()
    {
        if (_connection == null)
        {
            _connection = new MySqlConnection(_connectionString);
        }
        if (_connection.State != System.Data.ConnectionState.Open)
        {
            _connection.Open();
        }
    }
    
    public void CloseConnection()
    {
        if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
        {
            // 如果有事务未提交，则回滚
            if (_transaction != null)
            {
                RollbackTransaction();
            }
            _connection.Close();
        }
    }

    #endregion

    #region 事务管理

    /// Begin a transaction
    public void BeginTransaction()
    {
        OpenConnection();
        if (_transaction == null)
        {
            _transaction = _connection.BeginTransaction();
        }
    }
    
    /// Commit the transaction
    public void CommitTransaction()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
    }
    
    /// Rollback the transaction
    public void RollbackTransaction()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    #endregion


    #region 基础操作

    // 执行非查询命令（INSERT, UPDATE, DELETE）
    public int ExecuteNonQuery(string commandText, params MySqlParameter[] parameters)
    {
        try
        {
            OpenConnection();
            using (var command = new MySqlCommand(commandText, _connection, _transaction))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error executing non-query command: " + ex.Message);
        }
        finally
        {
            // Close the connection if it was opened in this method
            if (_transaction == null)
            {
                CloseConnection();
            }
        }
    }
    
    // 执行查询命令并返回 第一行第一列的值
    public object ExecuteScalar(string commandText, params MySqlParameter[] parameters)
    {
        try
        {
            OpenConnection();
            using (var command = new MySqlCommand(commandText, _connection, _transaction))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return command.ExecuteScalar();
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error executing scalar command: " + ex.Message);
        }
        finally
        {
            // Close the connection if it was opened in this method
            if (_transaction == null)
            {
                CloseConnection();
            }
        }
    }
    
    public async Task<object> ExecuteScalarAsync(string commandText, params MySqlParameter[] parameters)
    {
        try
        {
            OpenConnection();
            using (var command = new MySqlCommand(commandText, _connection, _transaction))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                return await command.ExecuteScalarAsync();
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error executing scalar command: " + ex.Message);
        }
        finally
        {
            // Close the connection if it was opened in this method
            if (_transaction == null)
            {
                CloseConnection();
            }
        }
    }
    
    /// 执行查询命令并返回结果集
    public DataTable ExecuteQuery(string commandText, params MySqlParameter[] parameters)
    {
        Console.WriteLine("ExecuteQuery:" + commandText);
        var dataTable = new DataTable();
        try
        {
            OpenConnection();
            using (var command = new MySqlCommand(commandText, _connection, _transaction))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    dataTable.Load(reader);
                }
            }
            return dataTable;
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error executing query command: " + ex.Message);
        }
        finally
        {
            // Close the connection if it was opened in this method
            if (_transaction == null)
            {
                CloseConnection();
            }
        }
    }
    
    public async Task<DataTable> ExecuteQueryAsync(string commandText, params MySqlParameter[] parameters)
    {
        try
        {
            OpenConnection();
            using (var command = new MySqlCommand(commandText, _connection, _transaction))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var adapter = new MySqlDataAdapter(command))
                {
                    var dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));
                    return dataTable;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log it, rethrow it, etc.)
            throw new Exception("Error executing query command: " + ex.Message);
        }
        finally
        {
            // Close the connection if it was opened in this method
            if (_transaction == null)
            {
                CloseConnection();
            }
        }
    }

    #endregion

    public void Dispose()
    {
        if (_transaction != null)
        {
            RollbackTransaction();
        }
        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }
}