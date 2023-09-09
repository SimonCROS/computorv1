namespace computorv1;

public static class MyMathF
{
    public static float Pow(float x, int n)
    {
        float pow = 1F;

        for (int i = 0; i < n; i++)
            pow *= x;

        return pow;
    }

    public static float Sqrt(float n)
    {
        float lo = Min(1, n);
        float hi = Max(1, n);
        float mid = 0;

        while (100 * lo * lo < n)
            lo *= 10;
        while (0.01 * hi * hi > n)
            hi *= 0.1f;

        for (int i = 0; i < 100; i++)
        {
            mid = (lo + hi) / 2;
            if (mid * mid == n)
                return mid;
            if (mid * mid > n)
                hi = mid;
            else
                lo = mid;
        }
        return mid;
    }

    public static float Min(float n1, float n2)
    {
        return n1 < n2 ? n1 : n2;
    }

    public static float Max(float n1, float n2)
    {
        return n1 > n2 ? n1 : n2;
    }
}
