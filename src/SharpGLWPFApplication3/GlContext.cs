using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO;//FOR "File" manipulation
using Microsoft.Win32;//for openfiledialog and savefiledialog
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Shaders;
using SharpGL.SceneGraph.Primitives;
using SharpGL;
using SharpGL.SceneGraph.Assets;


namespace SharpGLWPFApplication3
{
    public class GlContext
    {

        public VertexShader vertexShader;
        Texture texture = new Texture();
        ShaderProgram m_shaderProgram = new ShaderProgram();
        //  Create a fragment shader.
       public FragmentShader fragmentShader;
        private uint[] vbo = new uint[1];
        private uint[] vao = new uint[1];




        private float[] g_vertex_buffer_data = new float[] 
        { 
            -1.0f, 1.0f, 0.5f,  -1.0f, -1.0f, 0.5f, 1.0f, -1.0f, 0.5f, 
            1.0f, -1.0f, 0.5f,  1.0f, 1.0f, 0.5f,  -1.0f, 1.0f, 0.5f
        };

        float m_timer;



        public void OpenGLBegin(object sender, OpenGLEventArgs args)
        {
            

            // Clear The Screen And The Depth Buffer    
            args.OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            m_shaderProgram.Push(args.OpenGL, null);
            //Bind the VBO and VAO
            args.OpenGL.BindVertexArray(vao[0]);
            args.OpenGL.EnableVertexAttribArray(0);
            args.OpenGL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[0]);





        }

        public void setUniform1f(OpenGL gl, string location, float v1) { gl.Uniform1(gl.GetUniformLocation(m_shaderProgram.ProgramObject, location), v1); }
        public void setUniform2f(OpenGL gl, string location, float v1, float v2) { gl.Uniform2(gl.GetUniformLocation(m_shaderProgram.ProgramObject, location), v1, v2); }


        public void OpenGLRender(object sender, OpenGLEventArgs args)
        {

            //bind the textures to be used
            args.OpenGL.BindTexture(OpenGL.GL_TEXTURE_2D, 0);
            args.OpenGL.BindTexture(OpenGL.GL_TEXTURE_2D, 1);
            //render
            args.OpenGL.DrawArrays(OpenGL.GL_TRIANGLES, 0, 6);

            m_shaderProgram.Pop(args.OpenGL, null);
            //InkCanvas1.DefaultDrawingAttributes.Color = ColorCanvas1.SelectedColor.Value;
            m_timer += 0.1f;
        }



        public void OpenGLInit(object sender, OpenGLEventArgs args, string Vertex_shader_textbox, string Fragment_shader_textbox)
        {
            

            //CREATE THE VAO
            args.OpenGL.GenVertexArrays(1, vao);
            args.OpenGL.BindVertexArray(vao[0]);
            //CREATE THE VBO AND POPULATE IT WITH THE QUAD VERTICIES
            args.OpenGL.GenBuffers(1, vbo);
            args.OpenGL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, vbo[0]);
            args.OpenGL.BufferData(OpenGL.GL_ARRAY_BUFFER, g_vertex_buffer_data.Length * sizeof(float), GCHandle.Alloc(g_vertex_buffer_data, GCHandleType.Pinned).AddrOfPinnedObject(), OpenGL.GL_STATIC_DRAW);
            args.OpenGL.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 0, new IntPtr(0));



            //Tell opengl what attribute arrays we need
            args.OpenGL.EnableVertexAttribArray(0);
            args.OpenGL.EnableVertexAttribArray(1);
            args.OpenGL.EnableVertexAttribArray(2);
            args.OpenGL.EnableVertexAttribArray(3);

            //This is the position attribute pointer  (layout(location = 0) in vec2 vertexPosition;)	
            args.OpenGL.VertexAttribPointer(1, 2, OpenGL.GL_FLOAT, false, 0, new IntPtr(0));
            //This is the color attribute pointer layout(location = 1) in vec4 vertexColor;
            args.OpenGL.VertexAttribPointer(2, 4, OpenGL.GL_UNSIGNED_BYTE, false, 0, new IntPtr(2));
            //This is the UV attribute pointer layout(location = 2) in vec2 vertexUV;
            args.OpenGL.VertexAttribPointer(3, 2, OpenGL.GL_FLOAT, false, 0, new IntPtr(0));

            //UNBIND WHEN FINISHED
            args.OpenGL.BindVertexArray(0);
            args.OpenGL.BindBuffer(OpenGL.GL_ARRAY_BUFFER, 0);


            //SET SOME GL CONSTANTS
            args.OpenGL.Enable(OpenGL.GL_DEPTH_TEST);
            args.OpenGL.Enable(OpenGL.GL_BLEND);
            args.OpenGL.BlendFunc(OpenGL.GL_SRC_ALPHA, OpenGL.GL_ONE_MINUS_SRC_ALPHA);
            args.OpenGL.ShadeModel(OpenGL.GL_SMOOTH);



            //  Create a vertex shader.
            vertexShader = new VertexShader();
            vertexShader.CreateInContext(args.OpenGL);
            vertexShader.SetSource(Vertex_shader_textbox);

            //  Create a fragment shader.
            fragmentShader = new FragmentShader();
            fragmentShader.CreateInContext(args.OpenGL);
            fragmentShader.SetSource(Fragment_shader_textbox);

            //  Compile them both.
            vertexShader.Compile();
            fragmentShader.Compile();

          

            
            //  Build a program.
            m_shaderProgram.CreateInContext(args.OpenGL);

            //  Attach the shaders.
            m_shaderProgram.AttachShader(vertexShader);
            m_shaderProgram.AttachShader(fragmentShader);
            m_shaderProgram.Link();


            //SET UP TEXTURE 1

            texture.Create(args.OpenGL, Environment.CurrentDirectory + @"\1.png");
            texture.Bind(args.OpenGL);


        }

        public void swapShader(OpenGL gl, string Vertex_shader_textbox, string Fragment_shader_textbox)
        {
            

            //  Create a vertex shader.
            vertexShader = new VertexShader();
            vertexShader.CreateInContext(gl);
            vertexShader.SetSource(Vertex_shader_textbox);

            //  Create a fragment shader.
            fragmentShader = new FragmentShader();
            fragmentShader.CreateInContext(gl);
            fragmentShader.SetSource(Fragment_shader_textbox);

            //  Compile them both.
            vertexShader.Compile();
            fragmentShader.Compile();

            //  Build a program.
            m_shaderProgram.CreateInContext(gl);

            //  Attach the shaders.
            m_shaderProgram.AttachShader(vertexShader);
            m_shaderProgram.AttachShader(fragmentShader);
            m_shaderProgram.Link();
        }

        public string DEFAULT_VS = @"

