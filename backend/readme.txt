项目结构如下：

Furion.Extras.Admin.NET：框架核心层
Admin.NET.Application：业务应用层（业务代码主要编写层）
Admin.NET.Core：核心层（实体，仓储，其他核心代码）
Admin.NET.Database.Migrations：EFCore 架构迁移文件层
Admin.NET.EntityFramework.Core：EF Core 配置层
Admin.NET.Web.Core：Web 核心层（存放 Web 公共代码，如 过滤器、中间件、Web Helpers 等）
Admin.NET.Web.Entry：Web 入口层/启动层