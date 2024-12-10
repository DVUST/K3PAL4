using System;

public class BackpackItem
{
	public int Value { get; }
    public int Weight { get; }

	public BackpackItem(int minValue = 2, int maxValue = 11, int minWeight = 1, int maxWeight = 6)
	{
		var rand = new Random();
		Value = rand.Next(minValue, maxValue);
		Weight = rand.Next(minWeight, maxWeight);
	}
}
