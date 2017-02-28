//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using HalconDotNet;                          //引用halcon


//namespace Halcon_Programs
//{
//    class HalconWindow
//    {
//        /******************************************************************************************/
//        //缩放显示图像使用的类私有全局变量
//        /******************************************************************************************/
//        private int zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol;                     // 设定图像的窗口显示部分
//        private int current_beginRow, current_beginCol, current_endRow, current_endCol;         // 获取图像的当前显示部分

//        /******************************************************************************************/
//        //设置hanlcon的显示窗体
//        /******************************************************************************************/
//        public void SetWindow(HWindowControl hWindowControl)
//        {
//            HTuple hWindowHandle1 = hWindowControl.HalconWindow;

//            //系统参数初始化
//            HOperatorSet.SetColor(hWindowHandle1, "red");
//            HOperatorSet.SetDraw(hWindowHandle1, "margin");
//            //HOperatorSet.SetColored(hWindowHandle, 12);
//            HOperatorSet.SetSystem("int_zooming", "true");
//            HOperatorSet.SetSystem("border_shape_models", "false");
//            HOperatorSet.SetSystem("tsp_width", 6000);
//            HOperatorSet.SetSystem("tsp_height", 6000);
//        }

//        //按照指定的中心缩放当前图像
//        public void DispImageZoom(HObject t_image, HWindowControl hw_Ctrl, HTuple mode, double Mouse_row, double Mouse_col)
//        {
//            if (t_image != null)
//            {
//                HTuple width, height;
//                HOperatorSet.GetImageSize(t_image, out width, out height);      //获取图像的尺寸
//                int hv_imageWidth, hv_imageHeight;
//                hv_imageWidth = width;
//                hv_imageHeight = height;

//                try
//                {
//                    hw_Ctrl.HalconWindow.GetPart(out current_beginRow, out current_beginCol, out current_endRow, out current_endCol);
//                }
//                catch (Exception ex)
//                {
//                    return;
//                }

//                if (mode > 0)                 // 放大图像
//                {
//                    zoom_beginRow = (int)(current_beginRow + (Mouse_row - current_beginRow) * 0.300d);
//                    zoom_beginCol = (int)(current_beginCol + (Mouse_col - current_beginCol) * 0.300d);
//                    zoom_endRow = (int)(current_endRow - (current_endRow - Mouse_row) * 0.300d);
//                    zoom_endCol = (int)(current_endCol - (current_endCol - Mouse_col) * 0.300d);
//                }
//                else                // 缩小图像
//                {
//                    zoom_beginRow = (int)(Mouse_row - (Mouse_row - current_beginRow) / 0.700d);
//                    zoom_beginCol = (int)(Mouse_col - (Mouse_col - current_beginCol) / 0.700d);
//                    zoom_endRow = (int)(Mouse_row + (current_endRow - Mouse_row) / 0.700d);
//                    zoom_endCol = (int)(Mouse_col + (current_endCol - Mouse_col) / 0.700d);
//                }

//                try
//                {
//                    int hw_width, hw_height;
//                    hw_width = hw_Ctrl.WindowSize.Width;
//                    hw_height = hw_Ctrl.WindowSize.Height;

//                    bool _isOutOfArea = true;
//                    bool _isOutOfSize = true;
//                    bool _isOutOfPixel = true;  //避免像素过大

//                    _isOutOfArea = zoom_beginRow >= hv_imageHeight || zoom_endRow <= 0 || zoom_beginCol >= hv_imageWidth || zoom_endCol < 0;
//                    _isOutOfSize = (zoom_endRow - zoom_beginRow) > hv_imageHeight * 20 || (zoom_endCol - zoom_beginCol) > hv_imageWidth * 20;
//                    _isOutOfPixel = hw_height / (zoom_endRow - zoom_beginRow) > 500 || hw_width / (zoom_endCol - zoom_beginCol) > 500;

//                    if (_isOutOfArea || _isOutOfSize)
//                    {
//                        DispImageFit(t_image, hw_Ctrl);
//                    }
//                    else if (!_isOutOfPixel)
//                    {
//                        hw_Ctrl.HalconWindow.ClearWindow();

//                        hw_Ctrl.HalconWindow.SetPaint(new HTuple("default"));
//                        //              保持图像显示比例
//                        //hw_Ctrl.HalconWindow.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_endCol);
//                        hw_Ctrl.HalconWindow.SetPart(zoom_beginRow, zoom_beginCol, zoom_endRow, zoom_beginCol + (zoom_endRow - zoom_beginRow) * hw_width / hw_height);
//                        hw_Ctrl.HalconWindow.DispObj(t_image);
//                    }
//                }
//                catch (Exception ex)        //ex.Message;
//                {
//                    DispImageFit(t_image, hw_Ctrl);
//                }
//            }
//        }

