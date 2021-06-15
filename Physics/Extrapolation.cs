using System.Collections.Generic;


public static class Extrapolation
{
    public static (double, double) LinExtrapollation(IEnumerable<(double, double)> points)//это используем для зависимости U и КПД от I 
    {
        var res = new double[2];//ax+b
        var mat = new double[2][];
        mat[0] = new double[3] { 0, 0, 0 };
        mat[1] = new double[3] { 0, 0, 0 };
        foreach (var x in points)
        {
            mat[0][0] += x.Item1 * x.Item1;
            mat[0][1] += x.Item1;
            mat[0][2] += x.Item1 * x.Item2;
            mat[1][0] += x.Item1;
            mat[1][1] += 1;
            mat[1][2] += x.Item2;
        }
        var k = mat[1][0] / mat[0][0];
        for (int i = 0; i < 3; i++)
        {
            mat[1][i] -= mat[0][i] * k;
        }
        res[1] = mat[1][2] / mat[1][1];
        res[0] = (mat[0][2] - mat[0][1] * res[1]) / mat[0][0];
        return (res[0], res[1]);
    }

    public static (double, double) QuadExtrapollation(IEnumerable<(double, double)> points)//для P1 и P2
    {
        var res = new double[2];//ax^2+bx
        var mat = new double[2][];
        mat[0] = new double[3] { 0, 0, 0 };
        mat[1] = new double[3] { 0, 0, 0 };
        foreach (var x in points)
        {
            mat[0][0] += x.Item1 * x.Item1 * x.Item1 * x.Item1;
            mat[0][1] += x.Item1 * x.Item1 * x.Item1;
            mat[0][2] += x.Item1 * x.Item1 * x.Item2;
            mat[1][0] += x.Item1 * x.Item1 * x.Item1;
            mat[1][1] += x.Item1 * x.Item1;
            mat[1][2] += x.Item1 * x.Item2;
        }
        var k = mat[1][0] / mat[0][0];
        for (int i = 0; i < 3; i++)
        {
            mat[1][i] -= mat[0][i] * k;
        }
        res[1] = mat[1][2] / mat[1][1];
        res[0] = (mat[0][2] - mat[0][1] * res[1]) / mat[0][0];
        return (res[0], res[1]);
    }
    public static double Lin0Extrapollation(IEnumerable<(double, double)> points)//для P
    {
        double res = 0;
        double k = 0;
        foreach (var x in points)
        {
            res += x.Item1 * x.Item2;
            k += x.Item1 * x.Item1;
        }
        return res / k;
    }
}

