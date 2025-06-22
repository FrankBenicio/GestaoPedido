﻿using System.Text.Json;

namespace GestaoPedido.API.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
