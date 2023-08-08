using SuprimentosApi.Models;

namespace SuprimentosApi.Services;

public interface ITokenService
{
	string GetToken(string key, string issuer, string audience, UserModel user);
}
