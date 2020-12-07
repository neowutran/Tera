// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Tera.Game;

namespace Tera.Sniffing
{
    public interface ITeraSniffer
    {
        bool Enabled { get; set; }
        event Action<Message> MessageReceived;
        event Action<Server> NewConnection;

        event Action EndConnection;
        bool EnableMessageStorage { get; set; }
        ConcurrentQueue<Message> Packets { get; }
        bool Connected { get; set; }
        void ClearPackets();
        Queue<Message> GetPacketsLogsAndStop();
        event Action<string> Warning;
        void CleanupForcefully();
    }
}