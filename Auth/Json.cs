using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth
{
	public class Json
	{
		string filePath;
		private List<User> jsonData;

		public List<User> Data
		{
			get { return jsonData; }
		}

		public Json(string path)
		{
			filePath = path;

		}

		public Json()
		{
			jsonData = new List<User>();
		}


		/// <summary>
		/// Zapisywanie listy json do wskazanego pliku
		/// </summary>
		/// <param name="jsonList">Lista json z danymi</param>
		/// <param name="path">Scieżka do pliku w którym dane mają być zapisane</param>
		public void Save(List<User> jsonList, string path)
		{
			using (StreamWriter file = File.CreateText(path))
			{
				JsonSerializer serializer = new JsonSerializer();

				serializer.Serialize(file, jsonList);
			}
		}

		/// <summary>
		/// Zapisywanie listy json do wskazanego pliku podczas deklarowania klasy
		/// </summary>
		/// <param name="jsonList">Scieżka do pliku w którym dane mają być zapisane</param>
		public void Save(List<User> jsonList)
		{
			if (filePath == string.Empty)
				throw new Exception("Nie wskazano miejsca zapisu pliku z danymi JSON");

			this.Save(jsonList, filePath);
		}

		/// <summary>
		/// Odczyt danych z pliku i konwersja na json
		/// </summary>
		/// <param name="path">Ścieżka do pliku</param>
		/// <returns></returns>
		public List<User> Read(string path)
		{
			Uri uriResult;
			bool result = Uri.TryCreate(path, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

			if (result)
				return ReadFromWeb(path);
			else
				return ReadLocal(path);


		}

		/// <summary>
		/// Odczyt danych z pliku i konwersja na json
		/// </summary>
		/// <param name="path">Ścieżka do pliku</param>
		/// <returns></returns>
		public List<User> Read(Stream path)
		{
			List<User> jsonList;

			JsonSerializer serializer = new JsonSerializer();
			StreamReader reader = new StreamReader(path);
			string jsonString = reader.ReadToEnd();
			jsonList = JsonConvert.DeserializeObject<List<User>>(jsonString);

			path.Close();

			return jsonList;
		}

		/// <summary>
		/// Aktualizacja wybranego elementu JSON
		/// </summary>
		/// <param name="id">ID wybranego elementu</param>
		/// <param name="newData">Nowy dane wybranego elementu</param>
		public void Update(int id, User newData)
		{
			User item = jsonData.Find(s => s.id == id.ToString());
			item.name = newData.name;
			item.email = newData.email;
			item.updated_at = newData.updated_at;
			item.created_at = newData.created_at;
		}

		/// <summary>
		/// Aktualizacja tylko daty
		/// </summary>
		/// <param name="id">ID elementu JSON</param>
		/// <param name="date">Nowa wartość daty</param>
		public void UpdateName(int id, string date)
		{
			User item = jsonData.Find(s => s.id == id.ToString());
			item.name = date;
		}

		/// <summary>
		/// Usunięcie wybranego elementu
		/// </summary>
		/// <param name="id"></param>
		public void Remove(int id)
		{
			User item = jsonData.Find(s => s.id == id.ToString());
			jsonData.Remove(item);
		}

		/// <summary>
		/// Dodanie nowego elementu
		/// </summary>
		/// <param name="newData">Nowy element</param>
		public void Add(User newData)
		{
			newData.id = jsonData.Last().id + 1;
			jsonData.Add(newData);
		}

		/// <summary>
		/// Porównanie domyślnego JSON ze wskazanym w parametrze
		/// </summary>
		/// <param name="jsonToCheck">JSON do porównania</param>
		/// <returns>Zwraca elementy które się różnią</returns>
		public List<User> Compare(List<User> jsonToCheck)
		{
			List<User> differences = new List<User>();

			foreach (var item in jsonData)
			{
				//Debug.WriteLine(item.ID);
				User web = jsonToCheck.Find(s => s.id == item.id);
				if (web.name != item.name)
					differences.Add(web);

			}

			return differences;
		}

		private List<User> ReadLocal(string path)
		{
			List<User> jsonList;
			using (Stream file = (Stream)File.OpenRead(path))
			{
				jsonList = Serializer(file);

			}
			return jsonList;
		}

		private List<User> ReadFromWeb(string webPathJson)
		{
			List<User> jsonList;

			WebClient client = new WebClient();

			using (Stream dataWeb = client.OpenRead(webPathJson))
			{
				jsonList = Serializer(dataWeb);
			}
			return jsonList;
		}

		private List<User> Serializer(Stream s)
		{
			JsonSerializer serializer = new JsonSerializer();
			StreamReader reader = new StreamReader(s);
			string jsonString = reader.ReadToEnd();
			return JsonConvert.DeserializeObject<List<User>>(jsonString);
			// Wyjątek pojawia się prawdopodobnie bo na stronie plik json ma ciągle wpisane "Table"
		}

		public void Serializer(string jsonString)
		{
			JsonSerializer serializer = new JsonSerializer();
			//StreamReader reader = new StreamReader(s);
			//string jsonString = reader.ReadToEnd();
			var jsonData = JsonConvert.DeserializeObject<User>(jsonString);
			// Wyjątek pojawia się prawdopodobnie bo na stronie plik json ma ciągle wpisane "Table"
		}

		public void Send()
		{
			var cli = new WebClient();
			cli.Headers[HttpRequestHeader.ContentType] = "application/json";
			string response = cli.UploadString("http://some/address", "{some:\"json data\"}");
		}
	}
}
