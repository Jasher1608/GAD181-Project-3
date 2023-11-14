using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlotManager : MonoBehaviour
{
    public List<KeyPrefab> keyPrefabs;

    private Dictionary<Key, GameObject> keyDictionary;

    private void Awake()
    {
        keyDictionary = new Dictionary<Key, GameObject>();
        foreach (var keyPrefab in keyPrefabs)
        {
            keyDictionary[keyPrefab.key] = keyPrefab.prefab;
        }
    }

    public GameObject GetKeyPrefab (Key key)
    {
        return keyDictionary.TryGetValue(key, out var prefab) ? prefab : null;
    }
}

[System.Serializable]
public class KeyPrefab
{
    public Key key;
    public GameObject prefab;
}
