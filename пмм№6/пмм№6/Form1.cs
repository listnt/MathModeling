using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Threading;

namespace пмм_6
{
    public partial class Form1 : Form
    {
        bool loaded = false;
        bool loaded2 = false;
        bool loaded3 = false;
       
        double[][][] T_cur, T_next,tmp;
        int M, N, K;
        double l = 1;
        double dx=0.02, dy=0.02, dz=0.02;
        double dt,t;
        double D=3;
        double somex,somey,somez;

        
        double camPos = 100,camPos1=50,camPos2=50;

     
        Thread myThread;
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

      


        public Form1()
        {
            InitializeComponent();

            

            dt = Math.Min(dx * dx / (2 * D), dy * dy / (2 * D));
            dt = Math.Min(dt, dz * dz / (2 * D));
            dt *= 0.1;
            somex = dt * D / (dx * dx);
            somey = dt * D / (dy * dy);
            somez = dt * D / (dz * dz);

            Console.WriteLine("" + somex + " " + somey + " " + somez+"\ndt:"+dt);


            M = (int)(l / dx);
            N = (int)(l / dy);
            K = (int)(l / dz);

            T_cur = new double[M][][];
            T_next = new double[M][][];

            for (int i = 0; i < M; i++)
            {
                T_cur[i] = new double[N][];
                T_next[i] = new double[N][];
                for (int j = 0; j < N; j++)
                {
                    T_cur[i][j] = new double[K];
                    T_next[i][j] = new double[K];
                    for (int k = 0; k < K; k++)
                    {
                        T_cur[i][j][k] = 0;
                        T_next[i][j][k] = 0;
                    }
                }
            }





            myThread = new Thread(update);
            myThread.Start();

            timer.Interval = 30;
            timer.Tick += delegate
            {
                glControl1.Invalidate();
                glControl2.Invalidate();
                glControl3.Invalidate();
            };
            timer.Start();
        }

        private ManualResetEvent mrse=new ManualResetEvent(true);
       

        private void update()
        {
            while (true) {
                
                t += dt;
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < K; k++)
                    {
                        T_cur[0][j][k] = T_cur[1][j][k];
                        T_cur[M - 1][j][k] = T_cur[M-2][j][k];
                    }
                }
                for (int i = 0; i < M; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        T_cur[i][j][0] = T_cur[i][j][1];
                        T_cur[i][j][K - 1] = T_cur[i][j][K-2];
                         
                    }

                }
                
                for (int k = 0; k < K; k++)
                {
                    for (int i = 0; i < M; i++)
                    {
                        T_cur[i][0][k] = T_cur[i][1][k];
                        T_cur[i][N - 1][k] = T_cur[i][N - 2][k];
                    }
                }
                T_cur[25][25][K - 1] = 1000;
                //T_cur[45][45][4] = 100.0;

                for (int i = 1; i < M - 1; i++)
                {
                    for (int j = 1; j < N - 1; j++)
                    {
                        for (int k = 1; k < K - 1; k++)
                        {
                            T_next[i][j][k] =
                                somex * T_cur[i + 1][j][k] - 2.0 * somex * T_cur[i][j][k] + somex * T_cur[i - 1][j][k]
                                + somey * T_cur[i][j + 1][k] - 2.0 * somey * T_cur[i][j][k] + somey * T_cur[i][j - 1][k]
                                + somez * T_cur[i][j][k + 1] - 2.0 * somez * T_cur[i][j][k] + somez * T_cur[i][j][k - 1]
                                + T_cur[i][j][k];
                        }
                    }
                }
                
                tmp = T_cur;
                T_cur = T_next;
                T_next = tmp;
                
