<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ApiManager.aspx.cs" Inherits="ApiManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-type" content="text/html;charset=utf-8" />
    <title>API管理器</title>

    <script type="text/javascript">
    function checkType(){  
    
  //得到上传文件的值  
  var fileName=document.getElementById("FileUpload1").value;  
    
  //返回String对象中子字符串最后出现的位置.  
  var seat=fileName.lastIndexOf(".");  
   
  //返回位于String对象中指定位置的子字符串并转换为小写.  
  var extension=fileName.substring(seat).toLowerCase();  
   
  //判断允许上传的文件格式  
  //if(extension!=".jpg"&&extension!=".jpeg"&&extension!=".gif"&&extension!=".png"&&extension!=".bmp"){  
  //alert("不支持"+extension+"文件的上传!");  
  //return false;  
  //}else{  
  //return true;  
  //}  
    
  var allowed=[".dll"];  
  for(var i=0;i<allowed.length;i++){  
      if(!(allowed[i]!=extension)){  
          return true;  
      }  
  }  
  alert("不支持"+extension+"格式");  
  return false;  
} 
    </script>

</head>
<body enableviewstate="false">
    <form id="form1" runat="server">
    <div>
        <asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="文件上传" OnClientClick="return checkType()" /><asp:Label
            ID="lblMessage" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
