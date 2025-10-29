using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSequence", menuName = "Scriptable Objects/GridSequence")]
public class GridSequence : ScriptableObject
{
    public List<GridProfile> profiles;
}
