class BreakInfinity
{
    public static void Main() 
    {
        BigNumber newBig = new BigNumber { mantissa = 1, exponent = 2000 };
        newBig = NormalizeBigNumber(newBig);
        Console.WriteLine(newBig.ToString("Scientific"));
        Console.WriteLine(newBig.ToString("Engineering"));
        Console.WriteLine(newBig.ToString("Alphabetic"));
    }
    public static BigNumber NormalizeBigNumber(BigNumber num)
    {

        if (num.mantissa == 0) return new BigNumber { mantissa = 0, exponent = 0 };
        if (Math.Abs(num.mantissa) >= 1.0 && Math.Abs(num.mantissa) < 10.0) return num;

        long tempExponent = (long)Math.Floor(Math.Log10(Math.Abs(num.mantissa)));
        num.mantissa /= Math.Pow(10, tempExponent);
        num.exponent += tempExponent;

        return num;
    }
    public static BigNumber AddBigNumber(BigNumber a, BigNumber b)
    {
        BigNumber numA = NormalizeBigNumber(a);
        BigNumber numB = NormalizeBigNumber(b);
        BigNumber add = new BigNumber { };

        if (numB.exponent > numA.exponent)
        {
            BigNumber numTemp = numA;
            numA = numB;
            numB = numTemp;
        }

        long diff = numA.exponent - numB.exponent;
        if (diff >= 17) return numA;

        numB.mantissa /= Math.Pow(10, diff);
        numB.exponent += diff;
        add = new BigNumber { mantissa = numA.mantissa + numB.mantissa, exponent = numA.exponent };

        return NormalizeBigNumber(add);
    }
    public static BigNumber SubBigNumber(BigNumber a, BigNumber b)
    {
        BigNumber numA = NormalizeBigNumber(a);
        BigNumber numB = NormalizeBigNumber(b);

        long diff = numA.exponent - numB.exponent;
        if (diff >= 17) return numA;
        if (diff <= -17) return NormalizeBigNumber(new BigNumber { mantissa = -numB.mantissa, exponent = numB.exponent });

        double adjustB = numB.mantissa / Math.Pow(10, numA.exponent - numB.exponent);

        BigNumber sub = new BigNumber
        {
            mantissa = numA.mantissa - adjustB,
            exponent = numA.exponent
        };

        return NormalizeBigNumber(sub);
    }
    public static BigNumber MultBigNumber(BigNumber a, BigNumber b)
    {
        BigNumber numA = NormalizeBigNumber(a);
        BigNumber numB = NormalizeBigNumber(b);
        BigNumber mul = new BigNumber { mantissa = numA.mantissa * numB.mantissa, exponent = numA.exponent + numB.exponent };
        return NormalizeBigNumber(mul);
    }
    public static BigNumber DivBigNumber(BigNumber a, BigNumber b)
    {
        BigNumber numA = NormalizeBigNumber(a);
        BigNumber numB = NormalizeBigNumber(b);

        if (numA.mantissa == 0 || numB.mantissa == 0) return new BigNumber { };

        BigNumber div = new BigNumber { mantissa = numA.mantissa / numB.mantissa, exponent = numA.exponent - numB.exponent };
        return NormalizeBigNumber(div);
    }
    public struct BigNumber
    {
        public double mantissa;
        public long exponent;
        public static BigNumber operator +(BigNumber a, BigNumber b) => BreakInfinity.AddBigNumber(a, b);
        public static BigNumber operator -(BigNumber a, BigNumber b) => BreakInfinity.SubBigNumber(a, b);
        public static BigNumber operator *(BigNumber a, BigNumber b) => BreakInfinity.MultBigNumber(a, b);
        public static BigNumber operator /(BigNumber a, BigNumber b) => BreakInfinity.DivBigNumber(a, b);



        public string ToString(string format)
        {
            if (format == "Scientific") return $"{this.mantissa:F2}e{this.exponent}";
            else if (format == "Engineering") 
            {
                long exp = this.exponent;
                double man = this.mantissa;

                long remainder = exp % 3;

                if (remainder < 0) remainder += 3;
                double adjMan = man * Math.Pow(10, remainder);
                long adjExp = exp - remainder;

                return $"{adjMan:F2}e{adjExp}";
            }
            else if (format == "Alphabetic")
            {
                if (this.exponent < 3) return this.mantissa.ToString("F2");
                long exp = this.exponent;
                double man = this.mantissa;
                long remainder = exp % 3;
                if (remainder < 0) remainder += 3;

                double adjMan = man * Math.Pow(10, remainder);
                long groupIndex = (exp - remainder) / 3;

                string[] standard = { "", "K", "M", "B", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };
                if (groupIndex < standard.Length) return standard[groupIndex];

                string[] ones = { "", "U", "D", "T", "Qa", "Qi", "Sx", "Sp", "Oc", "No" };
                string[] tens = { "", "Dc", "Vg", "Tg", "Qg", "Pg", "Hg", "Og", "Ng", "Ce" };
                string[] hundreds = { "", "Ct", "Dt", "Tr", "Qd", "Qn", "Ss", "St", "Ot", "Nn" };
                string[] thousands = { "", "Mi", "Dmi", "Tmi", "Qmi", "Pmi", "Hmi", "Smi", "Omi", "Nmi" };

                long adj = groupIndex - 10; // Decillion 시작점 보정

                int u = (int)(adj % 10);          // 1의 자리
                int t = (int)((adj / 10) % 10);   // 10의 자리
                int h = (int)((adj / 100) % 10);  // 100의 자리
                int m = (int)(adj / 1000);        // 1000의 자리

                if (m >= thousands.Length) return "e" + (groupIndex * 3);

                string suffix = ones[u];

                if (t == 0 && h == 0 && m == 0) suffix += "Dc";
                else if (t == 1) suffix += "Dc";
                else suffix += tens[t];

                suffix += hundreds[h];
                suffix += thousands[m];

                return $"{adjMan:F2} {suffix}";
            }
            else return $"{this.mantissa:F2}e{this.exponent}";
        }


    }


}