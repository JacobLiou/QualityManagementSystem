# 定义服务器地址
$remoteIp = "172.16.16.33"

# 定义路径
$buildFolder = (Get-Item -Path "./" -Verbose).FullName
$vueFolder = Join-Path $buildFolder "../frontend"
$outputFolder = Join-Path $buildFolder "../outputs"

## 清空本地历史
Remove-Item $outputFolder -Force -Recurse -ErrorAction Ignore
New-Item -Path $outputFolder -ItemType Directory

## 发布前端

### 还原&打包
Set-Location $vueFolder
npm run build
Copy-Item (Join-Path $vueFolder "dist") (Join-Path $outputFolder "smart_prison_vue/") -Recurse

### 推送到服务器
Set-Location $outputFolder

ssh root@$remoteIp "rm -rf /wwwroot/smart_prison_vue; exit"
scp -r (Join-Path $outputFolder "smart_prison_vue") root@${remoteIp}:/wwwroot