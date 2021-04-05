using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace people
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static String localHost = "http://localhost:9200";


        static async Task Main()
        {

            try
            {
                client.BaseAddress =new Uri(localHost );
                HttpResponseMessage response = await client.GetAsync("");
                response.EnsureSuccessStatusCode();
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            
            try
            {
                HttpResponseMessage response = await client.PutAsync("/people", new StringContent(""));
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch(HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");	
                Console.WriteLine("Message :{0} ",e.Message);
            }
            
    
            try
            {
                var fileString = File.ReadAllText("..\\..\\..\\new-people.json");
                Console.WriteLine(fileString);

                var result = JsonConvert.DeserializeObject<List<JObject>>(fileString);
                foreach (JObject p in result)
                {
                    HttpResponseMessage response = await client.PostAsync("/people/_doc", new StringContent(p.ToString(),Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                }
               
                Console.WriteLine("done");
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }

   
    
}