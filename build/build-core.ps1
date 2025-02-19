# 定义服务器地址
$remoteIp = "172.16.16.33"

# supervisor 服务名称
$supervisorServername = "DotnetQMS"

# 定义路径
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$coreFolder = Join-Path $buildFolder "../backend"
$outputFolder = Join-Path $buildFolder "../outputs"

## 清空本地历史
Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## 发布后端

### 还原&打包
Set-Location $coreFolder
dotnet restore
dotnet publish --no-restore --output (Join-Path $outputFolder "dotnetqms_core") --configuration Release 

### 推送到服务器
Set-Location $outputFolder
ssh root@$remoteIp "rm -rf /wwwroot/dotnetqms_core; exit"
scp -r (Join-Path $outputFolder "dotnetqms_core") root@${remoteIp}:/wwwroot

### dotnet 命令运行
# ssh root@$remoteIp "cd /wwwroot/smart_prison_core; dotnet QMS.Web.Entry.dll --urls http://*:5566; exit"

### 如果是用 supervisor 守护进程的需要使用
ssh root@$remoteIp "sudo supervisorctl restart $supervisorServername; exit"