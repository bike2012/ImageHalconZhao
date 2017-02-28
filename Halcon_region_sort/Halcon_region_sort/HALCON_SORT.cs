using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;

namespace Halcon_region_sort
{
    public partial class HALCON_SORT : Form
    {
        public static HALCON_SORT form0 = null;
        public HALCON_SORT()
        {
            InitializeComponent();
            form0 = this;
        }
        //private HalconWindow wch = new HalconWindow();
        public HTuple hv_ExpDefaultWinHandle;
        // Local iconic variables 定义图像变量
        public HObject ho_Image, ho_GrayImage, ho_Regions, ho_meanImage, ho_ImageEdge;
        public HObject ho_Connection, ho_RegionResult, ho_SelectedRegions, ho_ImageInvert;
        public HObject ho_RegionFillUp, ho_RegionDilation, ho_RegionUnion;
        public HObject ho_ImageReduced, ho_Regions1, ho_RegionErosion;
        public HObject ho_ConnectedRegions, ho_SelectedRegions1, ho_RegionDilation1;
        public HObject ho_SortedRegions, ho_ObjectSelected_1 = null;

        // Local control variables 定义控制变量

        public HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
        public HTuple hv_UsedThreshold = null, hv_UsedThreshold1 = null;
        public HTuple hv_Number = null, hv_Area = null, hv_Row = null;
        public HTuple hv_Column = null, hv_i = null, hv_Area1 = null;
        public HTuple hv_Row1 = null, hv_Column1 = null, hv_Area2 = null;
        public HTuple hv_area = null, hv_bizhong = null,hv_ContLength = null;
        public HTuple hv_Circularity = null, hv_Ra = null, hv_Rb = null;
        public HTuple hv_Phi = null, hv_Quot = null, hv_all = null, hv_ro = null;
        public HTuple hv_ti = null, hv_pi = null;
        // 主窗体加载事件
        private void HALCON_SORT_Load(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);          
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
            HOperatorSet.GenEmptyObj(out ho_meanImage);
            HOperatorSet.GenEmptyObj(out ho_ImageEdge);
            HOperatorSet.GenEmptyObj(out ho_ImageInvert);
            HOperatorSet.GenEmptyObj(out ho_Regions);
            HOperatorSet.GenEmptyObj(out ho_Connection);
            HOperatorSet.GenEmptyObj(out ho_RegionResult);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Regions1);
            HOperatorSet.GenEmptyObj(out ho_RegionErosion);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_RegionDilation1);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_ObjectSelected_1);            
            hv_ExpDefaultWinHandle = hWindow_main.HalconWindow;  //获取这个form中的halcon的HWindowControl窗口的窗口句柄
        }
        private void hWindowControl1_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {

        }
        #region(读取图像)
        private void 读取图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hv_ExpDefaultWinHandle = hWindow_main.HalconWindow;
            //用于打开文件夹
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "JPG文件(*.jpg)|*.jpg|所有文件(*.*)|*.*||";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);       //清空显示
                string Path = openFile.FileName;
                HOperatorSet.ReadImage(out ho_Image, Path);
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                //wch.DispImageFit(ho_Image, hv_ExpDefaultWinHandle);
                HTuple hv_HeightWin, hv_WidthWin;
                HOperatorSet.GetImageSize(ho_Image, out hv_HeightWin, out hv_WidthWin);          // 获取输入图像的尺寸
                String str_imgSize = String.Format("Size:{0}x{1}", hv_HeightWin, hv_WidthWin);
                //wch.disp_message(hv_ExpDefaultWinHandle, str_imgSize, "window", hv_ExpDefaultWinHandle.Height - 20, 1, "blue", "false");

            }
        }
        #endregion

        private void 打开自动处理窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            halcon_form halconform = new halcon_form();
            halconform.Show();
            
            this.Hide();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            labelthreshold.Text = trackBar1.Value.ToString();
        }



        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void labelthreshold_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label1.Text = trackBar2.Value.ToString();
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("铁谱图像分析\n版本1.0.1\n设计：毕珂 ");
        }
        //  图像分割部分
        # region（图像分割部分）
        private void button3_Click(object sender, EventArgs e)
        {
            if (ho_ImageEdge.CountObj() != 0 || ho_meanImage.CountObj() != 0)
            {
                if (ho_ImageEdge.CountObj() != 0)
                {
                    int threshold_1 = trackBar1.Value;
                    int mode = comboBox2.SelectedIndex;
                    switch (mode)
                    {
                        case 0:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.Threshold(ho_ImageEdge, out ho_Regions, 0, threshold_1);
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        case 1:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                            HOperatorSet.BinaryThreshold(ho_ImageEdge, out ho_Regions, "max_separability", "dark", out hv_UsedThreshold);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        case 2:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                            HOperatorSet.BinaryThreshold(ho_ImageEdge, out ho_Regions, "smooth_histo", "dark", out hv_UsedThreshold);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        default:
                            MessageBox.Show("请选择处理模式");
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("请进行图像增强处理");
                }
            }
            else
            {
                if (ho_GrayImage.CountObj() != 0)
                {
                    int threshold_1 = trackBar1.Value;
                    int mode = comboBox2.SelectedIndex;
                    switch (mode)
                    {
                        case 0:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.Threshold(ho_GrayImage, out ho_Regions, 0, threshold_1);
                            HOperatorSet.DispObj(ho_GrayImage, hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        case 1:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_GrayImage, hv_ExpDefaultWinHandle);
                            HOperatorSet.BinaryThreshold(ho_GrayImage, out ho_Regions, "max_separability", "dark", out hv_UsedThreshold);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        case 2:
                            ho_Regions.Dispose();
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            HOperatorSet.DispObj(ho_GrayImage, hv_ExpDefaultWinHandle);
                            HOperatorSet.BinaryThreshold(ho_GrayImage, out ho_Regions, "smooth_histo", "dark", out hv_UsedThreshold);
                            HOperatorSet.DispObj(ho_Regions, hv_ExpDefaultWinHandle);
                            break;
                        default:
                            MessageBox.Show("请选择处理模式");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("请先进行灰度处理");
                }
            }
        }
        # endregion
        //  对图像进行灰度变换
        # region(灰度变换)
        private void button1_Click(object sender, EventArgs e)
        {
            if (ho_Image.CountObj()!=0)
            {
                ho_GrayImage.Dispose();
                HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
                HOperatorSet.DispObj(ho_GrayImage, hv_ExpDefaultWinHandle);
            }
            else
            {
                MessageBox.Show("请先读取图像");
            }
        }
        #endregion
        #region(图像平滑)
        private void button4_Click(object sender, EventArgs e)
        {
            if (ho_GrayImage.CountObj() != 0)
            {
                int mean_size = trackBar4.Value;//定义图像增强参数
                int mode = comboBox4.SelectedIndex;
                switch (mode)
                {
                    case 0:
                        if (mean_size % 2 == 1)
                        {
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            ho_meanImage.Dispose();
                            HOperatorSet.GaussFilter(ho_GrayImage, out ho_meanImage, mean_size);
                            HOperatorSet.DispObj(ho_meanImage, hv_ExpDefaultWinHandle);
                            break;
                        }
                        else
                        {
                            MessageBox.Show("请输入大于1的奇数");
                            break;
                        }
                    case 1:
                        if (mean_size >= 1)
                        {
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            ho_meanImage.Dispose();
                            HOperatorSet.MeanImage(ho_GrayImage, out ho_meanImage, mean_size, mean_size);
                            HOperatorSet.DispObj(ho_meanImage, hv_ExpDefaultWinHandle);
                            break;
                        }
                        else
                        {
                            MessageBox.Show("请输入系数");
                            break;
                        }
                    case 2:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_meanImage.Dispose();
                        HOperatorSet.MedianImage(ho_GrayImage, out ho_meanImage, "circle", mean_size, "mirrored");
                        HOperatorSet.DispObj(ho_meanImage, hv_ExpDefaultWinHandle);
                        break;
                    case 3:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_meanImage.Dispose();
                        HOperatorSet.SmoothImage(ho_GrayImage, out ho_meanImage, "deriche2", mean_size);
                        HOperatorSet.DispObj(ho_meanImage, hv_ExpDefaultWinHandle);
                        break;
                    case 4:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_meanImage.Dispose();
                        HOperatorSet.SigmaImage(ho_GrayImage, out ho_meanImage, mean_size, mean_size, 3);
                        HOperatorSet.DispObj(ho_meanImage, hv_ExpDefaultWinHandle);
                        break;
                    default:
                        MessageBox.Show("请选择处理模式");
                        break;
                }
            }
            else 
            {
                MessageBox.Show("请先进行灰度变换");
            }
        }
        #endregion
        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = trackBar4.Value.ToString();
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label11.Text = trackBar3.Value.ToString();
        }
        #region(图像增强)
        private void button5_Click(object sender, EventArgs e)
        {
            if (ho_meanImage.CountObj()!=0) 
            #region(处理平滑图像)
            {
                int em_size = trackBar3.Value;//定义图像增强参数
                int mode = comboBox3.SelectedIndex;
                switch (mode)
                {
                    case 0:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.FreiAmp(ho_meanImage, out ho_ImageEdge);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 1:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.KirschAmp(ho_meanImage, out ho_ImageEdge);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 2:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.PrewittAmp(ho_meanImage, out ho_ImageEdge);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 3:
                        if (em_size>0)
                        {
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.GrayErosionShape(ho_meanImage, out ho_ImageEdge, em_size, em_size, "rhombus");
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                        }
                        else
                        {
                            MessageBox.Show("请输入系数");
                            break;

                        }
                    case 4:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.Emphasize(ho_meanImage, out ho_ImageEdge, 7, 7, em_size);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 5:
                        if (em_size > 0 & em_size < 13)
                        {
                            //HObject a;
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            ho_ImageEdge.Dispose();
                            HOperatorSet.Illuminate(ho_meanImage, out ho_ImageEdge, 41, 41, em_size);
                            ho_ImageInvert.Dispose();
                            HOperatorSet.InvertImage(ho_ImageEdge, out ho_ImageInvert);
                            ho_ImageEdge = ho_ImageInvert;
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);

                            break;
                        }
                        else
                        {
                            MessageBox.Show("请输入1-12之内的系数");
                            break;
                        }
                    default:
                        MessageBox.Show("请选择处理模式");
                        break;
                }
            }
            #endregion
            else
            {
            #region(处理灰度图)
             if (ho_GrayImage.CountObj() != 0)
            {
                int em_size = trackBar3.Value;//定义图像增强参数
                int mode = comboBox3.SelectedIndex;
                switch (mode)
                {
                    case 0:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.FreiAmp(ho_GrayImage, out ho_ImageEdge);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 1:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.KirschAmp(ho_GrayImage, out ho_ImageEdge);
                        if (HDevWindowStack.IsOpen())
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 2:
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.PrewittAmp(ho_GrayImage, out ho_ImageEdge);
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                    case 3:
                        if (em_size > 0)
                        {
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            ho_ImageEdge.Dispose();
                            HOperatorSet.GrayErosionShape(ho_GrayImage, out ho_ImageEdge, em_size, em_size, "rhombus");
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                            break;
                        }
                        else
                        {
                            MessageBox.Show("请输入系数");
                            break;

                        }
                    case 4:
                        if (em_size > 0 & em_size<13)
                        {
                        HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                        ho_ImageEdge.Dispose();
                        HOperatorSet.Emphasize(ho_GrayImage, out ho_ImageEdge, 7, 7, em_size);                      
                        HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);
                        break;
                        }
                        else
                        {
                            MessageBox.Show("请输入1-12之内的系数");
                            break;
                        }
                    case 5:
                        if (em_size > 0 & em_size < 13)
                        {
                            HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);
                            ho_ImageEdge.Dispose();
                            HOperatorSet.Illuminate(ho_GrayImage, out ho_ImageEdge, 41, 41, em_size);
                            ho_ImageInvert.Dispose();
                            HOperatorSet.InvertImage(ho_ImageEdge, out ho_ImageInvert);
                            ho_ImageEdge = ho_ImageInvert;
                            HOperatorSet.DispObj(ho_ImageEdge, hv_ExpDefaultWinHandle);

                            break;
                        }
                        else
                        {
                            MessageBox.Show("请输入1-12之内的系数");
                            break;
                        }
                    default:
                        MessageBox.Show("请选择处理模式");
                        break;
                }
            }
            #endregion
            else
            {
                MessageBox.Show("请先进行灰度处理");
            }
            }

        }
        #endregion

        private void button7_Click(object sender, EventArgs e)
        {
            if (ho_Regions.CountObj() != 0)
            {
                HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
                HOperatorSet.AreaCenter(ho_Regions, out hv_Area1, out hv_Row1, out hv_Column1);
                hv_Area2 = hv_Area1.TupleReal();
                hv_area = ((hv_Width * hv_Height)).TupleReal();
                hv_bizhong = 100*hv_Area2 / hv_area;
                ///MessageBox.Show("磨粒覆盖面积比" + hv_bizhong + "%");
                textBox1.Text = hv_bizhong.ToString();
            }
            else 
            {
                MessageBox.Show("请先进行图像预处理");
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            int size = trackBar2.Value;
            if (ho_Regions.CountObj()!=0)
            {
                HOperatorSet.ClearWindow(hv_ExpDefaultWinHandle);//清除图像
                //图像腐蚀
                ho_RegionErosion.Dispose();
                HOperatorSet.ErosionCircle(ho_Regions, out ho_RegionErosion, size);
                //区域联通计算
                ho_Connection.Dispose();
                HOperatorSet.Connection(ho_RegionErosion, out ho_Connection);
                //选择合适的区域
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_Connection, out ho_SelectedRegions, "area", "and", 1000,9999900000);
                //填充区域孔洞
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
                //对颗粒进行膨胀处理
                ho_RegionDilation1.Dispose();
                HOperatorSet.DilationCircle(ho_RegionFillUp, out ho_RegionDilation1, size);
                //显示
                HOperatorSet.DispObj(ho_GrayImage, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_RegionDilation1, hv_ExpDefaultWinHandle);
            }
            else
            {
                MessageBox.Show("请先进行图像分割");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (ho_RegionDilation1.CountObj()!=0)
            {
                // 数颗粒的个数
                HOperatorSet.CountObj(ho_RegionDilation1, out hv_Number);
                // 对颗粒按照第一个点的行坐标进行排序
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_RegionDilation1, out ho_SortedRegions, "first_point", "false", "row");
                // 求颗粒的面积和中心坐标
                HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
                // 显示图像和选中的颗粒
                HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
                HOperatorSet.DispObj(ho_SortedRegions, hv_ExpDefaultWinHandle);
                //显示图像的代码
                HTuple end_val47 = hv_Number;
                HTuple step_val47 = 1;
                for (hv_i = 1; hv_i.Continue(end_val47, step_val47); hv_i = hv_i.TupleAdd(step_val47))
                {
                    ho_ObjectSelected_1.Dispose();
                    HOperatorSet.SelectObj(ho_SortedRegions, out ho_ObjectSelected_1, hv_i);
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "red");
                    HOperatorSet.DispObj(ho_ObjectSelected_1, hv_ExpDefaultWinHandle);
                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_Row.TupleSelect(hv_i - 1),
                        hv_Column.TupleSelect(hv_i - 1));
                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, "white");
                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, hv_i);
                    //write_string (WindowHandle, Area[i-1])

                }
                textBox2.Text = hv_Number.ToString();
            }
            else
            {
                MessageBox.Show("请先进行形态处理");
            
            }
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HALCON_BIAO help = new HALCON_BIAO();
            help.Show();
        }

        private void 保存图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {   
      
            saveImage.FileName = "TPT_01";
            saveImage.Title = "另存为";
            if (saveImage.ShowDialog() == DialogResult.OK)
            {
                // 保存文件
                HOperatorSet.DumpWindow(hv_ExpDefaultWinHandle, "jpeg", saveImage.FileName);
                //ho_Image.Dispose();
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (ho_SortedRegions.CountObj() != 0)
            {
                if (textBox3.Text != "" && textBox4.Text != "")
                {
                    int min = int.Parse(textBox3.Text);
                    int max = int.Parse(textBox4.Text);
                    if (min >= 0)
                    {
                        //计算颗粒的长度
                        HOperatorSet.Contlength(ho_SortedRegions, out hv_ContLength);
                        //计算颗粒的圆度
                        HOperatorSet.Circularity(ho_SortedRegions, out hv_Circularity);
                        //计算颗粒等效椭圆的长短轴
                        HOperatorSet.EllipticAxis(ho_SortedRegions, out hv_Ra, out hv_Rb, out hv_Phi);
                        //求长短轴之比
                        HOperatorSet.TupleDiv(hv_Ra, hv_Rb, out hv_Quot);
                        //定义总个数
                        hv_all = 0;
                        //定义圆形颗粒个数
                        hv_ro = 0;
                        //定义条形颗粒个数
                        hv_ti = 0;
                        //定义片状颗粒个数
                        hv_pi = 0;
                        HTuple end_val78 = hv_Number - 1;
                        HTuple step_val78 = 1;
                        for (hv_i = 0; hv_i.Continue(end_val78, step_val78); hv_i = hv_i.TupleAdd(step_val78))
                        {
                            if ((int)((new HTuple(((hv_ContLength.TupleSelect(hv_i))).TupleGreater(min))).TupleAnd(new HTuple(((hv_ContLength.TupleSelect(hv_i))).TupleLess(max)))) != 0)
                            {
                                hv_all = hv_all + 1;
                                if ((int)(new HTuple(((hv_Circularity.TupleSelect(hv_i))).TupleGreater(0.65))) != 0)
                                {
                                    hv_ro = hv_ro + 1;
                                }
                                if ((int)(new HTuple(((hv_Quot.TupleSelect(hv_i))).TupleGreater(3))) != 0)
                                {
                                    hv_ti = hv_ti + 1;
                                }
                                else
                                {
                                    hv_pi = hv_pi + 1;
                                }
                            }
                        }
                        textBox7.Text = hv_ro.ToString();
                        textBox6.Text = hv_ti.ToString();
                        textBox8.Text = hv_pi.ToString();
                        textBox5.Text = hv_all.ToString();
                    }

                }
                else
                {
                    MessageBox.Show("请输入长度范围");

                }

            }
            else
            {
                MessageBox.Show("请先进行磨粒个数计算");

            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            //IsNumber 的作用是判断输入按键是否为数字
            //（char）8是退格键的值，可允许用户敲退格键对输入进行更改
            //针对其他按键输入则提示错误，不允许输入
            //if ((Char.IsNumber(e.KeyChar)) || e.KeyChar == (char)8)
            //{
            //    e.Handled = true;
            //}
            //else
            //{
            //    MessageBox.Show("请输入数字！");
            //}
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            EXCEL_out excelout = new EXCEL_out();
            excelout.Show();
        }



    }
}
