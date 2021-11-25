using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsp
{
    class Program
    {
        static void Main(string[] args)
        {
            Ants ant = new Ants();
            //ant.AntColonyAlgorithm(true, 20);
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("_______________________________NEW EXPERIMENT_______________________________");
                //GenerParams(ant);
                ParamChangingMode(ant);
            }
            Console.ReadKey();
        }

        static void ParamChangingMode(Ants ant)
        {
            int[,] distances_matrix = ant.GenereteDistances(ant.CitiesNumber);
            int Alpha_optimal = ant.Alpha;
            int Beta_optimal = ant.Beta;
            double Rho_optimal = ant.Rho;
            int AntNumber_optimal = ant.AntNumber;
            int Iterations_optimal = ant.Iterations;
            int Lmin_optimal = int.MaxValue;
            int Lmin_Iteration_optimal = ant.Iterations;
            int result = 0;

            ant.AntColonyAlgorithm(distances_matrix, true, 20);

            int Alpha_optimal_prev = 0;
            int Beta_optimal_prev = 0;
            double Rho_optimal_prev = 0;
            int AntNumber_optimal_prev = 0;

            while (!IsSame(Alpha_optimal, Beta_optimal, Rho_optimal, AntNumber_optimal, Alpha_optimal_prev, Beta_optimal_prev, Rho_optimal_prev, AntNumber_optimal_prev))
            {
                Alpha_optimal_prev = Alpha_optimal;
                Beta_optimal_prev = Beta_optimal;
                Rho_optimal_prev = Rho_optimal;
                AntNumber_optimal_prev = AntNumber_optimal;


                Lmin_optimal = int.MaxValue;
                //for (int current_param = 10; current_param <= (ant.CitiesNumber / 2); current_param += 10)
                //{
                //    ant.AntNumber = current_param;
                //    result = ant.AntColonyAlgorithm(distances_matrix, false, 0);
                //    if (Lmin_optimal >= ant.Lmin && result < Lmin_Iteration_optimal)
                //    {
                //        Lmin_optimal = ant.Lmin;
                //        Lmin_Iteration_optimal = result;
                //        AntNumber_optimal = current_param;
                //    }
                //}
                ant.AntNumber = AntNumber_optimal;
                //Lmin_optimal = int.MaxValue;
                for (int current_param = 2; current_param < 5; current_param++)
                {
                    ant.Alpha = current_param;
                    result = ant.AntColonyAlgorithm(distances_matrix, false, 0);
                    if (Lmin_optimal >= ant.Lmin && result < Lmin_Iteration_optimal)
                    {
                        Lmin_optimal = ant.Lmin;
                        Lmin_Iteration_optimal = result;
                        Alpha_optimal = current_param;
                    }
                }
                ant.Alpha = Alpha_optimal;
                //Lmin_optimal = int.MaxValue;
                for (int current_param = 2; current_param < 5; current_param++)
                {
                    ant.Beta = current_param;
                    result = ant.AntColonyAlgorithm(distances_matrix, false, 0);
                    if (Lmin_optimal >= ant.Lmin && result < Lmin_Iteration_optimal)
                    {
                        Lmin_optimal = ant.Lmin;
                        Lmin_Iteration_optimal = result;
                        Beta_optimal = current_param;
                    }
                }
                ant.Beta = Beta_optimal;
                //Lmin_optimal = int.MaxValue;
                for (double current_param = 0.2; current_param < 0.89; current_param += 0.1)
                {
                    ant.Rho = current_param;
                    result = ant.AntColonyAlgorithm(distances_matrix, false, 0);
                    if (Lmin_optimal >= ant.Lmin && result < Lmin_Iteration_optimal)
                    {
                        Lmin_optimal = ant.Lmin;
                        Lmin_Iteration_optimal = result;
                        Rho_optimal = current_param;
                    }
                }
                ant.Rho = Rho_optimal;
                //Lmin_optimal = int.MaxValue;
                for (int current_param = 10; current_param <= (ant.CitiesNumber / 2); current_param += 10)
                {
                    ant.AntNumber = current_param;
                    result = ant.AntColonyAlgorithm(distances_matrix, false, 0);
                    if (Lmin_optimal >= ant.Lmin && result < Lmin_Iteration_optimal)
                    {
                        Lmin_optimal = ant.Lmin;
                        Lmin_Iteration_optimal = result;
                        AntNumber_optimal = current_param;
                    }
                }
                ant.AntNumber = AntNumber_optimal;
                //Lmin_optimal = int.MaxValue;
            }

            ant.AntColonyAlgorithm(distances_matrix, true, 20);
            Console.WriteLine();

            Console.WriteLine($"Optimal:\nAlpha: {ant.Alpha}\nBeta: {ant.Beta}\nRho: {ant.Rho}\nAntNumber: {ant.AntNumber}");
        }

        static void GenerParams(Ants ant)
        {
            Random rnd = new Random();
            ant.Alpha = rnd.Next(2, 5);
            ant.Beta = rnd.Next(2, 5);
            ant.AntNumber = rnd.Next(ant.CitiesNumber / 10, ant.CitiesNumber / 2);
            ant.Rho = rnd.NextDouble();
            Console.WriteLine($"Generated:\nAlpha: {ant.Alpha}\nBeta: {ant.Beta}\nRho: {ant.Rho}\nAntNumber: {ant.AntNumber}");
        }

        static bool IsSame(int a, int b, double r, int ant, int a_pr, int b_pr, double r_pr, int ant_pr)
        {
            if (a == a_pr && b == b_pr && ant == ant_pr && r == r_pr)
            {
                Console.WriteLine("true");
                return true;
            }
            Console.WriteLine("false");
            return false;
        }
    }
}
