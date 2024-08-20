shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec2 newUV = UV;
	
	
	newUV = vec2(abs(pow(UV.x, 4.5))*sign(UV.x),abs(pow(UV.y, 2.5))*sign(UV.y));
	newUV += vec2(0, TIME*.1);
	newUV += vec2(sin(TIME+UV.y*10.)*.2,sin(TIME+UV.y*10.)*.1);
	newUV;
	
	vec4 col1 = texture(TEXTURE, newUV);
	//newUV = vec2(abs(pow(UV.x, 1.5))*sign(UV.x),abs(pow(UV.y, .5))*sign(UV.y));
	newUV += vec2(sin(TIME*.1+UV.x*1.)*.2,sin(TIME*.1+UV.x*10.)*.1);
	newUV *= .2;
	vec4 col2 = texture(TEXTURE, newUV);
	newUV += vec2(sin(TIME*.1+UV.x*1.)*.2,sin(TIME*.1+UV.x*10.)*.1);
	newUV *= 20.;
	vec4 col3 = texture(TEXTURE, newUV);
	COLOR = max(min(col1,col2),col3);//*vec4(1,sin(floor(sin(UV.x)*30.)),floor(sin(UV.y)*30.),1.);
	
	//mix()
	
	// Called for every pixel the material is visible on.
}

//void light() {
	// Called for every pixel for every light affecting the CanvasItem.
	// Uncomment to replace the default light processing function with this one.
//}
