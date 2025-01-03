using System;
using System.IO;
using Tao.OpenGl;

namespace Graphics
{
  class Renderer
  {
    private const float zeroZ = 0.0f;
    Shader sh;
    uint vertexBufferID;

    public void Initialize()
    {
      // Declare the two shaders files to OpenGL
      string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
      sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader",
                      projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");

      // Set the background color of the shaders
      Gl.glClearColor(1, 1, 1, 1); // White background


      float[] points =
      {

        #region CrownPoints
        -0.7607f,  0.2939f, zeroZ, //point 1
        -0.1106f, -0.3282f, zeroZ, //point 2
        -0.0248f, -0.1908f, zeroZ, //point 3
         0.4312f, -0.0382f, zeroZ, //point 4
         0.1016f,  0.6870f, zeroZ, //point 5
        #endregion
         
        #region FacePoints
        -0.1106f, -0.3282f, zeroZ, //point 2
         #region NeckPoints
        -0.0790f, -0.3779f, zeroZ, //point 6
        -0.1738f, -0.6603f, zeroZ, //point 7
         0.0971f, -0.7023f, zeroZ, //point 8
         0.1874f, -0.5305f, zeroZ, //point 9
         0.4320f, -0.5687f, zeroZ, //point 10
        #endregion
        #endregion

        #region NosePoints
        0.4312f, -0.3130f, zeroZ, //point 11
         0.5000f, -0.3130f, zeroZ, //point 12
         0.4086f, -0.1794f, zeroZ, //point 13
         0.4086f, -0.0496f, zeroZ, //point 14
        #endregion

        #region CrownHorizontalLines
        -0.6930f,  0.2290f, zeroZ, //point 15
         0.1332f,  0.6145f, zeroZ, //point 16
        -0.6208f,  0.1565f, zeroZ, //point 17
         0.1738f,  0.5267f, zeroZ, //point 18
        -0.5711f,  0.1069f, zeroZ, //point 19
         0.2054f,  0.4618f, zeroZ, //point 20
        -0.4898f,  0.0344f, zeroZ, //point 21
         0.2415f,  0.3702f, zeroZ, //point 22
        -0.4402f, -0.0153f, zeroZ, //point 23
         0.2686f,  0.3130f, zeroZ, //point 24
        -0.3589f, -0.0916f, zeroZ, //point 25
         0.3093f,  0.2214f, zeroZ, //point 26
        -0.3093f, -0.1336f, zeroZ, //point 27
         0.3318f,  0.1641f, zeroZ, //point 28
        -0.2280f, -0.2099f, zeroZ, //point 29
         0.3770f,  0.0725f, zeroZ, //point 30
        #endregion

        #region FacePartitioning
        -0.1106f, -0.3282f, zeroZ, //point 2
        -0.0790f, -0.3779f, zeroZ, //point 6
         0.1874f, -0.5305f, zeroZ, //point 9
         0.4320f, -0.5687f, zeroZ, //point 10
         0.4312f, -0.3130f, zeroZ, //point 11
         0.4086f, -0.1794f, zeroZ, //point 13
         0.4086f, -0.0496f, zeroZ, //point 14
        -0.0248f, -0.1908f, zeroZ, //point 3
        #endregion
      
      };

      // Generate the vertex buffer in GPU memory
      vertexBufferID = GPU.GenerateBuffer(points);
    }

    public void Draw()
    {
      Gl.glClear(Gl.GL_COLOR_BUFFER_BIT); // Clear the screen

      // Use the shader program
      sh.UseShader();

      Gl.glLineWidth(1.5f);
      Gl.glPointSize(3.0f);

      // Bind the vertex buffer
      Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertexBufferID);

      // Enable the vertex attribute array
      Gl.glEnableVertexAttribArray(0);
      Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 0, IntPtr.Zero);

      // Location of the color uniform
      int colorLocation = Gl.glGetUniformLocation(sh.ProgramID, "shapeColor");

      #region Points'Drawing
      // Crown
      Gl.glUniform3f(colorLocation, 0.0f, 0.2f, 0.4f); // Dark Blue color
      Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 0, 5); // Crown fill

      // Face and Nose
      Gl.glUniform3f(colorLocation, 0.803f, 0.498f, 0.196f); // Bronze Color
      Gl.glDrawArrays(Gl.GL_POLYGON, 31, 8); // Face
      Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, 11, 3); // Nose

      // Neck
      Gl.glUniform3f(colorLocation, 0.76f, 0.60f, 0.42f); // Deep Beige Color
      Gl.glDrawArrays(Gl.GL_QUADS, 6, 4); // Neck

      // Crown lines
      Gl.glUniform3f(colorLocation, 1.0f, 0.843f, 0.0f); // Gold Color
      for (int i = 0; i < 16; i += 2)
        Gl.glDrawArrays(Gl.GL_LINE_STRIP, 15 + i, 2); // Lines in the Crown
      #endregion

      // Disable the vertex attribute array
      Gl.glDisableVertexAttribArray(0);
    }

    public void Update()
    {

    }

    public void CleanUp()
    {
      sh.DestroyShader();
    }
  }
}
