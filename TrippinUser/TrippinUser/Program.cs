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
			var added = await AddMissingUsers(fileUsers);

		}

		/// <summary>
		/// Reads a json file
		/// </summary>
		/// <param name="filename"></param>
		/// <returns>A string with the file content</returns>
		public static async Task<string> ReadFile(string filename)
		{
			var filetext = await File.ReadAllTextAsync(filename);

			return filetext;

		}

		/// <summary>
		/// Checks if a user from the .json- file is already in the API, if not, the user is added
		/// </summary>
		/// <param name="fileUsers"></param>
		public static async Task<string> AddMissingUsers(List<FileUser> fileUsers)
		{
			foreach (FileUser user in fileUsers)
			{
				//check if user is already in the list
				var userResponse = await client.GetAsync("People('" + user.UserName + "')");


				if (!userResponse.IsSuccessStatusCode)
				{

					HttpResponseMessage addResponse = await client.PostAsync("People", new StringContent(JsonSerializer.Serialize(new

					{
						UserName = user.UserName,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Emails = new[] {
							user.Emails
						},

						AddressInfo = new[] {
						new {
						Address = user.Address,
						City = new  {
							Name = user.CityName,
							CountryRegion = user.Country,
							Region = "unknown"
						}
					}
				}
					}), Encoding.UTF8, "application/json"));
					Console.WriteLine("Add user: " + addResponse.StatusCode.ToString() + "| " + user.UserName);

				}
				else
				{
					Console.WriteLine("User already exists: " + user.UserName);
				}

			}

			return " ";
		}
	}
}


