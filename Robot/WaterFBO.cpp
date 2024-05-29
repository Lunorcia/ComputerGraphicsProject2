#include "WaterFBO.h"

WaterFBO::WaterFBO()
{
	InitReflecFBO();
	InitRefracFBO();
}

void WaterFBO::InitReflecFBO()
{
	reflectionFBO.Bind();
	glDrawBuffer(GL_COLOR_ATTACHMENT0);
	reflecTexture = reflectionFBO.CreateTextureAttatch();
	//rbo
	glGenRenderbuffers(1, &reflecRBO);
	glBindRenderbuffer(GL_RENDERBUFFER, reflecRBO);
	glRenderbufferStorage(GL_RENDERBUFFER, GL_DEPTH_COMPONENT, SCREEN_WIDTH, SCREEN_HEIGHT);
	glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_DEPTH_ATTACHMENT, GL_RENDERBUFFER, reflecRBO);
	glBindRenderbuffer(GL_RENDERBUFFER, 0);//unbind
	reflectionFBO.Unbind();
}

void WaterFBO::InitRefracFBO()
{
	refractionFBO.Bind();
	glDrawBuffer(GL_COLOR_ATTACHMENT0);
	refracTexture = refractionFBO.CreateTextureAttatch();
	refracDepthTexture = refractionFBO.CreateDepthTextureAttatch();
	refractionFBO.Unbind();
}

void WaterFBO::BindReflecFBO()
{
	reflectionFBO.Bind();
}

void WaterFBO::BindRefracFBO()
{
	refractionFBO.Bind();
}

void WaterFBO::UnbindReflecFBO()
{
	reflectionFBO.Unbind();
}

void WaterFBO::UnbindRefracFBO()
{
	refractionFBO.Unbind();
}

void WaterFBO::Delete()
{
	glDeleteTextures(1, &reflecTexture);
	glDeleteRenderbuffers(1, &reflecRBO);
	glDeleteTextures(1, &refracTexture);
	glDeleteTextures(1, &refracDepthTexture);
	reflectionFBO.Delete();
	refractionFBO.Delete();
}
