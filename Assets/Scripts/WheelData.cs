using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Wheel", menuName = "Vertigo/Wheel Data")]

public class WheelData : ScriptableObject
{
    [Header("Visuals")]
    public Sprite wheelBaseSprite;
    public Sprite indicatorSprite;
    [Header("Slices")]
    public List<SliceData> slices = new List<SliceData>();
}
