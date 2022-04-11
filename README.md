<div align="center"><h1 align="center">Admin.NET</a></h1></div>
<div align="center"><h3 align="center">前后端分离架构，开箱即用，紧随前沿技术</h3></div>

<div align="center">

[![star](https://gitee.com/zuohuaijun/Admin.NET/badge/star.svg?theme=dark)](https://gitee.com/zuohuaijun/Admin.NET/stargazers)
[![fork](https://gitee.com/zuohuaijun/Admin.NET/badge/fork.svg?theme=dark)](https://gitee.com/zuohuaijun/Admin.NET/members)
[![GitHub license](https://img.shields.io/badge/license-Apache2-yellow)](https://gitee.com/dotnetchina/Furion/blob/master/LICENSE)

![今日诗词](https://v2.jinrishici.com/one.svg?font-size=20&spacing=2&color=Chocolate)
</div>

### 🍟 概述
* 基于.NET 6实现的通用管理平台（.NET 5版本，请[点击这里](https://gitee.com/zuohuaijun/Admin.NET/tree/net5)）。整合最新技术，模块插件式开发，前后端分离，开箱即用。
* 后台基于Furion框架，vue2前端基于小诺框架。
* 集成EF Core、多租户、缓存、数据校验、鉴权、事件总线、动态API、通讯、远程请求、任务调度、gRPC等众多黑科技。
* 核心模块包括：用户、角色、职位、组织机构、菜单、字典、日志、多应用管理、文件管理、定时任务等功能。
* 代码简洁、易扩展，让开发更简单、更通用、更流行！
* QQ交流群：[87333204](https://jq.qq.com/?_wv=1027&k=1t8iqf0G)
```
如果对您有帮助，点击右上角⭐Star⭐关注 ，感谢支持开源！
```
[![Stargazers over time](https://whnb.wang/stars/zuohuaijun/Admin.NET)](https://whnb.wang)

### 🍎 效果图

<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/1.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/2.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/3.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/4.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/5.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/6.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/7.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/8.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/9.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/10.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/11.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/12.png"/></td>
    </tr>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/13.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/14.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/15.png"/></td>
    </tr>
</table>

### 🐱‍🚀 模块/插件化开发

* 按照 Furion 框架脚手架或者本框架脚手架初始化工程。
* 创建自己业务的 Dll 插件库工程，引用 Furion.Extras.Admin.NET 包，编写自己的业务代码包括实体、服务等。
* 在 XXX.Web.Entry 层里面的 appsettings.json 配置此插件 Dll 的路径。[配置文档说明](https://dotnetchina.gitee.io/furion/docs/module-dev)
* 此时框架和自己业务实体可以同时做数据迁移，耦合度最低。也可以自行将数据库分系统库、业务库等。
* 将自己业务前端代码包括view和api文件复制到前端工程相应目录即可。

`仓库内 plugin 文件内为本框架模块/插件开发事例，供参考。`

### 🎭 插件市场

`欢迎大家勇于参与开源，贡献自己的应用插件，你我都可以做到，.NET正在迅速崛起，我们都是历史的见证人💪`

【核酸采集系统】

<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/covid19-1.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/covid19-2.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/covid19-3.png"/></td>
    </tr>
</table>

### 🎁 前后端一体化

将后台提供的Swagger接口直接生成对应前端的API文件，前端再也不需要手撸一个个的对应后后的API定义了。后台接口更新后，只需要重新生成一遍覆盖即可。

<table>
    <tr>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/bf-01.png"/></td>
        <td><img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/bf-02.png"/></td>
    </tr>
</table>

### 👑 多租户简介

框架目前采用基于共享数据库TenantId的方式实现，后期可无缝迁移转换到基于多库或者Schema模式。

* 平台超管对租户进行增删改查操作，对各租户进行权限（菜单）的分配，租户管理员密码默认123456
* 租户管理员根据平台分配的权限再对本租户内用户进一步权限划分
* 针对新开发的业务功能，平台超管可以针对性分配给各租户（比如某租户购买后才有此功能菜单等） 

### 🥞 更新日志

更新日志 [点击查看](https://gitee.com/zuohuaijun/Admin.NET/commits/master)

### 🍿 在线体验

- 开发者租户：用户名：superAdmin，密码：123456
- 公司1租户： 公司1租户管理员（用户名：zuohuaijun@163.com 密码：123456），公司1租户普通用户（用户名：dilon@163.com 密码：123456）           

- 地址1：[http://1.117.110.74:8080/](http://1.117.110.74:8080/) PS: 1m带宽小水管，首次加载20左右~
- 地址2： [http://xwxtest01.wogof.com:9001/](http://xwxtest01.wogof.com:9001/)
#### 演示用户：
- 超级管理员:superAdmin
- 系统管理员:admin
- 普通用户:zuohuaijun
- 以上默认密码：123456
#### 租户演示用户：
- 系统管理员:zuohuaijun@163.com
- 普通用户:dilon@163.com
- 以上默认密码：123456

### 🏀 分层说明
```
├─Admin.NET.Application             ->业务应用层，在此写您具体业务代码
├─Admin.NET.Core                    ->框架核心层，实体类
├─Admin.NET.Database.Migrations     ->架构维护层，主要存放迁移中间文件
├─Admin.NET.EntityFramework.Core    ->EF Core配置层，主要配置数据库及相关
├─Admin.NET.Web.Core                ->Web核心层，主要是服务注册及鉴权
├─Admin.NET.Web.Entry               ->Web入口层/启动层，可任意更换
├─Furion.Extras.Admin.NET           ->封装的框架核心层
```

### 🍖 详细功能

 1. 主控面板、控制台页面，可进行工作台，分析页，统计等功能的展示。
 2. 用户管理、对企业用户和系统管理员用户的维护，可绑定用户职务，机构，角色，数据权限等。
 3. 应用管理、通过应用来控制不同维度的菜单展示。
 4. 机构管理、公司组织架构维护，支持多层级结构的树形结构。
 5. 职位管理、用户职务管理，职务可作为用户的一个标签，职务目前没有和权限等其他功能挂钩。
 6. 菜单管理、菜单目录，菜单，和按钮的维护是权限控制的基本单位。
 7. 角色管理、角色绑定菜单后，可限制相关角色的人员登录系统的功能范围。角色也可以绑定数据授权范围。
 8. 字典管理、系统内各种枚举类型的维护。
 9. 访问日志、用户的登录和退出日志的查看和管理。
10. 操作日志、用户的操作业务的日志的查看和管理。
11. 服务监控、服务器的运行状态，CPU、内存、网络等信息数据的查看。
12. 在线用户、当前系统在线用户的查看。
13. 公告管理、系统的公告的管理。
14. 文件管理、文件的上传下载查看等操作，文件可使用本地存储，阿里云oss，腾讯cos接入，支持拓展。
15. 定时任务、定时任务的维护，通过cron表达式控制任务的执行频率。
16. 系统配置、系统运行的参数的维护，参数的配置与系统运行机制息息相关。
17. 邮件发送、发送邮件功能。
18. 短信发送、短信发送功能，可使用阿里云sms，腾讯云sms，支持拓展。

### 📖 帮助文档

* 关注公众号，进入公众号回复【手摸手】获取文档，100%成功进入调试
* 公众号中点外卖获取的收益用于反馈给提交PR的朋友
* 第一次反馈：2022年1月15日，给前5名PR的朋友送出新年小礼品（不足部分[@JerryFox](https://gitee.com/JerryFox)提供资金支持）
* <img src="https://gitee.com/zuohuaijun/Admin.NET/raw/master/doc/img/shoumoshou_qrcode.png" width="300px" />
👉后台文档：

* Furion后台框架文档 [https://dotnetchina.gitee.io/furion/docs/source](https://dotnetchina.gitee.io/furion/docs/source)

👉前端文档：
* 小诺前端业务文档 [https://doc.xiaonuo.vip/snowy_vue/bizs/](https://doc.xiaonuo.vip/snowy_vue/bizs/)

1. Ant Design Pro of Vue 使用文档 [https://pro.antdv.com/docs/getting-started](https://pro.antdv.com/docs/getting-started)
2. Ant Design of Vue 组件文档 [https://www.antdv.com/docs/vue/getting-started-cn/](https://www.antdv.com/docs/vue/getting-started-cn/)
3. Vue 开发文档 [https://cn.vuejs.org/v2/guide/](https://cn.vuejs.org/v2/guide/)

👉快捷部署到 linux 文档：

- [Admin.NET 快捷部署到 linux 方案 | Wynnyo Blog](http://wynnyo.com/archives/publish-linux)
- [本地 md文件](./doc/快捷部署到linux方案.md)

👉代码生成器使用教程：

- [本地 md文件](./doc/代码生成器使用.md)

👉fork项目后该这样做后续开发：

- [本地 md文件](./doc/fork项目后该这样做后续开发.md)

👉关于signalr使用：

-  [wynnyo/vue-signalr: Signalr client for vue js (github.com)](https://github.com/wynnyo/vue-signalr)

😎通读以上文档，您就可以玩转本项目了（其实您已经是高手了）。项目使用上的问题，文档中基本都可以找到答案。

### 🚗 前端优化（可选）

经过 [12df9c7](https://gitee.com/zuohuaijun/Admin.NET/commit/12df9c7980007afb3bcadfe10777c3b36c0ca17d)的优化之后，打包从200多个文件，到现在的十多个文件。
但是js的chunk-vendors文件大小还是高达5.8M。各位小伙伴可以通过以下方式来继续深度优化。
* 安装`webpack-bundle-analyzer`来分析打包文件方法(此步骤可忽略)：

``` 
//1.安装依赖
npm install --save-dev webpack-bundle-analyzer
//2.配置
vue.config.js:
  chainWebpack: (config) => {
    config
    .plugin('webpack-bundle-analyzer')
    .use(require('webpack-bundle-analyzer').BundleAnalyzerPlugin)
//3.启动 or 打包
```

* 移除表单设计器。主要移除`k-form-design`的依赖，此优化节省主js(chunk.vendors)体积`2.3M`,节省gizp体积`700kb`

```
1.main.js注释k-form-design相关
2.注释src/views/system/formDesign/index..vue 所有代码
```

* 移除`antv`，以及`viser-vue`(引入了antv/g2)。此优化节省了主js(chunk.vendors)体积`1.3M`,节省gizp体积`400kb`

```
1.全局搜索antv，删除components中使用antv的组件(MiniArea、MiniBar、TagCloud)
2.注释src\components\index.js中相关组件的引用
3.移除src\core\lazy_use.js的viser-vue相关引用和use
4.移除src\core\use.js的viser-vue相关引用和use
5.移除package.json的antv和viser-vue。
6.重新 npm i
```

目前，仅仅移除掉表单设计器和antv，主js(chunk.vendors)体积为**2.2M**，gzip体积为**680kb**。

### 🥇版本号风格

GNU 风格版本号
```
主版本号 . 子版本号 [. 修正版本号[ build- 编译版本号 ]]
英文对照 : Major_Version_Number.Minor_Version_Number[.Revision_Number[.Build_Number]]
示例：1.2.0 build-1234
```
GNU 风格的版本号管理策略
```
1.当项目在进行了局部修改或 bug 修正时，主版本号和子版本号都不变，修正版本号加 1;
2.当项目在原有的基础上增加了部分功能时，主版本号不变，子版本号加 1，修正版本号复位为 0，因而可以被忽略掉 ;
3.当项目在进行了重大修改或局部修正累积较多，而导致项目整体发生全局变化时，主版本号加 1;
4.另外，编译版本号一般是编译器在编译过程中自动生成的，我们只定义其格式，并不进行人为控制 .
```

### ⚡ 近期计划

- [x] 集成多租户功能
- [x] 集成代码生成器
- [x] 集成Excel导入导出
- [x] 在线用户及黑名单
- [x] 集成对象存储(Object Storage Service,简称OSS)
- [x] 上传下载
- [ ] 邮件发送
- [ ] 短信发送
- [ ] 集成微信开发
- [ ] 实现电商应用插件
- [ ] 集成工作流

### 😎 衍生版本

【Admin.NET】隆重推出SqlSugar版本

- 👉 SqlSugar版本：[https://gitee.com/zhengguojing/admin-net-sqlsugar](https://gitee.com/zhengguojing/admin-net-sqlsugar)

`如果集成其他ORM，请参照各自操作使用说明。系统默认EFCore不会处理其他ORM实体等，请自行处理。`

### 🥦 补充说明

* 基于.NET 6平台 Furion 开发框架与小诺Antd Vue版本相结合，实时跟随基架升级而升级！
* 持续集百家所长，完善与丰富本框架基础设施，为.NET生态增加一种选择！
* 后期会推出基于此框架的相关应用场景案例，提供给大家使用！

### 🍻 贡献代码

`Admin.NET` 遵循 `Apache-2.0` 开源协议，欢迎大家提交 `PR` 或 `Issue`。

感谢每一位贡献代码的朋友。**感谢 [TLog 作者](https://gitee.com/bryan31) 提供的贡献者实时头像。**

[![Giteye chart](https://chart.giteye.net/gitee/zuohuaijun/Admin.NET/JRFF5WLM.png)](https://giteye.net/chart/JRFF5WLM)


### 💐 特别鸣谢
- 👉 Furion：  [https://dotnetchina.gitee.io/furion](https://dotnetchina.gitee.io/furion)
- 👉 xiaonuo：[https://gitee.com/xiaonuobase/snowy](https://gitee.com/xiaonuobase/snowy)
- 👉 Vben-Admin：[https://vvbin.cn/doc-next/](https://vvbin.cn/doc-next/)
- 👉 k-form-design：[https://gitee.com/kcz66/k-form-design](https://gitee.com/kcz66/k-form-design)
- 👉 MiniExcel：[https://gitee.com/dotnetchina/MiniExcel](https://gitee.com/dotnetchina/MiniExcel)
- 👉 SqlSugar：[https://gitee.com/dotnetchina/SqlSugar](https://gitee.com/dotnetchina/SqlSugar)
- 👉 IdGenerator：[https://github.com/yitter/idgenerator](https://github.com/yitter/idgenerator)
- 👉 ua-parser：[https://github.com/ua-parser/uap-csharp/](https://github.com/ua-parser/uap-csharp/)
- 👉 Zack.EFCore.Batch：[https://github.com/yangzhongke/Zack.EFCore.Batch](https://github.com/yangzhongke/Zack.EFCore.Batch)
- 👉 OnceMi.AspNetCore.OSS：[https://github.com/oncemi/OnceMi.AspNetCore.OSS](https://github.com/oncemi/OnceMi.AspNetCore.OSS)

如果对您有帮助，您可以点右上角 💘Star💘支持一下，这样我们才有持续下去的动力，谢谢！！！