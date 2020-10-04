using System.Threading.Tasks;

using Npgsql;

namespace DbGeneration
{
	public class DataAccessLayer
	{
		private readonly string connString;

		public DataAccessLayer(string connString)
		{
			this.connString = connString;
		}

		public async Task Add()
		{
			try
			{
				await using var conn = new NpgsqlConnection(connString);
				await conn.OpenAsync();

				// Insert some data
				await using (var cmd = new NpgsqlCommand("INSERT INTO public.\"SelectionMatrix\" (Id, firstName, lastName, age ) VALUES (@Id, @firstName, @lastName, @age)", conn))
				{
					cmd.Parameters.AddWithValue("Id", 1);
					cmd.Parameters.AddWithValue("firstName", "John");
					cmd.Parameters.AddWithValue("lastName", "Doe");
					cmd.Parameters.AddWithValue("age", 33);
					await cmd.ExecuteNonQueryAsync();
				}
			}
			catch (System.Exception ex)
			{

				throw;
			}
		}

		public async Task<string> Get()
		{
			try
			{
				string firstname = "";
				await using var conn = new NpgsqlConnection(connString);
				await conn.OpenAsync();

				// Insert some data
				await using var cmd = new NpgsqlCommand("SELECT * FROM public.\"SelectionMatrix\"", conn);
				await using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					firstname = reader.GetString(1);
					break;
				}

				return firstname;
			}
			catch (System.Exception ex)
			{

				throw;
			}
		}
	}
}
