using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionLoadScript : MonoBehaviour {

    [SerializeField]
    private Text percentVal;

    [SerializeField]
    private Image loadingBar;

    int currentVal = 0;

    private void Awake()
    {
        AssignDelegates();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, ((float)currentVal) / 100f, Time.deltaTime);
    }

    void increaseBar(int percent) {
        if (percent < currentVal) {
            return;
        }

        percentVal.text = percent + "%";

        currentVal = percent;

        if (currentVal == 100) {
            loadingBar.fillAmount = 1f;
        }
    }

    void AssignDelegates() {
        QuestionHandler.PercentSetsAdded += increaseBar;
    }
}
