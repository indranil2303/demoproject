using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TCSDemoProjectAlcoa.Models {
	public class WebApi{
		
		private HttpMethod method = null;  
		private string requestUri = "";  
		private HttpContent content = null;  
		private string acceptHeader = "application/json";  

		private WebApi AddMethod(HttpMethod method)  
		{  
			  this.method = method;  
			  return this;  
		}  
        
      private WebApi AddRequestUri(string requestUri)  
      {  
          this.requestUri = requestUri;  
          return this;  
      }  
  
      private WebApi AddContent(HttpContent content)  
      {  
          this.content = content;  
          return this;  
      }
		
	  private WebApi AddAcceptHeader(string acceptHeader)  
      {  
          this.acceptHeader = acceptHeader;  
          return this;  
      } 

	  private async Task<HttpResponseMessage> SendAsync() {
		try {

				var request = new HttpRequestMessage() { 
					Method = this.method,
					RequestUri = new Uri(this.requestUri)
				};


				request.Content = this.content;

				request.Headers.Accept.Clear();  
				if(!string.IsNullOrEmpty(this.acceptHeader))  
				   request.Headers.Accept.Add(  
					  new MediaTypeWithQualityHeaderValue(this.acceptHeader));  
         
				   // Setup client  
				   var client = new System.Net.Http.HttpClient();  
				   return await client.SendAsync(request); 
			}
			catch(Exception) {

				throw;
			}
		}


	  /* Http Methods */
	  public async Task<HttpResponseMessage> GetReq(string uri) {
			var req = new WebApi()
					.AddMethod(HttpMethod.Get)
					.AddRequestUri(uri);
			return await req.SendAsync();
	  }

	  public async Task<HttpResponseMessage> PostReq(string uri,object val){
			var req = new WebApi()  
                       .AddMethod(HttpMethod.Post)  
                          .AddRequestUri(uri)  
                        .AddContent(new StringContent(
							JsonConvert.SerializeObject(val), 
								Encoding.UTF8,"application/json")) ;  
  
          return await req.SendAsync();  
		}
	}
}
