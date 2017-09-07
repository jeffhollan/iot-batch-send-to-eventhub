using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunctionSimulatorConsoleApp
{
    class Program
    {
        private const int LIMIT = 10000;


        private static Random rnd;
        private static string[] locations = new string[] { "Redmond", "Seattle", "Las Vegas", "New York", "Orlando", "Snoqualmie" };
        private static EventHubClient client = EventHubClient.CreateFromConnectionString("Endpoint=sb://ready2017eventhub.servicebus.windows.net/;SharedAccessKeyName=Console;SharedAccessKey=TNhNwEPCJq91eu+rn4NPLYoe5HqQbEU7vZz+Tbfg2sI=;EntityPath=button");

        static void Main(string[] args)
        {
            rnd = new Random();
            Parallel.For(0, LIMIT, s =>
            {
                var result = SendMessage(new DeviceMessage { Device = Guid.NewGuid().ToString(), Location = (string)locations.GetValue(rnd.Next(locations.Length)), Type = rnd.Next(100) == 1 ? "Alert" : "Information" }).Result;
            });
            Console.ReadLine();
        }

        static async Task<bool> SendMessage(DeviceMessage message)
        {
            await client.SendAsync(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message))));
            Console.WriteLine("MESSAGE SENT\n" + JsonConvert.SerializeObject(message, Formatting.Indented));
            return true;
        }
    }

    internal class DeviceMessage
    {
        public string Device { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
    }
}
