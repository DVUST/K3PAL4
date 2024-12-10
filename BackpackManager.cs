using System;
using System.Diagnostics;

public class BackpackManager
{
    public int BackpackSize { get; }
    public int ItemsCount { get; }
    public int BackpacksCount { get; }
    public int MaxIterations { get; }
    public List<Backpack> Backpacks { get; } = [];
    public List<BackpackItem> ItemsPool { get; } = [];
    public Backpack Best { get; private set; }

    public BackpackManager(int backpackSize, int itemsCount, int backpacksCount, int maxIterations)
    {
        Debug.Assert(backpackSize > 0);
        Debug.Assert(itemsCount > 0);
        Debug.Assert(backpacksCount > 0);
        Debug.Assert(maxIterations > 0);

        BackpackSize = backpackSize;
        ItemsCount = itemsCount;
        BackpacksCount = backpacksCount;
        MaxIterations = maxIterations;
        GenerateItems();
        GenerateBackpacks();
        SortBackpacks();
        Best = Backpacks[0];
    }

    private void GenerateItems()
    {
        ItemsPool.Clear();
        for (int i = 0; i < ItemsCount; i++)
        {
            ItemsPool.Add(new BackpackItem());
        }
    }

    private void GenerateBackpacks()
    {
        Backpacks.Clear();
        for (int i = 0; i < BackpacksCount; i++)
        {
            Backpacks.Add(new Backpack(BackpackSize, ItemsPool));
            Backpacks[i].SelectItem(i);
        }
    }

    private void SortBackpacks()
    {
        Backpacks.Sort((a, b) => b.CurValue - a.CurValue);
    }

    public (List<Backpack>, List<Backpack>) Split()
    {
        List<Backpack> a = [], b = [];
        int turn = 0;
        foreach (var backpack in Backpacks)
        {
            if (turn % 2 == 0)
                a.Add(backpack);
            else
                b.Add(backpack);
            turn = (turn + 1) % 2;
        }
        return (a, b);
    }

    public Backpack StartGeneticAlgo()
    {
        for (int i = 0; i < MaxIterations; i++)
        {            
            var (firstHalf, secondHalf) = Split();
            for (int j = 0; j < firstHalf.Count; j++)
            {
                var newBackpack = firstHalf[j].Cross(secondHalf[j]);
                if (!newBackpack.IsAlive()) continue;                
                var mutatedBackpack = newBackpack.Mutate();
                if (mutatedBackpack is not null && mutatedBackpack.IsAlive())
                    newBackpack = mutatedBackpack;
                newBackpack.Improve();
                InsertBackpack(newBackpack);
            }
            Best = Backpacks[0];
            if (i % 20 == 0)
                Console.WriteLine($"[{i}] Best: {Best.CurValue}");
        }
        return Best;
    }

    private void InsertBackpack(Backpack backpack)
    {
        int index = FindValue(backpack.CurValue);
        if (index <= ItemsCount)
        {
            Backpacks.Insert(index, backpack);
            Backpacks.RemoveAt(BackpacksCount - 1);
        }
    }

    private int FindValue(int value)
    {
        int low = 0, high = BackpacksCount - 1;
        while (low <= high)
        {
            int mid = (low + high) / 2;
            if (Backpacks[mid].CurValue == value)
                return mid;
            else if (Backpacks[mid].CurValue > value)
                low = mid + 1;
            else
                high = mid - 1;
        }
        if (Backpacks[low].CurValue >= value) low++;
        return low;
    }
}
