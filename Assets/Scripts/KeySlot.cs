using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySlot : MonoBehaviour
{
    [SerializeField] private KeySlotManager keySlotManager;

    public Key key;
    public PlayerNumber playerNumber;

    [SerializeField] private int slotNumber;

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;

    private bool isCycling = false;

    private Animator animator;
    
    void Start()
    {
        NewKey(this.gameObject);
    }

    void Update()
    {
        if (slotNumber == 1)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), System.Enum.GetName(typeof(Key), key))) && !isCycling)
            {
                StartCoroutine(CorrectKeyInput());
            }
        }
    }

    public void NewKey(GameObject slot)
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            slot.GetComponent<KeySlot>().key = (Key)Random.Range(0, 8);
        }
        else if (playerNumber == PlayerNumber.PlayerTwo)
        {
            slot.GetComponent<KeySlot>().key = (Key)Random.Range(9, 17);
        }

        var prefab = keySlotManager.GetKeyPrefab(slot.GetComponent<KeySlot>().key);
        if (prefab != null)
        {
            Instantiate(prefab, slot.transform);
        }
        else if (prefab == null)
        {
            Debug.LogError("Could not find prefab for " + key);
        }
    }

    private IEnumerator CorrectKeyInput()
    {
        //Debug.Log("You pressed the correct key!");

        isCycling = true;
        // Cycle keys
        if (slot1.transform.childCount > 0)
        {
            animator = slot1.transform.GetChild(0).GetComponent<Animator>();
            animator.SetTrigger("Pressed");

            yield return new WaitForSeconds(0.17f);
        }

        if (slot2.transform.childCount > 0)
        {
            Transform child = slot2.transform.GetChild(0);
            child.SetParent(slot1.transform);
            child.localPosition = new Vector3(0, 0, 0);

            slot1.GetComponent<KeySlot>().key = slot2.GetComponent<KeySlot>().key;
        }

        if (slot3.transform.childCount > 0)
        {
            Transform child = slot3.transform.GetChild(0);
            child.SetParent(slot2.transform);
            child.localPosition = new Vector3(0, 0, 0);

            slot2.GetComponent<KeySlot>().key = slot3.GetComponent<KeySlot>().key;

            NewKey(slot3);
        }

        isCycling = false;
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
