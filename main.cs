using System;

public static void Main()
{
    BigNumber a = {1, 10};
    BigNumber b = {2, 20};


}

public BigNumber AddBigNumber(BigNumber a, BigNumber b)
{
    if (a.exponent == b.exponent)
        return new BigNumber { a.mantissa + b.mantissa;, a.exponent };
    else if (a.exponent > b.exponent)
    {
        long diff = a.exponent - b.exponent;
        if (diff > 17) return a;

        b.exponent += diff;
        b.mantissa /= 10 ^ diff;

        return new BigNumber { a.mantissa + b.mantissa; a.exponent };
    }
    else if (b.exponent > a.exponent)
    {
        long diff = b.exponent - a.exponent;
        if (diff > 17) return b;

        a.exponent += diff;
        a.mantissa /= 10 ^ diff;

        return new BigNumber { a.mantissa + b.mantissa; a.exponent };
    }
}

public struct BigNumber
{
    public float mantissa,
    public long exponent,
}