using System.Collections.Generic;
using System;
using UnityEngine;


namespace Assets.Scripts.Utilities
{
	public class PriorityQueue<T>
	{
		List<Pair<T, double>> nodes { get; set; }

		public int Count { get { return nodes.Count; } }

		public Pair<T, double> Top
		{
			get
			{
				if (nodes.Count < 2)
					throw new IndexOutOfRangeException();
				return nodes[1];
			}
		}

		public PriorityQueue()
		{
			nodes = new List<Pair<T, double>>();
			nodes.Add(null); //simplifies parent-child relations
		}

		void Swap(int first, int second)
		{
			Pair<T, double> temp = nodes[first];
			nodes[first] = nodes[second];
			nodes[second] = temp;
		}

		public void Push(T value, double priority)
		{
			int currentIndex = nodes.Count;
			int parentIndex = currentIndex / 2;
			nodes.Add(new Pair<T, double>(value, priority));

			while (nodes[parentIndex] != null)
			{
				if (nodes[currentIndex].second > nodes[parentIndex].second)//if current has lower priority (higher weight in our case) than parent
					break;

				Swap(currentIndex, parentIndex);
				currentIndex = parentIndex;
				parentIndex = currentIndex / 2;
			}
		}

		public T Pop()
		{
			if (nodes.Count < 2)
				throw new IndexOutOfRangeException();

			T result = nodes[1].first;
			nodes.RemoveAt(1); //remove root

			Rebuild();

			return result;
		}

		void Rebuild()
		{
			nodes.Insert(1, nodes[nodes.Count - 1]); //insert last leaf in place of root
			nodes.RemoveAt(nodes.Count - 1);

			int currentIndex = 1;
			int leftIndex = 2 * currentIndex;
			int rightIndex = 2 * currentIndex + 1;

			while (leftIndex < nodes.Count)
			{


				if (nodes[currentIndex].second > nodes[leftIndex].second && rightIndex >= nodes.Count)
				{
					Swap(currentIndex, leftIndex);
					currentIndex = leftIndex;
					leftIndex = 2 * currentIndex;
					rightIndex = 2 * currentIndex + 1;
				}
				else if (rightIndex < nodes.Count)
				{
					int childIndex = nodes[leftIndex].second <= nodes[rightIndex].second ? leftIndex : rightIndex;
					if (nodes[currentIndex].second > nodes[childIndex].second)
					{
						Swap(currentIndex, childIndex);
						currentIndex = childIndex;
						leftIndex = 2 * currentIndex;
						rightIndex = 2 * currentIndex + 1;
					}
					else
						break;
				}
				else
					break;
			}
		}
	}
}