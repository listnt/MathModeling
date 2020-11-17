using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.DirectX.DirectDraw;
namespace ПММ_5
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Game game = new Game())
            {
                game.Run(30.0);
            }
        }
    }
}
