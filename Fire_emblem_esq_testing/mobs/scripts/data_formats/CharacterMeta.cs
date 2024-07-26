using System.Collections.Generic;
using Godot;

public partial class CharacterMeta : Node {
    public Vector2I tileCoord;
    public string characterPath;
    public List<AttackMeta> attacks;

    public int group_id;
    public CharacterMeta(Vector2I tileCoord, string characterPath, List<AttackMeta> attacks, int group_id = 0) {
        this.tileCoord = tileCoord;
        this.characterPath = characterPath;
        this.attacks = attacks;
        this.group_id = group_id;
    }
};