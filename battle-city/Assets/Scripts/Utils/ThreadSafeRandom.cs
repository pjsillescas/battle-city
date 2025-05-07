using System.Collections.Generic;
using System.Threading;
using System;

public static class ThreadSafeRandom
{
	[ThreadStatic] private static Random Local;

	public static Random ThisThreadsRandom
	{
		get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)); }
	}

	public static void FisherYatesShuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = ThisThreadsRandom.Next(n + 1);
			(list[n], list[k]) = (list[k], list[n]); // swap values k and n
		}
	}
}