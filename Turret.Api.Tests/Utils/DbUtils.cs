namespace Turret.Api.Tests.Utils;

public static class DbUtils
{
    public static string GenerateDbName(string prefix)
    {
        var id = Guid.NewGuid().ToString().Replace("-", "");
        return prefix + "_" + id;
    }

    public static string GenerateConnectionString(string dbName)
    {
        return $"Host=localhost;Port=7272;Database={dbName};Username=user;Password=password";
    }
}