using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ПММ_5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
    
    class Game : GameWindow
    {
        public Game()
            : base(800, 600, GraphicsMode.Default, "OpenTK Quick Start Sample")
        {
            VSync = VSyncMode.On;
        }

        Vector2 C_const,C_fluct;
        Vector2[,] C;
        double[,] p_cur, p_next,D,tmp=null;
        double dt=int.MaxValue,dx,dy,t=0;
        int N=100, M=100,X_lenght=1000,Y_lenght=1000;
        double Cmin = 2, Dmin = 10;
        double alpha = 0;
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);
            dx = X_lenght / ((double)N);
            dy = Y_lenght / ((double)M);
            p_cur = new double[N,M];
            p_next = new double[N,M];
            C = new Vector2[N,M];
            D = new double[N,M];
            for(int i = 0; i < N; i++)
            {
                for(int j = 0; j < M; j++)
                {
                    p_cur[i,j] = 0;
                    p_next[i,j] = 0;
                }
            }

            C_const.X = -4f;
            C_const.Y = 5f;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreateOrthographic(100,100,1,1000);
            GL.MatrixMode(MatrixMode.Projection);
            
            GL.LoadMatrix(ref projection);
           

        }

    

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Random rand=new Random();
            double var;
            
            
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    var = rand.NextDouble();
                    var = 2*Math.PI*1 / Math.Sqrt(2 * Math.PI * 1) * Math.Exp(-(var * 2 * Math.PI - 3 * Math.PI / 2) / 2);

                    C_fluct.X = C_const.X * (float)Math.Cos(var)-C_const.Y*(float)Math.Sin(var);
                    C_fluct.Y= C_const.X * (float)Math.Sin(var)+ C_const.Y * (float)Math.Cos(var);

                    C[i,j] = C_const+C_fluct;

                    D[i, j] = Dfunc(C[i, j].Length);
                   
                    dt =Math.Min( 0.3 * Math.Min(Math.Min(dx*dx/(2*D[i,j]),dx/Math.Abs(C[i,j].X)),Math.Min(dy*dy/(2*D[i,j]),dy/Math.Abs(C[i,j].Y))),dt);
                }
            }
            //Console.WriteLine(0.5*Math.Min(dx * dx / (2 * D[50][50]) + dx / Math.Abs(C[50][50].X), dy * dy / (2 * D[50][50])+dy/ Math.Abs(C[50][50].Y)) + " "+dt);
            // Гран условия
            double s1, s2, s3, s4;


            for (int i = 0; i < N; i++)
            {
                p_cur[i, 0] = p_cur[i, 1];
                p_cur[i, M - 1] = p_cur[i, M - 2];
            }
            for (int j = 0; j < M; j++)
            {
                p_cur[0, j] = p_cur[1, j];
                p_cur[N - 1, j] = p_cur[N - 2, j];
            }

            double m1=0, m2=0, m3=0, m4=0,d1=0,d2=0,d3=0,d4=0;
            for(int i = 1; i < N-1; i++)
            {
                for (int j = 1; j < M - 1; j++)
                {
                    if (C[i,j].X > 0)
                    {
                        m1 = dy * dt * (p_cur[i,j] * (C[i+1,j].X+C[i,j].X)/2);
                        m2 = dy * dt * (p_cur[i - 1,j] * (C[i - 1, j].X + C[i, j].X) / 2);
                    }
                    else if(C[i,j].X<0)
                    {
                        m1 = dy * dt * (p_cur[i+1,j] * (C[i + 1, j].X + C[i, j].X) / 2);
                        m2 = dy * dt * (p_cur[i,j] * (C[i - 1, j].X + C[i, j].X) / 2);
                    }

                    if (C[i,j].Y > 0)
                    {
                        m3 = dx * dt * (p_cur[i, j] * (C[i, j + 1].Y + C[i, j].Y) / 2);
                        m4 = dx * dt * (p_cur[i,j - 1] * (C[i, j - 1].Y + C[i, j].Y) / 2);
                    }
                    else if(C[i,j].Y<0)
                    {
                        m3 = dx * dt * (p_cur[i,j+1] * (C[i, j + 1].Y + C[i, j].Y) / 2);
                        m4 = dx * dt * (p_cur[i,j] * (C[i, j - 1].Y + C[i, j].Y) / 2);
                    }

                    d1 = Dfunc((C[i + 1, j].Length + C[i, j].Length) / 2);
                    d2 = Dfunc((C[i - 1, j].Length + C[i, j].Length) / 2);
                    d3 = Dfunc((C[i, j + 1].Length + C[i, j].Length) / 2);
                    d4 = Dfunc((C[i, j - 1].Length + C[i, j].Length) / 2);

                    s1 = -(1 / (dx * dy)) * (m1 - m2 + m3 - m4);
                    s2 = (dt / (dx * dx)) * (d1 * (p_cur[i + 1,j] - p_cur[i,j]) - d2 * (p_cur[i,j] - p_cur[i - 1,j]));
                    s3 = (dt / (dy * dy)) * (d3 * (p_cur[i,j + 1] - p_cur[i,j]) - d4 * (p_cur[i,j] - p_cur[i,j - 1]));
                    if (i == 50 && j == 50)
                        s4 = dt * 1000 - alpha * p_cur[i,j];
                    else
                        s4 = 0;

                    p_next[i,j] = p_cur[i,j]
                        + s1
                        + s2
                        + s3
                        + s4; //
                    if(i == 50 && j == 50)
                        Console.WriteLine(d1 + " " + d2 + " " + d3 + " " + d4 + " " + m1 + " " + m2 + " " + m3 + " " + m4);
                }
            }
            Console.WriteLine(p_next[50,50] + " " +dy+ " "+dx+" "+dt);

            tmp = p_cur;
            p_cur = p_next;
            p_next = tmp;

            

            t += dt;

            dt = int.MaxValue;
        }
        private double Dfunc(double c)
        {
            if (c < Cmin)
                return Dmin;
            return Dmin* c / Cmin;
        }

        double a = 0.1;
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Translate(-50, 50,10);
            a += 1;
            GL.Rotate(90, 1, 0, 0);
           
           
            GL.PointSize(0.1f);
            for(int i = 0; i < N-1; i++)
            {
                GL.Begin(BeginMode.Quads);
                for (int j = 0; j < M-1; j ++)
                {
                    GL.Color3(p_cur[i, j] / 20, p_cur[i, j] / 200, 1 - p_cur[i, j] / 20); GL.Vertex3(i, 0, j);

                    GL.Color3(p_cur[i+1, j] / 20, p_cur[i+1, j] / 200, 1 - p_cur[i+1, j] / 20); GL.Vertex3(i + 1, 0, j);

                    GL.Color3(p_cur[i+1, j+1] / 20, p_cur[i+1, j+1] / 200, 1 - p_cur[i+1, j+1] / 20); GL.Vertex3(i + 1, 0, j + 1);

                    GL.Color3(p_cur[i, j+1] / 20, p_cur[i, j+1] / 200, 1 - p_cur[i, j+1] / 20); GL.Vertex3(i, 0, j + 1);

                }
                GL.End();
            }
           
            GL.Begin(BeginMode.Lines);
            GL.Vertex3(0, 0, 0); GL.Vertex3(100, 0, 0);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 100, 0);
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 100);
            GL.End();
            
        
            SwapBuffers();
        }

       
    }
}
