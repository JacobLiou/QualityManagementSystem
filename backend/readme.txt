项目结构如下：

Furion.Extras.Admin.NET：基础框架层
QMS.Application.Issues：问题管理服务层（业务代码主要编写层）
QMS.Application.System：基础数据管理服务层（业务代码主要编写层）
QMS.Core：核心层（实体，仓储，其他核心代码）
QMS.Database.Migrations：EFCore 架构迁移文件层
QMS.EntityFramework.Core：EF Core 配置层
QMS.Web.Core：Web 核心层（存放 Web 公共代码，如 过滤器、中间件、Web Helpers 等）
QMS.Web.Entry：Web 入口层/启动层