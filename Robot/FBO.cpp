#include "FBO.h"

FBO::FBO()
{
	glGenFramebuffers(1, &this->ID);
	this->Bind();
}


GLuint FBO::CreateTextureAttatch()
{
	this->Bind();
	GLuint textureMap;
	glGenTextures(1, &textureMap);
	glBindTexture(GL_TEXTURE_2D, textureMap);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, SCREEN_WIDTH, SCREEN_HEIGHT, 0, GL_RGB, GL_UNSIGNED_BYTE, NULL);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, textureMap, 0);
	return textureMap;
}

GLuint FBO::CreateDepthTextureAttatch()
{
	this->Bind();
	GLuint textureMap;
	glGenTextures(1, &textureMap);
	glBindTexture(GL_TEXTURE_2D, textureMap);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, SCREEN_WIDTH, SCREEN_HEIGHT, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, textureMap, 0);
	return textureMap;
}

void FBO::Bind()
{
	glBindTexture(GL_TEXTURE_2D, 0);
	glBindFramebuffer(GL_FRAMEBUFFER, this->ID);
}

void FBO::Unbind()
{
	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

void FBO::Delete()
{
	glDeleteFramebuffers(1, &this->ID);
}
