using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static tsp.HelpMeth;

namespace tsp
{
    public class Ants
    {
		public int Iterations { get; set; } = 200;
		public int AntNumber { get; set; } = 16;
		public int CitiesNumber { get; set; } = 100;
		public int MinDistanceBetweenCities { get; set; } = 5;
		public int MaxDistanceBetweenCities { get; set; } = 150;
		public int Lmin { get; set; } = 0;
		public int Alpha { get; set; } = 2;
		public int Beta { get; set; } = 2;
		public double Rho { get; set; } = 0.799999;
		public int AntColonyAlgorithm(int[,] distances_matrix, bool display, int iterations_separator)
        {
			int[] Lk = new int[AntNumber];
			Random rnd_position = new Random();
			HashSet<int> visited_cities_collection = new HashSet<int>();
			//int[,] distances_matrix = GenereteDistances(CitiesNumber);
			//DrawMatrix(distances_matrix, CitiesNumber);
			double[,] pheromone_matrix = new double[CitiesNumber, CitiesNumber];

			for (int i = 0; i < pheromone_matrix.GetLength(0); i++)
            {
				for (int j = 0; j < pheromone_matrix.GetLength(1); j++)
                {
					pheromone_matrix[i, j] = 1;
				}
			}				

			Lmin = GreedyAgorithmLengthCalculation(0, distances_matrix);
			bool[,,] ant_memory = new bool[AntNumber, CitiesNumber, CitiesNumber];
			int[] current_shortest_pheromone_path;
			int Lpr;

			int CurrentAntPosition = 0;
			int NewAntPosition = 0;

			int Lmin_iteration = 0;

			for (int i = 0; i < Iterations; i++)
			{
				for (int j = 0; j < AntNumber; j++)
				{
					CurrentAntPosition = rnd_position.Next(0, CitiesNumber);
					visited_cities_collection.Add(CurrentAntPosition);
					while (visited_cities_collection.Count != CitiesNumber)
					{
						NewAntPosition = MoveAntToNewPosition(distances_matrix, CurrentAntPosition, visited_cities_collection, pheromone_matrix);
						if (NewAntPosition == int.MinValue)
						{
							if (display)
							{
								Console.WriteLine("__Conclusion__:");
								Console.WriteLine($"[Pure Minimun] on ({Lmin_iteration}) = [{Lmin}]");
								Console.WriteLine($"[Length by Greed Algorithm] = [{GreedyAgorithmLengthCalculation(0, distances_matrix)}]");
							}
							return Iterations;
						}
						Lk[j] += distances_matrix[CurrentAntPosition, NewAntPosition];
						ant_memory[j, CurrentAntPosition, NewAntPosition] = true;
						CurrentAntPosition = NewAntPosition;
						visited_cities_collection.Add(CurrentAntPosition);
					}
					visited_cities_collection.Clear();
				}
				UpdateThePheromoneMatrix(ref pheromone_matrix, ant_memory, Lk);
				current_shortest_pheromone_path = GetShortestPheromonePath(pheromone_matrix);
				Lpr = GetLpr(current_shortest_pheromone_path, distances_matrix);
				if (Lpr < Lmin)
                {
					Lmin = Lpr;
					Lmin_iteration = i;
				}
				visited_cities_collection.Clear();
				if (iterations_separator!=0 && i % iterations_separator == 0 && display)
				{
					Console.WriteLine($"[Iteration]: [{i}]");
					Console.WriteLine($"[Lpr] (Current minimum length) = [{Lpr}]");
					Console.WriteLine($"[Lmin] (Last minimun length -> ({Lmin_iteration})) = [{Lmin}]");
					//Console.WriteLine($"[The pass]:\n[{PrintThePath(current_shortest_pheromone_path)}]\n");
				}
			}
			if(display)
            {
				Console.WriteLine("__Conclusion__:");
				Console.WriteLine($"[Pure Minimun] = [{Lmin}]");
				Console.WriteLine($"[Length by Greed Algorithm] = [{GreedyAgorithmLengthCalculation(0, distances_matrix)}]");
			}

			return Lmin_iteration;
		}

