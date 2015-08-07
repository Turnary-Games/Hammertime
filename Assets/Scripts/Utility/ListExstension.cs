using System;
using System.Collections;
using System.Collections.Generic;

// Source
// http://stackoverflow.com/questions/273313/randomize-a-listt-in-c-sharp

public static class ListExstension {

	private static Random rng = new Random();

	// Shuffle list
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = rng.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}

	// Pop item, aka remove and get
	public static T Pop<T>(this List<T> list) {
		return list.Pop (list.Count - 1);
	}

	public static T Pop<T>(this List<T> list, int index) {
		T item = list [index];
		list.RemoveAt(index);
		return item;
	}
}

