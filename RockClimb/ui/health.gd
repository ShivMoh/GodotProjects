extends Node2D

var heart_scene = preload("res://ui/heart.tscn")
var current_heart : Heart 
var heart_animation : AnimationPlayer
var heart_animated_sprite : AnimatedSprite2D
var health : int
var hearts : Array[Heart]

@export var mob : Mob
@export var spacing : int = 20

func _ready():
	initialize_health()
	generate_health_bar()
	initialize()

func _physics_process(_delta):
	substract_health()		

func initialize_health() -> void:
	if mob:
		health = mob.get_health()
	else:
		health = 6


func generate_health_bar() -> void:
	for x in range(health/2):
		var heart : Heart = create_heart(x)
		hearts.append(heart)


func substract_health() -> void:

	if mob:
		initialize_health()
	
	if is_odd():
		heart_animated_sprite.play("half_heart")
		

	if is_heart_lost():
		heart_animated_sprite.play("no_heart")
		heart_animation.stop()
		pop()
		initialize()

	
func is_heart_lost():
	if ((len(hearts) * 2) - health) >= 2:
		return true
	return false

func is_odd() -> bool:
	if health % 2 != 0:
		return true
	return false

func pop() -> void:
	hearts.pop_back()
	
func initialize() -> void:
	
	if mob.get_health() <= 0:
		print(len(hearts))
		mob.queue_free()
		return
		
	current_heart = hearts[len(hearts) - 1]
	heart_animation = current_heart.get_node("animation")
	heart_animated_sprite = current_heart.get_node("animated_sprite")	

func create_heart(id) -> Heart:
	var heart_instance : Heart = heart_scene.instantiate()
	heart_instance.position = self.position + Vector2(id * spacing, 0)
	self.add_child(heart_instance)

	return heart_instance
