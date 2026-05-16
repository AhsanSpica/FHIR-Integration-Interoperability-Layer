using System;

public class LoginRequest
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class TokenResponse
{
    public string AccessToken { get; set; }
    public Error Error { get; set; }
}

public class Error
{
    public bool IsError { get; set; }
    public string Code { get; set; }
    public string Message { get; set; }
}
