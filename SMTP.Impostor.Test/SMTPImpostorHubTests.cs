using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMTP.Impostor.Worker.Hubs;

namespace SMTP.Impostor.Test
{
    [TestClass]
    public class SMTPImpostorHubTests
    {
        readonly SMTPImpostorHubMessage MESSAGE;
        readonly string MESSAGE_JSON;

        public SMTPImpostorHubTests()
        {
            MESSAGE = new SMTPImpostorHubMessage("TYPE", "DATA");
            MESSAGE_JSON = JsonSerializer.Serialize(MESSAGE);
        }

        [TestMethod]
        public void connects()
        {
            using var hub = GetHub();

            IEnumerable<ISMTPImpostorHubClient> clients = null;
            using (hub.Clients.Subscribe(c => clients = c))
            {

                var client = new SMTPImpostorHubClient();

                Task.Run(async () =>
                {
                    await hub.ConnectAsync(client);
                });

                Task.Delay(100).GetAwaiter().GetResult();
                Assert.AreEqual(1, clients.Count());
            }
        }

        [TestMethod]
        public async Task sends()
        {
            using var hub = GetHub();

            var client = new SMTPImpostorHubClient();

            _ = Task.Run(async () =>
            {
                await hub.ConnectAsync(client);
            });

            await hub.SendMessage(MESSAGE);

            Assert.AreEqual(MESSAGE_JSON, client.Sent);
        }

        [TestMethod]
        public async Task sends_to_particular_client()
        {
            using var hub = GetHub();

            var clientA = new SMTPImpostorHubClient();
            var clientB = new SMTPImpostorHubClient();

            _ = Task.Run(async () =>
            {
                await hub.ConnectAsync(clientA);
            });
            _ = Task.Run(async () =>
            {
                await hub.ConnectAsync(clientB);
            });

            await hub.SendMessage(MESSAGE, new[] { clientA });

            Assert.AreEqual(MESSAGE_JSON, clientA.Sent);
            Assert.AreEqual(null, clientB.Sent);
        }

        [TestMethod]
        public void receives()
        {
            using var hub = GetHub();

            SMTPImpostorHubMessage message = null;
            using (hub.Messages.Subscribe(m => message = m))
            {
                var client = new SMTPImpostorHubClient();

                _ = Task.Run(async () =>
                {
                    await hub.ConnectAsync(client);
                });

                client.TriggerReceive(MESSAGE_JSON);

                Task.Delay(100).GetAwaiter().GetResult();
                Assert.AreEqual(MESSAGE_JSON, JsonSerializer.Serialize(message));
            }
        }

        SMTPImpostorHubService GetHub()
        {
            return new SMTPImpostorHubService(
                NullLogger<SMTPImpostorHubService>.Instance);
        }

        class SMTPImpostorHubClient :
            ISMTPImpostorHubClient
        {
            public SMTPImpostorHubClient()
            {
                _messageSemaphore = new SemaphoreSlim(0);
                State = WebSocketState.Open;
            }

            readonly SemaphoreSlim _messageSemaphore;
            string _message;

            public WebSocketState State { get; set; }
            public string Sent { get; private set; }

            public void TriggerReceive(string data)
            {
                _message = data;
                _messageSemaphore.Release();
            }

            public Task CloseAsync()
            {
                State = WebSocketState.Closed;
                return Task.CompletedTask;
            }

            public async Task<string> ReceiveAsync()
            {
                await _messageSemaphore.WaitAsync();
                return _message;
            }

            public Task SendAsync(string data)
            {
                Sent = data;
                return Task.CompletedTask;
            }
        }
    }
}
