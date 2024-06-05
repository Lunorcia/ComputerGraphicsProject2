#ifndef FBO_H
#define FBO_H

#include <glad/glad.h>
#include <GLFW/glfw3.h>

#include <glm/glm.hpp>

#include <vector>

class FBO
{
public:
	GLuint ID;
	GLuint RBO;
	GLuint texture;
	GLuint depthTexture;
	// constructor
	FBO();

	GLuint CreateTextureAttatch();
	GLuint CreateDepthTextureAttatch();
	void CreateRBO();

	// bind
	void Bind();
	// unbind
	void Unbind();
	//delete
	void Delete();

private:
	const int SCREEN_WIDTH = 1600;
	const int SCREEN_HEIGHT = 900;
};


#endif // !VFO_H