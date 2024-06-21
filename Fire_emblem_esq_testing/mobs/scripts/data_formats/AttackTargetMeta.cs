using Godot;

public partial class AttackTargetMeta {
    public int range = 0;

    public int targetableCount = 0;

    public bool closeRange = true;
    public bool areaOfEffect = false;

    public int? radius = 1;

    public AttackTargetMeta(
        int range, 
        int targetableCount, 
        bool closeRange = true,
        bool areaOfEffect = false, 
        int radius = 1
    )
    {
        this.range = range;
        this.targetableCount = targetableCount;
        this.closeRange = closeRange;
        this.areaOfEffect = areaOfEffect;
        this.radius = radius;    
    }
}