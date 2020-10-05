using System;
using System.Threading.Tasks;

using Npgsql;

namespace ApiTester
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
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			// Insert some data
			await using (var cmd = new NpgsqlCommand("INSERT INTO public.\"SelectionMatrix\" (\"Id\",\"firstName\", \"lastName\", age ) VALUES (@Id, @firstName, @lastName, @age)", conn))
			{
				cmd.Parameters.AddWithValue("Id", 3);
				cmd.Parameters.AddWithValue("firstName", "John");
				cmd.Parameters.AddWithValue("lastName", "Doe");
				cmd.Parameters.AddWithValue("age", 33);
				await cmd.ExecuteNonQueryAsync();
			}
		}

		public async Task<string> Get()
		{
			string firstname = "";
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			// Insert some data
			await using var cmd = new NpgsqlCommand("SELECT * FROM public.\"SelectionMatrix\" LIMIT 1", conn);
			await using var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				firstname = reader.GetString(1);
				break;
			}

			return firstname;
		}

		public async Task UpdateFirstName(string from, string to)
		{
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			// Insert some data
			await using (var cmd = new NpgsqlCommand("UPDATE public.\"SelectionMatrix\" SET \"firstName\" = @to WHERE \"firstName\"= @from", conn))
			{
				cmd.Parameters.AddWithValue("from", from);
				cmd.Parameters.AddWithValue("to", to);
				await cmd.ExecuteNonQueryAsync();
			}
		}


		public async Task DeleteAll()
		{
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			// Insert some data
			await using var cmd = new NpgsqlCommand("DELETE FROM public.\"SelectionMatrix\";", conn);
			await using var reader = await cmd.ExecuteReaderAsync();
		}


		public async Task<long> GetCount()
		{
			long count = 0;
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			await using (var cmd = new NpgsqlCommand("SELECT count (*) FROM public.\"SelectionMatrix\";", conn))
			{
				await using var reader = await cmd.ExecuteReaderAsync();
				while (await reader.ReadAsync())
				{
					count = reader.GetInt64(0);
					break;
				}
			}

			return count;
		}
	}
}
