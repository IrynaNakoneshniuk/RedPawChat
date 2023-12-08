using Dapper;
using System.Data;
using RedPaw.Models;
using RedPawChat.Server.DataAccess.DapperContext;

namespace DBAccess.DBAccess
{
    /// <summary>
    /// A data access class for interacting with a SQL Server database implementing the ISqlDataAccess interface.
    /// </summary>
    public class SqlDataAccess : ISqlDataAccess
    {
        private readonly IDapperContext _context;

        public SqlDataAccess(IDapperContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Asynchronously loads data from the database using a stored procedure.
        /// </summary>
        /// <typeparam name="T">The type of the object to be returned.</typeparam>
        /// <typeparam name="U">The type of parameters for the stored procedure.</typeparam>
        /// <param name="storedProcedure">Name of the stored procedure to be executed.</param>
        /// <param name="parameters">Parameters for the stored procedure.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>A task representing the asynchronous operation that returns a collection of objects.</returns>
        public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters)
        {
            // Establishing a new connection to the database using IDbConnection interface
            using IDbConnection connection = _context.CreateConnection();
            try
            {
                return await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Logging the error message
                Console.WriteLine($"Error: {ex.Message}");

                // Returning an empty collection as an indicator of failure
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// Asynchronously saves data to the database using a stored procedure.
        /// </summary>
        /// <typeparam name="T">The type of the object to be saved.</typeparam>
        /// <param name="storedProcedure">Name of the stored procedure to be executed for saving data.</param>
        /// <param name="parameters">Data parameters to be saved.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <returns>A task representing the asynchronous operation for saving data.</returns>
        public async Task SaveData<T>(string storedProcedure, T parameters)
        {
            // Establishing a new connection to the database using IDbConnection interface
            using IDbConnection connection = _context.CreateConnection();

            try
            {
                await connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                // Logging the error message
                Console.WriteLine($"Error: {ex.Message}");

                throw;
            }
        }


        /// <summary>
        /// Retrieves contact information for a user from the database.
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure to execute.</param>
        /// <param name="connectionString">The connection string for the database.</param>
        /// <param name="user">The user object for whom to fetch contact information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task GetContactInfo(string storedProcedure,User user)
        {
            // Creating a new database connection using the IDbConnection interface
            using IDbConnection connection = _context.CreateConnection();

            try
            {
                // Defining parameters for the stored procedure
                var parameters = new { UserId = user.Id.ToString() };

                // Executing a stored procedure that returns multiple result sets
                using var results = await connection.QueryMultipleAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

                // Checking if the results are not null (indicating non-empty result sets)
                if (results != null)
                {
                    // Reading the first result set into a list of Contacts
                    var contacts = await results.ReadAsync<Contacts>();

                    // Reading the second result set into a list of GroupMembers
                    var groupMembers = await results.ReadAsync<GroupMember>();

                    // Reading the third result set into a list of BlackList
                    var blackList = await results.ReadAsync<BlackList>();

                    // Populating the user object with the retrieved data
                    user.ContactUsers = contacts.ToList();
                    user.GroupMembers = groupMembers.ToList();
                    user.BlackListUsers = blackList.ToList();
                }
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error: {ex.Message}");

                // Rethrow the exception to propagate it up the call stack
                throw;
            }
        }


        /// <summary>
        /// Retrieves information about conversations and messages for a user.
        /// </summary>
        /// <param name="storedProcedure">Name of the stored procedure for retrieving information.</param>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="user">User object for which information is retrieved.</param>
        /// <returns>Asynchronous task.</returns>
        public async Task GetConversationsInfo(string storedProcedure,User user)
        {
            // Establishes a connection to the database
            using IDbConnection connection = _context.CreateConnection();

            try
            {
              // Parameters to pass to the stored procedure
                 var parameters = new { UserId = user.Id.ToString() };

              // Calls the stored procedure and retrieves the results
                using var results = await connection.QueryMultipleAsync(storedProcedure, parameters, 
                    commandType: CommandType.StoredProcedure);

                // Checks if there are results
                if (results is not null)
                {
                    // Retrieves and converts the list of conversations to a collection
                    user.Conversations = (await results.ReadAsync<Conversations>()).ToList();
                    var messagesList = (await results.ReadAsync<Messages>()).ToList();

                    // For each conversation, retrieves and converts the messages
                    foreach (var conversation in user.Conversations)
                    {
                        conversation.Messages = messagesList
                            // Selects messages that belong to the current conversation
                            .Where(msg => msg.ConversationId == conversation.Id)
                            .ToList();
                    }
                }
            }
            catch(Exception ex )
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }

        public async Task<int?> GetScalarValue(string storedProcedure, User user, string roleName)
        {
            // Establishes a connection to the database
            using IDbConnection connection = _context.CreateConnection();

            var parameters = new { Id = user.Id, NameRole = roleName.ToUpper()};

            try
            {
                return await connection.ExecuteScalarAsync<int?>(storedProcedure, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;
            }
        }
    }

}
