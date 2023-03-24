using Microsoft.VisualStudio.TestTools.UnitTesting;
using лн1._2;
using System;
namespace TestMO1._2
{
    [TestClass]
    public class UnitTest1:Program
    {
        [TestMethod]
        public void Test1()
        {
           
            const int N = 3;
            double[,] P = new double[N + 1, N];
            double[] F = new double[N + 1];
            var (max_ind, min_ind) = (0, 0);
            double[] C = new double[N];
            double[] R = new double[N];
            double[] E = new double[N];
            P = initial_approximation(P);
            while (true)
            {
                F = calculation_function(P);
                (max_ind, min_ind) = search_extremes(F);
                C = center_gravity(P, max_ind);
                if (deviation(C, F))
                    break;
                else
                {
                    R = reflection(P, C, max_ind);
                    double FR = f(R);
                    if (FR < F[min_ind])
                    {
                        stretching(C, R, P, max_ind);
                    }
                    else
                    {
                        compression(C, R, P, F, max_ind, min_ind);
                    }

                }
            }

            Console.WriteLine("x1= " + C[0] + "  x2= " + C[1] + "  x3= " + C[2]);
            double res = f(C);
            Console.WriteLine("result = " + res);

            Assert.AreEqual(res, 0.007173842123317258);
            //0.000326879
        }
        
    }
    
}
