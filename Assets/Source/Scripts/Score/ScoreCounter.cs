using System;
using System.Collections;

public abstract class ScoreCounter : IScoreCounter
{
    protected Bike Bike;

    public abstract event Action<int> ScoreChanged;

    protected abstract void Inject(Bike bike);

    protected void Init(Bike bike)
    {
        Bike = bike;
    }

    protected IEnumerator Counter(Func<bool> condition, Action action)
    {
        while (IsAlive)
        {
            if (condition())
            {
                yield return null;
                continue;
            }

            action();

            yield return null;
        }
    }
}
