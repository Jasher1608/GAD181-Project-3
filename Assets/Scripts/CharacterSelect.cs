using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private Image playerOneImage;
    [SerializeField] private Image playerTwoImage;
    [SerializeField] private TMP_Dropdown playerOneDropdown;
    [SerializeField] private TMP_Dropdown playerTwoDropdown;

    [SerializeField] private Sprite[] wizardSprites;
    [SerializeField] private Sprite[] witchSprites;

    private void Start()
    {
        playerOneDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(playerOneDropdown, playerOneImage);
        });
        playerTwoDropdown.onValueChanged.AddListener(delegate
        {
            DropdownValueChanged(playerTwoDropdown, playerTwoImage);
        });
    }

    private void DropdownValueChanged(TMP_Dropdown change, Image image)
    {
        switch (change.value)
        {
            case 0:
                image.gameObject.GetComponent<ImageAnimation>().sprites = wizardSprites;
                image.gameObject.GetComponent<ImageAnimation>().index = 0;
                image.gameObject.GetComponent<ImageAnimation>().frame = 0;
                break;
            case 1:
                image.gameObject.GetComponent<ImageAnimation>().sprites = witchSprites;
                image.gameObject.GetComponent<ImageAnimation>().index = 0;
                image.gameObject.GetComponent<ImageAnimation>().frame = 0;
                break;
        }
    }
}