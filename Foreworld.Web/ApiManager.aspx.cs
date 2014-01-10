using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Foreworld.Rest;

public partial class ApiManager : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            //判断文件是否小于10Mb  
            if (FileUpload1.PostedFile.ContentLength < 10485760)
            {
                try
                {
                    //上传文件并指定上传目录的路径  
                    FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Cmds/")
                        + FileUpload1.FileName);
                    /*注意->这里为什么不是:FileUpload1.PostedFile.FileName 
                    * 而是:FileUpload1.FileName? 
                    * 前者是获得客户端完整限定(客户端完整路径)名称 
                    * 后者FileUpload1.FileName只获得文件名. 
                    */

                    //当然上传语句也可以这样写(貌似废话):  
                    //FileUpload1.SaveAs(@"D:\"+FileUpload1.FileName);  

                    lblMessage.Text = "上传成功!";
                    CmdManager.INSTANCE.Reload();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "出现异常,无法上传!" + ex;
                    //lblMessage.Text += ex.Message;  
                }

            }
            else
            {
                lblMessage.Text = "上传文件不能大于10MB!";
            }
        }
        else
        {
            lblMessage.Text = "尚未选择文件!";
        }
    }
}
