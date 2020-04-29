using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerLevelData
{
    public int level;
    public float speed;
    public float jumpPower;
    public float gravity;
    public PlayerLevelData(int level, float speed, float jumpPower, float gravity)
    {
        this.level = level;
        this.speed = speed;
        this.jumpPower = jumpPower;
        this.gravity = gravity;
    }
};

public class PlayerControler : MonoBehaviour
{
    JsonManager<PlayerLevelData> pld = new JsonManager<PlayerLevelData>();
    Rigidbody rig;

    bool isJumping;
    bool isTrappedState;
    bool isDead;
    bool isPlayedSound;
    public bool isGoal;
    GameObject winceStar;
    public Vector3 direction = new Vector3(0, 0, 0);
    public PlayerLevelData player;
    public int damagedCount = 0;
    public int starCount = 0;

    [HideInInspector]
    public Animation ani;
    SoundManager sm;

    private PlayerLevelData GetLevelData(int level)
    {
        return pld.list.Find(x => x.level == level);
    }
    public int UpdateLevelData(int level)
    {
        player.speed = GetLevelData(level).speed;
        player.jumpPower = GetLevelData(level).jumpPower;
        player.gravity = GetLevelData(level).gravity;
        return pld.list.Count;
    }
    public int GetMaxPlayerLevel() { return pld.list.Count; }
    public float GetSpeed() { return player.speed; }


    public void Setting()
    {
        //pld.list.Add(new PlayerLevelData(0, 1.5f, 3.0f, 10.0f));
        //pld.list.Add(new PlayerLevelData(1, 2.0f, 4.6f, 30.0f));
        //pld.Save("LevelPlayerData.json");
        pld.Load("LevelPlayerData.json");
        ani = GetComponent<Animation>();
        rig = GetComponent<Rigidbody>();
        sm = GameObject.FindGameObjectWithTag("GameManager").gameObject.GetComponent<SoundManager>();
        winceStar = gameObject.transform.Find("wince_star").gameObject;
    }
    public void Initialize()
    {
        UpdateLevelData(0);
        isJumping = false;
        isDead = false;
        isGoal = false;
        isTrappedState = false;
        isPlayedSound = false;
        winceStar.SetActive(false);
    }
    public bool IsMoving()
    {
        if (isTrappedState) return false;
        return true;
    }
    public void Movement()
    {
        if (!IsMoving()) return;
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            direction.y = 00.0f;
            transform.rotation = Quaternion.Euler(direction);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            direction.y = 90.0f;
            transform.rotation = Quaternion.Euler(direction);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (!isJumping)
            {
                rig.AddForce(Vector3.up * player.jumpPower * 100);

                sm.PlayPlayerSound(PLAYERSOUNDTYPE.JUMP);
                isJumping = true;
            }
        }
        rig.AddForce(Vector3.down * player.gravity);
    }
    IEnumerator TrapMovement()
    {
        isTrappedState = true;
        winceStar.SetActive(true);
        ani.CrossFade(ani.GetClip("11_wince").name, 1.0f);
        sm.PlayPlayerSound(PLAYERSOUNDTYPE.WINCE);
        yield return new WaitForSeconds(1.0f);

        isTrappedState = false;
        winceStar.SetActive(false);
        ani.CrossFade(ani.GetClip("02_Move").name, 1.0f);
    }
    public void finishGame()
    {
        player.speed = 0;
        rig.constraints = RigidbodyConstraints.FreezePositionY;
        if (!isDead)
        {
            if (!isPlayedSound)
            {
                sm.PlayPlayerSound(PLAYERSOUNDTYPE.CLEAR);
                isPlayedSound = true;
            }
        }
    }
    public bool IsDead()
    {
        if (isDead || transform.position.y < -3.0f)
        {
            if (!isPlayedSound)
            {
                sm.PlayPlayerSound(PLAYERSOUNDTYPE.DEAD);
                isPlayedSound = true;
            }
            return true;
        }
        return false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trap"))
        {
            sm.PlayPlayerSound(PLAYERSOUNDTYPE.MOMSTERMOVE);
            StartCoroutine(TrapMovement());
            damagedCount++;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            isDead = true;
        }
        if (other.gameObject.CompareTag("Goal"))
        {
            isGoal = true;
            sm.PlayPlayerSound(PLAYERSOUNDTYPE.CLEAR);
        }
        if (other.gameObject.CompareTag("Star"))
        {
            starCount++;
            sm.PlayPlayerSound(PLAYERSOUNDTYPE.GETSTAR);
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
