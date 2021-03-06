﻿using ClientServerUsingNamedPipes.Utilities;
using System.Threading.Tasks;

namespace ClientServerUsingNamedPipes.Interfaces
{
    /// <summary>
    /// Interface ICommunicationClient
    /// </summary>
    /// <seealso cref="ClientServerUsingNamedPipes.Interfaces.ICommunication" />
    public interface ICommunicationClient : ICommunication
    {
        /// <summary>
        /// This method sends the given message asynchronously over the communication channel
        /// </summary>
        /// <param name="message"></param>
        /// <returns>A task of TaskResult</returns>
        Task<TaskResult> SendMessage(string message);
    }
}