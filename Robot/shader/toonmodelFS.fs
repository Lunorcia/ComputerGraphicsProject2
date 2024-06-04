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

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;

vec3 CalDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec3 CalSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);

const int toonColorLevel = 4;
const float toonScaleFactor = 1.0f / toonColorLevel;

void main()
{
	vec3 norm = normalize(Normal);
	vec3 viewDir = normalize(viewPos - FragPos);
	vec3 result;

	if(lightController.dirLightOn == true)
		result += CalDirLight(dirLight, norm, viewDir);
	if(lightController.pointLightOn == true)
		result += CalPointLight(pointLight, norm, FragPos, viewDir);
	if(lightController.spotLightOn == true)
		result += CalSpotLight(spotLight, norm, FragPos, viewDir);
	
    vec3 lightDir = normalize(lightPos - FragPos);

	//toon shading
    // float diffFactor = max(dot(norm, lightDir), 0.0);
    // vec3 diffColor = vec3(0, 0, 0);
    // if(diffFactor > 0)
    // {
    //     diffFactor = ceil(diffFactor * toonColorLevel) * toonScaleFactor;
    //     diffColor = pointLight.diffuse * diffFactor * material.diffuse;

    // }
    // result *= diffColor;

    vec3 pixel;
    float intensity = max(dot(norm, lightDir), 0.0);
    
    if (intensity > 0.85)
        pixel = vec3(1.0, 1.0, 1.0);
    else if (intensity > 0.50)
        pixel = vec3(0.7, 0.7, 0.7);
    else if (intensity > 0.25)
        pixel = vec3(0.5, 0.5, 0.5);
    else
        pixel = vec3(0.3, 0.3, 0.3);

    result *= pixel;

	FragColor = vec4(result, 1.0);
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
    //toon shading
    if(diff > 0)
    {
        diff = ceil(diff * toonColorLevel) * toonScaleFactor;
        diffuse = light.diffuse * diff * material.diffuse;
    }

	// specular highlight
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;

	return (ambient + diffuse);
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
    //toon shading
    if(diff > 0)
    {
        diff = ceil(diff * toonColorLevel) * toonScaleFactor;
        diffuse = light.diffuse * diff * material.diffuse;
    }

	// specular highlight
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3 specular = light.specular * spec * material.specular;

	// calculate attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0/(light.constant + light.linear * distance 
						+ light.quadratic * (distance * distance));

	diffuse *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse);
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
    //toon shading
    if(diff > 0)
    {
        diff = ceil(diff * toonColorLevel) * toonScaleFactor;
        diffuse = light.diffuse * diff * material.diffuse;
    }

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

	return (ambient + diffuse );
}