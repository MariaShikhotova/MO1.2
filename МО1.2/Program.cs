using System;
namespace МО1._2
{
    public class Program
    {
        const int N = 3;
        const double beta = 2;
        const double alfa = 1;
        const double gama = 0.5;
        const double eps = 0.001;
        
        public static double f(double[] p)
        {
            return 4 * Math.Pow(p[0], 2) + 5 * Math.Pow(p[1] - 7, 2) + 2 * Math.Pow(p[2] + 3, 2);
        }
       
        public static double[,] initial_approximation(double[,] P)
        {
            
            for (int j = 0; j < N + 1; j++)//Начальное приближение
            {
                for (int k = 0; k < N; k++)
                {
                    if (j == 0)
                    {
                        P[j,k] = 0;
                    }
                    else
                    {
                        if (j-1==k)
                        {
                            P[j,k] = 1;
                        }
                        else
                        {
                            P[j,k] = 0;
                        }
                    }
                }
            }
            
            return P;
        }

        public static double[] calculation_function(double[,] P)
        {
           
            double[] F = new double[N+1];
            double[] row = new double[N];
            for (int j = 0; j < N + 1; j++)//Расчет функции
            {
                
                for (int i = 0; i < N; i++) 
                    row[i] = P[j, i];
                F[j] = f(row);
            }
            for (int j = 0; j < N + 1; j++)
            {
                for (int k = 0; k < N; k++)
                {
                    Console.Write("{0:F4}", P[j, k]);
                    Console.Write(" ");
                   
                }
                Console.Write("Значение функции - ");
                Console.Write("{0:F4}", F[j]);
                Console.WriteLine();
            }
            Console.WriteLine(new string('_', 50));
            
            return F;
        }
        public static (int,int) search_extremes (double[] F) {//поиск макс и мин значения индексов
            int max_ind = 0, min_ind = 0;
        for (int j = 0; j<N + 1; j++)
        {
            if (F[j] > F[max_ind])
            {
                    max_ind = j;
            }
            if (F[j] < F[min_ind])
            {
                    min_ind = j;
            }
        }
            return (max_ind, min_ind);
    }
        public static double[] center_gravity(double[,] P, int max_ind)//поиск центров тяжести
        {
            double[] C = new double[N];
            for (int k = 0; k <N; k++)
            {
                double SumP = 0;
                for (int j = 0; j < N + 1; j++)
                {
                    SumP = SumP + P[j,k];
                }
                C[k] = (1.0 / N) * (SumP - P[max_ind, k]);
                
            }
            
            return C;
        }
       

        public static bool deviation(double[] C, double[] F)//проверка погрешности
        {
           
            double fc = f(C);
           
            double Fsum = 0;
            for (int j = 0; j < N + 1; j++)
            {
                Fsum = Fsum + Math.Pow(F[j] - fc, 2);
            }
            double z = Math.Sqrt((1.0 / (N + 1)) * Fsum);
            
            if (z <= eps)
            {
                return true;//точность достигнута
            }
            else
                return false;//точность не достигнута
        }
        public static double[] reflection(double[,] P, double[] C, int max_ind)//отражение
        {
            double[] R = new double[N];
            for (int k = 0; k <= N - 1; k++)
            {
                R[k] = C[k] + alfa * (C[k] - P[max_ind, k]);
            }
            return R;
        }

        public static void stretching(double[] C, double[] R, double[,] P, int max_ind)//Растяжение
        {
           
           
            double[] E = new double[N];
            Console.WriteLine("Выполнено астяжение");
            for (int k = 0; k <= N - 1; k++)
            {
                E[k] = C[k] + gama * (R[k] - C[k]);
            }
            if (f(E) < f(R))//Проверка растяжения
            {
              
                for (int k = 0; k <= N - 1; k++)
                {
                    P[max_ind, k] = E[k];
                }
            }
            else
            {
                
                for (int k  = 0; k <= N - 1; k++)
                {
                    P[max_ind, k] = R[k];
                }
            }
        }

        public static void compression(double[] C, double[] R, double[,] P, double[] F, int max_ind, int min_ind)//сжатие
        {
           
            bool s = true;
            double FR = f(R);
            double[] S = new double[N];
            for (int j = 0; j < N + 1; j++)//Проверка
            {
                if (j != max_ind)
                {
                    if (FR < F[j])
                    {
                        s = false;
                        break;
                    }
                }
            }
            Console.WriteLine("Выполнено сжатие");
            if (s)
            {
                
                if (FR < F[max_ind])
                {
                  
                    for (int k = 0; k <= N - 1; k++)
                    {
                        P[max_ind, k] = R[k];
                    }
                }
               
                for (int k = 0; k <= N - 1; k++)//Сжатие
                {
                    S[k] = C[k] + beta * (P[max_ind, k] - C[k]);
                }
                if (f(S) < F[max_ind])
                {
                    
                    for (int k  = 0; k <= N - 1; k++)
                    {
                        P[max_ind, k] = S[k];
                    }
                }
                else
                {
                    
                    for (int j = 0; j < N + 1; j++)
                    {
                        for (int k  = 0; k <= N - 1; k++)
                        {
                            if (j != min_ind)
                            {
                                P[j,k] = P[min_ind, k] + gama * (P[j,k] - P[min_ind, k]);
                            }
                        }
                    }
                }
            }
            else
            {
              
                for (int k = 0; k <= N - 1; k++)
                {
                    P[max_ind, k] = R[k];
                }
            }
        }


        static void Main(string[] args)
        {
            double[,] P = new double[N+1,N];
            double[] F = new double[N+1];
            var (max_ind, min_ind)=(0,0);
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
                    R = reflection(P,C, max_ind);
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

            Console.WriteLine("x1= "+C[0]+"  x2= " +C[1] +"  x3= " +C[2]);
            double res = f(C);
            Console.WriteLine("result = " + res);


        }
    }


}
