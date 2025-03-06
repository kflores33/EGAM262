using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct NameAndScore // outside of monobehavior so its easier to reference across scripts
{
    public int Score;
    public string Name;
}
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int MaxScoreCount = 10;

    private List<NameAndScore> _highscoreList = new List<NameAndScore>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void Start()
    {
        string scoreFilePath = Path.Combine(Application.persistentDataPath, "scores.txt"); // credit to kaine for this addition btw

        if (!File.Exists(scoreFilePath)) // create file if it doesnt already exist
        {
            File.WriteAllText(scoreFilePath, "");
        }

        string[] fileLines = File.ReadAllLines(scoreFilePath); // put each line in the file into an array

        for (int i = 0; i < fileLines.Length; i += 2) // += 2 because each group of info takes 2 lines
        {
            if (i + 1 >= fileLines.Length) break; // break out of loop if there are no other lines in the file to run through

            NameAndScore score = new NameAndScore();
            score.Name = fileLines[i];
            bool couldParse = int.TryParse(fileLines[i + 1], out int parsedScore); // return true if parse works
            if (couldParse) score.Score = parsedScore; _highscoreList.Add(score);
        }
        _highscoreList.Sort(CompareScores);
    }

    private int CompareScores(NameAndScore a, NameAndScore b)
    {
        return b.Score.CompareTo(a.Score);
    }

    public int AcceptNewScore(NameAndScore newScore)
    {
        int newPlace = -1; // defines newPlace as outside of the high score range

        for (int i = 0; i < _highscoreList.Count; i++)
        {
            NameAndScore thisScore = _highscoreList[i];
            if (newScore.Score > thisScore.Score)
            {
                // THEY MADE IT, WOW!
                // let's insert them into the list
                _highscoreList.Insert(i, newScore);
                newPlace = i;

                // did this now make the list have more than the maximum score count?
                if (_highscoreList.Count > MaxScoreCount)
                {
                    _highscoreList.RemoveAt(_highscoreList.Count - 1); //remove bottom score
                }
                break;
            }
        }
        // catch the scenario where we didn't surpass any score, BUT
        // there is room for us in the list!
        if (newPlace < 0 && _highscoreList.Count < MaxScoreCount)
        {
            _highscoreList.Add(newScore);
            newPlace = _highscoreList.Count - 1;
        }
        return newPlace; // put the score into the new place
    }

    public int GetScoreCount() // to be referenced by other scripts
    {
        return _highscoreList.Count;
    }
    public void SaveScores()
    {
        string fileContent = "";

        for (int i = 0; i < _highscoreList.Count; i++)
        {
            NameAndScore thisScore = _highscoreList[i];

            fileContent += $"{thisScore.Name}\n{thisScore.Score}\n";
        }

        string scoreFilePath = Path.Combine(Application.persistentDataPath, "scores.txt");
        Debug.Log($"scores saved to: {scoreFilePath}");

        File.WriteAllText(scoreFilePath, fileContent);
    }
    public NameAndScore GetScoreAt(int idx)
    {
        return _highscoreList[idx];
    }

    public int GetMyRank(int score)
    {
        for (int i = 0; i < _highscoreList.Count; i++)
        {
            if (score > _highscoreList[i].Score)
            {
                return i; //this should be the position that the player will be at
            }
        }

        // add me to the botom of the list if the list isnt full yet
        if (_highscoreList.Count < MaxScoreCount)
        {
            return _highscoreList.Count;
        }

        return -1; //if i didnt get a high score at all and the lsit is full, give back a -1
    }
}
