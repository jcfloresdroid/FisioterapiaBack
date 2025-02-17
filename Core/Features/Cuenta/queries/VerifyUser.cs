using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MediatR;
using Microsoft.IdentityModel.Tokens;

namespace Core.Features.Cuenta.queries;

public record VerifyUser : IRequest<VerifyUserResponse>
{
    public string token { get; set; }
};

public class VerifyUserHandler : IRequestHandler<VerifyUser, VerifyUserResponse>
{
    public async Task<VerifyUserResponse> Handle(VerifyUser request, CancellationToken cancellationToken)
    {
        bool response = ValidateToken(request.token);

        var resp = new VerifyUserResponse()
        {
            verify = response
        };
        
        return resp;
    }
    
    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("jWsHs48F2v5Pj9TzY3d7QgD6eM1qZyRvXoWnEgGw"); 
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "https://localhost:5054/",
                ValidAudience = "localhost",
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            // Token es válido
            return true;
        }
        catch
        {
            // Token no es válido
            return false;
        }
    }
}

public record VerifyUserResponse
{
    public bool verify { get; set; }
}