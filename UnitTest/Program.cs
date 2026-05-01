using BCrypt.Net;

// 在程式中產生 hash：
string password = "Admin@1234";

string hash = BCrypt.Net.BCrypt.HashPassword(password);

Console.WriteLine($"Hash: {hash}");
