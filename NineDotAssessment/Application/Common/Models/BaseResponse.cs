

namespace NineDotAssessment.Application.Common.Models;


public class BaseResponse<T>
{
    public BaseResponse()
    {
        StatusCode = 400;
        Message = "request unssuccessful";
        Data = default!;
    }


    public BaseResponse(T data, int statusCode = 200, string message = "Request successful")
    {
        StatusCode = statusCode;
        Message = message;
        Data = data;
    }
    public int StatusCode { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
}
