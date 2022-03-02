using System;

namespace Dm.net.buffer
{
	internal static class Arrays
	{
		internal static T[] CopyOf<T>(T[] original, int newLength)
		{
			T[] array = new T[newLength];
			Array.Copy(original, array, newLength);
			return array;
		}

		internal static T[] CopyOfRange<T>(T[] original, int fromIndex, int toIndex)
		{
			int num = toIndex - fromIndex;
			T[] array = new T[num];
			Array.Copy(original, fromIndex, array, 0, num);
			return array;
		}

		internal static void Fill<T>(T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
		}

		internal static void Fill<T>(T[] array, int fromIndex, int toIndex, T value)
		{
			for (int i = fromIndex; i < toIndex; i++)
			{
				array[i] = value;
			}
		}
	}
}
