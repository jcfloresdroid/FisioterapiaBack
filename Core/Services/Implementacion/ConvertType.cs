using Core.Services.Interfaz;
using Microsoft.AspNetCore.Http;

namespace Core.Services.Implementacion;

public class ConvertType : IConvertType
{
    /// <summary>
    /// Convierte un archivo a formato BLOB
    /// </summary>
    public async Task<byte[]> uploadFile(IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();

        return fileBytes;
    }

    public async Task<byte[]> profilePicture()
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Utils", "Perfil.png");
        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

        return fileBytes;
    }
}