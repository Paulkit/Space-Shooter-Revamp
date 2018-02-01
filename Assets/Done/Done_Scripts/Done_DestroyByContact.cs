using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;
    private Done_GameController gameController;

    //for implement hp
    public float minusHpValue;
    private Done_PlayerController playerController;

    //for implement enemy hp
    private EnemyHpController enemyHpController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        //access hp bar class
        GameObject playerControllerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<Done_PlayerController>();
        }
        //access enemy hp controller
        enemyHpController = gameObject.GetComponent<EnemyHpController>();
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "BoostUp")
        {
            return;
        }


        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            enemyHpController.minusEnemyHp(playerController.damagePerShot);
            if (enemyHpController.isDead())
            {
                gameController.AddScore(scoreValue);
                Destroy(gameObject);
            }
        }

        if (other.tag == "Player")
        {

            //touch User ship
            //explosion when get damage
            Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            gameController.AddScore(scoreValue);
            Destroy(gameObject);

            if (!playerController.playerAlive(minusHpValue))
            {
                Destroy(other.gameObject);
                gameController.GameOver();
            }
        }

        //Destroy (gameObject);
    }
}