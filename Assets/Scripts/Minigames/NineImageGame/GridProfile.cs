using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "GridProfile", menuName = "Scriptable Objects/GridProfile")]
public class GridProfile : ScriptableObject
{
    public Sprite[] sprites;
    public bool[] solution;
    public string message;
}
