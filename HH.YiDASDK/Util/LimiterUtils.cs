namespace HH.YiDASDK;

internal class LimiterUtils(long duration, int limit)
{
    private int seq = 0;
    private readonly long bucket = duration;
    private readonly long[] t = new long[limit];
    private readonly Mutex mutex = new();

    public void TryBeforeRun()
    {
        mutex.WaitOne();
        int idx = seq;
        long now = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
        long interval = now - t[idx];
        if (interval < 0)
        {
            mutex.ReleaseMutex();
            Thread.Sleep((int)(bucket - interval));
            TryBeforeRun();
            return;
        }

        if (interval < bucket)
        {
            t[idx] += bucket;
            seq = (idx + 1) % t.Length;
            mutex.ReleaseMutex();
            Thread.Sleep((int)(bucket - interval));
        }
        else
        {
            t[idx] = now;
            seq = (idx + 1) % t.Length;
            mutex.ReleaseMutex();
        }
    }
}