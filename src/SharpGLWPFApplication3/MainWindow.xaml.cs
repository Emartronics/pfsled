using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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




    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //list of fragment shaders....(or will be)
        public List<string> FS_stringList = new List<string>();
        //list of vertex shaders....(or will be)
        public List<string> VS_stringList = new List<string>();
        //list of SCRIPT FILES....(or will be)
        public List<string> SCRIPT_stringList = new List<string>();
        float CurrentTick = 0;
        bool isPlaying = false;

        GlContext glContext = new GlContext();
           
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);
            [DllImport("sunvox.dll")]
            public static extern int sv_init( int dev, int freq, int channels, int flags );
            [DllImport("sunvox.dll")]
            public static extern int sv_open_slot(int slot);
           [DllImport("sunvox.dll")]
            public static extern int sv_load( int slot, string name );
           [DllImport("sunvox.dll")]
           public static extern int sv_play(int slot);
           [DllImport("sunvox.dll")]
           public static extern int sv_play_from_beginning(int slot);
           [DllImport("sunvox.dll")]
           public static extern int sv_stop(int slot);
           [DllImport("sunvox.dll")]
           public static extern int sv_get_ticks();
          [DllImport("sunvox.dll")]
           public static extern int sv_get_current_line(int slot);
           [DllImport("sunvox.dll")]
           public static extern int sv_get_current_signal_level(int slot, int channel);
         [DllImport("sunvox.dll")]
           public static extern uint sv_get_song_length_lines(int slot);
        [DllImport("sunvox.dll")]
         public static extern IntPtr sv_get_song_name(int slot);






        
        public MainWindow() {           
            InitializeComponent();
            
            //Load sunvox.dll and load initial song
            LoadLibrary("sunvox.dll");
            sv_init(0, 22050, 2, 0);
            sv_open_slot(0);

            TxtBox_FS.Text = glContext.DEFAULT_FS;
        }

        private void OpenGLControl_OpenGLDraw(object sender, OpenGLEventArgs args){
            // MousePosition = Mouse.GetPosition(WINDOW);



            if (isPlaying)
            {

            lock (args.OpenGL)
            {
                glContext.OpenGLBegin(sender, args);//start render process             
                //set shader uniforms          

                glContext.setUniform1f(args.OpenGL, "time", (float)sv_get_ticks());
                glContext.setUniform2f(args.OpenGL, "level", (float)sv_get_current_signal_level(0, 1), (float)sv_get_current_signal_level(0, 1));
     
                
                glContext.OpenGLRender(sender, args);
            }

                CurrentTick ++;
                EQ.Height = sv_get_current_signal_level(0, 0) / 1.2;
                EQ2.Height = sv_get_current_signal_level(0, 1)/1.2;
            }
            else
            {
                EQ.Height = 0;
                EQ2.Height = 0;
            }

        }

        private void OpenGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            glContext.OpenGLInit(sender, args, glContext.DEFAULT_VS, TxtBox_FS.Text);

        }



//MENU BAR CONTROLS

        private void SaveFS(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "GLSL Files | *.fs";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, TxtBox_FS.Text);
            }
        }

        private void LoadFS(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GLSL Files | *.fs";
            if (openFileDialog.ShowDialog() == true)
            {
                TxtBox_FS.Text = File.ReadAllText(openFileDialog.FileName);
            }
        }
        private void LOADMOD_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SUNVOX Files | *.sunvox";
            if (openFileDialog.ShowDialog() == true)
            {
                sv_stop(0);
                sv_load(0, openFileDialog.FileName);
                sv_play(0);
                isPlaying = true;
                IntPtr cstr = sv_get_song_name(0);
                //String str = Marshal.PtrToStringAnsi(cstr);
                //SongnameLabel.Content = str;
            }


        }
        private void compile_button_Click(object sender, RoutedEventArgs e)
        {

            ERRORBOX.Content = " ";

            glContext.swapShader(OpenGLcontext.OpenGL, glContext.DEFAULT_VS, TxtBox_FS.Text);
            ERRORBOX.Content += glContext.fragmentShader.InfoLog + "\n";


            if (glContext.fragmentShader.InfoLog.Length < 1 && glContext.vertexShader.InfoLog.Length < 1)
            {
                ERRORBOX.BorderBrush = Brushes.Transparent;
            }
            else
            {
                ERRORBOX.BorderBrush = Brushes.Red;
            }

///////////////////////////////////////////////////////////////////////////////
        }
//PLAYBACK BOX COTNROLS
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = true;
            sv_play_from_beginning(0);
            CurrentTick = 0;
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            sv_stop(0);
            CurrentTick = 0;
        }

//EDIT BOX CONTROLS

    }
}
