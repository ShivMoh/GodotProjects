extends Mob
class_name Enemy

var data = preload("res://global/data.gd")
@export var direction : int = -1

func _ready():
	var raycast: RayCast2D = self.get_node("raycast")
	var sprite : AnimatedSprite2D = self.get_node("topguy")

	if direction == 1:
		sprite.flip_h = true
		raycast.rotation_degrees = 180
	else: 
		sprite.flip_h = false
		raycast.rotation_degrees = 0

func _physics_process(delta):
	pass

func damage_player():
	data.player_health = data.player_health - 1
	
func if_colliding() -> int:
	
	var raycast: RayCast2D = self.get_node("raycast")
	var sprite : AnimatedSprite2D = self.get_node("topguy")
	
	if raycast.is_colliding:
		var object = raycast.get_collider()

		if object != null:
			
			if object.name == "player":
				return direction

			sprite.flip_h = !sprite.flip_h
			raycast.rotation_degrees += 180
			
			if direction == -1:
				return 1
			else: 
				return -1

	return direction

func take_damage():
	set_health(get_health() - 1)
