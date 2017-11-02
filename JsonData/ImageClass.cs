using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Slide_Code.JsonData
{
    /// <summary>
    /// 图片生成
    /// </summary>
    public class ImageClass
    {
        public static void SaveImage(string filename)
        {
            #region 获取拼图模型 像素点 需要优化
            List<Point> pl = new List<Point>();
            System.Drawing.Image bitmap1 = new System.Drawing.Bitmap(HttpContext.Current.Server.MapPath("/Drag/Template.png"));
            Bitmap bb = new Bitmap(bitmap1);
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    Color color = bb.GetPixel(i, j);
                    if (color.Name != "0")
                    {
                        pl.Add(new Point()
                        {
                            X = i,
                            Y = j
                        });
                    }
                }
            }
            #endregion

            Bitmap GGround = new System.Drawing.Bitmap(filename);//底图
            Bitmap GMap = new Bitmap(50, GGround.Height); //滑图
            Random r = new Random();
            int Y = r.Next(0, GGround.Height - 50);
            int X = r.Next(0, GGround.Width - 50);

            foreach (Point p in pl)
            {
                Color color = GGround.GetPixel(p.X + X, p.Y + Y);//根据模型获取像素点进行处理

                GMap.SetPixel(p.X, p.Y + Y, color);
                color = ChangeColor(color, -0.2f);
                GGround.SetPixel(p.X + X, p.Y + Y, color);
            }
            string gid = Guid.NewGuid().ToString();
            GGround.Save(HttpContext.Current.Server.MapPath("/Drag/Pic/" + gid + "_Ground.jpg"));
            GMap.Save(HttpContext.Current.Server.MapPath("/Drag/Pic/" + gid + "_Map.png"));

            File.AppendAllText(HttpContext.Current.Server.MapPath("/JsonData/PicData.txt"), Guid.NewGuid().ToString() + "#/Drag/Pic/" + gid + "_Ground.jpg" + "#/Drag/Pic/" + gid + "_Map.png" + "#" + X + "\r\n");
        }

        /// <summary>
        /// 加深：correctionFactor<0        
        /// 变亮：correctionFactor>0
        /// -1.0f <= correctionFactor <= 1.0f
        /// </summary>
        /// <param name="color"></param>
        /// <param name="correctionFactor"></param>
        /// <returns></returns>
        public static Color ChangeColor(Color color, float correctionFactor)
        {
            float red = (float)color.R;
            float green = (float)color.G;
            float blue = (float)color.B;
            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            if (red < 0) red = 0;
            if (red > 255) red = 255;
            if (green < 0) green = 0;
            if (green > 255) green = 255;
            if (blue < 0) blue = 0;
            if (blue > 255) blue = 255;

            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }

    }
}