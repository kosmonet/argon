namespace argon.client;

internal class Client
{
    private static void Main(string[] args)
    {
        using var game = new client.Game();
        game.Run();
    }
}