//        //最大化图像，适应窗体尺寸显示图像
//        public void DispImageFit(HObject t_image, HWindowControl hw_Ctrl)
//        {
//            if (t_image != null)
//            {
//                hw_Ctrl.HalconWindow.ClearWindow();
//                HTuple hWindowHandle = hw_Ctrl.HalconWindow;        //图像显示句柄
//                int hw_width = hw_Ctrl.WindowSize.Width;            //图像显示尺寸
//                int hw_height = hw_Ctrl.WindowSize.Height;

//                HTuple width, height;
//                HOperatorSet.GetImageSize(t_image, out width, out height);
//                if (1.0 * width[0].I / hw_width > 1.0 * height[0].I / hw_height)
//                {
//                    double real = 1.0 * width[0].I / hw_width;
//                    HOperatorSet.SetPart(hWindowHandle, 0, 0, real * hw_height, real * hw_width);
//                    HOperatorSet.DispObj(t_image, hWindowHandle);
//                }
//                else
//                {
//                    double real = 1.0 * height[0].I / hw_height;
//                    HOperatorSet.SetPart(hWindowHandle, 0, 0, real * hw_height, real * hw_width);
//                    HOperatorSet.DispObj(t_image, hWindowHandle);
//                }
//            }
//        }

//        //  public HTuple hv_ExpDefaultWinHandle;

//        // Chapter: Graphics / Text //hv_WindowHandle
//        // Short Description: This procedure writes a text message. 
//        public void disp_message(HTuple hv_ExpDefaultWinHandle, HTuple hv_String, HTuple hv_CoordSystem,
//            HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
//        {



//            // Local iconic variables 

//            // Local control variables 

//            HTuple hv_M = null, hv_N = null, hv_Red = null;
//            HTuple hv_Green = null, hv_Blue = null, hv_RowI1Part = null;
//            HTuple hv_ColumnI1Part = null, hv_RowI2Part = null, hv_ColumnI2Part = null;
//            HTuple hv_RowIWin = null, hv_ColumnIWin = null, hv_WidthWin = new HTuple();
//            HTuple hv_HeightWin = null, hv_I = null, hv_RowI = new HTuple();
//            HTuple hv_ColumnI = new HTuple(), hv_StringI = new HTuple();
//            HTuple hv_MaxAscent = new HTuple(), hv_MaxDescent = new HTuple();
//            HTuple hv_MaxWidth = new HTuple(), hv_MaxHeight = new HTuple();
//            HTuple hv_R1 = new HTuple(), hv_C1 = new HTuple(), hv_FactorRowI = new HTuple();
//            HTuple hv_FactorColumnI = new HTuple(), hv_UseShadow = new HTuple();
//            HTuple hv_ShadowColor = new HTuple(), hv_Exception = new HTuple();
//            HTuple hv_Width = new HTuple(), hv_Index = new HTuple();
//            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
//            HTuple hv_W = new HTuple(), hv_H = new HTuple(), hv_FrameHeight = new HTuple();
//            HTuple hv_FrameWidth = new HTuple(), hv_R2 = new HTuple();
//            HTuple hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
//            HTuple hv_CurrentColor = new HTuple();
//            HTuple hv_Box_COPY_INP_TMP = hv_Box.Clone();
//            HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
//            HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
//            HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
//            HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

