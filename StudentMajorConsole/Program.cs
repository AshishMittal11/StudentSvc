using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StudentMajorConsole
{
    internal class Program
    {
        // connection string to your Service Bus namespace
        static string connectionString = "Endpoint=sb://skilleddev.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+pCqoho0u2NpEysCt4zRSEm/IMsmRZGtCW1o566nw0c=";

        // name of the Service Bus topic
        static string topicName = "mytopic";

        // name of the subscription to the topic
        static string subscriptionName = "majorsubscription";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;

        // the processor that reads and processes messages from the subscription
        static ServiceBusProcessor processor;

        static async Task Main(string[] args)
        {
            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly.
            //
            // Create the clients that we'll use for sending and processing messages.
            client = new ServiceBusClient(connectionString);

            // create a processor that we can use to process the messages
            processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync().ConfigureAwait(false);

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync().ConfigureAwait(false);
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync().ConfigureAwait(false);
                await client.DisposeAsync().ConfigureAwait(false);
            }
        }

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string body = args.Message.Body.ToString();
                    var payload = JsonConvert.DeserializeObject<Payload>(body);
                    string url = "http://localhost:5228/api/email";

                    // posting the data to the email service....
                    var response = await client.SendAsync(new HttpRequestMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(url)
                    }).ConfigureAwait(false);

                    Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");

                    // complete the message. messages is deleted from the subscription. 
                    await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while posting the email to EmailSvc");
                Console.WriteLine(ex.Message);
            }
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
