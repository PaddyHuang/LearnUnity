using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sample : MonoBehaviour {

    public CardDeckInterface cardDeck;

    public Text selectedCardText;

    public Button nextSampleBtn;

    public Button prevSampleBtn;

    private string sampleDescription;
    	
    void Start()
    {
        nextSampleBtn.onClick.AddListener(OnNextClick);
        prevSampleBtn.onClick.AddListener(OnPrevClick);

        sampleDescription = selectedCardText.text;
    }

	// Update is called once per frame
	void Update () {
        Card currentCard = cardDeck.GetCurrentCard();
        
        if(currentCard != null)
            selectedCardText.text = sampleDescription + currentCard.tooltipMessage;
	}

    void OnPrevClick()
    {
		if (SceneManager.GetActiveScene().buildIndex > 0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    void OnNextClick()
    {
		if (SceneManager.GetActiveScene().buildIndex < 3)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
