using System;

namespace YandexDiscSync.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(System.Exception inner, string message)
            : base(message, inner)
        {
        }

        public ConfigurationException(string message)
            : base(message)
        {

        }
    }
}