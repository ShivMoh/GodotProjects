extends Node2D

var enemy = preload("res://scenes/mobs/enemy/enemy.tscn")

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	
func spawn():
	var ene = enemy.instantiate()
	var rng = RandomNumberGenerator.new()
	var randint = rng.randf_range(0, 2000)
	ene.global_position = Vector2(randint, 0)
	add_child(ene)
	print("spawning")


func _on_timer_timeout():
	spawn()
