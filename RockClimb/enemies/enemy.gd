extends CharacterBody2D
class_name  Enemy

var data = preload("res://global/data.gd")
@export var test = 0

func _ready():
    pass

func _physics_process(delta):
    pass

func damage_player():
    data.player_health = data.player_health - 1
    print(data)
