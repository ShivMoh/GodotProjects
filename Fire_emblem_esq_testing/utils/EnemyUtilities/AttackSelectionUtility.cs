using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class AttackSelectionUtility {

    public static List<AttackMeta> sortAttacksByRange(List<AttackMeta> attackMetas) {
        return (List<AttackMeta>)attackMetas.OrderByDescending(attack => attack.attackTargetMeta.range);
    }

    public static List<AttackMeta> getAvailableAttacks(Character character) {
        if (character.attacks.Count() == 0) return new List<AttackMeta>();
        return character.attacks.FindAll(attack => attack.timesUsableUntilReset > 0);
    }
}