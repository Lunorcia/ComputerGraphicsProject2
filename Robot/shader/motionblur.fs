#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D currentFrame; //texture 0
uniform sampler2D previousFrame;//texture 1
uniform sampler2D velocityMap;	//texture 2
uniform int blurFactor;

void main()
{
    vec2 velocity = texture(velocityMap, TexCoords).rg;
    vec4 currentColor = texture(currentFrame, TexCoords);
    vec4 previousColor = texture(previousFrame, TexCoords);
	for(int i=1;i<=blurFactor;++i)
	{
		previousColor+=texture(previousFrame, TexCoords+ velocity);
	}
	previousColor /= (blurFactor+1);
	FragColor = mix(currentColor, previousColor, 0.5);

	// float blurScale = 10.0;
    // vec2 velocity = texture(velocityMap, TexCoords).xy * blurScale;
	// vec2 texOffset = blurScale * velocity / textureSize(currentFrame, 0); // 计算 texOffset

    // vec4 color = texture(currentFrame, TexCoords); // 当前像素的颜色
    // vec4 accumulatedColor = color;
    // int samples = 10; // 采样次数

	// for (int i = 1; i <= samples; ++i)
    // {
    //     // 正向采样
    //     accumulatedColor += texture(currentFrame, TexCoords + i * texOffset);
    //     // 反向采样
    //     accumulatedColor += texture(currentFrame, TexCoords - i * texOffset);
    // }

    // // 取平均值
    // accumulatedColor /= (2.0 * samples + 1.0);

    // FragColor = accumulatedColor;

}