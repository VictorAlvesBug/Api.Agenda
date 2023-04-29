using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Api.Agenda.DataLayer.ConnectionFactories
{
	public class ConnectionFactory
	{
		private readonly static string _serverAddress =
			Environment.GetEnvironmentVariable("DB_ADDRESS");
		private readonly static string _userID =
			Environment.GetEnvironmentVariable("DB_USER_ID") ?? "sa";
		private readonly static string _password =
			Environment.GetEnvironmentVariable("DB_PASSWORD");

		private async Task<SqlConnection> AbrirConexaoAsync(string banco = "master")
		{
			if (string.IsNullOrEmpty(_serverAddress)
				|| string.IsNullOrEmpty(_password))
			{
				throw new Exception("Configure as variáveis de ambiente \"DB_ADDRESS\" e \"DB_PASSWORD\" e reinicie o Visual Studio");
			}

			string connectionString =
				$"server={_serverAddress};database={banco};UID={_userID};password={_password}";

			SqlConnection con = new SqlConnection(connectionString);

			con.OpenAsync();

			return con;
		}

		public static async Task<IDbConnection> ConexaoAsync(string banco, bool webConfig = false)
		{
			return await new ConnectionFactory().AbrirConexaoAsync(banco);
		}

	}
}
