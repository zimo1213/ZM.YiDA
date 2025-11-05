namespace HH.YiDASDK.Core;

internal static class LimiterSingleton
{
    private static readonly LimiterUtils DefaultLimiter = new(1000, 3);

    public static void Invoke()
    {
        DefaultLimiter.TryBeforeRun();
    }

    static LimiterSingleton()
    {
    }
}