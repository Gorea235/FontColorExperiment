using System;

namespace FontColorExperiment.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }

        public Response() { }

        public Response(bool success)
        {
            Success = success;
        }

        public Response(bool success, Exception error) : this(success)
        {
            Error = error;
        }
    }
}
