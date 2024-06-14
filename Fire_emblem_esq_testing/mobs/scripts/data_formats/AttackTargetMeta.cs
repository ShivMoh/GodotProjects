using Godot;

public partial class AttackTargetMeta {
    public int range = 0;

    public int targetableCount = 0;

    public bool areaOfEffect = false;

    public int? radius = 0;

    public AttackTargetMeta(
        int range, 
        int targetableCount, 
        bool areaOfEffect = false, 
        int radius = 0
    )
    {
        this.range = range;
        this.targetableCount = targetableCount;
        this.areaOfEffect = areaOfEffect;
        this.radius = radius;    
    }
}