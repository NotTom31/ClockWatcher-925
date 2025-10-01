using UnityEngine;
using System.Collections.Generic;

//Calls Serializable to process the data into a JSON or JSON into a Dictionary object.
[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();

    [SerializeField] private List<TValue> values = new List<TValue>();

    /// <summary>
    /// Prepares the dictoranry before serialzing to JSON by making the keys and values into a seperate lists.
    /// </summary>
    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();

        //Puts the data Dictionary object into the two lists.
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    /// <summary>
    /// Prepares the dictoranry after deserialzing to JSON by converting hte two lists back into a Dictionary.
    /// </summary>
    public void OnAfterDeserialize()
    {
        this.Clear();

        //If the count does not match, data got corrupted along the way.
        if (keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializeableDictionary but the keys: " + keys.Count.ToString() + " do not match the values: " + values.ToString());
        }

        //recreate the dictonary from the two lists.
        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
