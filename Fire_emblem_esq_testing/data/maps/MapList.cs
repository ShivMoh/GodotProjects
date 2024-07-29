using System.Collections.Generic;
using System.IO;
using Godot;


public record MapNode
(
    string name,
    string path,
    string left,
    string right,
    string down,
    string up
);


public static partial class MapList {
    public static string map1 = "res://maps/test.tscn";
    
    public static readonly List<MapNode> mapNodes = new List<MapNode> {
        new MapNode("level_1", "res://maps/test.tscn", "none", "level_1", "none", "none"),
        new MapNode("level_2", "res://maps/test_2.tscn", "level_1", "level_1", "none", "none")
    };
    
}