using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Randomizer;
using Turret.Api.Options;
using Turret.Api.Utils;

namespace Turret.Api.Services;

public interface ISecurityService
{
    byte[] GenerateSalt();
    byte[] HashPassword(IEnumerable<byte> plain, IEnumerable<byte> salt);
}

public class SecurityService : ISecurityService
{
    private readonly IOptions<TurretOptions> _options;

    public SecurityService(IOptions<TurretOptions> options)
    {
        _options = options;
    }

    public byte[] GenerateSalt()
    {
        return Encoding.UTF32.GetBytes(new RandomAlphanumericStringGenerator().GenerateValue());
    }

    public byte[] HashPassword(IEnumerable<byte> plain, IEnumerable<byte> salt)
    {
        using var hashAlgorithm = SHA256.Create();
        
        var input = plain.Concat(salt);
        if (!_options.Value.ApplicationPepper.IsNullOrWhiteSpace())
        {
            var pepperBytes = Encoding.UTF8.GetBytes(_options.Value.ApplicationPepper);
            input = input.Concat(pepperBytes);
        }
        var inputBytes = input.ToArray();
        
        var result = hashAlgorithm.ComputeHash(inputBytes);
        return result;
    }
}