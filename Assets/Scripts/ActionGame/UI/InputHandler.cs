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

    private ActionCommand buttonFire1;
    private ActionCommand buttonFire2;
    private ActionCommand buttonRoll;
    private ActionCommand buttonJump;
    private MoveCommand joyStickMove;
    private MoveCommand ? repel;

    public MoveCommand Repel
    {
        get
        {
            return repel.Value;
        }
    }

    void Start()
    {
        joyStickMove = new MoveCommand(0, 0);
        buttonFire1 = ActionCommand.Sword;
        buttonFire2 = ActionCommand.Shoot;
        buttonRoll = ActionCommand.Roll;
        buttonJump = ActionCommand.Jump;    
    }

    public MoveCommand HandleJoyStickInput()
    {   
        if (repel != null)
        {
            return repel.Value;
        }
        if (joyStick.isDragged())
        {
            joyStickMove.horizontal = NormalizeInt(joyStick.PointPos.x);
            joyStickMove.vertical = NormalizeInt(joyStick.PointPos.y);
        }
        else
        {
            joyStickMove.horizontal = NormalizeInt(Input.GetAxisRaw("Horizontal"));
        }
        return joyStickMove;
    }

    public ActionCommand HandleButtonInput()
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
            return buttonJump;
        }

        //控制对象模式
        return ActionCommand.None;
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

    public void SetRepel(MoveCommand repel_)
    {
        if (repel_.type == MoveCommand.MoveType.repel)
        {
            repel = repel_;
        }
    }

    public void DestroyRepel()
    {
        repel = null;
    }
}