copy D:\GitHub\MyREST\Foreworld.Cmd.Sysmanage\obj\Debug\Foreworld.Cmd.Sysmanage.dll D:\GitHub\MyREST\Foreworld.Web\App_Data\Foreworld.Cmd.Sysmanage.dll /y
copy D:\GitHub\MyREST\Foreworld.Cmd.BookManage\obj\Debug\Foreworld.Cmd.BookManage.dll D:\GitHub\MyREST\Foreworld.Web\App_Data\Foreworld.Cmd.BookManage.dll /y
copy D:\GitHub\MyREST\Foreworld.Cmd.Build\obj\Debug\Foreworld.Cmd.Build.dll D:\GitHub\MyREST\Foreworld.Web\App_Data\Foreworld.Cmd.Build.dll /y
copy D:\GitHub\MyREST\Foreworld.Cmd.Privilege\obj\Debug\Foreworld.Cmd.Privilege.dll D:\GitHub\MyREST\Foreworld.Web\App_Data\Foreworld.Cmd.Privilege.dll /y
cd C:\Program Files\Common Files\microsoft shared\DevServer\9.0
start WebDev.WebServer /port:81 /path:D:\GitHub\MyREST\Foreworld.Web\
####start WebDev.WebServer /port:81 /path:D:\GitHub\MyREST\PrecompiledWeb\Foreworld.Web\
