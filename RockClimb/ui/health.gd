extends Node2D

@onready var current_heart : Heart = $heart
@export var mob : Mob

func _process(delta):
	mob.set_health(10)
