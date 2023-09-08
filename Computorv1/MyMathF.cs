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

    public static float Sqrt(float number)
    {
        
    }
}
