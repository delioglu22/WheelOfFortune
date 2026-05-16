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
}
