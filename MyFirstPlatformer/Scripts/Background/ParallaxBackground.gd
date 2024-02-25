extends ParallaxBackground

@export var scrolling_speed = 300

func _process(delta):
	scroll_offset.x -=  scrolling_speed * delta
	scroll_offset.y += scrolling_speed * delta
