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
                token = "YfTo8hatjhZNLQ";
                page = "https://passport.dev/";
                
            }
            else
            {
                token = "0RkWR5Pdnc";
                page = "https://passport.pl/";
               
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
