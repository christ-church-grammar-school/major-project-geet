﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Major_project
{
    public class BackendConnect
    {
        static readonly string ip = "127.0.0.1";
        static readonly string port = "3000";
        static readonly string protocol = "http";
        public static readonly string server = protocol + "://" + ip + ":" + port + "/";

        static HttpClient httpClient = new HttpClient();
        static readonly JavaScriptSerializer jss = new JavaScriptSerializer();

        public class Send_message_class
        {
            public int Chat_id { get; set; }
            public int User_id { get; set; }
            public string Message { get; set; } 
            public long Current_time { get; set; }
            public string Username { get; set; }
        }

        public class Get_messages_class
        {
            public int Id { get; set; }
            public int User_id { get; set; }
            public int Time_submitted { get; set; }
            public string Message { get; set; }
            public string Username { get; set; }
            public string Chat { get; set; }
            public string Name { get; set; }
        }

        public List<Get_messages_class> Get(String request)
        {
            List<Get_messages_class> content = null;

            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = httpClient.GetAsync(request).Result;

                if (response.IsSuccessStatusCode)   
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    content = JsonConvert.DeserializeObject<List<Get_messages_class>>(json);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return content;
        }

        public async Task<string> Post(Send_message_class data, String request)
        {
            var response = await httpClient.PostAsJsonAsync(request, data);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}