using Entities.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.Response;

public class Response
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public Object? ResponseData { get; set; }
    public ErrorDetails? Error { get; set; }

    public static Response CreateErrorResponse(ErrorDetails errorDetails, string responseMessage, string responseCode)
    {
        return new Response
        {
            Error = errorDetails,
            ResponseData = null,
            ResponseCode = responseCode,
            ResponseMessage = responseMessage
        };
    }

    public static Response CreateSuccessResponse(Object responseData, string responseMessage)
    {
        return new Response
        {
            Error = null,
            ResponseCode = "00",
            ResponseData = responseData,
            ResponseMessage = responseMessage
        };
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
