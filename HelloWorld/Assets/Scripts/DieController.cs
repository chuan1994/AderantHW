using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieController : MonoBehaviour {

    [SerializeField]
    int maxNumOnDie = 6;

    [SerializeField]
    List<Sprite> diceFaces = new List<Sprite>();

    [SerializeField]
    Image diceImage;
    public delegate void rollDice(int value);
    public static event rollDice GetRandomRoll;

    private void Awake()
    {
    }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        GetComponentInChildren<Button>().interactable = true;
    }

    public void DiceRoll() {
        Random.InitState((int)System.DateTime.Now.Ticks);
        int value = Random.Range(0, maxNumOnDie) + 1;
        StartCoroutine(DiceRollAnimation(value));
    }

    IEnumerator DiceRollAnimation(int endVal) {
        float secondsElapsed = 0.0f;
        Random.InitState((int)System.DateTime.Now.Ticks);

        while (secondsElapsed < 2.0f) {
            float random = Random.Range(0.02f, 0.05f);
            secondsElapsed += random;
            yield return new WaitForSeconds(random);
            diceImage.sprite = diceFaces[ Random.Range(0, 6)];
        }

        diceImage.sprite = diceFaces[endVal - 1];
        yield return new WaitForSeconds(1f);

        GetRandomRoll(endVal);
    }
}
