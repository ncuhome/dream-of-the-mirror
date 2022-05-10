public struct MoveCommand
{
    public float horizontal { get; set; }
    public float vertical { get; set; }
    public MoveType type { get; } 
    public MoveCommand(float _x, float _y)
    {
        horizontal = _x;
        vertical = _y;
        type = MoveType.move;
    }

    public MoveCommand(float _x, float _y, MoveType _m)
    {
        horizontal = _x;
        vertical = _y;
        type = _m;
    }

    public enum MoveType
    {
        move,
        repel
    }
}