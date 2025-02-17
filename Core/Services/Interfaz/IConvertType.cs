using Microsoft.AspNetCore.Http;

namespace Core.Services.Interfaz;

public interface IConvertType
{
    Task<byte[]> uploadFile(IFormFile file);
    Task<byte[]> profilePicture();
}