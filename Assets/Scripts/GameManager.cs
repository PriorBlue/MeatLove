using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Slider Player1Score;
    public Slider Player2Score;
    public Text RemainingTime;
    public Text Score;
    public float ScrollingSpeed = 10f;
    public float PlatformHeight = 20f;
    public Vector2 PlatformWidth = new Vector2(20f, 100f);
    public Vector2 PlatformDelay = new Vector2(1f, 3f);
    public Platform[] Platform;
    public Player Player1;
    public Player Player2;
    public float maxTime = 15f;

    public Image Menu;
    public Image MeatWin;
    public Image MeatLost;

    public Color Player1Color;
    public Color Player2Color;

    private int[] playerScore = { 0, 0 };
    private float time = 0f;
    private int playerSwitch = 0;

    private bool isPause = false;

    void Start()
    {
        SetScore();

        Invoke("CreatePlatform", Random.Range(PlatformDelay.x, PlatformDelay.y));

        time = maxTime;

        Menu.gameObject.SetActive(false);
        MeatWin.gameObject.SetActive(false);
        MeatLost.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPause == false)
        {
            time = Mathf.Max(0f, time - Time.deltaTime);

            RemainingTime.text = string.Format("{0}:{1:00.}", Mathf.FloorToInt(time / 60f), time % 60f);

            if (time <= 0)
            {
                playerScore[1 - playerSwitch] += 1;
                WinLostScreen(1, 1 - playerSwitch);
                SetScore();
            }
        }
    }

    public void PlayerCollision()
    {
        playerScore[playerSwitch] += 1;
        WinLostScreen(0,  playerSwitch);
        SetScore();
    }

    public void SetScore()
    {
        Player1Score.value = playerScore[0] / 5f;
        Player2Score.value = playerScore[1] / 5f;

        if(playerScore[0] >= 5)
        {
            Application.LoadLevel("Kiss");
        }
        else if (playerScore[1] >= 5)
        {
            Application.LoadLevel("Cry");
        }
    }

    public void CreatePlatform()
    {
        var platform = Instantiate(Platform[Random.Range(0, int.MaxValue) % Platform.Length]);

        platform.GameManager = this;
        platform.transform.position = new Vector3(Random.Range(-8f, 8f), 8f, 0f);

        var width = Random.Range(PlatformWidth.x, PlatformWidth.y);

        platform.GetComponent<SpriteRenderer>().size = new Vector2(width, PlatformHeight);
        platform.GetComponent<BoxCollider2D>().size = new Vector2(width, 0.2f);

        //platform.transform.localScale = new Vector3(Random.Range(PlatformWidth.x, PlatformWidth.y), PlatformHeight, 0f);

        Invoke("CreatePlatform", Random.Range(PlatformDelay.x, PlatformDelay.y));
    }

    public void WinLostScreen(int type, int player)
    {
        isPause = true;

        Menu.gameObject.SetActive(true);
        MeatWin.gameObject.SetActive(type == 0);
        MeatLost.gameObject.SetActive(type == 1);

        Player1.gameObject.SetActive(false);
        Player2.gameObject.SetActive(false);

        Score.text = string.Format("Player {0} ({1})\nGets A Star", player + 1, player == 0 ? "WASD" : "ARROWS");
        Score.color = (player == 0 ? Player1Color : Player2Color);

        Invoke("NextRound", 3f);
    }

    public void NextRound()
    {
        isPause = false;

        Menu.gameObject.SetActive(false);

        Player1.gameObject.SetActive(true);
        Player2.gameObject.SetActive(true);

        time = maxTime;
        playerSwitch = 1 - playerSwitch;

        var tmpPos = Player2.startPosition;
        Player2.startPosition = Player1.startPosition;
        Player1.startPosition = tmpPos;

        Player1.ResetPosition();
        Player2.ResetPosition();

        var tmp = Player2.KeyLeft;
        Player2.KeyLeft = Player1.KeyLeft;
        Player1.KeyLeft = tmp;

        tmp = Player2.KeyRight;
        Player2.KeyRight = Player1.KeyRight;
        Player1.KeyRight = tmp;

        tmp = Player2.KeyJump;
        Player2.KeyJump = Player1.KeyJump;
        Player1.KeyJump = tmp;
    }
}
