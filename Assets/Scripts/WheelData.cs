using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wheel", menuName = "Vertigo/Wheel Data")]

public class WheelData : ScriptableObject
{
    public List<SliceData> slices = new List<SliceData>();
}
