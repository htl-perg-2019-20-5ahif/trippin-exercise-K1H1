using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrippinUser
{
	class Program
	{
		public static HttpClient client = new HttpClient() { BaseAddress = new Uri("https://services.odata.org/TripPinRESTierService/(S(4ry5xpsxhfj33xna2sghsnlw))/") };
		static string filename = "users.json";

		static async Task Main(string[] args)
		{

			// read users from file
			var fileText = await ReadFile(filename);
			var fileUsers = JsonSerializer.Deserialize<List<FileUser>>(fileText);

			//add missing users
			AddMissingUsers(fileUsers);

		}

		/// <summary>
		/// Reads a json file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>A string with the file content</returns>
		public static async Task<string> ReadFile(string filename)
		{

			Console.WriteLine("readFile");
			var filetext = await File.ReadAllTextAsync(filename);

			return filetext;

		}

		/// <summary>
		/// Checks if a user from the .json- file is already in the API, if not, the user is added
		/// </summary>
		/// <param name="fileUsers"></param>
		public async static void AddMissingUsers(List<FileUser> fileUsers)
		{
			Console.WriteLine("fileUser size:" + fileUsers.Count);
			Console.WriteLine(fileUsers[1].FirstName);

			foreach (FileUser user in fileUsers)
			{
				Console.WriteLine("in Loop");

				var userResponse = await client.GetAsync("People('" + user.UserName + "')");

				Console.WriteLine("in Loop 2");

				if (!userResponse.IsSuccessStatusCode)
				{
					Console.WriteLine("in if");

					Console.WriteLine("User added: " + user.UserName);
					HttpResponseMessage response = await client.PostAsync("People", new StringContent(JsonSerializer.Serialize(new
					{
						user.UserName,
						user.FirstName,
						user.LastName,
						Email = new List<string> { user.Emails },
						AddressInfo = new List<object>{
									new
									{
										user.Address,
										City = new
										{
											Name = user.CityName,
											CountryRegion = user.Country,
											Region = user.Country
										}
									}
					}

					}), Encoding.UTF8, "application/json"));
					
				}
				else
				{
					Console.WriteLine("in else");
					Console.WriteLine("User already exists: " + user.UserName);
				}

				Console.WriteLine("End of code");
			}


		}
	}
}


