extends State

var bullet_scene  = preload("res://player/bullet.tscn")

func Enter():
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
	var tween2 : Tween = get_tree().create_tween()

	tween.tween_property(bullet, "position", bullet.position + Vector2(500, 0), 0.5)
	tween2.tween_property(bullet, "modulate:a", 0, 0.5);
	tween.tween_callback(bullet.queue_free)
