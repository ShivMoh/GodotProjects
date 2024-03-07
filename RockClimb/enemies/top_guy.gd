extends Enemy


var animated_sprite : AnimatedSprite2D
var hide_timer : Timer
var cool_down_timer : Timer
var random : RandomNumberGenerator
var cool_down : bool
var hidden_state : bool

func _ready():
	animated_sprite = get_node("sprite")
	random = RandomNumberGenerator.new()
	random.randomize()
	hide_timer = get_node("hide_timer")
	cool_down_timer = get_node("cool_down_timer")
	cool_down = false
	hidden_state = false
	SPEED = 200

func _physics_process(delta):
	if not is_on_floor():
		velocity.y += gravity * delta

	direction = if_colliding()
	
	if (randi() % 5000) < 40 and !cool_down:
		hide_()
	
	if !hidden_state:
		if direction:
			velocity.x = direction * SPEED
		else:	
			velocity.x = move_toward(velocity.x, 0, SPEED)
	else:
		velocity.x = 0
		
	move_and_slide()

func hide_() -> void:
	animated_sprite.play("hide")
	hide_timer.start()
	hidden_state = true
	
func unhide() -> void:
	animated_sprite.play("walk")
	hidden_state = false

func take_damage() -> void:
	if hidden_state:
		return

	set_health(get_health() - 2)

func _on_hide_timer_timeout():
	cool_down = true
	hide_timer.stop()
	unhide()
	cool_down_timer.start()

func _on_cool_down_timer_timeout():
	cool_down = false
	cool_down_timer.stop()
