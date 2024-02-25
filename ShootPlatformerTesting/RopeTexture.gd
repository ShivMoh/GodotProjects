extends TextureRect
class_name Rope

@export var powerMeter : PowerMeter = null;

func _ready():
	assert(powerMeter != null)

func _physics_process(delta):
	
	if(Input.is_action_just_released("ui_text_delete")):
		# activates when fire button is released
		var tween = get_tree().create_tween()
		tween.tween_property(self, "size", Vector2(calculate_size(), 5), 0.2)
		tween.tween_callback(reset)
	
	if(Input.is_action_pressed("ui_text_delete")):
		# activates when fire button pressed on held down
		powerMeter.set_power_value(1)
	
	if(Input.is_action_pressed("RotateUp")):
		self.rotateRope(-1)
	
	if(Input.is_action_pressed("RotateDown")):
		self.rotateRope(1)
	
func calculate_size():
	if(powerMeter.get_power_value() < 8):
		return 80
	return powerMeter.get_power_value() * 10
	
func reset():
	powerMeter.value = 0
	var tween = get_tree().create_tween()
	tween.tween_property(self, "size", Vector2(80, 5), 0.5)
	
func rotateRope(direction):
	self.rotation_degrees += 1 * direction
	

