extends State

var bullet_scene  = preload("res://player/bullet.tscn")
var direction_indicator : bool 
var direction : int = 1
func Enter(_args = []):
	direction_indicator = _args[0]

	if direction_indicator:
		direction = 1
	else: 
		direction = -1

	print("I am on shoot state")
		# Transitioned.emit(self, "start")

func Physics_Update(delta: float):
	fire_bullet(spawn_bullet())
	Transitioned.emit(self, "start")
	
func spawn_bullet():
	var bulletInstance : Bullet = bullet_scene.instantiate()
	bulletInstance.position = bulletInstance.position + Vector2(10, 0)
	self.add_child(bulletInstance)
	return bulletInstance

func fire_bullet(bullet : Bullet):
	var tween : Tween = get_tree().create_tween()
	bullet.add_constant_central_force(Vector2(400 * direction, 0))
	tween.tween_property(bullet, "modulate:a", 0, 2);
	tween.tween_callback(bullet.queue_free)
