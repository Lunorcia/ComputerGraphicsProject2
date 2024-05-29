#version 330 core
out vec4 FragColor;
struct Material
{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float shininess;
};

struct DirLight
{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct PointLight
{
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};

struct SpotLight
{
	vec3 position;
	vec3 direction;
	float cutOff;
	float outerCutOff;

	float constant;
	float linear;
	float quadratic;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct LightController
{
	bool dirLightOn;
	bool pointLightOn;
	bool spotLightOn;
};

uniform Material material;
uniform DirLight dirLight;
uniform PointLight pointLight;
uniform SpotLight spotLight;

uniform LightController lightController;

uniform vec3 viewPos;
uniform vec3 lightPos;
uniform vec3 cameraPos;
uniform samplerCube skybox; //texture 0
uniform sampler2D shadowMap;//texture 1

uniform bool shadows;
uniform bool reflectionOn;


in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;
in vec4 FragPosLightSpace;

vec3 CalShadowLight(vec3 normal, vec3 viewDir);
vec3 CalDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
float ShadowCalculation(vec4 fragPosLightSpace);

void main()
{
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 result;
	if (reflectionOn)
	{
		vec3 I = normalize(FragPos - cameraPos);
		vec3 R = reflect(I, normalize(Normal));
		result = texture(skybox, R).rgb;
	}

	if(lightController.dirLightOn == true)
		result += CalDirLight(dirLight, norm, viewDir);
	if(lightController.pointLightOn == true)
		result += CalPointLight(pointLight, norm, FragPos, viewDir);
	if(lightController.spotLightOn == true)
		result += CalSpotLight(spotLight, norm, FragPos, viewDir);
	
	//vec3 emission = texture(material.emission, TexCoords).rgb;
	if(shadows)
	{
		result += CalShadowLight(norm, viewDir);
	}

	FragColor = vec4(result, 1.0);
}

vec3 CalShadowLight(vec3 normal, vec3 viewDir)
{
    vec3 lightColor = vec3(0.3);
    vec3 lightDir = normalize(lightPos - FragPos);
    // Ambient
    vec3 ambient = 0.2 * lightColor;

    // Diffuse
    float diff = max(dot(normal, lightDir), 0.0);
    vec3 diffuse = diff * lightColor;

    // Specular
    float spec = 0.0;
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
    vec3 specular = spec * lightColor;    
    // Calculate shadow
    float shadow = shadows ? ShadowCalculation(FragPosLightSpace) : 0.0;                      
    shadow = min(shadow, 0.75); // reduce shadow strength a little: allow some diffuse/specular light in shadowed regions
    vec3 lighting = (ambient + (1.0 - shadow) * (diffuse + specular));
    return lighting;
}

vec3 CalDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);
	vec3 reflectDir = reflect(-lightDir, normal);

	// ambient
	vec3 ambient = light.ambient * material.ambient;

	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * material.diffuse;

	// specular highlight
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;

	return (ambient + diffuse + specular);
}

vec3 CalPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);
	vec3 reflectDir = reflect(-lightDir, normal);

	// ambient
	vec3 ambient = light.ambient * material.ambient;

	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * material.diffuse;

	// specular highlight
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;

	// calculate attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0/(light.constant + light.linear * distance 
						+ light.quadratic * (distance * distance));

	diffuse *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}

vec3 CalSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
	vec3 lightDir = normalize(light.position - fragPos);
	vec3 reflectDir = reflect(-lightDir, normal);

	// ambient
	vec3 ambient = light.ambient * material.ambient;

	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuse = light.diffuse * diff * material.diffuse;

	// specular highlight
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;

	// calculate attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0/(light.constant + light.linear * distance 
						+ light.quadratic * (distance * distance));

	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	// calculate intensity
	float theta = dot(lightDir, normalize(-light.direction));
	float epsilon = light.cutOff - light.outerCutOff;
	float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

	diffuse *= intensity;
	specular *= intensity;

	return (ambient + diffuse + specular);
}

float ShadowCalculation(vec4 fragPosLightSpace)
{
    // perform perspective divide
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    // Transform to [0,1] range
    projCoords = projCoords * 0.5 + 0.5;
    // Get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
    float closestDepth = texture(shadowMap, projCoords.xy).r; 
    // Get depth of current fragment from light's perspective
    float currentDepth = projCoords.z;
    // Calculate bias (based on depth map resolution and slope)
    vec3 normal = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float bias = max(0.05 * (1.0 - dot(normal, lightDir)), 0.005);
    // Check whether current frag pos is in shadow
    // float shadow = currentDepth - bias > closestDepth  ? 1.0 : 0.0;
    // PCF
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);
    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }
    shadow /= 9.0;
    
    // Keep the shadow at 0.0 when outside the far_plane region of the light's frustum.
    if(projCoords.z > 1.0)
        shadow = 0.0;
        
    return shadow;
}