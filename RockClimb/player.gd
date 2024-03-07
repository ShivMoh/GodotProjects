extends Mob
class_name Player

var timer : Timer 
var hitbox : Area2D
var animation : AnimationPlayer
var is_detecting_type : bool
var object_type : String

func _ready():
	timer = get_node("timer")
	hitbox = get_node("hitbox")
	animation = get_node("animation")
	is_detecting_type = false
	
func _physics_process(delta):
	pass

func _on_hitbox_body_entered(body):	
	if body.is_in_group("enemy"):
		set_health(get_health() - 1) 
		set_collision(false)
		timer.start()
		animation.play("phase")

func _on_timer_timeout():
	set_collision(true)
	timer.stop()	
	animation.stop()
	
func set_collision(value : bool) -> void:
	hitbox.set_collision_mask_value(4, value)
	set_collision_mask_value(4, value)

func detect_object_type(type: String) -> void:
	object_type = type
	
func is_detecting() -> bool:
	return is_detecting_type
	
func _on_detectbox_body_entered(body):
	if body.is_in_group(object_type):
		is_detecting_type = true

func _on_detectbox_body_exited(body):
	if body.is_in_group(object_type):
		is_detecting_type = false
		
