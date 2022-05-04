using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("虚拟轴脚本")]
    public JoyStick joyStick;

    [Header("实例化的ActionButton脚本")]
    public ActionButton jumpBtn;
    public ActionButton rollBtn;
    public ActionButton swordAttackBtn;
    public ActionButton bulletAttackBtn;

    private Command buttonFire1;
    private Command buttonFire2;
    private Command buttonRoll;
    private Command buttonJump;
    private TranslationCommand joyStickMove;
    private TranslationCommand repel;

    public TranslationCommand Repel
    {
        get
        {
            return repel;
        }
    }

    void Start()
    {
        buttonFire1 = new SwordCommand();
        buttonFire2 = new ShootCommand();
        buttonRoll = new RollCommand();
        buttonJump = new JumpCommand();    
    }

    public TranslationCommand HandleJoyStickInput()
    {   
        if (repel != null)
        {
            return repel;
        }

        if (joyStick.isDragged())
        {
            joyStickMove = new MoveCommand(NormalizeInt(joyStick.PointPos.x), NormalizeInt(joyStick.PointPos.y));
        }
        else
        {
            joyStickMove = new MoveCommand(NormalizeInt(Input.GetAxisRaw("Horizontal")), 0);
        }
        return joyStickMove;
    }

    public Command HandleButtonInput()
    {
        if (Input.GetButtonDown("Fire1") || swordAttackBtn.GetActionButtonDown())
        {
            return buttonFire1;
        }
        if (Input.GetButtonDown("Fire2") || bulletAttackBtn.GetActionButtonDown())
        {
            return buttonFire2;
        }
        if (Input.GetButtonDown("Roll") || rollBtn.GetActionButtonDown())
        {
            return buttonRoll;
        }
        if (Input.GetButtonDown("Jump") || jumpBtn.GetActionButtonDown())
        {
            if (buttonJump == null)
            {
                Debug.Log("nullkkk");
            }
            return buttonJump;
        }

        //控制对象模式
        return null;
    }

    public int NormalizeInt(float x)
    {
        if (x > 0)
        {
            return 1;
        }
        if (x < 0)
        {
            return -1;
        }
        return 0;
    }

    public void SetRepel(TranslationCommand repel_)
    {
        if (repel == null)
        {
            repel = repel_;
        }
    }

    public void DestroyRepel()
    {
        repel = null;
    }
}