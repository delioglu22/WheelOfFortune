using UnityEngine;

public enum SliceType
{
    Reward,
    Bomb
}

[System.Serializable]
public class SliceData
{
    public SliceType sliceType = SliceType.Reward;
    public Sprite icon;
    public int amount;

    public string GetAmountLabel()
    {
        if (amount >= 1000000)
            return "x" + (amount / 1000000f).ToString("0.#") + "M";

        if (amount >= 1000)
            return "x" + (amount / 1000f).ToString("0.#") + "K";

        return "x" + amount.ToString();
    }
}
