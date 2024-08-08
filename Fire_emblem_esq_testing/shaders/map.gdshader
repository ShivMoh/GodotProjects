shader_type canvas_item;
render_mode blend_premul_alpha;

uniform float darkness_factor = 0.0;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.

	COLOR = texture(TEXTURE, UV);
	
	//if (darkness_factor != 0.0) {
		//COLOR = COLOR - vec4(0.0, 0.0, 0.0, 0.0);
	//}

}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
