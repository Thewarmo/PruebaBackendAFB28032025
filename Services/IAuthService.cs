public interface IAuthService
{
    Task<string> GenerateTokenAsync(string username);
    bool ValidateCredentials(string username, string password);
}