using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveController : MonoBehaviour
{
    public static HeroMoveController instance;

    //当人物生成后，人物脚本会赋值给SceneController与HeroMoveController
    public Hero leftHero;
    public Hero rightHero;

    //地图脚本，判断人物是否可以移动
    public Map leftMap;
    public Map rightMap;

    public bool leftHeroWillMove;
    public bool rightHeroWillMove;

    [Header("Movement Attribute")]
    //移动一格需要的时间（除以每秒的物理帧数是秒数）
    public int TIME_SET;

    [Header("Movement Timer")]
    //剩余移动物理帧数
    public int timer;

    private InputManager inputManager;
    private Direction dir;
    //人物下一个FixedUpdate要走到的地方
    private Vector2 pos;

    void Awake()
    {
        if (this != null)
            instance = this;
        inputManager = gameObject.AddComponent<InputManager>();
    }

    void FixedUpdate()
    {
        //因为人物是子对象，加载较慢
        if (leftHero == null || rightHero == null)
        {
            return;
        }

        //完成本次移动再进行下次移动
        if(timer > 0)
        {
            timer--;
            if (leftHeroWillMove)
            {
                leftHero.Move(dir, pos);
            }
            if (rightHeroWillMove)
            {
                rightHero.Move(dir, pos);
            }
            return;
        }

        //让人物动画静止
        leftHero.Idle(dir);
        rightHero.Idle(dir);

        //如果该方向均无法移动，则不移动
        if (leftMap.WillAgainstTheWall(dir, leftHero.transform.localPosition)
            && rightMap.WillAgainstTheWall(dir, rightHero.transform.localPosition))
        {
            return;
        }

        //判断这一次指令两个人物的移动状态
        if (leftMap.WillAgainstTheWall(dir, leftHero.transform.localPosition))
        {
            leftHeroWillMove = false;
        }
        else
        {
            leftHeroWillMove = true;
        }
        if (rightMap.WillAgainstTheWall(dir, rightHero.transform.localPosition))
        {
            rightHeroWillMove = false;
        }
        else
        {
            rightHeroWillMove = true;
        }

        switch (dir)
        {
            case Direction.Idle:
                leftHeroWillMove = false;
                rightHeroWillMove = false;
                break;
            case Direction.Up:
                pos = Vector2.up / TIME_SET;
                timer = TIME_SET;
                break;
            case Direction.Down:
                pos = Vector2.down / TIME_SET;
                timer = TIME_SET;
                break;
            case Direction.Left:
                pos = Vector2.left / TIME_SET;
                timer = TIME_SET;
                break;
            case Direction.Right:
                pos = Vector2.right / TIME_SET;
                timer = TIME_SET;
                break;
        }
    }

    private void Update()
    {
        if (inputManager == null)
        {
            return;
        }

        dir = inputManager.GetDirection();
    }
}
