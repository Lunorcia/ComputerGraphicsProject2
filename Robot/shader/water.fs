#version 330 core
out vec4 FragColor;

in vec4 clipSpace;
in vec2 texCoords;
in vec3 FragPos;

uniform sampler2D reflecTexture;//texture 0
uniform sampler2D refracTexture;//texture 1
uniform sampler2D dudvMap;//texture 2
uniform sampler2D normalMap;//texture 3

uniform float moveFactor;
uniform vec3 cameraPos;
uniform vec3 lightPos;
uniform vec3 lightColour;

const float waveStrength = 0.02;
const float shineDamper = 20.0;
const float reflectivity = 0.6;

void main()
{
    
    vec2 ndc = (clipSpace.xy / clipSpace.w) / 2.0 + 0.5;
    vec2 refracTexCoords = vec2(ndc.x, ndc.y);
    vec2 reflecTexCoords = vec2(ndc.x, 1.0 - ndc.y);

    vec2 distortedTexCoords = (texture(dudvMap, vec2(texCoords.x + moveFactor, texCoords.y)).rg) * 0.1;
    distortedTexCoords = texCoords + vec2(distortedTexCoords.x, distortedTexCoords.y + moveFactor);
    vec2 totalDistortion = (texture(dudvMap, distortedTexCoords).rg * 2.0 - 1.0) * waveStrength;

    refracTexCoords += totalDistortion;
    refracTexCoords = clamp(refracTexCoords, 0.001, 0.999);

    reflecTexCoords += totalDistortion;
    reflecTexCoords = clamp(reflecTexCoords, 0.001, 0.999);


    vec4 reflecColour = texture(reflecTexture, reflecTexCoords);
	vec4 refracColour = texture(refracTexture, refracTexCoords);

    vec4 normalMapColour = texture(normalMap, distortedTexCoords);
    vec3 normal = vec3(normalMapColour.r * 2.0 - 1.0, normalMapColour.b, normalMapColour.g * 2.0 - 1.0);
    normal = normalize(normal);

    vec3 viewDir = normalize(FragPos - cameraPos);
    float refracFactor = dot(viewDir, vec3(0.0, 1.0, 0.0));

    vec3 lightDir = normalize(FragPos - lightPos);
    vec3 reflectedLight = reflect(lightDir, normal);
    float specular = max(dot(reflectedLight, viewDir), 0.0);
    specular = pow(specular, shineDamper);
    vec3 specularHightlight = lightColour * specular * reflectivity;


	FragColor = mix(reflecColour, refracColour, refracFactor);
    FragColor = mix(FragColor, vec4(0.0, 0.3, 0.5, 1.0), 0.2) + vec4(specularHightlight, 0.0);
}