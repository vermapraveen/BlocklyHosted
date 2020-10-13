using System;
using System.Threading.Tasks;

using Common;

using Microsoft.Extensions.Options;

using Npgsql;

namespace FastApi
{

	public class DataAccess
	{
		private readonly string connString;

		public DataAccess(IOptions<ApiConfigs> options)
		{
			connString = options.Value.ConnString;
			Console.Out.WriteLine($"connString: {connString.Substring(0, 10)} ");
		}

		public async Task<long> Add(string json)
		{
			Console.Out.WriteLine("adding json");
			var id = GeneralUtils.GetNextLong2;

			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			await using var cmd = new NpgsqlCommand("INSERT INTO public.\"apidata\" (id, response) VALUES (@id, @response)", conn);
			_ = cmd.Parameters.AddWithValue("id", id);
			_ = cmd.Parameters.AddWithValue("response", json);
			var count = await cmd.ExecuteNonQueryAsync();

			Console.Out.WriteLine($"numbers of rows affected: {count}");

			return id;
		}

		public async Task<string> Get(long id)
		{
			string jsonData = "";
			await using var conn = new NpgsqlConnection(connString);
			await conn.OpenAsync();

			await using var cmd = new NpgsqlCommand("SELECT  \"response\" FROM public.\"apidata\" WHERE id = @id LIMIT 1", conn);
			_ = cmd.Parameters.AddWithValue("id", id);

			await using var reader = await cmd.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				jsonData = reader.GetString(0);
				break;
			}

			if(string.IsNullOrEmpty(jsonData))
			{
				Console.Out.WriteLine("404 not found");
			}

			return jsonData;
		}
	}
}
