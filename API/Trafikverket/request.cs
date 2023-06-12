global using RestSharp;

namespace Trafikverket
{ 
    class Request
    {
        static void Main()
        {
            Request req = new Request();
            req.Parking();
        }
        //General API request for fetching fata from trafikverket
        public string GetData (string objectType, string schemaVersion, string limit)
        {
            var client = new RestClient("https://api.trafikinfo.trafikverket.se/v2/data.json");
            var request = new RestRequest();

            request.AddHeader("Content-Type", "text/plain");

            var body = @"<REQUEST>" + "\n" +
            @"  <LOGIN authenticationkey=""3641922b131142b4b92ff45f502a6f74"" />" + "\n" +
            @$"  <QUERY objecttype=""{ objectType }"" schemaversion=""{ schemaVersion }"" limit=""{ limit }"">" + "\n" +
            @"  </QUERY>" + "\n" +
            @"</REQUEST>";

            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            RestResponse response = client.Post(request);
            var data = response.Content;

            return data;
        }

        //General TimeStamp method
        public long UnixTimeStamp()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        //Specific data request over parkings from Trafikverket
        public void Parking ()
        {
            Request req = new Request();
            var response = req.GetData("parking", "1.4", "10");
            req.CreateJsonFile(response, "parking");
        }

        public void CreateJsonFile(string data, string name)
        {
            Request req = new Request();
            var timestamp = req.UnixTimeStamp();
            var filePath = @"" + name +"_" + timestamp + ".json";

             File.Create(filePath).Close();
             File.WriteAllText(filePath, data);
        }
    }
}


