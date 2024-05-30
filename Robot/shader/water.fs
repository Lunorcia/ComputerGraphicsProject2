#version 330 core
out vec4 FragColor;

in vec4 clipSpace;
in vec2 texCoords;

uniform sampler2D reflecTexture;//texture 0
uniform sampler2D refracTexture;//texture 1
uniform sampler2D dudvMap;//texture 2
uniform float moveFactor;

const float waveStrength = 0.02;

void main()
{
    vec2 ndc = (clipSpace.xy / clipSpace.w) / 2.0 + 0.5;
    vec2 refracTexCoords = vec2(ndc.x, ndc.y);
    vec2 reflecTexCoords = vec2(ndc.x, 1.0 - ndc.y);

    vec2 distortion = (texture(dudvMap, vec2(texCoords.x + moveFactor, texCoords.y)).rg * 2.0 - 1.0) * waveStrength;
    vec2 distortion2 = (texture(dudvMap, vec2(-texCoords.x + moveFactor, texCoords.y + moveFactor)).rg * 2.0 - 1.0) * waveStrength;
    vec2 totalDistortion = distortion + distortion2;

    refracTexCoords += totalDistortion;
    refracTexCoords = clamp(refracTexCoords, 0.001, 0.999);

    reflecTexCoords += totalDistortion;
    reflecTexCoords = clamp(reflecTexCoords, 0.001, 0.999);


    vec4 reflecColour = texture(reflecTexture, reflecTexCoords);
	vec4 refracColour = texture(refracTexture, refracTexCoords);

	FragColor = mix(reflecColour, refracColour, 0.5);
    FragColor = mix(FragColor, vec4(0.0, 0.3, 0.5, 1.0), 0.2);
}