//            // Initialize local and output iconic variables 
//            //This procedure displays text in a graphics window.
//            //
//            //Input parameters:
//            //WindowHandle: The WindowHandle of the graphics window, where
//            //   the message should be displayed
//            //String: A tuple of strings containing the text message to be displayed
//            //CoordSystem: If set to 'window', the text position is given
//            //   with respect to the window coordinate system.
//            //   If set to 'image', image coordinates are used.
//            //   (This may be useful in zoomed images.)
//            //Row: The row coordinate of the desired text position
//            //   If set to -1, a default value of 12 is used.
//            //   A tuple of values is allowed to display text at different
//            //   positions.
//            //Column: The column coordinate of the desired text position
//            //   If set to -1, a default value of 12 is used.
//            //   A tuple of values is allowed to display text at different
//            //   positions.
//            //Color: defines the color of the text as string.
//            //   If set to [], '' or 'auto' the currently set color is used.
//            //   If a tuple of strings is passed, the colors are used cyclically...
//            //   - if |Row| == |Column| == 1: for each new textline
//            //   = else for each text position.
//            //Box: If Box[0] is set to 'true', the text is written within an orange box.
//            //     If set to' false', no box is displayed.
//            //     If set to a color string (e.g. 'white', '#FF00CC', etc.),
//            //       the text is written in a box of that color.
//            //     An optional second value for Box (Box[1]) controls if a shadow is displayed:
//            //       'true' -> display a shadow in a default color
//            //       'false' -> display no shadow
//            //       otherwise -> use given string as color string for the shadow color
//            //
//            //It is possible to display multiple text strings in a single call.
//            //In this case, some restrictions apply:
//            //- Multiple text positions can be defined by specifying a tuple
//            //  with multiple Row and/or Column coordinates, i.e.:
//            //  - |Row| == n, |Column| == n
//            //  - |Row| == n, |Column| == 1
//            //  - |Row| == 1, |Column| == n
//            //- If |Row| == |Column| == 1,
//            //  each element of String is display in a new textline.
//            //- If multiple positions or specified, the number of Strings
//            //  must match the number of positions, i.e.:
//            //  - Either |String| == n (each string is displayed at the
//            //                          corresponding position),
//            //  - or     |String| == 1 (The string is displayed n times).
//            //
//            if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
//            {
//                hv_Color_COPY_INP_TMP = "";
//            }
//            if ((int)(new HTuple(hv_Box_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
//            {
//                hv_Box_COPY_INP_TMP = "false";
//            }
//            //
//            //
//            //Check conditions
//            //
//            hv_M = (new HTuple(hv_Row_COPY_INP_TMP.TupleLength())) * (new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
//                ));
//            hv_N = new HTuple(hv_Row_COPY_INP_TMP.TupleLength());
//            if ((int)((new HTuple(hv_M.TupleEqual(0))).TupleOr(new HTuple(hv_String_COPY_INP_TMP.TupleEqual(
//                new HTuple())))) != 0)
//            {

