   M S B A M L   `   `   `  ���� R  NSharpGL.WPF, Version=2.3.0.1, Culture=neutral, PublicKeyToken=27fc851303210b27>.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPFSharpGL.WPF   
KefkaMatic'   !SharpGLWPFApplication3.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     BsharpGL.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPF     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -   $��WINDOW��  � 
ResizeMode$  NoResize=� � Title$ PFSLED��$	��720��$
��1280��
���   �.��� $��0,0,0.333,0.333q� � RowDefinitions �� $	��53* ��� $	��62* �.+�� -   $��MENUBARGRID��$
��Left=�$��25��$��10,2,0,0q�$	��Top=�$	��342�����	#66000000.+�o� -    o�Name$ ___MenuBar_��$��2��$
��Left=�$	��Top=�
������ .@�m� $
��File��
�������.@�m� $	��New��
���   �m� -   $��Save FS��
���   �m� -   $��Load FS��
���   �m� -   $��Load Module��
���   ��� -    ��Name$ compile_button��$��	Recompile��$��37,2,0,0q�$��23��$	��Top=�$
��Left=�$��64���� -   $ 
PlayButton��$
��Play��$��0,1,85,0q�$	��Top=�$��Right=�$��75��$��24���� -	    ��Name$ EQ��
��� � � J� Opacity$
 0.35Q�$��16��$��	0,1,166,0q�$	��Top=�$��Right=�$��18���� -
   $	 EQ2��
��� � �$
 0.35Q�$��16��$��	0,1,185,0q�$	��Top=�$��Right=�$��18���� -   $ 
StopButton��$
��Stop��$��0,1,5,0q�$	��Top=�$��Right=�$��75��$��24��   SharpGL.WPF.OpenGLControl  -     Name$ OpenGLcontext��   RenderContextType$	 FBO=�	   	FrameRate$	 60Q�$��0,10,1.667,-18q�$	��700��$	��Top=�$��2��$��Right=�$	��700���� -   $��ERRORBOX��$	��LOG��
�������$��2q�
��� � �$
��Left=�$��
10,275,0,0q�$	��Top=�$��87��$	��547��$��1���� $
��Left=�$	��556��$��	10,32,0,0q�$	��Top=�$	��547��
������ $��2��.���� -   
 ��Name$
 	TxtBox_FS��$
��Left=�$
��2550�� �� TextWrapping$
 Wrap=�$��TextBox��$	��Top=�$	��537��
������ 
��� � � �� AcceptsReturn . 