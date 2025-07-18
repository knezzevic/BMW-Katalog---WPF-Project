﻿using System;
using System.Xml.Serialization;
using BMW_Katalog.Model;

[Serializable]
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }

    public User() { }

    public User(string username, string password, UserRole role)
    {
        Username = username;
        Password = password;
        Role = role;
    }
}
