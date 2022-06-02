using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Client.SignalRClient
{
    public abstract class SignalRClientBase : ISignalRClient, IAsyncDisposable
    {
        protected bool Started { get; private set; }
        protected HubConnection HubConnection { get; private set; }
        protected SignalRClientBase(string hubPath) =>
            HubConnection = new HubConnectionBuilder()
                .WithUrl(hubPath)
                .AddMessagePackProtocol()
                .WithAutomaticReconnect()
                .Build();

        public bool IsConnected =>
            HubConnection.State == HubConnectionState.Connected;

        public async Task Start()
        {
            if (!Started)
            {
                await HubConnection.StartAsync();
                Started = true;
            }
        }

        ~SignalRClientBase()
        {
            Dispose();
        }
        
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(disposing: false);
            GC.SuppressFinalize(this);
        }
        
        protected virtual async void Dispose(bool disposing)
        {
            if (disposing && HubConnection != null)
            {
                await HubConnection.DisposeAsync();
                HubConnection = null;
            }
        }
        
        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (HubConnection is not null)
            {
                await HubConnection.DisposeAsync().ConfigureAwait(false);
            }

            HubConnection = null;
        }
    }
}