#pragma once
#ifndef WFBO_H
#define WFBO_H

#include "FBO.h"
class WaterFBO
{
public:
	WaterFBO();

	void InitReflecFBO();
	void InitRefracFBO();

	void BindReflecFBO();
	void BindRefracFBO();

	void UnbindReflecFBO();
	void UnbindRefracFBO();

	void Delete();

	FBO reflectionFBO;
	FBO refractionFBO;

	GLuint reflecTexture;
	GLuint reflecRBO;

	GLuint refracTexture;
	GLuint refracDepthTexture;

	int REFLECTION_WIDTH = 320;
	int REFLECTION_HEIGHT = 180;
	int REFRACTION_WIDTH = 1280;
	int REFRACTION_HEIGHT = 720;

private:
	const int SCREEN_WIDTH = 1600;
	const int SCREEN_HEIGHT = 900;
};

#endif