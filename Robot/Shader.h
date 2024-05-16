#ifndef  SHADER_H
#define SHADER_H

#include <glad/glad.h>

#include <iostream>
#include <string>
#include <fstream>
#include <sstream>

#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

class Shader
{
public:
	// program ID
	unsigned int ID;

	//Ū���óЫ�shader program
	Shader(const char* vertexPath, const char* fragmentPath);

	// �ϥ�
	void use();

	// uniform �u��function
	void setBool(const std::string& name, bool value) const;
	void setInt(const std::string& name, int value) const;
	void setFloat(const std::string& name, float value) const;
	void setMat4(const std::string& name, glm::mat4 matrix) const;
	void setVec3(const std::string& name, float x, float y, float z) const;
	void setVec3(const std::string& name, glm::vec3 vec) const;

	// �Ѻcfunciton
	void Delete();
};

#endif // ! SHADER_H