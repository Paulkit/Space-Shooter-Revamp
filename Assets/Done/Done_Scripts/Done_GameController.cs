using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;

public class Done_GameController : MonoBehaviour
{
	public GameObject[] hazards;
	public GameObject darkRegion;
	public Vector3 spawnValues;
	public int hazardCount;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	
	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;
	public GUIText levelText;

	private bool gameOver;
	private bool winGame;
	private bool restart;
	private int level;
	private int score;
	private float targetScale = 0.461291f;// darkregion target scale size
	private float shrinkSpeed = 1.2f; // darkregion grow speed
	private bool shrinking = false;

    //vars for score rank
    public GUIText scoreRankText;
    public string path;
    private ScoreList _scoreList = new ScoreList();

    //vars for boost up items
    public GameObject[] boostUpItems;


    void Start ()
	{
		level = 1;
		winGame = false;
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		score = 0;
		UpdateScore ();
		StartCoroutine (SpawnWaves ());

        //for implement xml
        scoreRankText.text = "";
        path = Path.Combine(Application.dataPath, path);
        if (!File.Exists(path))
        {
            _scoreList.xmlCreator(path);
        }
        Load();
    }
	
	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}

		if (shrinking) {

			darkRegion.transform.GetChild(0).transform.localScale = Vector3.Lerp(darkRegion.transform.GetChild(0).transform.localScale, new Vector3(targetScale, 0.461291f, targetScale), Time.deltaTime*shrinkSpeed);
			darkRegion.transform.GetChild(1).transform.localScale = Vector3.Lerp(darkRegion.transform.GetChild(1).transform.localScale, new Vector3(targetScale, 0.461291f, targetScale), Time.deltaTime*shrinkSpeed);
			darkRegion.transform.GetChild(2).transform.localScale = Vector3.Lerp(darkRegion.transform.GetChild(2).transform.localScale, new Vector3(targetScale, 0.461291f, targetScale), Time.deltaTime*shrinkSpeed);
			darkRegion.transform.GetChild(3).transform.localScale = Vector3.Lerp(darkRegion.transform.GetChild(3).transform.localScale, new Vector3(targetScale, 0.461291f, targetScale), Time.deltaTime*shrinkSpeed);



				shrinking = false;
		}

    }
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);

		while (true)
		{
			
		
		
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
            itemGenerator();
            yield return new WaitForSeconds (waveWait);
            
			AddLevel (); // add level for each wave, 10 

			if (gameOver||winGame)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
		}
	}

	public void AddLevel(){
		level += 1;
		levelText.text = "Level " + level;

		if(level==2){ // remove prefab - Done_Asteroid 01 to Done_Enemy Ship
			hazards[0] = hazards[3];

		}

		if(level==3){ // remove prefab - Done_Asteroid 02 to Done_Enemy Ship Type B
			hazards[1] = hazards[4];

		}

		if(level==4){ // remove prefab - Done_Asteroid 03 to Done_Enemy Ship Type C
			hazards[2] = hazards[5];

		}

		if(level==5){  // increase dark space size
			
			targetScale = 30f;// darkregion target scale size

			shrinking = true;

		}

		if(level==6){  // increase dark space size

			targetScale = 60f;// darkregion target scale size

			shrinking = true;

		}




		if(level==7){  // increase dark space size

			targetScale = 200f;// darkregion target scale size

			shrinking = true;

		}

		if(level==8){  // increase dark space size

			targetScale = 400f;// darkregion target scale size

			shrinking = true;

		}


		if(level==9){  // increase dark space size

			targetScale = 800f;// darkregion target scale size

			shrinking = true;

		}


		if(level==10){ // level 10, win and end the game
			winGame = true;
			WinGame ();
		}

	}

	void Shrink() {
		shrinking = true;
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
	
	void UpdateScore ()
	{
		scoreText.text = "Score: " + score;
	}

	public void WinGame ()
	{
		gameOverText.text = "Win Game!\n\nRank List";
		scoreRankText.text = _scoreList.updateScoreList(score);
		Save();
		winGame = true;
	}


	public void GameOver ()
	{
        gameOverText.text = "Game Over!\n\nRank List";
        scoreRankText.text = _scoreList.updateScoreList(score);
        Save();
		gameOver = true;
	}

    private void Load()
    {
        var _serializer = new XmlSerializer(typeof(ScoreList));
        var _fstream = new FileStream(path, FileMode.Open);
        _scoreList = _serializer.Deserialize(_fstream) as ScoreList;
        _fstream.Close();
    }

    private void Save()
    {
        var _serializer = new XmlSerializer(typeof(ScoreList));
        var _fstream = new FileStream(path, FileMode.Create);
        _serializer.Serialize(_fstream, _scoreList);
        _fstream.Close();
    }

    //generate boost up item
    //can alos use by InvokeRepeating function
    void itemGenerator()
    {
        GameObject boostUpItem = boostUpItems[Random.Range(0, boostUpItems.Length)];
        Instantiate(boostUpItem);
    }
}