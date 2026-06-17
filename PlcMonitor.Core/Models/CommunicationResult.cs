namespace PlcMonitor.Core
{
    public class CommunicationResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static CommunicationResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static CommunicationResult<T> Fail(string msg) => new() { Success = false, ErrorMessage = msg };
    }
}
