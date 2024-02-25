extends Area2D

var health = 100

func _ready():
	$Control.hide()
	
func _process(delta):
	pass

func set_value(value: int):
	$Control/TextureProgressBar.value = value

func _on_bullet_entered(area):
	if(area.name == "Bullet"):
		$Control.show()
		self.health -= 10
		set_value(health)
		
		if(health <= 0):
			enemy_death()
		
func enemy_death():
	var vanish_tween = get_tree().create_tween()
	vanish_tween.tween_property(self, "modulate:a", 0, 0.2)
	vanish_tween.tween_callback(self.queue_free)
	
