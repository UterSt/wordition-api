namespace Wordition.Application.DTO;

public class ErrorResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public string TraceId { get; set; }
}