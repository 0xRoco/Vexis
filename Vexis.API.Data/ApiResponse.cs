namespace Vexis.API.Data;

public class ApiResponse
{
    public string Message { get; }

    public bool Success { get; }
    public dynamic? Data { get; }

    public ApiResponse(string message, bool success, dynamic? data)
    {
        Message = message;
        Success = success;
        Data = data;
    }
}