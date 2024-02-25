extends Control


var health = 100

# Called when the node enters the scene tree for the first time.
func _ready():
	hide()

func set_value(value: int):
	print(value)
	$TextureProgressBar.value = value
