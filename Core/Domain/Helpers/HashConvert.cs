using HashidsNet;
using Microsoft.Extensions.Configuration;

namespace Core.Domain.Helpers;

public static class HashConvert
{
    private static IConfiguration _config;
    public static void Configure(IConfiguration config) {
        _config = config;
    }
    
    public static string HashId(this int number) =>
        GetHasher().Encode(number);

    public static int HashIdInt(this string encoded) =>
        GetHasher().Decode(encoded).FirstOrDefault();
    
    private static Hashids GetHasher() => new(_config["HashIdSalt"], 16);
}