using System;

namespace ClassesLibrary.Client
{
    public static class GenerateClientId
    {
        public static (string, string) Id()
        {
            return (Guid.NewGuid().ToString(), Environment.MachineName);
        }
    }
}
