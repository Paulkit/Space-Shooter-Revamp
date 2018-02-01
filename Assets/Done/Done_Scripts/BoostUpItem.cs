using UnityEngine;
using System.Collections;

public class BoostUpItem : MonoBehaviour
{
    public string boostUpTag = "";
    public float speed;
    public float boostUpTime;
    public float lifetime;
    public float remindTime;

    private float xPos, zPos;
    private Color color;
    private Done_PlayerController playerController;

    void Start()
    {
        GameObject playerControllerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerControllerObject != null)
        {
            playerController = playerControllerObject.GetComponent<Done_PlayerController>();
        }
        xPos = Random.Range(0, 14);
        zPos = Random.Range(0, 19);
        transform.position = new Vector3(xPos * speed - 7.0f, transform.position.y, zPos * speed - 4.5f);

        color = GetComponent<Renderer>().material.color;

        StartCoroutine(destroyRemind());
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        xPos += Time.deltaTime;
        zPos += Time.deltaTime;
        transform.position = new Vector3(Mathf.PingPong(xPos * speed, 14) - 7, transform.position.y, Mathf.PingPong(zPos * speed, 19) - 4.5f);
    }

    IEnumerator destroyRemind()
    {
        yield return new WaitForSeconds(lifetime - remindTime);
        while (true)
        {
            GetComponent<Renderer>().material.color = new Color(0.0f, 0.0f, 1.0f);
            yield return new WaitForSeconds(0.15f);
            GetComponent<Renderer>().material.color = color;
            yield return new WaitForSeconds(0.15f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (boostUpTag)
            {
                case "hpRecover":
                    playerController.HpRecover();
                    break;
                case "speedUp":
                    playerController.speedUp(boostUpTime);
                    break;
                case "damageUp":
                    playerController.DamageUp(boostUpTime);
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }

}



