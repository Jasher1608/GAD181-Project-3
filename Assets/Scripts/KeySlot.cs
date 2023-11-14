using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlot : MonoBehaviour
{
    [SerializeField] private KeySlotManager keySlotManager;

    public Key key;
    public PlayerNumber playerNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            key = (Key)Random.Range(0, 8);
        }
        else if (playerNumber == PlayerNumber.PlayerTwo)
        {
            key = (Key)Random.Range(9, 17);
        }

        NewKey();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewKey()
    {
        var prefab = keySlotManager.GetKeyPrefab(key);
        if (prefab != null)
        {
            Instantiate(prefab, this.transform);
        }
        else if (prefab == null)
        {
            Debug.LogError("Could not find prefab for " + key);
        }
    }
}

public enum Key {
    Q,
    W,
    E,
    A,
    S,
    D,
    Z,
    X,
    C,
    Keypad1,
    Keypad2,
    Keypad3,
    Keypad4,
    Keypad5,
    Keypad6,
    Keypad7,
    Keypad8,
    Keypad9,
}

public enum PlayerNumber
{
    PlayerOne,
    PlayerTwo
}
