extends Node2D


var gem = preload("res://objects/gem.tscn")
var eagle = preload("res://objects/eagle_ammo.tscn")

func _physics_process(delta):
	if Input.is_action_just_pressed("ui_text_delete"):
		fire_eagle()


func _on_timer_timeout():
	var gemTemp = gem.instantiate()
	var rng = RandomNumberGenerator.new()
	var randint = rng.randi_range(25, 1000)
	gemTemp.position = Vector2(randint, 455)
	add_child(gemTemp)

func fire_eagle():
	print("I am here")
	var eagleTemp = eagle.instantiate()
	var rng = RandomNumberGenerator.new()
	var randint = rng.randi_range(25, 1000)
	eagleTemp.position = $"../player".position 
	eagleTemp.position.x -= 400
	add_child(eagleTemp)
