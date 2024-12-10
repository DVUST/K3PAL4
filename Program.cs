internal class Program
{
    static void Main()
    {
        const int backpackSize = 150;
        const int itemsCount = 100;
        const int backpacksCount = 100;
        const int maxIterations = 1000;

        BackpackManager manager = new(backpackSize, itemsCount, backpacksCount, maxIterations);
        manager.StartGeneticAlgo().Print();
    }
}