in vec2 vertexPosition;
in vec4 vertexColor;
in vec2 vertexUV;

out vec2 fragmentPosition;
out vec4 fragmentColor;
out vec2 fragmentUV;

void main()
{
     //Set the x,y position on the screen
    gl_Position.xy = (vec4(vertexPosition, 0.0, 1.0)).xy;
    //the z position is zero since we are in 2D
    gl_Position.z = 0.0;
    
    //Indicate that the coordinates are normalized
    gl_Position.w = 1.0;
                                
    fragmentPosition = vertexPosition;
    
    fragmentColor = vertexColor;
    
    fragmentUV = vec2(0.0, 1.0);
                               
}";





        public string DEFAULT_FS = @"
#version 330

uniform float time;
 uniform vec2 level;
uniform sampler2D tex1;


 vec2 resolution = vec2(700.0,700.0);

vec3 sphere_pos = vec3(0.0, 0.0, 2.0 + 0.5*sin(time*1.5));

float fDist(vec3 pos) {
	float radius = 1.0;
	float dist = length(pos-sphere_pos) - radius;
	dist += (sin(pos.x*25.0 + mod(time*6.0, 2.0*3.14159)) + sin(pos.y*15.0))*0.05;
	return dist;
}

vec3 fNorm(vec3 pos) {
	vec2 dd = vec2(0.0, 0.00001);
	float d = fDist(pos);
	return -normalize(vec3(
		d-fDist(pos+dd.yxx),
		d-fDist(pos+dd.xyx),
		d-fDist(pos+dd.xxy)
	));
}


void main( void ) {
	vec2 uv = gl_FragCoord.xy/resolution;
	
	uv = 2.0*uv-1.0;
	float aspect_ratio = resolution.x/resolution.y;
	uv.x *= aspect_ratio;
	
	uv.xy += 0.0001;
	
	vec3 color = vec3(0.0);
	
	vec3 ray_direction = normalize(vec3(uv, 1.0));
	
	float dist = 0.0;
	for (int i=0; i<100; i++)
		dist += min(fDist(dist*ray_direction), 0.01);
	
	if (dist > 10.0)
		color = vec3(0.5, 0.4, 0.7);
	else {
		
		color = vec3(1.0);
		vec3 pos = dist*ray_direction * vec3(level.x,level.y,0.0);
		vec3 norm = fNorm(pos);
		
		vec3 lightDir = normalize(vec3(1.0, -1.0, 1.0));
		float diffuse = dot(lightDir, -norm);
		float ambient = 0.2;
		float specular = pow(dot(normalize(lightDir + ray_direction), norm), 32.0);
		
		color = diffuse*vec3(1.0, 1.0, 0.7) + ambient*vec3(0.6, 0.6, 1.0) + specular*vec3(1.0);
		
		color *= vec3(sin(pos.y*100.0)*0.2+0.5, 1.0, 0.0);
		color.r += sqrt(1.0-dist);
	}
	
	
	gl_FragColor = vec4(color*4.0, 1.0) * texture(tex1, -gl_FragCoord.xy/resolution);
}";




    }
}
