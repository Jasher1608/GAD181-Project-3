using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class KeySlot : MonoBehaviour
{
    [SerializeField] private KeySlotManager keySlotManager;
    [SerializeField] private PlayerController playerController;

    public Key key;
    public PlayerNumber playerNumber;

    KeyCode[] playerOneKeys = { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Z, KeyCode.X, KeyCode.C };
    KeyCode[] playerTwoKeys = { KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };

    [SerializeField] private int slotNumber;

    [SerializeField] private GameObject slot1;
    [SerializeField] private GameObject slot2;
    [SerializeField] private GameObject slot3;

    private bool isCycling = false;

    private Animator animator;

    [SerializeField] private Color incorrectColour;

    public int combo = 0;
    
    void Start()
    {
        NewKey(this.gameObject);
    }

    void Update()
    {
        if (slotNumber == 1 && !isCycling)
        {
            CheckInput();
        }
    }

    private void CheckInput() // Bad code, needs rewriting
    {
        if (playerNumber == PlayerNumber.PlayerOne)
        {
            // Iterate through Player One's keys
            foreach (KeyCode key in playerOneKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    // Check if the pressed key is the correct key
                    if (key == (KeyCode)Enum.Parse(typeof(KeyCode), Enum.GetName(typeof(Key), this.key)) && !isCycling)
                    {
                        StartCoroutine(CorrectKeyInput());
                    }
                    else
                    {
                        StartCoroutine(IncorrectKeyInput());
                    }
                    // Exit the loop to avoid checking unnecessary keys after one is found
                    break;
                }
            }
        }
        else if (playerNumber == PlayerNumber.PlayerTwo)
        {
            // Iterate through Player Two's keys
            foreach (KeyCode key in playerTwoKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    // Check if the pressed key is the correct key
                    if (key == (KeyCode)System.Enum.Parse(typeof(KeyCode), Enum.GetName(typeof(Key), this.key)) && !isCycling)
                    {
                        StartCoroutine(CorrectKeyInput());
                    }
                    else
                    {
                        StartCoroutine(IncorrectKeyInput());
                    }
                    // Exit the loop to avoid checking unnecessary keys after one is found
                    break;
                }
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

        combo++;

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

        if (combo % 3 == 0)
        {
            StartCoroutine(playerController.Attack());
        }

        isCycling = false;
    }

    private IEnumerator IncorrectKeyInput()
    {
        isCycling = true;

        combo = 0;

        slot1.transform.GetChild(0).GetComponent<SpriteRenderer>().color = incorrectColour;
        slot2.transform.GetChild(0).GetComponent<SpriteRenderer>().color = incorrectColour;
        slot3.transform.GetChild(0).GetComponent<SpriteRenderer>().color = incorrectColour;

        yield return new WaitForSeconds(2f);

        slot1.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        slot2.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        slot3.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;

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