		public int[,] GenereteDistances(int length)
		{
			Random rnd = new Random();
			int[,] distances_matrix = new int[length, length];
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length; j++)
				{
					distances_matrix[i, j] = (int)rnd.Next(MinDistanceBetweenCities, MaxDistanceBetweenCities);
					//distances_matrix[j, i] = distances_matrix[i, j];
				}

				distances_matrix[i, i] = int.MaxValue;
			}
			return distances_matrix;
		}

		int MoveAntToNewPosition(int[,] distances_matrix, int previous_position, HashSet<int> visited_cities_collection, double[,] pheromone_matrix)
		{
			Random move_rnd = new Random();
			int[] availablePasses =
			Enumerable.Range(0, distances_matrix.GetUpperBound(1) + 1)
												.Select(i => distances_matrix[previous_position, i])
												.ToArray();
			double fullProbability = 0;
			double pherToDistRelation;
			double sum = 0;
			for (int i = 0; i < availablePasses.Length; i++)
			{
				if (!visited_cities_collection.Contains(i))
				{
					pherToDistRelation = Math.Pow(pheromone_matrix[previous_position, i], Alpha) * Math.Pow((double)1 / availablePasses[i], Beta);
					fullProbability += pherToDistRelation;
				}
			}

			double[] availableProbabilities = availablePasses.Select((n, index) =>
			{
				if (visited_cities_collection.Contains(index))
					return 0;

				pherToDistRelation = Math.Pow(pheromone_matrix[previous_position, index], Alpha) * Math.Pow((double)1 / n, Beta);

				double probability = pherToDistRelation / fullProbability;
				sum += probability;
				return probability;
			}).ToArray();

			double sum1 = 0;
			double randomNum = move_rnd.NextDouble();
			for (int i = 0; i < availableProbabilities.Length; i++)
			{
				sum1 += availableProbabilities[i];
				if (sum1 >= randomNum) return i;
			}
			return int.MinValue;
		}

		int[] GetShortestPheromonePath(double[,] pheromone_matrix)
		{
			Dictionary<int, double> resultPath = new Dictionary<int, double>();
			int resultLength = 0,
				current_node = 0;
			resultPath.Add(0, int.MaxValue);
			while (resultPath.Count < pheromone_matrix.GetLength(0))
			{
				resultPath = resultPath
				.Concat(
					Enumerable.Range(0, pheromone_matrix.GetUpperBound(1) + 1)
					.Select(i => KeyValuePair.Create(i, pheromone_matrix[current_node, i]))
				.OrderByDescending(n => resultPath.ContainsKey(n.Key) ? .0 : n.Value))
				.Take(++resultLength)
				.ToDictionary(n => n.Key, n => n.Value);

				current_node = resultPath.Last().Key;
			}
			return resultPath.Keys.ToArray();
		}
		
		void UpdateThePheromoneMatrix(ref double[,] pheromone_matrix, bool[,,] ant_memory, int[] Lk)
		{
			for (int i = 0; i < pheromone_matrix.GetLength(0); i++)
			{
				for (int j = 0; j < pheromone_matrix.GetLength(1); j++)
				{
					pheromone_matrix[i, j] *= (1-Rho);
					pheromone_matrix[i, j] += СalculateDeltaPheromone(i, j, ant_memory, Lk);
				}
				pheromone_matrix[i, i] = 1;
			}
		}

		double СalculateDeltaPheromone(int from, int to, bool[,,] ant_memory, int[] Lk)
		{
			double num = 0.0;
			for (int i = 0; i < AntNumber; i++)
			{
				bool flag = ant_memory[i, from, to];
				num = flag ? (num + (((double)Lmin) / ((double)Lk[i]))) : (num + 0.0);
			}
			return num;
		}

		int GetLpr(int[] citiesMap, int[,] distances_matrix)
		{
			int num = 0;
			for (int i = 0; i < (citiesMap.Length - 1); i++)
			{
				num += distances_matrix[citiesMap[i], citiesMap[i + 1]];
			}
			return num;
		}
	}
}
