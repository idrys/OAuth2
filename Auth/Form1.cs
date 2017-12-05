using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Auth
{
	public partial class Form1 : Form
	{
        bool dev = false;
        string token;
        string page;

        public Form1()
		{
            if ( dev)
            {
                token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6ImRmMTNhZTU2NzIwM2E1NzAzMWI2NWZhNzU2ZmU0NmEyMzcxYWFiNzY4YjhhMWFjZWJhODIzZGY2ZGJmYjZiOTk5NGEwZTYwYmI3YWU3MzQzIn0.eyJhdWQiOiIzIiwianRpIjoiZGYxM2FlNTY3MjAzYTU3MDMxYjY1ZmE3NTZmZTQ2YTIzNzFhYWI3NjhiOGExYWNlYmE4MjNkZjZkYmZiNmI5OTk0YTBlNjBiYjdhZTczNDMiLCJpYXQiOjE1MTIyMTMyNDUsIm5iZiI6MTUxMjIxMzI0NSwiZXhwIjoxNTQzNzQ5MjQ1LCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.eIXpzXRluPyg0S7GjJbh6s4p7XzET-RgzdxIYdOMGewg2uVN3Co5Cq4Ei19yMiCOko5qLaOtV1wzklHUckaY3rJ35iLnqrnnaywV7RgfWdzp3UCZ62Oam9wb0hv0Ib2j0laaLw_PpLTX5l3x1IsYLLo9b-XxbssxDwz2aZ6OKHx92e-A49uYpuzvJb6Lu_ZAFVJSDvdtSpI193ey_kOEl3AwnXhTJVUVcfZqE8GkQXT2GrZ9jMRwUIb-1_WI-kRvN1fiZZJgYiO5ZXRDkrJ7hMNifNaPCeaFhX-BXNdwUAxR8z5EtffVNTHss0GjQP6BiKGTRSdlIuNBIo_Tt2qSTUYtTL540U-UsNMJZDZiaSPGA_vMuqEHnJCFSr9CB-Ee1kxYfBNZS7BM_uggFnQY_9S1SnTCYr3siTOkq5RthqZ4U-OCyccnFfEEA_c_gKgOFffCafulE3O5_q6Pk1xLhX3NnJWxmCDtpsyXcfqZAixHHrW9b-pEQy3uchyAxIGQN6cfJL80O3TlWAL74Fug51dDQDoa0y268ddQS-ZunNaACoqQDvTP7xhJkFvY4sQaXtFUHHdQCfvAMYP0q8V7CGNe6bbh2U9VPChJOubY1jCHFBKClYyLKoc4zOlOCc5ziZ2_XkCGq0fjGpSXqV55spWw12jyfYfTo8hatjhZNLQ";
                page = "https://passport.dev/";
                
            }
            else
            {
                token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImp0aSI6IjNmYzIyNzBlODViOTU5NjhiNjJlZWI1NDJjNTliMTZjNDQ4MjYyMmM4NzllMTRjOGIzN2RjOGRmMWUwZTMwZGI3NmMyMWQwNDVlMTllYTJhIn0.eyJhdWQiOiIzIiwianRpIjoiM2ZjMjI3MGU4NWI5NTk2OGI2MmVlYjU0MmM1OWIxNmM0NDgyNjIyYzg3OWUxNGM4YjM3ZGM4ZGYxZTBlMzBkYjc2YzIxZDA0NWUxOWVhMmEiLCJpYXQiOjE1MTIzOTkzODEsIm5iZiI6MTUxMjM5OTM4MSwiZXhwIjoxNTQzOTM1MzgxLCJzdWIiOiIxIiwic2NvcGVzIjpbXX0.pWTvSqcicdlEpuw2m-mQTLlRctvRh0u9hQ4GBGDKtf9NY0Mz6ad0Y31VIcCwUp_9-Ji8OOLA5r08C9tPo5Q17LTvLG6ctyXkucgjTVDOnam2-WxYazDkXWFKZ9Nq4e13H8KJ6ltRzYlLvzS2fXT9VxeYW5KAObEMYeIRhAS_LxjFc74curjzp82hJQ0k7sG0GPl-ex37gp_r-W9N8ZbjuJ4yixbg0iDlI1SZBaPca51edLj3OZUqlV6L69hBqJsVpJB97cwQCIutcFb_y0l6upfDIlNHwwKOeSlzfTEr0i7q1f6uWYB4_9g7tjJWlXTQnGilTJEThxkGWevWXgO2ZoDwnko4Aa370HwZAIj7VxW_YI3lwj6XlQuNqIfA3Z6gnDd-H8JNRL0Qc3elT28SOPuzU_RUE4JeT49sE2kzbtgDNl3SP9-Z5mNHYBve2gIDyTFLKhtc5R6zgVeGUisEt3UVrfSWywG2i7nJcd8Hhory9GtjbkMnLUnbsElY2hZUyDtxt9CjVC7CuQbrOPv-GIbXg2P9dGLkUEGkODMTYtJvP6MJvEnlh8Qx-AIny-FfTXvMb7PWoZYLybgJ94mYV5izSjG7feo4ZwgdPfFv42LKHqjkRYUI3KJuhjDiZHIJD3gFu2wfP8jg-pQeyoyOKKssPokffSGs-0RkWR5Pdnc";
                page = "https://1sw.pl/";
               
            }

            

            InitializeComponent();
            this.label1.Text = page;
        }

		private void Form1_Load(object sender, EventArgs e)
		{
			//GetUsers();
			
		}

		public async void GetUsers()
		{
			Json json = new Json();
			HttpClient client = GetClient(token, new Uri("https://password.dev/api/user"));

			using (HttpResponseMessage response = await client.GetAsync(page))
			using (HttpContent content = response.Content)
			{
				// ... Read the string.
				string result = await content.ReadAsStringAsync();

				// ... Display the result.
				if (result != null &&
					result.Length >= 50)
				{
					json.Serializer(result);
				}
			}
		}

		public async void PutUsers()
		{
           
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

           
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            Json json = new Json();
			string updateInfo = "'desktop':'386', 'name': 'Jan', 'lastname': 'Nowak', 'email':'jan.nowak@tgs.pl', 'phone':'626111221' 'department':'Ruda Śląska', 'file':'opoczno.exe', 'start':'21-01-2017 10:00:00', 'end':'21-01-2017 10:08:32'";

			var jsonUser = JsonConvert.SerializeObject(updateInfo);

			HttpClient client = GetClient(token, new Uri("https://www.passport.dev/"));
            
            StringContent content = new StringContent(jsonUser, Encoding.UTF8, "application/json");
            
			HttpResponseMessage response = await client.PostAsync(page + "api/user", content) ;
            
			var responseString = response.Content.ReadAsStringAsync().Result;
           // MessageBox.Show( response.StatusCode.ToString(), response.ReasonPhrase);
            richTextBox1.Text = responseString.ToString();
		 }


        private static bool ValidateRemoteCertificate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            if (error == System.Net.Security.SslPolicyErrors.None)
                return true;
            //MessageBox.Show( cert.Subject, error.ToString() );
            Debug.WriteLine("X509Certificate [{0}] Policy Error '{1}'",
                cert.Subject,
                error.ToString());

            return true;
        }

		public static HttpClient GetClient(string token, Uri path)
		{
			var authValue = new AuthenticationHeaderValue("Bearer", token);
            
            ServicePointManager.ServerCertificateValidationCallback += ValidateRemoteCertificate;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11;



            var client = new HttpClient()
            {
                DefaultRequestHeaders = { Authorization = authValue }
            };


            return client;
		}

        private void button1_Click(object sender, EventArgs e)
        {
            
            PutUsers();
        }


    }
}