//                return;
//            }
//            if ((int)(new HTuple(hv_M.TupleNotEqual(1))) != 0)
//            {
//                //Multiple positions
//                //
//                //Expand single parameters
//                if ((int)(new HTuple((new HTuple(hv_Row_COPY_INP_TMP.TupleLength())).TupleEqual(
//                    1))) != 0)
//                {
//                    hv_N = new HTuple(hv_Column_COPY_INP_TMP.TupleLength());
//                    HOperatorSet.TupleGenConst(hv_N, hv_Row_COPY_INP_TMP, out hv_Row_COPY_INP_TMP);
//                }
//                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
//                    )).TupleEqual(1))) != 0)
//                {
//                    HOperatorSet.TupleGenConst(hv_N, hv_Column_COPY_INP_TMP, out hv_Column_COPY_INP_TMP);
//                }
//                else if ((int)(new HTuple((new HTuple(hv_Column_COPY_INP_TMP.TupleLength()
//                    )).TupleNotEqual(new HTuple(hv_Row_COPY_INP_TMP.TupleLength())))) != 0)
//                {
//                    throw new HalconException("Number of elements in Row and Column does not match.");
//                }
//                if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength())).TupleEqual(
//                    1))) != 0)
//                {
//                    HOperatorSet.TupleGenConst(hv_N, hv_String_COPY_INP_TMP, out hv_String_COPY_INP_TMP);
//                }
//                else if ((int)(new HTuple((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
//                    )).TupleNotEqual(hv_N))) != 0)
//                {
//                    throw new HalconException("Number of elements in Strings does not match number of positions.");
//                }
//                //
//            }
//            //
//            //Prepare window
//            HOperatorSet.GetRgb(hv_ExpDefaultWinHandle, out hv_Red, out hv_Green, out hv_Blue);
//            HOperatorSet.GetPart(hv_ExpDefaultWinHandle, out hv_RowI1Part, out hv_ColumnI1Part,
//                out hv_RowI2Part, out hv_ColumnI2Part);
//            HOperatorSet.GetWindowExtents(hv_ExpDefaultWinHandle, out hv_RowIWin, out hv_ColumnIWin,
//                out hv_WidthWin, out hv_HeightWin);
//            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
//            //
//            //Loop over all positions
//            HTuple end_val89 = hv_N - 1;
//            HTuple step_val89 = 1;
//            for (hv_I = 0; hv_I.Continue(end_val89, step_val89); hv_I = hv_I.TupleAdd(step_val89))
//            {
//                hv_RowI = hv_Row_COPY_INP_TMP.TupleSelect(hv_I);
//                hv_ColumnI = hv_Column_COPY_INP_TMP.TupleSelect(hv_I);
//                //Allow multiple strings for a single position.
//                if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
//                {
//                    hv_StringI = hv_String_COPY_INP_TMP.Clone();
//                }
//                else
//                {
//                    //In case of multiple positions, only single strings
//                    //are allowed per position.
//                    //For line breaks, use \n in this case.
//                    hv_StringI = hv_String_COPY_INP_TMP.TupleSelect(hv_I);
//                }
//                //Default settings
//                //-1 is mapped to 12.
//                if ((int)(new HTuple(hv_RowI.TupleEqual(-1))) != 0)
//                {
//                    hv_RowI = 12;
//                }
//                if ((int)(new HTuple(hv_ColumnI.TupleEqual(-1))) != 0)
//                {
//                    hv_ColumnI = 12;
//                }
//                //
//                //Split string into one string per line.
//                hv_StringI = ((("" + hv_StringI) + "")).TupleSplit("\n");
//                //
//                //Estimate extentions of text depending on font size.
//                HOperatorSet.GetFontExtents(hv_ExpDefaultWinHandle, out hv_MaxAscent, out hv_MaxDescent,
//                    out hv_MaxWidth, out hv_MaxHeight);
//                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
//                {
//                    hv_R1 = hv_RowI.Clone();
//                    hv_C1 = hv_ColumnI.Clone();
//                }
//                else
//                {
//                    //Transform image to window coordinates.
//                    hv_FactorRowI = (1.0 * hv_HeightWin) / ((hv_RowI2Part - hv_RowI1Part) + 1);
//                    hv_FactorColumnI = (1.0 * hv_WidthWin) / ((hv_ColumnI2Part - hv_ColumnI1Part) + 1);
//                    hv_R1 = (((hv_RowI - hv_RowI1Part) + 0.5) * hv_FactorRowI) - 0.5;
//                    hv_C1 = (((hv_ColumnI - hv_ColumnI1Part) + 0.5) * hv_FactorColumnI) - 0.5;
//                }
//                //
//                //Display text box depending on text size.
//                hv_UseShadow = 1;
//                hv_ShadowColor = "gray";
//                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleEqual("true"))) != 0)
//                {
//                    if (hv_Box_COPY_INP_TMP == null)
//                        hv_Box_COPY_INP_TMP = new HTuple();
//                    hv_Box_COPY_INP_TMP[0] = "#fce9d4";
//                    hv_ShadowColor = "#f28d26";
//                }
//                if ((int)(new HTuple((new HTuple(hv_Box_COPY_INP_TMP.TupleLength())).TupleGreater(
//                    1))) != 0)
//                {
//                    if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual("true"))) != 0)
//                    {
//                        //Use default ShadowColor set above
//                    }
//                    else if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(1))).TupleEqual(
//                        "false"))) != 0)
//                    {
//                        hv_UseShadow = 0;
//                    }
//                    else
//                    {
//                        hv_ShadowColor = hv_Box_COPY_INP_TMP.TupleSelect(1);
//                        //Valid color?
//                        try
//                        {
//                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_Box_COPY_INP_TMP.TupleSelect(
//                                1));
//                        }
//                        // catch (Exception) 
//                        catch (HalconException HDevExpDefaultException1)
//                        {
//                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                            hv_Exception = new HTuple("Wrong value of control parameter Box[1] (must be a 'true', 'false', or a valid color string)");
//                            throw new HalconException(hv_Exception);
//                        }
//                    }
//                }
//                if ((int)(new HTuple(((hv_Box_COPY_INP_TMP.TupleSelect(0))).TupleNotEqual("false"))) != 0)
//                {
//                    //Valid color?
//                    try
//                    {
//                        HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_Box_COPY_INP_TMP.TupleSelect(
//                            0));
//                    }
//                    // catch (Exception) 
//                    catch (HalconException HDevExpDefaultException1)
//                    {
//                        HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                        hv_Exception = new HTuple("Wrong value of control parameter Box[0] (must be a 'true', 'false', or a valid color string)");
//                        throw new HalconException(hv_Exception);
//                    }
//                    //Calculate box extents
//                    hv_StringI = (" " + hv_StringI) + " ";
//                    hv_Width = new HTuple();
//                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength()
//                        )) - 1); hv_Index = (int)hv_Index + 1)
//                    {
//                        HOperatorSet.GetStringExtents(hv_ExpDefaultWinHandle, hv_StringI.TupleSelect(
//                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
//                        hv_Width = hv_Width.TupleConcat(hv_W);
//                    }
//                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_StringI.TupleLength()));
//                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
//                    hv_R2 = hv_R1 + hv_FrameHeight;
//                    hv_C2 = hv_C1 + hv_FrameWidth;
//                    //Display rectangles
//                    HOperatorSet.GetDraw(hv_ExpDefaultWinHandle, out hv_DrawMode);
//                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, "fill");
//                    //Set shadow color
//                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_ShadowColor);
//                    if ((int)(hv_UseShadow) != 0)
//                    {
//                        HOperatorSet.DispRectangle1(hv_ExpDefaultWinHandle, hv_R1 + 1, hv_C1 + 1, hv_R2 + 1,
//                            hv_C2 + 1);
//                    }
//                    //Set box color
//                    HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_Box_COPY_INP_TMP.TupleSelect(
//                        0));
//                    HOperatorSet.DispRectangle1(hv_ExpDefaultWinHandle, hv_R1, hv_C1, hv_R2,
//                        hv_C2);
//                    HOperatorSet.SetDraw(hv_ExpDefaultWinHandle, hv_DrawMode);
//                }
//                //Write text.
//                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_StringI.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
//                {
//                    //Set color
//                    if ((int)(new HTuple(hv_N.TupleEqual(1))) != 0)
//                    {
//                        //Wiht a single text position, each text line
//                        //may get a different color.
//                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
//                            )));
//                    }
//                    else
//                    {
//                        //With multiple text positions, each position
//                        //gets a single color for all text lines.
//                        hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_I % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
//                            )));
//                    }
//                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
//                        "auto")))) != 0)
//                    {
//                        //Valid color?
//                        try
//                        {
//                            HOperatorSet.SetColor(hv_ExpDefaultWinHandle, hv_CurrentColor);
//                        }
//                        // catch (Exception) 
//                        catch (HalconException HDevExpDefaultException1)
//                        {
//                            HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                            hv_Exception = ((("Wrong value of control parameter Color[" + (hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
//                                )))) + "] == '") + hv_CurrentColor) + "' (must be a valid color string)";
//                            throw new HalconException(hv_Exception);
//                        }
//                    }
//                    else
//                    {
//                        HOperatorSet.SetRgb(hv_ExpDefaultWinHandle, hv_Red, hv_Green, hv_Blue);
//                    }
//                    //Finally display text
//                    hv_RowI = hv_R1 + (hv_MaxHeight * hv_Index);
//                    HOperatorSet.SetTposition(hv_ExpDefaultWinHandle, hv_RowI, hv_C1);
//                    HOperatorSet.WriteString(hv_ExpDefaultWinHandle, hv_StringI.TupleSelect(hv_Index));
//                }
//            }
//            //Reset changed window settings
//            HOperatorSet.SetRgb(hv_ExpDefaultWinHandle, hv_Red, hv_Green, hv_Blue);
//            HOperatorSet.SetPart(hv_ExpDefaultWinHandle, hv_RowI1Part, hv_ColumnI1Part, hv_RowI2Part,
//                hv_ColumnI2Part);

