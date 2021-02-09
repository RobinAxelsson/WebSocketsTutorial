using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace u2b_Client_Server_Library
{
    public abstract class Channel<TProtocol, TMessageType> : IDisposable
        where TProtocol : Protocol<TMessageType>, new()
    {
        protected bool _isDisposed = false;
        readonly TProtocol _protocol = new TProtocol();
        readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        NetworkStream _networkStream;
        Task _receiveLoopTask;

        public void Attach(Socket socket)
        {
            _networkStream = new NetworkStream(socket, true);
            _receiveLoopTask = Task.Run(ReceiveLoop, _cancellationTokenSource.Token);
        }

        public void Close()
        {
            _cancellationTokenSource.Cancel();
            _networkStream?.Close();
        }

        protected virtual async Task ReceiveLoop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                //TODO: Pass Cancellation Toke nto Protocol methods
                var msg = await _protocol.ReceiveAsync(_networkStream).ConfigureAwait(false);
            }
        }
        ~Channel() => Dispose(false);
        public void Dispose() => Dispose(true);
        protected void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;

                //TODO: Clean up socket, stream, etc.
                _networkStream?.Dispose();

                if (isDisposing)
                    GC.SuppressFinalize(this);
            }
        }
    }
}
