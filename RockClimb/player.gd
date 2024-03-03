extends Mob
class_name Player

var bullet  = preload("res://player/bullet.tscn")

func _ready():
	pass

func _physics_process(delta):

	if Input.is_action_just_pressed("shoot"):
		fire_bullet(spawn_bullet())

func spawn_bullet():
	var bulletInstance : Bullet = bullet.instantiate()
	bulletInstance.position = bulletInstance.position + Vector2(10, 0)
	self.add_child(bulletInstance)
	
	return bulletInstance

func fire_bullet(bullet : Bullet):
	var tween : Tween = get_tree().create_tween()
	var tween2 : Tween = get_tree().create_tween()

	tween.tween_property(bullet, "position", bullet.position + Vector2(500, 0), 0.5)
	tween2.tween_property(bullet, "modulate:a", 0, 0.5);
	tween.tween_callback(bullet.queue_free)	
	