//            return;
//        }

//        // Chapter: Graphics / Text
//        // Short Description: Set font independent of OS 
//        public void set_display_font(HTuple hv_ExpDefaultWinHandle, HTuple hv_Size, HTuple hv_Font,
//            HTuple hv_Bold, HTuple hv_Slant)
//        {



//            // Local iconic variables 

//            // Local control variables 

//            HTuple hv_OS = null, hv_BufferWindowHandle = new HTuple();
//            HTuple hv_Ascent = new HTuple(), hv_Descent = new HTuple();
//            HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
//            HTuple hv_Scale = new HTuple(), hv_Exception = new HTuple();
//            HTuple hv_SubFamily = new HTuple(), hv_Fonts = new HTuple();
//            HTuple hv_SystemFonts = new HTuple(), hv_Guess = new HTuple();
//            HTuple hv_I = new HTuple(), hv_Index = new HTuple(), hv_AllowedFontSizes = new HTuple();
//            HTuple hv_Distances = new HTuple(), hv_Indices = new HTuple();
//            HTuple hv_FontSelRegexp = new HTuple(), hv_FontsCourier = new HTuple();
//            HTuple hv_Bold_COPY_INP_TMP = hv_Bold.Clone();
//            HTuple hv_Font_COPY_INP_TMP = hv_Font.Clone();
//            HTuple hv_Size_COPY_INP_TMP = hv_Size.Clone();
//            HTuple hv_Slant_COPY_INP_TMP = hv_Slant.Clone();

