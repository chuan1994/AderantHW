using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionDisplay : MonoBehaviour {

    [SerializeField]
    private Text category;

    [SerializeField]
    private Text question;

    [SerializeField]
    private InputField answer;

    public delegate void stringDelegate(string answer);
    public static event stringDelegate sendAnswer;

    private void Awake()
    {
        GameController.setQuestion += setQuestion;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setQuestion(Question question) {
        this.category.text = question.category;
        this.question.text = question.question;
    }

    public void SubmitAnswer() {
        string result = answer.text;
        answer.text = "";
        sendAnswer(result);
    }
}
