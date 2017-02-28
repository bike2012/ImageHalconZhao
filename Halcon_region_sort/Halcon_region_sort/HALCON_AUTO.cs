using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;                          //引用halcon
using System.IO;

namespace Halcon_region_sort
{
    public partial class halcon_form : Form
    {

        public halcon_form()
        {
            InitializeComponent();
        }
       //private HalconWindow wch = new HalconWindow();
        public HTuple hv_ExpDefaultWinHandle;
        // Local iconic variables 定义图像变量
        public HObject ho_Image, ho_GrayImage, ho_Regions;
        public HObject ho_Connection, ho_RegionResult, ho_SelectedRegions;
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
        private void halcon_form_Load(object sender, EventArgs e)
        {
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
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
            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
        }
        //

        private void read_button_Click(object sender, EventArgs e)
        {

            
            //程序开始
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
            HOperatorSet.SetColored(hv_ExpDefaultWinHandle, 12);
            //读取图片
            //ho_Image.Dispose();
            //HOperatorSet.ReadImage(out ho_Image, "E:/WORKS/halcon/zhao_jobs/zhao_jobs/1-500-2.JPG");
            //dev_close_window(...);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //获取图像窗口
            //dev_open_window(...);
            //彩色图转成灰度图
            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
            //阈值处理
            ho_Regions.Dispose();
            HOperatorSet.BinaryThreshold(ho_GrayImage, out ho_Regions, "max_separability",
                "dark", out hv_UsedThreshold);
            //求图像联通域
            ho_Connection.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_Connection);
            //将区域闭合
            ho_RegionResult.Dispose();
            HOperatorSet.CloseEdges(ho_Connection, ho_GrayImage, out ho_RegionResult, 16);
            //选择合适的区域
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_RegionResult, out ho_SelectedRegions, "area", "and", 2040.82, 200000);
            //填充区域孔洞
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
            //区域膨胀处理
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationCircle(ho_RegionFillUp, out ho_RegionDilation, 15.5);
            //将区域群合成一个区域
            ho_RegionUnion.Dispose();
            HOperatorSet.Union2(ho_RegionDilation, ho_RegionDilation, out ho_RegionUnion);
            // 获取刚兴趣区域图像
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionUnion, out ho_ImageReduced);
            //对感兴趣区域图像进行阈值处理
            ho_Regions1.Dispose();
            HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Regions1, "max_separability", "dark", out hv_UsedThreshold1);
            //区域腐蚀处理，用于去除区域之间的粘结
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_Regions1, out ho_RegionErosion, 1.5);
            //求图像连通域
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionErosion, out ho_ConnectedRegions);
            //选择合适的颗粒，去除无用的杂物
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "area", "and", 2000, 9999999);
            //对颗粒进行膨胀处理
            ho_RegionDilation1.Dispose();
            HOperatorSet.DilationCircle(ho_SelectedRegions1, out ho_RegionDilation1, 10.5);
            //数颗粒的个数
            HOperatorSet.CountObj(ho_RegionDilation1, out hv_Number);
            //对颗粒按照第一个点的行坐标进行排序
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_RegionDilation1, out ho_SortedRegions, "first_point", "false", "row");
            //求颗粒的面积和中心坐标
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
            //显示图像和选中的颗粒
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
            //求面积占比
            HOperatorSet.AreaCenter(ho_Regions, out hv_Area1, out hv_Row1, out hv_Column1);
            hv_Area2 = hv_Area1.TupleReal();
            hv_area = ((hv_Width * hv_Height)).TupleReal();
            hv_bizhong = hv_Area2 / hv_area;
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
                if ((int)((new HTuple(((hv_ContLength.TupleSelect(hv_i))).TupleGreater(200))).TupleAnd(new HTuple(((hv_ContLength.TupleSelect(hv_i))).TupleLess(80000)))) != 0)
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
            //decompose3 (Image, Image1, Image2, Image3)
            ho_Image.Dispose();
            ho_GrayImage.Dispose();
            ho_Regions.Dispose();
            ho_Connection.Dispose();
            ho_RegionResult.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_RegionUnion.Dispose();
            ho_ImageReduced.Dispose();
            ho_Regions1.Dispose();
            ho_RegionErosion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_RegionDilation1.Dispose();
            ho_SortedRegions.Dispose();
            ho_ObjectSelected_1.Dispose();
            MessageBox.Show("占图比重为：" + hv_bizhong + "\n颗粒个数：" + hv_Number + "\n圆形颗粒个数：" + hv_ro + "\n条形颗粒个数" + hv_ti + "\n片形颗粒个数" + hv_pi);

        }
       
        private void button_prcoessing_Click(object sender, EventArgs e)
        {

            hv_ExpDefaultWinHandle = hWindowControl1.HalconWindow;
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

        private void hWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {

        }

        private void halcon_form_Click(object sender, EventArgs e)
        {

            MessageBox.Show("程序属性\n铁谱处理2");
        }

        private void halcon_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            HALCON_SORT.form0.Show();
        }

    }
    public partial class HDevelopExport
    {
        public HTuple hv_ExpDefaultWinHandle;
        public double bizhong;
        // Local iconic variables 定义图像变量
        public HObject ho_Image, ho_GrayImage, ho_Regions;
        HObject ho_Connection, ho_RegionResult, ho_SelectedRegions;
        HObject ho_RegionFillUp, ho_RegionDilation, ho_RegionUnion;
        HObject ho_ImageReduced, ho_Regions1, ho_RegionErosion;
        HObject ho_ConnectedRegions, ho_SelectedRegions1, ho_RegionDilation1;
        HObject ho_SortedRegions, ho_ObjectSelected_1 = null;

        // Local control variables 定义控制变量

        public HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
        HTuple hv_UsedThreshold = null, hv_UsedThreshold1 = null;
        HTuple hv_Number = null, hv_Area = null, hv_Row = null;
        HTuple hv_Column = null, hv_i = null, hv_Area1 = null;
        HTuple hv_Row1 = null, hv_Column1 = null, hv_Area2 = null;
        public HTuple hv_area = null, hv_bizhong = null;
        // Main procedure 
        private void action(HObject x)
        {

            ho_Image = x;
             //ho_Image.Dispose();
             //Local iconic variables 定义图像变量
            //HObject ho_Image, ho_GrayImage, ho_Regions;
            //HObject ho_Connection, ho_RegionResult, ho_SelectedRegions;
            //HObject ho_RegionFillUp, ho_RegionDilation, ho_RegionUnion;
            //HObject ho_ImageReduced, ho_Regions1, ho_RegionErosion;
            //HObject ho_ConnectedRegions, ho_SelectedRegions1, ho_RegionDilation1;
            //HObject ho_SortedRegions, ho_ObjectSelected_1 = null;

            //// Local control variables 定义控制变量

            //HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
            //HTuple hv_UsedThreshold = null, hv_UsedThreshold1 = null;
            //HTuple hv_Number = null, hv_Area = null, hv_Row = null;
            //HTuple hv_Column = null, hv_i = null, hv_Area1 = null;
            //HTuple hv_Row1 = null, hv_Column1 = null, hv_Area2 = null;
            //HTuple hv_area = null, hv_bizhong = null;
            // Initialize local and output iconic variables 初始化本地图像变量和输出图像变量
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
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

            //程序开始
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
            HOperatorSet.SetColored(hv_ExpDefaultWinHandle, 12);
            //读取图片
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "E:/WORKS/halcon/zhao_jobs/zhao_jobs/1-500-2.JPG");
            //dev_close_window(...);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            //获取图像窗口
            //dev_open_window(...);
            //彩色图转成灰度图
            ho_GrayImage.Dispose();
            HOperatorSet.Rgb1ToGray(ho_Image, out ho_GrayImage);
            //阈值处理
            ho_Regions.Dispose();
            HOperatorSet.BinaryThreshold(ho_GrayImage, out ho_Regions, "max_separability",
                "dark", out hv_UsedThreshold);
            //求图像联通域
            ho_Connection.Dispose();
            HOperatorSet.Connection(ho_Regions, out ho_Connection);
            //将区域闭合
            ho_RegionResult.Dispose();
            HOperatorSet.CloseEdges(ho_Connection, ho_GrayImage, out ho_RegionResult, 16);
            //选择合适的区域
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_RegionResult, out ho_SelectedRegions, "area", "and", 2040.82, 200000);
            //填充区域孔洞
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_SelectedRegions, out ho_RegionFillUp);
            //区域膨胀处理
            ho_RegionDilation.Dispose();
            HOperatorSet.DilationCircle(ho_RegionFillUp, out ho_RegionDilation, 15.5);
            //将区域群合成一个区域
            ho_RegionUnion.Dispose();
            HOperatorSet.Union2(ho_RegionDilation, ho_RegionDilation, out ho_RegionUnion);
            // 获取刚兴趣区域图像
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(ho_GrayImage, ho_RegionUnion, out ho_ImageReduced);
            //对感兴趣区域图像进行阈值处理
            ho_Regions1.Dispose();
            HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Regions1, "max_separability", "dark", out hv_UsedThreshold1);
            //区域腐蚀处理，用于去除区域之间的粘结
            ho_RegionErosion.Dispose();
            HOperatorSet.ErosionCircle(ho_Regions1, out ho_RegionErosion, 1.5);
            //求图像连通域
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionErosion, out ho_ConnectedRegions);
            //选择合适的颗粒，去除无用的杂物
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions1, "area", "and", 2000, 9999999);
            //对颗粒进行膨胀处理
            ho_RegionDilation1.Dispose();
            HOperatorSet.DilationCircle(ho_SelectedRegions1, out ho_RegionDilation1, 10.5);
            //数颗粒的个数
            HOperatorSet.CountObj(ho_RegionDilation1, out hv_Number);
            //对颗粒按照第一个点的行坐标进行排序
            ho_SortedRegions.Dispose();
            HOperatorSet.SortRegion(ho_RegionDilation1, out ho_SortedRegions, "first_point", "false", "row");
            //求颗粒的面积和中心坐标
            HOperatorSet.AreaCenter(ho_SortedRegions, out hv_Area, out hv_Row, out hv_Column);
            //显示图像和选中的颗粒
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
            //求面积占比
            HOperatorSet.AreaCenter(ho_Regions, out hv_Area1, out hv_Row1, out hv_Column1);
            hv_Area2 = hv_Area1.TupleReal();
            hv_area = ((hv_Width * hv_Height)).TupleReal();
            hv_bizhong = hv_Area2 / hv_area;
            bizhong = hv_bizhong;
            //decompose3 (Image, Image1, Image2, Image3)
            ho_Image.Dispose();
            ho_GrayImage.Dispose();
            ho_Regions.Dispose();
            ho_Connection.Dispose();
            ho_RegionResult.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionFillUp.Dispose();
            ho_RegionDilation.Dispose();
            ho_RegionUnion.Dispose();
            ho_ImageReduced.Dispose();
            ho_Regions1.Dispose();
            ho_RegionErosion.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_RegionDilation1.Dispose();
            ho_SortedRegions.Dispose();
            ho_ObjectSelected_1.Dispose();


        }
        public HObject showimg(HTuple Window)
        {
            hv_ExpDefaultWinHandle = Window;
            //HObject ho_Image, ho_GrayImage, ho_Regions;
            //HObject ho_Connection, ho_RegionResult, ho_SelectedRegions;
            //HObject ho_RegionFillUp, ho_RegionDilation, ho_RegionUnion;
            //HObject ho_ImageReduced, ho_Regions1, ho_RegionErosion;
            //HObject ho_ConnectedRegions, ho_SelectedRegions1, ho_RegionDilation1;
            //HObject ho_SortedRegions, ho_ObjectSelected_1 = null;

            // Local control variables 

            //HTuple hv_Width = null, hv_Height = null, hv_WindowHandle = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_GrayImage);
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
            //程序开始
            HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
            HOperatorSet.SetColored(hv_ExpDefaultWinHandle, 12);
            ho_Image.Dispose();
            HOperatorSet.ReadImage(out ho_Image, "E:/WORKS/halcon/zhao_jobs/zhao_jobs/1-500-2.JPG");
            //dev_close_window(...);
            HOperatorSet.GetImageSize(ho_Image, out hv_Width, out hv_Height);
            HOperatorSet.DispObj(ho_Image, hv_ExpDefaultWinHandle);
            return (ho_Image);
        }

        public void InitHalcon()
        {
            // Default settings used in HDevelop 
            HOperatorSet.SetSystem("width", 512);
            HOperatorSet.SetSystem("height", 512);
        }

        public void RunHalcon(HTuple Window)
        {
            hv_ExpDefaultWinHandle = Window;
            action(ho_Image);
        }

    }
}



