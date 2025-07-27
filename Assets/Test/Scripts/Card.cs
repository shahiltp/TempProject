using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static event Action<Card> OnCardClicked;

    public int cardValue { get; private set; }
    public bool isFlipped { get; private set; }
    public bool isMatched { get; private set; }

    private Button button;

    [SerializeField] private Image cardBackImage;
    [SerializeField] private Image cardFaceImage;
    [SerializeField] private TextMeshProUGUI faceText;

    void Awake()
    {
        button = GetComponent<Button>();

        //button= 

        button.onClick.AddListener(() => OnCardClicked?.Invoke(this));
    }

    public void Initialize(int value)
    {
        cardValue = value;
        faceText.text = cardValue.ToString();
    }

    public void FlipUp()
    {
        isFlipped = true;
        button.interactable = false;
        cardBackImage.gameObject.SetActive(false);
        cardFaceImage.gameObject.SetActive(true);
    }

    public void FlipDown()
    {
        isFlipped = false;
        button.interactable = true;
        cardFaceImage.gameObject.SetActive(false);
        cardBackImage.gameObject.SetActive(true);
    }

    public void OnMatchFound()
    {
        isMatched = true;
        isFlipped = true;
        button.interactable = false;

        var colors = button.colors;
        colors.disabledColor = new Color(0.7f, 1f, 0.7f, 0.7f);
        button.colors = colors;
    }
}