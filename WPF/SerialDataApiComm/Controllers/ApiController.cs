using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace SerialDataApiComm.Controllers
{
	internal static class ApiController
	{
		internal static async Task SendDataToWebhook(Models.BathroomStatusModel bathroomStatus)
		{
			try
			{
				using (var client = new HttpClient())
				{
				
					client.BaseAddress = new Uri("http://nibodev.azurewebsites.net/v1/");
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					HttpResponseMessage response = await client.PostAsJsonAsync("Bathroom/SaveStatus", bathroomStatus);
					response.EnsureSuccessStatusCode();

					if (response.IsSuccessStatusCode)
					{

					}
				}
			}
			catch (Exception ex)
			{
				///TODO
			}
		}
	}
}
