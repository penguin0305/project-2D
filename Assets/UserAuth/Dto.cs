using UnityEngine;
using System;

[Serializable]
public class RegisterRequest
{
    public string Id;
    public string Name;
    public string Email;
    public string Password;
    public string Nickname;
}

[Serializable]
public class LoginRequest
{
    public string Id;
    public string Password;
}

// 서버가 보내주는 응답을 받기 위한 클래스
[Serializable]
public class AuthResponse
{
    public string message;
    public string token;
    public string userId;
}
