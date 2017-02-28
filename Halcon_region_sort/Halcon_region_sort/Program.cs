using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using HalconDotNet;

namespace Halcon_region_sort
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HALCON_SORT begin = new HALCON_SORT();
            Application.Run(begin);
        }
    }
}
