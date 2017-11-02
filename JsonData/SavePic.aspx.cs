using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Slide_Code.JsonData
{
    /// <summary>
    /// 生成图片
    /// </summary>
    public partial class SavePic : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ImageClass.SaveImage(HttpContext.Current.Server.MapPath("/test.jpg"));
        }
    }
}