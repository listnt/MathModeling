using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ПММ_7
{
    public partial class Form1 : Form
    {
        Thread thread;

        long N, M;
        double dx=0.02, dt,D=1;
        double t=0;
        double l = 1;

        public Form1()
        {
            InitializeComponent();
            thread = new Thread(graph);
            thread.IsBackground = true;
            thread.Start();
            chart1.ChartAreas[0].AxisX.Maximum = 1;
            chart1.ChartAreas[0].AxisY.Maximum = 1;
        }

        private void graph()
        {
            int jin = 0;
            double[] T_cur, T_next,deltaT, tmp,D;
            double[] A, B;
            double D0 = 1,dMinusHalf,dPlusHalf, b = 2,alpha,beta,ceta,deta,e;
            N = (long)(l / dx);
            //начальные значения
           
            
            M = (long)(t / dt);
            T_cur = new double[N + 1];                           //выделение памяти под массив
            T_next = new double[N + 1];
            deltaT = new double[N + 1];
            D = new double[N + 1];
            for (int i = 0; i < T_cur.Length; i++)
            {
                T_cur[i] = 0;
                T_next[i] = 0;
            }

            T_cur[0] = 0;
            T_next[0] = 0;


            dt = dx * dx / (2*D0);

            A = new double[N];
            B = new double[N];


            while (true)
            {

                t+=dt;

                
                T_cur[0] = (t-dt) / (1 + t-dt);
                T_next[0] = t / (1 + t);
                T_next[N] = 0;
                for (int q = 0; q < 4; q++)
                {

                    A[0] = 0;
                    B[0] = 0;
                    A[N-1] = 0;
                    B[N-1] = 0;
                    deltaT[0] = 0;
                    deltaT[N] = 0;

                    for (int k = 1; k < N; k++)
                    {
                        D[k] = (D0 * Math.Pow(T_next[k], b));
                        
                    }
                    for(int k = 1; k < N; k++)
                    {
                        dPlusHalf = (D0 * Math.Pow((T_next[k + 1] + T_next[k]) / 2, b));
                        dMinusHalf = (D0 * Math.Pow((T_next[k - 1] + T_next[k]) / 2, b));

                        alpha = (dPlusHalf + 0.5 * (D[k + 1] - D[k])) / dx;
                        beta = (-dPlusHalf + 0.5 * (D[k + 1] - D[k]) ) / dx
                            - (dMinusHalf + 0.5 * (D[k] - D[k - 1]) ) / dx
                            -dx/dt;
                        ceta= (-dMinusHalf + 0.5 * (D[k] - D[k - 1]) )/dx;

                        deta = (T_next[k] - T_cur[k]) / dt * dx 
                            - ((T_next[k + 1] - T_next[k]) / dx * dPlusHalf - (T_next[k] - T_next[k - 1]) / dx * dMinusHalf);

                        e = alpha * A[k - 1] + beta;
                        A[k] = -ceta / e;
                        B[k] = (deta - alpha * B[k - 1]) / e;
                        
                    }
                    deltaT[N] = 0;
                    for(long k = N-1; k > -1; k--)
                    {
                        deltaT[k] = A[k] * deltaT[k + 1] + B[k];
                    }
                    T_next[N] = 0;
                    for (int k = 0; k < N ; k++)
                    {
                        T_next[k] += deltaT[k];
                    }
                    
                }
                Console.WriteLine(""+T_next[0]);
                for (int k = 1; k < N; k++)
                {
                    dt = Math.Min(dt, 0.5 * dx * dx / (2 * D[k]));
                }

                tmp = T_cur;                                            //переопределяем массив T_cur на T_new, а массив T_new на T_cur
                T_cur = T_next;
                T_next = tmp;

               
                jin++;
                if (jin > 10)
                {
                    //Console.WriteLine(T_cur[0]);
                    jin = 0;
                    this.Invoke((MethodInvoker)delegate
                    {
                        chart1.Series[0].Points.Clear();

                        for (int i = 0; i <= N; i += 1)
                        {
                            chart1.Series[0].Points.AddXY(i * dx, T_cur[i]);
                        }
                        chart1.Update();
                    });
                }

                
                
                Thread.Sleep(1);
            }
        }
    }
}
