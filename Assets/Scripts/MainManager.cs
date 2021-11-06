using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public Text inputText;
    public int bestScore;
    
    public GameObject GameOverText;
    public string input;
   
    



    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        LoadName();
        BestScoreText.text ="Name: " + input + " Best score: " + bestScore;




        if (sceneName == "main")
        {

                const float step = 0.6f;
            int perLine = Mathf.FloorToInt(4.0f / step);

            int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
            for (int i = 0; i < LineCount; ++i)
            {
                for (int x = 0; x < perLine; ++x)
                {
                    Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }
        } }
    public void LoadName()

    {
        


        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            input = data.input;
            bestScore = data.bestScore;
            
            
            Debug.Log("Your name is " + input);
      

        }
       

    }
   



    public void ReadName()
    {
        input = inputText.text;
        BestScoreText.text = input;





        Debug.Log(input);

    }
    [System.Serializable]
    class SaveData
    {
        public string input;
        public int bestScore;
    }
    public void SavePlayerName()

    {
        SaveData data = new SaveData();
        data.input = input;
        data.bestScore = bestScore;
        string json = JsonUtility.ToJson(data);
        

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);



    }

    private void Update()
    {

        
       

        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
           


            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
   

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";

        if (m_Points > bestScore)
        {
            bestScore = m_Points;
            SavePlayerName();
        }


    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
