using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class QuestionHandler : MonoBehaviour {

    [SerializeField]
    string url = "http://wolfewylie.com/cgi-bin/jeopardy.py?callback=?";


    List<string> countries = new List<string>();

    Dictionary<string, Question[]> questionMap = new Dictionary<string, Question[]>();

    int difficulty;

    private void Awake()
    {
        AssignDelegates();
    }

    // Use this for initialization
    void Start () {
        fetchQuestions();
	}

	// Update is called once per frame
	void Update () {
	}

    void fetchQuestions() {
        StartCoroutine(LoopRoutine());
    }

    IEnumerator LoopRoutine() {
        foreach(string country in countries) { 
            WWWForm form = new WWWForm();
            form.AddField("question", country );
            WWW www = new WWW(url, form);
            yield return StartCoroutine(WaitFor(www, country));
        }
    }

    IEnumerator WaitFor(WWW www, string country) {
        yield return www;

        if (www.error == null)
        {
            string result = ProcessResponse(www.text);
            Wrapper<Question> questionList = JsonUtility.FromJson<Wrapper<Question>>(result);
            
            Debug.Log(country + " : " + questionList.items.Length);
            questionMap.Add(country, questionList.items);
        }

        else {
            Debug.Log(www.error.ToString());
        }
    }

    string ProcessResponse(string input) {
        string pattern = "(\\?( )*\\()(.+)(\\)( )*)";
        Match match = Regex.Match(input, pattern);
        string result = match.Groups[3].ToString();

        StringBuilder sb = new StringBuilder();
        sb.Append("{\"items\" : ");
        sb.Append(result);
        sb.Append("}");

        return sb.ToString();
    }

    void AssignDelegates()
    {
        GameController.setGlobalDifficulty += setDifficulty;
        CountryAssignment.ListReadyEvent += setCountryList;
        LandingController.landingActivated += getRandomQuestion;
    }

    void setCountryList(List<string> countries) {
        this.countries = countries;
    }

    Question getRandomQuestion(string country) {
        if (questionMap.ContainsKey(country)) {
            Question[] list = questionMap[country];

            //Get list of questions with difficulty lower than specified value (ignore questions where country was answer)
            List<Question> narrowedList = new List<Question>();
            foreach (Question q in list) {
                if (q.dollars.Trim().ToLower().Equals("none")) {
                    narrowedList.Add(q);
                    continue;
                }

                int currentDifficulty = int.Parse(Regex.Replace(q.dollars,"[^0-9]+",string.Empty));
                if (currentDifficulty <= difficulty && !q.answer.Trim().ToLower().Contains(country.Trim().ToLower())) {
                    narrowedList.Add(q);
                }
            }

            if (narrowedList.Count > 0) {
                Random.InitState((int)System.DateTime.Now.Ticks);
                int rand = Random.Range(0, narrowedList.Count);
                return narrowedList[rand];
            }
        }
        return null;
    }

    public static void printQuestion(Question qD) {
        Debug.Log("Category: " + qD.category);
        Debug.Log("Question: " + qD.question);
        Debug.Log("Difficulty: " + qD.dollars);
        Debug.Log("Answer: " + qD.answer);
    }

    void setDifficulty(int diff) {
        this.difficulty = diff;
    }
}