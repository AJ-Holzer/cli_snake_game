namespace Fruits;

internal class Fruit
{
    int saturation = 0;

    public int Saturation
    {
        get => saturation;
        set
        {
            if (value < 0)
                throw new Exception("Saturation must be grater than 0!");

            saturation = value;
        }
    }
}