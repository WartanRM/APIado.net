using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using APIDemo.Models;

namespace APIDemo.Data
{
    public class PeopleRepository
    {
        private readonly string _connectionString;

        public PeopleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<PeopleData>> GetAllAsync()
        {
            var people = new List<PeopleData>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM PeopleData", connection);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        people.Add(new PeopleData
                        {
                            Id = reader.GetString(reader.GetOrdinal("Id")),
                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        });
                    }
                }
            }

            return people;
        }
        public async Task<PeopleData> GetByIdAsync(string id)
        {
            PeopleData person = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM PeopleData WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        person = new PeopleData
                        {
                            Id = reader.GetString(reader.GetOrdinal("Id")),
                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Gender = reader.GetString(reader.GetOrdinal("Gender")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                    }
                }
            }

            return person;
        }

        public async Task<int> AddAsync(PeopleData person)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("INSERT INTO PeopleData (Id, Age, Name, Gender, Email) VALUES (@Id, @Age, @Name, @Gender, @Email)", connection);
                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@Age", person.Age);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Gender", person.Gender);
                command.Parameters.AddWithValue("@Email", person.Email);

                return await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> UpdateAsync(PeopleData person)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("UPDATE PeopleData SET Age = @Age, Name = @Name, Gender = @Gender, Email = @Email WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", person.Id);
                command.Parameters.AddWithValue("@Age", person.Age);
                command.Parameters.AddWithValue("@Name", person.Name);
                command.Parameters.AddWithValue("@Gender", person.Gender);
                command.Parameters.AddWithValue("@Email", person.Email);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("DELETE FROM PeopleData WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

    }
}

