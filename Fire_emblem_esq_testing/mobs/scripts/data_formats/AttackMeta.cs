using System;
using Godot;

public enum AttackType {
    CLOSE,
    RANGE,
    CLOSE_MULTI,
    RANGE_MULTI,
    CLOSE_OR_RANGE,
    CLOSE_OR_RANGE_MULTI    
}
public partial class AttackMeta : Node {

    public string name;
    public int power;

    public AttackType attackType;
    // perhaps effect in future
    // Effect effect
    public AttackMeta(string name, int power, AttackType attackType ) {
        this.name = name;
        this.power = power;
        this.attackType = attackType;
    }

}