using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TypingData", menuName = "Scriptable Objects/TypingData")]
public class TypingData : ScriptableObject
{
    public List<string> stringList;
}
