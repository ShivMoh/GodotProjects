shader_type canvas_item;
uniform sampler2D normal_map;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	NORMAL = 2.0 * texture(normal_map, UV).rgb - 1.0;
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