                Thread.Sleep(1);
                //Console.WriteLine("" + T_next[N - 2][N - 2][N - 2]);
            }

        }



        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            glControl1.MakeCurrent();
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl1.Width / glControl1.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(100, 100, 100, 25, 25, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Flush();
        }
        private void glControl2_Load(object sender, EventArgs e)
        {
            
            loaded2 = true;
            glControl2.MakeCurrent();
            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl2.Width / glControl2.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(50, 50, 50, 25, 0, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Flush();
        }
        private void glControl3_Load(object sender, EventArgs e)
        {

            GL.ClearColor(Color.SkyBlue);
            GL.Enable(EnableCap.DepthTest);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl3.Width / glControl3.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(50, 50, 50, 25, 25, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }


        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            glControl1.MakeCurrent();
            if (!loaded)
                return;
         


            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            //for (int i = 0; i < M; i++)
            //{
            //    for (int j = 0; j < N; j++)
            //    {
            //        for (int k = 0; k < K; k++)
            //        {
            //            GL.Color3(T_cur[i][j][k] / 10, 0, 1 - T_cur[i][j][k] / 10);
            //            DrawCube(i, j, k, 20 * dx, 20 * dy, 20 * dz);
            //        }
            //    }
            //}

            for (int j = 0; j < N-1; j++)
            {
                GL.Begin(BeginMode.Quads);
                for (int k = 0; k < K-1; k++)
                {
                    GL.Color3(T_cur[0][j][k] / 10.0, 0, 1 - T_cur[0][j][k]/10.0); GL.Vertex3(j, 0, k);
                    GL.Color3(T_cur[0][j + 1][k] / 10.0, 0, 1 - T_cur[0][j + 1][k]/10.0); GL.Vertex3(j + 1, 0, k);
                    GL.Color3(T_cur[0][j + 1][k + 1] / 10.0, 0, 1 - T_cur[0][j + 1][k + 1]/10.0); GL.Vertex3(j + 1, 0, k + 1);
                    GL.Color3(T_cur[0][j][k+1] / 10.0, 0, 1 - T_cur[0][j][k+1]/10.0); GL.Vertex3(j, 0, k + 1);

                    GL.Color3(T_cur[M-1][j][k] / 10.0, 0, 1 - T_cur[M-1][j][k] / 10.0); GL.Vertex3(j, M-1, k);
                    GL.Color3(T_cur[M-1][j + 1][k] / 10.0, 0, 1 - T_cur[M-1][j + 1][k] / 10.0); GL.Vertex3(j + 1, M-1, k);
                    GL.Color3(T_cur[M-1][j + 1][k + 1] / 10.0, 0, 1 - T_cur[M-1][j + 1][k + 1] / 10.0); GL.Vertex3(j + 1, M-1, k + 1);
                    GL.Color3(T_cur[M-1][j][k + 1] / 10.0, 0, 1 - T_cur[M-1][j][k + 1] / 10.0); GL.Vertex3(j, M-1, k + 1);
                }
                GL.End();
            }
            for (int i = 0; i < M-1; i++)
            {
                GL.Begin(BeginMode.Quads);
                for (int j = 0; j < N-1; j++)
                {
                    GL.Color3(T_cur[i][j][0] / 10.0, 0, 1 - T_cur[i][j][0] / 10.0); GL.Vertex3(j, i, 0);
                    GL.Color3(T_cur[i][j+1][0] / 10.0, 0, 1 - T_cur[i][j+1][0] / 10.0); GL.Vertex3(j+1, i, 0);
                    GL.Color3(T_cur[i+1][j+1][0] / 10.0, 0, 1 - T_cur[i+1][j+1][0] / 10.0); GL.Vertex3(j+1, i+1, 0);
                    GL.Color3(T_cur[i+1][j][0] / 10.0, 0, 1 - T_cur[i+1][j][0] / 10.0);GL.Vertex3(j, i+1, 0);

                    GL.Color3(T_cur[i][j][K-1] / 10.0, 0, 1 - T_cur[i][j][K-1] / 10.0); GL.Vertex3(j, i, K-1);
                    GL.Color3(T_cur[i][j+1][K-1] / 10.0, 0, 1 - T_cur[i][j+1][K-1] / 10.0); GL.Vertex3(j+1, i, K-1);
                    GL.Color3(T_cur[i+1][j+1][K-1] / 10.0, 0, 1 - T_cur[i+1][j+1][K-1] / 10.0); GL.Vertex3(j+1, i+1, K-1);
                    GL.Color3(T_cur[i+1][j][K-1] / 10.0, 0, 1 - T_cur[i+1][j][K-1] / 10.0); GL.Vertex3(j, i+1, K-1);

                }
                GL.End();
            }
            for (int k = 0; k < K-1; k++)
            {
                GL.Begin(BeginMode.Quads);
                for (int i = 0; i < M-1; i++)
                {
                    GL.Color3(T_cur[i][0][k] / 10.0, 0, 1 - T_cur[i][0][k] / 10.0); GL.Vertex3(0, i, k);
                    GL.Color3(T_cur[i+1][0][k] / 10.0, 0, 1 - T_cur[i+1][0][k] / 10.0); GL.Vertex3(0, i+1, k);
                    GL.Color3(T_cur[i+1][0][k+1] / 10.0, 0, 1 - T_cur[i+1][0][k+1] / 10.0); GL.Vertex3(0, i+1, k+1);
                    GL.Color3(T_cur[i][0][k+1] / 10.0, 0, 1 - T_cur[i][0][k+1] / 10.0); GL.Vertex3(0, i, k+1);

                    GL.Color3(T_cur[i][N-1][k] / 10.0, 0, 1 - T_cur[i][N-1][k] / 10.0); GL.Vertex3(N-1, i, k);
                    GL.Color3(T_cur[i+1][N - 1][k] / 10.0, 0, 1 - T_cur[i+1][N - 1][k] / 10.0); GL.Vertex3(N-1, i+1, k);
                    GL.Color3(T_cur[i+1][N - 1][k+1] / 10.0, 0, 1 - T_cur[i+1][N - 1][k+1] / 10.0); GL.Vertex3(N-1, i+1, k+1);
                    GL.Color3(T_cur[i][N - 1][k+1] / 10.0, 0, 1 - T_cur[i][N - 1][k+1] / 10.0); GL.Vertex3(N-1, i, k+1);

                }
                GL.End();
            }

            //Console.WriteLine(T_cur[N - 2][M - 2][K - 2]);

            glControl1.SwapBuffers();

            
        }
        private void glControl2_Paint(object sender, PaintEventArgs e)
        {
            glControl2.MakeCurrent();
            if (!loaded2)
                return;
           

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            for (int i = 0; i < M; i++)
            {
                GL.Begin(BeginMode.LineStrip);
                for (int k = 0; k < K; k++)
                {
                    GL.Color3(T_cur[i][45][k] / 10, 0, 1 - T_cur[i][45][k]/10.0);
                    GL.Vertex3(i , sigmoid(T_cur[i][45][k],10,0,0.1), k );

                }
                GL.End();
            }
            for (int k = 0; k < K; k++)
            {
                GL.Begin(BeginMode.LineStrip);
                for (int i = 0; i < M; i++)
                {
                    GL.Color3(T_cur[i][45][k] / 10, 0, 1 - T_cur[i][45][k]/10.0);
                    GL.Vertex3(i, sigmoid(T_cur[i][45][k], 10, 0, 0.1), k);

                }
                GL.End();
            }


            glControl2.SwapBuffers();
           
        }
        private void glControl3_Paint(object sender, PaintEventArgs e)
        {
            glControl3.MakeCurrent();
            if (!loaded2)
                return;
           

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);



            for (int i = 0; i < M; i++)
            {
                GL.Begin(BeginMode.LineStrip);
                for (int k = 0; k < K; k++)
                {
                    GL.Color3(T_cur[i][25][k]/10, 0, 1 - T_cur[i][25][k]/10);
                    GL.Vertex3(i, sigmoid(T_cur[i][25][k],10,0,0.1), k);

                }
                GL.End();
            }
            for (int k = 0; k < K; k++)
            {
                GL.Begin(BeginMode.LineStrip);
                for (int i = 0; i < M; i++)
                {
                    GL.Color3(T_cur[i][25][k] / 10, 0, 1 - T_cur[i][25][k] / 10);
                    GL.Vertex3(i, sigmoid(T_cur[i][25][k],10,0,0.1), k);

                }
                GL.End();
            }


            glControl3.SwapBuffers();
           
        }

        

        private void glControl1_MouseClick(object sender, MouseEventArgs e)
        {
            glControl1.MakeCurrent();
            
            if (e.Button == MouseButtons.Left)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.Button == MouseButtons.Right)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            glControl1.Invalidate();
        }
        private void glControl2_MouseClick(object sender, MouseEventArgs e)
        {
            glControl2.MakeCurrent();
            if (e.Button == MouseButtons.Left)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.Button == MouseButtons.Right)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            glControl2.Invalidate();
        }
        private void glControl3_MouseClick(object sender, MouseEventArgs e)
        {
            glControl3.MakeCurrent();
            if (e.Button == MouseButtons.Left)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.Button == MouseButtons.Right)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            glControl3.Invalidate();
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            
            glControl1.MakeCurrent();
            GL.Viewport(glControl1.ClientRectangle);
            
            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl1.Width/glControl1.Height, 20, 500);
            //p = Matrix4.CreateOrthographic(glControl1.Width, glControl1.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(100, 100, 100, 25, 25, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Flush();
        }
        private void glControl2_Resize(object sender, EventArgs e)
        {
            glControl2.MakeCurrent();
            GL.Viewport(glControl2.ClientRectangle);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl2.Width / glControl2.Height, 20, 500);
            //p = Matrix4.CreateOrthographic(glControl1.Width, glControl1.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(50, 50, 50, 25, 0, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
            GL.Flush();
        }
        private void glControl3_Resize(object sender, EventArgs e)
        {
            GL.Viewport(glControl3.ClientRectangle);

            Matrix4 p = Matrix4.CreatePerspectiveFieldOfView((float)(80 * Math.PI / 180), (float)glControl3.Width / glControl3.Height, 20, 500);
            //p = Matrix4.CreateOrthographic(glControl1.Width, glControl1.Height, 20, 500);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref p);

            Matrix4 modelview = Matrix4.LookAt(100, 100, 100, 25, 25, 25, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded || !loaded2) return;
            glControl1.MakeCurrent();
            if (e.KeyCode == Keys.A)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.KeyCode == Keys.D)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.KeyCode == Keys.W)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 1, 0, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.KeyCode == Keys.S)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 1, 0, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }

            if (e.KeyCode == Keys.Oemplus)
            {
                camPos--;
                Matrix4 modelview = Matrix4.LookAt((float)camPos, (float)camPos, (float)camPos, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }
            if (e.KeyCode == Keys.OemMinus)
            {
                camPos++;
                Matrix4 modelview = Matrix4.LookAt((float)camPos, (float)camPos, (float)camPos, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }
            

            glControl1.Invalidate();
        }
        private void glControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded || !loaded2) return;
            glControl2.MakeCurrent();
            if (e.KeyCode == Keys.A)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.KeyCode == Keys.D)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }

            if (e.KeyCode == Keys.Oemplus)
            {
                camPos1--;
                Matrix4 modelview = Matrix4.LookAt((float)camPos1, (float)camPos1, (float)camPos1, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }
            if (e.KeyCode == Keys.OemMinus)
            {
                camPos1++;
                Matrix4 modelview = Matrix4.LookAt((float)camPos1, (float)camPos1, (float)camPos1, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }


            glControl2.Invalidate();
        }
        private void glControl3_KeyDown(object sender, KeyEventArgs e)
        {
            if (!loaded || !loaded2) return;
            glControl3.MakeCurrent();
            if (e.KeyCode == Keys.A)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }
            if (e.KeyCode == Keys.D)
            {
                GL.Translate(25.0, 25.0, 25.0);
                GL.Rotate(-10, 0, 1, 0);
                GL.Translate(-25.0, -25.0, -25.0);
            }

            if (e.KeyCode == Keys.Oemplus)
            {
                camPos2--;
                Matrix4 modelview = Matrix4.LookAt((float)camPos2, (float)camPos2, (float)camPos2, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }
            if (e.KeyCode == Keys.OemMinus)
            {
                camPos2++;
                Matrix4 modelview = Matrix4.LookAt((float)camPos2, (float)camPos2, (float)camPos2, 25, 25, 25, 0, 1, 0);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadMatrix(ref modelview);
            }


            glControl3.Invalidate();
        }

        private void glControl1_MouseMove(object sender, MouseEventArgs e)
        {
            
        }

        private void DrawCube(double x, double y, double z, double height, double width, double depth)
        {
            GL.Begin(BeginMode.Quads);

            GL.Vertex3(x, y, z);
            GL.Vertex3(x + width, y, z);
            GL.Vertex3(x + width, y + height, z);
            GL.Vertex3(x, y + height, z);

            GL.Vertex3(x, y, z);
            GL.Vertex3(x, y + height, z);
            GL.Vertex3(x, y + height, z + depth);
            GL.Vertex3(x, y, z + depth);

            GL.Vertex3(x, y, z);
            GL.Vertex3(x + width, y, z);
            GL.Vertex3(x + width, y, z + depth);
            GL.Vertex3(x, y, z + depth);

            GL.Vertex3(x+width, y+height, z+depth);
            GL.Vertex3(x + width, y, z + depth);
            GL.Vertex3(x , y , z + depth);
            GL.Vertex3(x , y + height, z + depth);

            GL.Vertex3(x + width, y + height, z + depth);
            GL.Vertex3(x + width, y , z + depth);
            GL.Vertex3(x + width, y , z );
            GL.Vertex3(x + width, y + height, z );

            GL.Vertex3(x + width, y + height, z + depth);
            GL.Vertex3(x , y + height, z + depth);
            GL.Vertex3(x , y + height, z );
            GL.Vertex3(x + width, y + height, z );

            GL.End();
        }
        private double sigmoid(double x,double a,double b,double t)
        {
            return a / (1 + Math.Pow(Math.E, -(x+b)*t));
        }
            
    }
}
