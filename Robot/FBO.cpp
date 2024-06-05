#include "FBO.h"

FBO::FBO()
{
	glGenFramebuffers(1, &this->ID);
	this->Bind();
}


GLuint FBO::CreateTextureAttatch()
{
	this->Bind();
	glGenTextures(1, &this->texture);
	glBindTexture(GL_TEXTURE_2D, this->texture);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, SCREEN_WIDTH, SCREEN_HEIGHT, 0, GL_RGB, GL_UNSIGNED_BYTE, NULL);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_TEXTURE_2D, this->texture, 0);
	return this->texture;
}

GLuint FBO::CreateDepthTextureAttatch()
{
	this->Bind();
	glGenTextures(1, &this->depthTexture);
	glBindTexture(GL_TEXTURE_2D, this->depthTexture);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_DEPTH_COMPONENT, SCREEN_WIDTH, SCREEN_HEIGHT, 0, GL_DEPTH_COMPONENT, GL_FLOAT, NULL);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);

	glFramebufferTexture2D(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_TEXTURE_2D, this->depthTexture, 0);
	return this->depthTexture;
}

void FBO::CreateRBO()
{
	this->Bind();
	//rbo
	glGenRenderbuffers(1, &this->RBO);
	glBindRenderbuffer(GL_RENDERBUFFER, this->RBO);
	glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT, SCREEN_WIDTH, SCREEN_HEIGHT);
	glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, this->RBO);
	glBindRenderbuffer(GL_RENDERBUFFER, 0);//unbind
	this->Unbind();
}

void FBO::Bind()
{
	glBindTexture(GL_TEXTURE_2D, 0);
	glBindFramebuffer(GL_FRAMEBUFFER, this->ID);
}

void FBO::Unbind()
{
	glBindRenderbuffer(GL_RENDERBUFFER, 0);//unbind RBO
	glBindFramebuffer(GL_FRAMEBUFFER, 0);
}

void FBO::Delete()
{
	glDeleteFramebuffers(1, &this->ID);
}
