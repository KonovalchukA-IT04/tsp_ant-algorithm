using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tsp
{
    public class HelpMeth
    {
		public static int GreedyAgorithmLengthCalculation(int startingIndex, int[,] graph)
		{
			int length = 0;
			int minLengthIndex = 0;
			HashSet<int> visited = new HashSet<int>() { startingIndex };
			while (visited.Count != graph.GetLength(0))
			{
				for (int i = 0; i < graph.GetLength(1); i++)
				{
					if (graph[startingIndex, i] < graph[startingIndex, minLengthIndex] && !visited.Contains(i))
						minLengthIndex = i;
				}
				visited.Add(minLengthIndex);
				length += graph[startingIndex, minLengthIndex];
				startingIndex = minLengthIndex;
			}
			return length;
		}

		public static string PrintThePath(int[] shortestPath)
		{
			string s = "";
			foreach (int position in shortestPath)
				s += position + "->";
			return s;
		}

		public static void DrawMatrix(int[,] mat, int matrix_length)
		{
			for (int i = 0; i < matrix_length; i++)
			{
				for (int j = 0; j < matrix_length; j++)
				{
					Console.Write(mat[i, j] + ", ");
				}
				Console.WriteLine();
			}
		}
	}
}
