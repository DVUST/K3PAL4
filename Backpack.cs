using System;

public class Backpack
{
	public int MaxWeight { get; }
	public int CurWeight { get; private set; }
    public int CurValue { get; private set; }
    public List<BackpackItem> Items { get; }
	public bool[] SelectedItems { get; private set; }

	public Backpack(int size, List<BackpackItem> items, bool initializeSelectedItems = false)
	{
		MaxWeight = size;
		CurWeight = 0;
		CurValue = 0;
		Items = items;
		SelectedItems = new bool[Items.Count];
    }

	public void Fill()
	{
        var rand = new Random();
        while (SelectItem(rand.Next(Items.Count))) { }
    }

	public bool SelectItem(int itemIndex)
	{
		if (Items.Count == 0) return false;
		if (SelectedItems[itemIndex]) return true;
        if (!Fits(itemIndex)) return false;
        SelectedItems[itemIndex] = true;
		CurWeight += Items[itemIndex].Weight;
		CurValue += Items[itemIndex].Value;
		return true;
	}

    public bool Fits(int itemIndex)
    {
        return CurWeight + Items[itemIndex].Weight <= MaxWeight;
    }

    public Backpack Cross(Backpack another)
	{
		var newBacpack = new Backpack(MaxWeight, Items);
		var rand = new Random();
        for (int i = 0; i < SelectedItems.Length; i++)
        {
			if (rand.Next(100) < 50)
				newBacpack.CopySelection(this, i);
			else
				newBacpack.CopySelection(another, i);            
        }
        return newBacpack;
    }

	private void CopySelection(Backpack another, int itemIndex)
	{
        if (another.SelectedItems[itemIndex])
            SelectItem(itemIndex);
    }

	public Backpack? Mutate()
	{
		var rand = new Random();
		if (rand.Next(100) >= 10) return null;
		var newBackpack = Clone();
		int l = rand.Next(SelectedItems.Length);
		int r = rand.Next(SelectedItems.Length);
		(newBackpack.SelectedItems[l], newBackpack.SelectedItems[r]) = (newBackpack.SelectedItems[r], newBackpack.SelectedItems[l]);
		return newBackpack;
    }

	public void Improve()
	{
		if (CurWeight >= MaxWeight) return;
		Fill();
	}

	public Backpack Clone()
	{
		Backpack newBackpack = new(MaxWeight, Items);
        for (int i = 0; i < SelectedItems.Length; i++)
        {
			newBackpack.CopySelection(this, i);
        }
		return newBackpack;
    }
	
	public void Print()
	{
        for (int i = 0; i < SelectedItems.Length; i++)
        {
			if (SelectedItems[i])
				Console.Write($"[{i}]{Items[i].Weight}:{Items[i].Value} ");
        }
		Console.WriteLine();
        Console.WriteLine($"Value = {CurValue}; Weight = {CurWeight}; Items inside = {CountSelected()}");
    }
	
	public bool IsAlive()
	{
		int weight = 0;
        for (int i = 0; i < SelectedItems.Length; i++)
        {
			if (SelectedItems[i])
				weight += Items[i].Weight;
        }
		return weight <= MaxWeight;
    }

	public int CountSelected()
	{
		return SelectedItems.Where(i => i).Count();
	}
}
