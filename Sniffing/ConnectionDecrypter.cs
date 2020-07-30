using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Tera.PacketLog;
using Tera.Sniffing.Crypt;

namespace Tera.Sniffing
{
    public class ConnectionDecrypter
    {
        private readonly string _region;
        private MemoryStream _client = new MemoryStream();
        private MemoryStream _server = new MemoryStream();
        private Session _session;

        public ConnectionDecrypter(string region = "Unknown")
        {
            _region = region;
        }

        public bool Initialized => _session != null;

        public event Action<byte[]> ClientToServerDecrypted;
        public event Action<byte[]> ServerToClientDecrypted;

        protected void OnClientToServerDecrypted(byte[] data)
        {
            var action = ClientToServerDecrypted;
            action?.Invoke(data);
        }

        protected void OnServerToClientDecrypted(byte[] data)
        {
            var action = ServerToClientDecrypted;
            action?.Invoke(data);
        }

        private void TryInitialize()
        {
            if (Initialized)
                throw new InvalidOperationException("Already initalized");
            if (_client.Length < 256 + 4 || _server.Length < 256 + 4)
                return;

            _server.Position = 0;
            _client.Position = 0;

            var magicBytes = _server.ReadBytes(4);
            if (!magicBytes.SequenceEqual(new byte[] {1, 0, 0, 0}))
                throw new FormatException("Not a Tera connection");

            var clientKey1 = _client.ReadBytes(128);
            var clientKey2 = _client.ReadBytes(128);
            var serverKey1 = _server.ReadBytes(128);
            var serverKey2 = _server.ReadBytes(128);
            var session = new Session(clientKey1, clientKey2, serverKey1, serverKey2);
            var classicSession = new Session(clientKey1, clientKey2, serverKey1, serverKey2, false);
            var checkVersion = _client.ReadBytes(4);
            var checkNew = checkVersion.ToArray();
            session.Decrypt(checkNew);
            var checkClassic = checkVersion.ToArray();
            classicSession.Decrypt(checkClassic);
            if (checkNew[2] == 0xbc && checkNew[3] == 0x4d) {
                _session = session;
                OnClientToServerDecrypted(checkNew);
            }else if (checkClassic[2] == 0xbc && checkClassic[3] == 0x4d) {
                _session = classicSession;
                OnClientToServerDecrypted(checkClassic);
            }else throw new FormatException("Failed to decrypt");
            ClientToServer(_client.ReadBytes((int) (_client.Length - _client.Position)));
            ServerToClient(_server.ReadBytes((int) (_server.Length - _server.Position)));
            _client = null;
            _server = null;
        }

        public void Skip(MessageDirection direction, int needToSkip)
        {
            if (needToSkip > 0)
            {
                if (direction == MessageDirection.ServerToClient)
                {
                    var skip = new byte[needToSkip];
                    _session.Encrypt(skip);
                }
                else
                {
                    var skip = new byte[needToSkip];
                    _session.Decrypt(skip);
                }
            }
        }

        public void ClientToServer(byte[] data, int needToSkip=0)
        {
            if (Initialized)
            {
                if (needToSkip > 0)
                {
                    var skip = new byte[needToSkip];
                    _session.Decrypt(skip);
                }
                var result = data.ToArray();
                _session.Decrypt(result);

                OnClientToServerDecrypted(result);
            }
            else
            {
                _client.Write(data, 0, data.Length);
                TryInitialize();
            }
        }

        public void ServerToClient(byte[] data, int needToSkip=0)
        {
            if (Initialized)
            {
                if (needToSkip > 0)
                {
                    var skip = new byte[needToSkip];
                    _session.Encrypt(skip);
                }
                var result = data.ToArray();
                _session.Encrypt(result);
                OnServerToClientDecrypted(result);
            }
            else
            {
                _server.Write(data, 0, data.Length);
                TryInitialize();
            }
        }
    }
}