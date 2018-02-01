using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Done_Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Done_Boundary boundary;

    public GameObject shot, powerUpShot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    //for HP bar implement
    public float playerTotalHP;
    private float current_HP = 0f;
    public GameObject healthBar;
    public Text hpText;

    //for shot damage of player ship
    public float damagePerShot;

    //for boost up item
    bool hasSpeedUp, hasDamageUp;
    float cur_speed, cur_damage, cur_fireRate;
    float speedUpTimer = 10.0f, damageUpTimer = 10.0f;
    public AudioClip boostUpSFX;

    void Start()
    {
        current_HP = playerTotalHP;
        hpText.text = ((int)playerTotalHP).ToString() + "/" + ((int)playerTotalHP).ToString();
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (hasDamageUp)
                Instantiate(powerUpShot, shotSpawn.position, shotSpawn.rotation);
            else
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            GetComponent<AudioSource>().Play();
        }

        if (hasSpeedUp)
        {
            if (speedUpTimer > 0)
                speedUpTimer -= Time.deltaTime;
            else {
                speed = cur_speed;
                speedUpTimer = 10.0f;
                hasSpeedUp = false;
            }
        }

        if (hasDamageUp)
        {
            if (damageUpTimer > 0)
                damageUpTimer -= Time.deltaTime;
            else {
                fireRate = cur_fireRate;
                damagePerShot = cur_damage;
                damageUpTimer = 10.0f;
                hasDamageUp = false;
            }
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3
        (
            Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
            0.0f,
            Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
        );
        GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

    public bool playerAlive(float _Value)
    {
        current_HP -= _Value;

        //this is to prevent player hp overflow by boostup item
        current_HP = current_HP > playerTotalHP ? playerTotalHP : current_HP;

        float cal_health = current_HP / playerTotalHP;
        hpText.text = ((int)current_HP <= 0 ? 0 : current_HP).ToString() + "/" + ((int)playerTotalHP).ToString();
        SetHealthBar(cal_health);
        if (current_HP <= 0)
            return false;   //hp less than 0
        return true;        //hp more than 0
    }

    public void SetHealthBar(float health)
    {
        healthBar.transform.localScale = new Vector3(Mathf.Clamp(health, 0f, 1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);
    }

    public void speedUp(float existTime)
    {
        GetComponent<AudioSource>().PlayOneShot(boostUpSFX);
        if (!hasSpeedUp)
        {
            cur_speed = speed;
            speed = speed * 1.5f;
            hasSpeedUp = true;
            speedUpTimer = existTime;
        }
        else
        {
            speedUpTimer += existTime;
        }
    }

    public void DamageUp(float existTime)
    {
        GetComponent<AudioSource>().PlayOneShot(boostUpSFX);
        if (!hasDamageUp)
        {
            cur_fireRate = fireRate;
            cur_damage = damagePerShot;
            fireRate = fireRate * 0.8f;
            damagePerShot = damagePerShot * 2.5f;
            hasDamageUp = true;
            damageUpTimer = existTime;
        }
        else
        {
            damageUpTimer += existTime;
        }
    }

    public void HpRecover()
    {
        GetComponent<AudioSource>().PlayOneShot(boostUpSFX);
        playerAlive(-playerTotalHP);
    }
}
