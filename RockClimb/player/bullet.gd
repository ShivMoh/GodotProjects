extends RigidBody2D
class_name Bullet

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_hitbox_body_entered(body):
	
	if body.is_in_group("enemy"):
		body.take_damage()
		queue_free()
