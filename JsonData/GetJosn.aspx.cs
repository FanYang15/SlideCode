using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slide_Code.JsonData
{
    /// <summary>
    /// json 接口 
    /// </summary>
    public partial class GetJosn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request["type"].ToString();

            if (type == "get")
            {
                List<ImageModel> ml = new List<ImageModel>();
                StreamReader sR = File.OpenText(HttpContext.Current.Server.MapPath("/JsonData/PicData.txt"));
                string nextLine;
                while ((nextLine = sR.ReadLine()) != null)
                {
                    if (nextLine != "")
                        ml.Add(new ImageModel()
                        {
                            ID = nextLine.Split('#')[0],
                            Ground = nextLine.Split('#')[1],
                            Map = nextLine.Split('#')[2],
                            X = nextLine.Split('#')[3],
                        });
                }
                sR.Close();

                ml = ml.OrderBy(p => Guid.NewGuid()).ToList();

                Session["GeelyDragID"] = ml[0].ID;
                Session["GeelyDragX"] = ml[0].X;

                JavaScriptSerializer ser = new JavaScriptSerializer();
                String jsonStr = ser.Serialize(new
                {
                    ground = ml[0].Ground,
                    map = ml[0].Map,
                    dragid = ml[0].ID,
                });

                Response.Clear();
                Response.Write(jsonStr);
                Response.End();
            }
            else
            {
                bool Yes = false;
                string GeelyDragID = Request["GeelyDragID"].ToString();
                int GeelyDragX = int.Parse(Request["GeelyDragX"].ToString());
                if (GeelyDragID == Session["GeelyDragID"].ToString())
                {
                    int x = int.Parse(Session["GeelyDragX"].ToString());
                    if ((x - 10) <= GeelyDragX && GeelyDragX <= (x + 10))
                        Yes = true;
                    else
                        Yes = false;
                }
                JavaScriptSerializer ser = new JavaScriptSerializer();
                String jsonStr = ser.Serialize(new
                {
                    IsYes = Yes
                });
                Response.Clear();
                Response.Write(jsonStr);
                Response.End();
            }

        }
    }
}