//            // Initialize local and output iconic variables 
//            //This procedure sets the text font of the current window with
//            //the specified attributes.
//            //It is assumed that following fonts are installed on the system:
//            //Windows: Courier New, Arial Times New Roman
//            //Mac OS X: CourierNewPS, Arial, TimesNewRomanPS
//            //Linux: courier, helvetica, times
//            //Because fonts are displayed smaller on Linux than on Windows,
//            //a scaling factor of 1.25 is used the get comparable results.
//            //For Linux, only a limited number of font sizes is supported,
//            //to get comparable results, it is recommended to use one of the
//            //following sizes: 9, 11, 14, 16, 20, 27
//            //(which will be mapped internally on Linux systems to 11, 14, 17, 20, 25, 34)
//            //
//            //Input parameters:
//            //WindowHandle: The graphics window for which the font will be set
//            //Size: The font size. If Size=-1, the default of 16 is used.
//            //Bold: If set to 'true', a bold font is used
//            //Slant: If set to 'true', a slanted font is used
//            //
//            HOperatorSet.GetSystem("operating_system", out hv_OS);
//            // dev_get_preferences(...); only in hdevelop
//            // dev_set_preferences(...); only in hdevelop
//            if ((int)((new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
//                new HTuple(hv_Size_COPY_INP_TMP.TupleEqual(-1)))) != 0)
//            {
//                hv_Size_COPY_INP_TMP = 16;
//            }
//            if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
//            {
//                //Set font on Windows systems
//                try
//                {
//                    //Check, if font scaling is switched on
//                    //open_window(...);
//                    HOperatorSet.SetFont(hv_ExpDefaultWinHandle, "-Consolas-16-*-0-*-*-1-");
//                    HOperatorSet.GetStringExtents(hv_ExpDefaultWinHandle, "test_string", out hv_Ascent,
//                        out hv_Descent, out hv_Width, out hv_Height);
//                    //Expected width is 110
//                    hv_Scale = 110.0 / hv_Width;
//                    hv_Size_COPY_INP_TMP = ((hv_Size_COPY_INP_TMP * hv_Scale)).TupleInt();
//                    //close_window(...);
//                }
//                // catch (Exception) 
//                catch (HalconException HDevExpDefaultException1)
//                {
//                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                    //throw (Exception)
//                }
//                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
//                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "Courier New";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "Consolas";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "Arial";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "Times New Roman";
//                }
//                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    hv_Bold_COPY_INP_TMP = 1;
//                }
//                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
//                {
//                    hv_Bold_COPY_INP_TMP = 0;
//                }
//                else
//                {
//                    hv_Exception = "Wrong value of control parameter Bold";
//                    throw new HalconException(hv_Exception);
//                }
//                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    hv_Slant_COPY_INP_TMP = 1;
//                }
//                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
//                {
//                    hv_Slant_COPY_INP_TMP = 0;
//                }
//                else
//                {
//                    hv_Exception = "Wrong value of control parameter Slant";
//                    throw new HalconException(hv_Exception);
//                }
//                try
//                {
//                    HOperatorSet.SetFont(hv_ExpDefaultWinHandle, ((((((("-" + hv_Font_COPY_INP_TMP) + "-") + hv_Size_COPY_INP_TMP) + "-*-") + hv_Slant_COPY_INP_TMP) + "-*-*-") + hv_Bold_COPY_INP_TMP) + "-");
//                }
//                // catch (Exception) 
//                catch (HalconException HDevExpDefaultException1)
//                {
//                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                    //throw (Exception)
//                }
//            }
//            else if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Dar"))) != 0)
//            {
//                //Set font on Mac OS X systems. Since OS X does not have a strict naming
//                //scheme for font attributes, we use tables to determine the correct font
//                //name.
//                hv_SubFamily = 0;
//                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    hv_SubFamily = hv_SubFamily.TupleBor(1);
//                }
//                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
//                {
//                    hv_Exception = "Wrong value of control parameter Slant";
//                    throw new HalconException(hv_Exception);
//                }
//                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    hv_SubFamily = hv_SubFamily.TupleBor(2);
//                }
//                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleNotEqual("false"))) != 0)
//                {
//                    hv_Exception = "Wrong value of control parameter Bold";
//                    throw new HalconException(hv_Exception);
//                }
//                if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))) != 0)
//                {
//                    hv_Fonts = new HTuple();
//                    hv_Fonts[0] = "Menlo-Regular";
//                    hv_Fonts[1] = "Menlo-Italic";
//                    hv_Fonts[2] = "Menlo-Bold";
//                    hv_Fonts[3] = "Menlo-BoldItalic";
//                }
//                else if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("Courier"))).TupleOr(
//                    new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
//                {
//                    hv_Fonts = new HTuple();
//                    hv_Fonts[0] = "CourierNewPSMT";
//                    hv_Fonts[1] = "CourierNewPS-ItalicMT";
//                    hv_Fonts[2] = "CourierNewPS-BoldMT";
//                    hv_Fonts[3] = "CourierNewPS-BoldItalicMT";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
//                {
//                    hv_Fonts = new HTuple();
//                    hv_Fonts[0] = "ArialMT";
//                    hv_Fonts[1] = "Arial-ItalicMT";
//                    hv_Fonts[2] = "Arial-BoldMT";
//                    hv_Fonts[3] = "Arial-BoldItalicMT";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
//                {
//                    hv_Fonts = new HTuple();
//                    hv_Fonts[0] = "TimesNewRomanPSMT";
//                    hv_Fonts[1] = "TimesNewRomanPS-ItalicMT";
//                    hv_Fonts[2] = "TimesNewRomanPS-BoldMT";
//                    hv_Fonts[3] = "TimesNewRomanPS-BoldItalicMT";
//                }
//                else
//                {
//                    //Attempt to figure out which of the fonts installed on the system
//                    //the user could have meant.
//                    HOperatorSet.QueryFont(hv_ExpDefaultWinHandle, out hv_SystemFonts);
//                    hv_Fonts = new HTuple();
//                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
//                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
//                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
//                    hv_Fonts = hv_Fonts.TupleConcat(hv_Font_COPY_INP_TMP);
//                    hv_Guess = new HTuple();
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP);
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Regular");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "MT");
//                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
//                    {
//                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
//                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
//                        {
//                            if (hv_Fonts == null)
//                                hv_Fonts = new HTuple();
//                            hv_Fonts[0] = hv_Guess.TupleSelect(hv_I);
//                            break;
//                        }
//                    }
//                    //Guess name of slanted font
//                    hv_Guess = new HTuple();
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Italic");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-ItalicMT");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Oblique");
//                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
//                    {
//                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
//                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
//                        {
//                            if (hv_Fonts == null)
//                                hv_Fonts = new HTuple();
//                            hv_Fonts[1] = hv_Guess.TupleSelect(hv_I);
//                            break;
//                        }
//                    }
//                    //Guess name of bold font
//                    hv_Guess = new HTuple();
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-Bold");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldMT");
//                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
//                    {
//                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
//                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
//                        {
//                            if (hv_Fonts == null)
//                                hv_Fonts = new HTuple();
//                            hv_Fonts[2] = hv_Guess.TupleSelect(hv_I);
//                            break;
//                        }
//                    }
//                    //Guess name of bold slanted font
//                    hv_Guess = new HTuple();
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalic");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldItalicMT");
//                    hv_Guess = hv_Guess.TupleConcat(hv_Font_COPY_INP_TMP + "-BoldOblique");
//                    for (hv_I = 0; (int)hv_I <= (int)((new HTuple(hv_Guess.TupleLength())) - 1); hv_I = (int)hv_I + 1)
//                    {
//                        HOperatorSet.TupleFind(hv_SystemFonts, hv_Guess.TupleSelect(hv_I), out hv_Index);
//                        if ((int)(new HTuple(hv_Index.TupleNotEqual(-1))) != 0)
//                        {
//                            if (hv_Fonts == null)
//                                hv_Fonts = new HTuple();
//                            hv_Fonts[3] = hv_Guess.TupleSelect(hv_I);
//                            break;
//                        }
//                    }
//                }
//                hv_Font_COPY_INP_TMP = hv_Fonts.TupleSelect(hv_SubFamily);
//                try
//                {
//                    HOperatorSet.SetFont(hv_ExpDefaultWinHandle, (hv_Font_COPY_INP_TMP + "-") + hv_Size_COPY_INP_TMP);
//                }
//                // catch (Exception) 
//                catch (HalconException HDevExpDefaultException1)
//                {
//                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                    //throw (Exception)
//                }
//            }
//            else
//            {
//                //Set font for UNIX systems
//                hv_Size_COPY_INP_TMP = hv_Size_COPY_INP_TMP * 1.25;
//                hv_AllowedFontSizes = new HTuple();
//                hv_AllowedFontSizes[0] = 11;
//                hv_AllowedFontSizes[1] = 14;
//                hv_AllowedFontSizes[2] = 17;
//                hv_AllowedFontSizes[3] = 20;
//                hv_AllowedFontSizes[4] = 25;
//                hv_AllowedFontSizes[5] = 34;
//                if ((int)(new HTuple(((hv_AllowedFontSizes.TupleFind(hv_Size_COPY_INP_TMP))).TupleEqual(
//                    -1))) != 0)
//                {
//                    hv_Distances = ((hv_AllowedFontSizes - hv_Size_COPY_INP_TMP)).TupleAbs();
//                    HOperatorSet.TupleSortIndex(hv_Distances, out hv_Indices);
//                    hv_Size_COPY_INP_TMP = hv_AllowedFontSizes.TupleSelect(hv_Indices.TupleSelect(
//                        0));
//                }
//                if ((int)((new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("mono"))).TupleOr(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual(
//                    "Courier")))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "courier";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("sans"))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "helvetica";
//                }
//                else if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("serif"))) != 0)
//                {
//                    hv_Font_COPY_INP_TMP = "times";
//                }
//                if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    hv_Bold_COPY_INP_TMP = "bold";
//                }
//                else if ((int)(new HTuple(hv_Bold_COPY_INP_TMP.TupleEqual("false"))) != 0)
//                {
//                    hv_Bold_COPY_INP_TMP = "medium";
//                }
//                else
//                {
//                    hv_Exception = "Wrong value of control parameter Bold";
//                    throw new HalconException(hv_Exception);
//                }
//                if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("true"))) != 0)
//                {
//                    if ((int)(new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("times"))) != 0)
//                    {
//                        hv_Slant_COPY_INP_TMP = "i";
//                    }
//                    else
//                    {
//                        hv_Slant_COPY_INP_TMP = "o";
//                    }
//                }
//                else if ((int)(new HTuple(hv_Slant_COPY_INP_TMP.TupleEqual("false"))) != 0)
//                {
//                    hv_Slant_COPY_INP_TMP = "r";
//                }
//                else
//                {
//                    hv_Exception = "Wrong value of control parameter Slant";
//                    throw new HalconException(hv_Exception);
//                }
//                try
//                {
//                    HOperatorSet.SetFont(hv_ExpDefaultWinHandle, ((((((("-adobe-" + hv_Font_COPY_INP_TMP) + "-") + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
//                }
//                // catch (Exception) 
//                catch (HalconException HDevExpDefaultException1)
//                {
//                    HDevExpDefaultException1.ToHTuple(out hv_Exception);
//                    if ((int)((new HTuple(((hv_OS.TupleSubstr(0, 4))).TupleEqual("Linux"))).TupleAnd(
//                        new HTuple(hv_Font_COPY_INP_TMP.TupleEqual("courier")))) != 0)
//                    {
//                        HOperatorSet.QueryFont(hv_ExpDefaultWinHandle, out hv_Fonts);
//                        hv_FontSelRegexp = (("^-[^-]*-[^-]*[Cc]ourier[^-]*-" + hv_Bold_COPY_INP_TMP) + "-") + hv_Slant_COPY_INP_TMP;
//                        hv_FontsCourier = ((hv_Fonts.TupleRegexpSelect(hv_FontSelRegexp))).TupleRegexpMatch(
//                            hv_FontSelRegexp);
//                        if ((int)(new HTuple((new HTuple(hv_FontsCourier.TupleLength())).TupleEqual(
//                            0))) != 0)
//                        {
//                            hv_Exception = "Wrong font name";
//                            //throw (Exception)
//                        }
//                        else
//                        {
//                            try
//                            {
//                                HOperatorSet.SetFont(hv_ExpDefaultWinHandle, (((hv_FontsCourier.TupleSelect(
//                                    0)) + "-normal-*-") + hv_Size_COPY_INP_TMP) + "-*-*-*-*-*-*-*");
//                            }
//                            // catch (Exception) 
//                            catch (HalconException HDevExpDefaultException2)
//                            {
//                                HDevExpDefaultException2.ToHTuple(out hv_Exception);
//                                //throw (Exception)
//                            }
//                        }
//                    }
//                    //throw (Exception)
//                }
//            }
//            // dev_set_preferences(...); only in hdevelop

//            return;
//        }




//    }
//}
