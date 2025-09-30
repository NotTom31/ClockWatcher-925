using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
   [SerializeField] private List<TKey> keys = new List<TKey>();
   [SerializeField] private List<TValue> values = new List<TValue>();

    /// <summary>
    /// Prepares the dictoranry before serialzing to JSON
    /// </summary>
   public void OnBeforeSerialize() 
   { 
        keys.Clear();
        values.Clear();
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
   }

    /// <summary>
    /// Prepares the dictoranry after deserialzing to JSON
    /// </summary>
    public void OnAfterDeserialize() 
   {
        this.Clear();
        if(keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializeableDictionary but the keys: " + keys.Count.ToString() + " do not match the values: " + values.ToString());
        }

        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
   }
}
