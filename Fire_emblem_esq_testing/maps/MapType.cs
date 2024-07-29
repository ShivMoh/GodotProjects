using System;
using Godot;

public partial class MapType : TileMap
{

    [Export]
    public string name = String.Empty;


    public readonly string path = String.Empty;

    [Export]
    public string next = String.Empty;

    public MapType(string name, string next) {
        this.name = name;
        this.next = next;
        this.path = this.GetPath();
    }
}