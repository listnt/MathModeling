using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Threading;

namespace лаба_1ПММ
{
    public partial class UserControl1 : UserControl
    {

        public UserControl1()
        {
            InitializeComponent();
            button2.Enabled = false;
           
        }
        private Thread myThread = null;
        bool paused = false;
        ManualResetEvent mrse = new ManualResetEvent(true);

        public long M, N, t = 0;
       
        private double dt, D = 3, dx = 0.02;
        private int l = 1;
        public delegate void InvokeDelegate();
        public int jin = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            t = 0;
            myThread = new Thread(new ThreadStart(this.Graph));
            myThread.IsBackground = true;
            myThread.Start();
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            maskedTextBox3.Enabled = false;
            
        }
        public void form_button1_click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            clicked_button.InOfClBu = 0;
            groupBox1.Enabled = true;
            groupBox1.Visible = true;
            button1.Left = 115;
            button2.Left = 115;
            button3.Left = 115;
            chart1.ChartAreas[0].AxisX.Maximum = l;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
           
        }
        public void form_button3_click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            clicked_button.InOfClBu = 1;
            groupBox1.Enabled = false;
            groupBox1.Visible = false;
            button1.Left = 13;
            button2.Left = 13;
            button3.Left = 13;
            chart1.ChartAreas[0].AxisX.Maximum = l;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Minimum = -1;
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            if (myThread != null)
            {
                if (paused == true)
                    mrse.Set();
                myThread.Abort();
            }
            
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = false;
            button3.Text = "Pause";
            maskedTextBox3.Enabled = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                maskedTextBox3.Enabled = true;
            else
                maskedTextBox3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (myThread != null)
            {

                if (paused == false)
                {
                    paused = true;
                    mrse.Reset();
                    button3.Text = "Resume";
                }
                else if (paused == true)
                {
                    paused = false;
                    mrse.Set();
                    button3.Text = "Pause";
                }
            }

        }



        private void Graph()
        {

            if (textBox1.Text != "")
            {
                dx = (double)Convert.ToDouble(textBox1.Text);
            }
            textBox1.BeginInvoke((MethodInvoker)delegate { InvokeMethod2(); });
            if (clicked_button.InOfClBu == 0)
            {
                dt = dx * dx / (2 * D);
                maskedTextBox1.BeginInvoke((MethodInvoker)delegate { InvokeMethod3(); });
                if (radioButton2.Checked)
                    oldfunc();
                else
                    newfunc();
            }
            else if (clicked_button.InOfClBu == 1)
            {
                dt = dx /  D;
                maskedTextBox1.BeginInvoke((MethodInvoker)delegate { InvokeMethod3(); });
                voln();
            }
        }


        public void InvokeMethod()
        {
            maskedTextBox2.Text = Convert.ToString(t * dt);
        }

        public void InvokeMethod2()
        {
            textBox1.Enabled = false;
            textBox1.Text = Convert.ToString(dx);
            textBox1.Update();
        }

        public void InvokeMethod1()
        {
            button3.Text = "Resume";
        }


        public void InvokeMethod3()
        {
            maskedTextBox1.Text = Convert.ToString(dt);
            maskedTextBox1.Update();
        }

        private void UserControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.L)
            {
                MessageBox.Show("hello");
            }
        }

        private void oldfunc()
        {
            double[] T_cur, T_next, tmp;
            N = (long)(l / dx);
            //начальные значения

            if (checkBox1.Checked && maskedTextBox3.Text != "")
                M = (long)(Convert.ToInt64(maskedTextBox3.Text) / dt);

            T_cur = new double[N + 1];                           //выделение памяти под массив
            T_next = new double[N + 1];

            for (int i = 0; i < T_cur.Length; i++)
            {
                T_cur[i] = 0;
            }

            T_cur[0] = 0;
            T_next[0] = 0;
            while (true)
            {
                if (checkBox1.Checked)
                {
                    if (t > M)
                    {
                        paused = true;
                        mrse.Reset();
                        button3.BeginInvoke((MethodInvoker)delegate { InvokeMethod1(); });
                       
                    }
                }
                mrse.WaitOne();
                t++;

                T_cur[0] = (t * dt) / (1 + t * dt);
                T_cur[N] = 0;                          //граничное условие	на конце стержня		T(L,t)=t-3
                for (int k = 1; k < N; k++)
                {
                    T_next[k] = (double)((dt * D / (dx * dx)) * T_cur[k + 1] + (1.0 - 2.0 * (dt * D / (dx * dx))) * T_cur[k] + (dt * D / (dx * dx)) * T_cur[k - 1]);//каждый кадр пересчитывает массив точек
                }

                tmp = T_cur;                                            //переопределяем массив T_cur на T_new, а массив T_new на T_cur
                T_cur = T_next;
                T_next = tmp;

                maskedTextBox2.BeginInvoke((MethodInvoker)delegate { InvokeMethod(); });
                if (chart1.IsHandleCreated)
                {
                    jin++;
                    if (jin > 1000)
                    {

                        jin = 0;
                        this.Invoke((MethodInvoker)delegate {
                            chart1.Series["Series1"].Points.Clear();

                            for (int i = 0; i <= N; i += 1)
                            {
                                chart1.Series["Series1"].Points.AddXY(i * dx, T_cur[i]);
                            }
                            chart1.Update();
                        });
                    }

                }
                else
                {
                    break;
                }

            }
            
        }
        
        private void newfunc()
        {
            double[] A, B;
            double[] T_cur, T_next, tmp;
            double e;
            
            N = (long)(l / dx);
            //начальные значения

            if (checkBox1.Checked && maskedTextBox3.Text != "")
                M = (long)(Convert.ToInt64(maskedTextBox3.Text) / dt);

            T_cur = new double[N + 1];                           //выделение памяти под массив
            T_next = new double[N + 1];
            A = new double[N];
            B = new double[N];


            for (int i = 0; i < T_cur.Length; i++)
            {
                T_cur[i] = 0;
            }

            T_cur[0] = 0;
            T_next[0] = 0;
            while (true)
            {
                if (checkBox1.Checked)
                {
                    if (t > M)
                    {
                        paused = true;
                        mrse.Reset();
                        button3.BeginInvoke((MethodInvoker)delegate { InvokeMethod1(); });
                       
                    }
                }
                mrse.WaitOne();
                t++;
                    
                
                A[0] = 0;
                B[0]= (t * dt) / (1 + t * dt);

                for (int k = 1; k < N; k++)
                {
                    e = 1/(dx * dx + dt * D * (2 - A[k - 1]));
                    A[k] = (dt * D) *e;
                    B[k] = (dx*dx*T_cur[k]+dt*D*B[k-1]) * e;
                }

                T_next[N] = 0;
                for (long k = N - 1; k > -1; k--)
                {
                    T_next[k] = A[k] * T_next[k + 1] + B[k];
                }

                tmp = T_cur;                                            //переопределяем массив T_cur на T_new, а массив T_new на T_cur
                T_cur = T_next;
                T_next = tmp;

                maskedTextBox2.BeginInvoke((MethodInvoker)delegate { InvokeMethod(); });
                if (chart1.IsHandleCreated)
                {
                    jin++;
                    if (jin > 1000)
                    {

                        jin = 0;
                        this.Invoke((MethodInvoker)delegate {
                            chart1.Series["Series1"].Points.Clear();

                            for (int i = 0; i <= N; i += 1)
                            {
                                chart1.Series["Series1"].Points.AddXY(i * dx, T_cur[i]);
                            }
                            chart1.Update();
                        });
                    }

                }
                else
                {
                    break;
                }

            }
            
        }

        private void voln()
        {
            
            double[] u_past, u_cur, u_next,tmp;
            N = (long)(l / dx);
            //начальные значения

            if (checkBox1.Checked && maskedTextBox3.Text != "")
                M = (long)(Convert.ToInt64(maskedTextBox3.Text) / dt);
            u_past = new double[N + 1];
            u_cur = new double[N + 1];
            u_next = new double[N + 1];
            for(int i = 0; i < N + 1 ; i++)
            {
                if ((i * dx ) <= (1.0 / 3.0 * l))
                {
                    u_past[i] = 0.2 * Math.Sin(3 * Math.PI * i * dx) * Math.Sin(3 * Math.PI * i * dx);
                    u_cur[i] = u_past[i] + dt * (-0.6 * Math.PI * Math.Sin(6 * Math.PI * i * dx));
                }
                else
                {
                    u_past[i] = 0;
                    u_cur[i] = u_past[i];
                }
                
            }
            u_past[0] = 0;
            u_cur[0] = 0;
            u_next[0] = 0;
            u_past[N] = 0;
            u_cur[N] = 0;
            u_next[N] = 0;
            
            while (true)
            {
                
                if (checkBox1.Checked)
                {
                    if (t > M)
                    {
                        paused = true;
                        mrse.Reset();
                        button3.BeginInvoke((MethodInvoker)delegate { InvokeMethod1(); });
                       
                    }
                }
                mrse.WaitOne();
                t++;
                u_next[0] = 0;
                u_next[N] = 0;
                for (int k = 1; k < N; k++)
                {
                    u_next[k] = (dt * dt * D / (dx * dx)) * u_cur[k + 1] + (2.0 - 2.0 * (dt * dt * D / (dx * dx))) * u_cur[k] + (dt * dt * D / (dx * dx)) * u_cur[k - 1] - u_past[k];
                }

                tmp = u_past;
                u_past = u_cur;
                u_cur = u_next;
                u_next = tmp;

                maskedTextBox2.BeginInvoke((MethodInvoker)delegate { InvokeMethod(); });
                Thread.Sleep(16);
                if (chart1.IsHandleCreated)
                {
                    jin++;
                    if (jin > 0)
                    {

                        jin = 0;
                        this.Invoke((MethodInvoker)delegate {
                            chart1.Series["Series1"].Points.Clear();

                            for (int i = 0; i <= N; i += 1)
                            {
                                chart1.Series["Series1"].Points.AddXY(i * dx, u_next[i]);
                            }
                            chart1.Update();
                        });
                    }

                }
                else
                {
                    break;
                }

            }
        }
    }
}