using Furion;
using Furion.DatabaseAccessor;
using Furion.Extras.Admin.NET;
using Furion.Extras.Admin.NET.Entity;
using Furion.Extras.Admin.NET.Service;
using Furion.FriendlyException;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using QMS.Core;
using System.Linq.Expressions;
using Yitter.IdGenerator;

namespace QMS.EntityFramework.Core
{
    [AppDbContext("IssuesConnection", DbProvider.MySql)]
    public class IssuesDbContext : AppDbContext<IssuesDbContext, IssuesDbContextLocator>, IMultiTenantOnTable, IModelBuilderFilter
    {
        //缓存服务
        private readonly ISysCacheService _sysCacheService;
        private readonly IConfiguration _configuration;

        public IssuesDbContext(ISysCacheService sysCacheService, IConfiguration configuration, DbContextOptions<IssuesDbContext> options) : base(options)
        {
            //缓存服务
            _sysCacheService = sysCacheService;

            // 启用实体数据更改监听
            EnabledEntityChangedListener = true;
            //配置文件
            _configuration = configuration;
            // 忽略空值更新
            InsertOrUpdateIgnoreNullValues = true;
        }

        /// <summary>
        /// 获取租户Id
        /// </summary>
        /// <returns></returns>
        public object GetTenantId()
        {
            if (App.User == null) return null;
            //{
            //    return _configuration["TenantId"].ToString();
            //}
            //这个Convert，嗯，有用
            return Convert.ToInt64(App.User.FindFirst(ClaimConst.TENANT_ID)?.Value);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (Database.ProviderName == DbProvider.Sqlite)
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in builder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        builder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
            //处理mysql时区问题 https://gitee.com/dotnetchina/Furion/issues/I3RSCO#note_5685893_link
            else if (Database.ProviderName == DbProvider.MySql || Database.ProviderName == DbProvider.MySqlOfficial)
            {
                var converter = new ValueConverter<DateTimeOffset, DateTime>(v => v.LocalDateTime, v => v);

                // 扫描程序集，获取数据库实体相关类型
                var types = App.EffectiveTypes.Where(t => (typeof(IPrivateEntity).IsAssignableFrom(t) || typeof(IPrivateModelBuilder).IsAssignableFrom(t))
                     && t.IsClass && !t.IsAbstract && !t.IsGenericType && !t.IsInterface && !t.IsDefined(typeof(ManualAttribute), true));

                if (types.Any())
                {
                    foreach (var item in types)
                    {
                        if (item.IsSubclassOf(typeof(DEntityBase<long, IssuesDbContextLocator>)) || item.IsSubclassOf(typeof(EntityBase<long, IssuesDbContextLocator>)))
                        {
                            foreach (var property in item.GetProperties())
                            {
                                if (property.PropertyType == typeof(DateTimeOffset?) || property.PropertyType == typeof(DateTimeOffset))
                                {
                                    builder.Entity(item).Property(property.Name).HasConversion(converter);
                                }
                            }
                        }
                    }
                }
            }

            base.OnModelCreating(builder);
        }

        /// <summary>
        /// 配置租户Id过滤器
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="dbContextLocator"></param>
        public void OnCreating(ModelBuilder modelBuilder, EntityTypeBuilder entityBuilder, DbContext dbContext, Type dbContextLocator)
        {
            // 配置租户Id以及假删除过滤器
            LambdaExpression expression = TenantIdAndFakeDeleteQueryFilterExpression(entityBuilder, dbContext);
            if (expression != null)
                entityBuilder.HasQueryFilter(expression);
            // 配置数据权限动态表达式
            LambdaExpression dataScopesExpression = DataScopesFilterExpression(entityBuilder, dbContext);
            if (dataScopesExpression != null)
                entityBuilder.HasQueryFilter(dataScopesExpression);
        }

        protected override void SavingChangesEvent(DbContextEventData eventData, InterceptionResult<int> result)
        {
            // 获取当前事件对应上下文
            var dbContext = eventData.Context;
            // 获取所有更改，删除，新增的实体，但排除审计实体（避免死循环）
            var entities = dbContext.ChangeTracker.Entries()
                  .Where(u => u.Entity.GetType() != typeof(SysLogAudit) && u.Entity.GetType() != typeof(SysLogOp) &&
                              u.Entity.GetType() != typeof(SysLogVis) && u.Entity.GetType() != typeof(SysLogEx) &&
                        (u.State == EntityState.Modified || u.State == EntityState.Deleted || u.State == EntityState.Added)).ToList();
            if (entities == null || entities.Count < 1) return;

            // 判断是否是演示环境
            var demoEnvFlag = App.GetService<ISysConfigService>().GetDemoEnvFlag().GetAwaiter().GetResult();
            if (demoEnvFlag)
            {
                var sysUser = entities.Find(u => u.Entity.GetType() == typeof(SysUser));
                if (sysUser == null || string.IsNullOrEmpty((sysUser.Entity as SysUser).LastLoginTime.ToString())) // 排除登录
                    throw Oops.Oh(ErrorCode.D1200);
            }

            // 当前操作者信息
            var userId = App.User?.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
            var userName = App.User?.FindFirst(ClaimConst.CLAINM_ACCOUNT)?.Value;

            foreach (var entity in entities)
            {
                if (entity.Entity.GetType().IsSubclassOf(typeof(DEntityTenant)))
                {
                    var obj = entity.Entity as DEntityTenant;
                    switch (entity.State)
                    {
                        // 自动设置租户Id
                        case EntityState.Added:
                            var tenantId = entity.Property(nameof(Entity.TenantId)).CurrentValue;
                            var currTenantId = GetTenantId();
                            if ((tenantId == null || (long)tenantId == 0) && currTenantId != null)
                                entity.Property(nameof(Entity.TenantId)).CurrentValue = Convert.ToInt64(currTenantId);

                            obj.Id = obj.Id == 0 ? YitIdHelper.NextId() : obj.Id;
                            obj.CreatedTime = DateTimeOffset.Now;
                            if (!string.IsNullOrEmpty(userId))
                            {
                                obj.CreatedUserId = long.Parse(userId);
                                obj.CreatedUserName = userName;
                            }
                            break;

                        case EntityState.Modified:
                            // 排除租户Id
                            entity.Property(nameof(DEntityTenant.TenantId)).IsModified = false;
                            // 排除创建人
                            entity.Property(nameof(DEntityTenant.CreatedUserId)).IsModified = false;
                            entity.Property(nameof(DEntityTenant.CreatedUserName)).IsModified = false;
                            // 排除创建日期
                            entity.Property(nameof(DEntityTenant.CreatedTime)).IsModified = false;

                            obj.UpdatedTime = DateTimeOffset.Now;
                            if (!string.IsNullOrEmpty(userId))
                            {
                                obj.UpdatedUserId = long.Parse(userId);
                                obj.UpdatedUserName = userName;
                            }
                            break;
                    }
                }
                else if (entity.Entity.GetType().IsSubclassOf(typeof(DEntityBase<long, IssuesDbContextLocator>)))
                {
                    var obj = entity.Entity as DEntityBase<long, IssuesDbContextLocator>;
                    if (entity.State == EntityState.Added)
                    {
                        obj.Id = obj.Id == 0 ? YitIdHelper.NextId() : obj.Id;
                        obj.CreatedTime = DateTimeOffset.Now;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            obj.CreatedUserId = long.Parse(userId);
                            obj.CreatedUserName = userName;
                        }
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        // 排除创建人
                        entity.Property(nameof(DEntityBase.CreatedUserId)).IsModified = false;
                        entity.Property(nameof(DEntityBase.CreatedUserName)).IsModified = false;
                        // 排除创建日期
                        entity.Property(nameof(DEntityBase.CreatedTime)).IsModified = false;

                        obj.UpdatedTime = DateTimeOffset.Now;
                        if (!string.IsNullOrEmpty(userId))
                        {
                            obj.UpdatedUserId = long.Parse(userId);
                            obj.UpdatedUserName = userName;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 构建租户Id以及假删除过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="isDeletedKey"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        protected static LambdaExpression TenantIdAndFakeDeleteQueryFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string onTableTenantId = null, string isDeletedKey = null, object filterValue = null)
        {
            onTableTenantId ??= "TenantId";
            isDeletedKey ??= "IsDeleted";
            IMutableEntityType metadata = entityBuilder.Metadata;
            if (metadata.FindProperty(onTableTenantId) == null && metadata.FindProperty(isDeletedKey) == null)
            {
                return null;
            }

            Expression finialExpression = Expression.Constant(true);
            ParameterExpression parameterExpression = Expression.Parameter(metadata.ClrType, "u");

            // 租户过滤器
            if (entityBuilder.Metadata.ClrType.BaseType.Name == typeof(DEntityTenant).Name)
            {
                if (metadata.FindProperty(onTableTenantId) != null)
                {
                    ConstantExpression constantExpression = Expression.Constant(onTableTenantId);
                    MethodCallExpression right = Expression.Call(Expression.Constant(dbContext), dbContext.GetType().GetMethod("GetTenantId"));

                    var dbProperty = Expression.Call(typeof(EF), "Property", new Type[1]
                    {
                        typeof(object)
                    }, parameterExpression, constantExpression);

                    var nullWhere = Expression.Constant(null);
                    Expression conditionExpr = Expression.Condition(Expression.Equal(nullWhere, right)
                        , Expression.AndAlso(finialExpression, Expression.NotEqual(dbProperty, nullWhere))
                        , Expression.AndAlso(finialExpression, Expression.Equal(dbProperty, right)));

                    finialExpression = conditionExpr.Reduce();
                }
            }

            // 假删除过滤器
            if (metadata.FindProperty(isDeletedKey) != null)
            {
                ConstantExpression constantExpression = Expression.Constant(isDeletedKey);
                ConstantExpression right = Expression.Constant(filterValue ?? false);
                var fakeDeleteQueryExpression = Expression.Equal(Expression.Call(typeof(EF), "Property", new Type[1]
                {
                    typeof(bool)
                }, parameterExpression, constantExpression), right);
                finialExpression = Expression.AndAlso(finialExpression, fakeDeleteQueryExpression);
            }

            return Expression.Lambda(finialExpression, parameterExpression);
        }

        #region 数据权限

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <returns></returns>
        public object GetUserId()
        {
            if (App.User == null) return null;
            return App.User.FindFirst(ClaimConst.CLAINM_USERID)?.Value;
        }

        /// <summary>
        /// 获取数据范围
        /// </summary>
        /// <returns></returns>
        public List<object> GetDataScopes()
        {
            var userId = this.GetUserId();
            if (userId == null)
            {
                return new List<object>();
            }

            var dataScopes = _sysCacheService.GetDataScope(Convert.ToInt64(userId)).Result; // 先从缓存里面读取
            if (dataScopes != null)
            {
                var dataScopesList = dataScopes.ConvertAll(i => (object)i);
                return dataScopesList;
            }
            return new List<object>();
        }

        /// <summary>
        /// 构建数据范围过滤器
        /// </summary>
        /// <param name="entityBuilder"></param>
        /// <param name="dbContext"></param>
        /// <param name="onTableCreatedUserId"></param>
        /// <param name="onTableCreatedUserOrgId"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        protected LambdaExpression DataScopesFilterExpression(EntityTypeBuilder entityBuilder, DbContext dbContext, string onTableCreatedUserId = null, string onTableCreatedUserOrgId = null, object filterValue = null)
        {
            onTableCreatedUserId ??= nameof(IDataPermissions.CreatedUserId);//用户id字段
            onTableCreatedUserOrgId ??= nameof(IDataPermissions.CreatedUserOrgId);//用户部门字段

            IMutableEntityType metadata = entityBuilder.Metadata;
            if (metadata.FindProperty(onTableCreatedUserId) == null || metadata.FindProperty(onTableCreatedUserOrgId) == null)
            {
                return null;
            }

            Expression finialExpression = Expression.Constant(true);
            ParameterExpression parameterExpression = Expression.Parameter(metadata.ClrType, "u");

            // 个人用户数据过滤器
            if (metadata.FindProperty(onTableCreatedUserId) != null)
            {
                ConstantExpression constantExpression = Expression.Constant(onTableCreatedUserId);
                MethodCallExpression right = Expression.Call(Expression.Constant(dbContext), dbContext.GetType().GetMethod("GetUserId"));
                finialExpression = Expression.AndAlso(finialExpression, Expression.Equal(Expression.Call(typeof(EF), "Property", new Type[1]
                {
                        typeof(object)
                }, parameterExpression, constantExpression), right));
            }

            //数据权限过滤器
            if (metadata.FindProperty(onTableCreatedUserOrgId) != null)
            {
                ConstantExpression constantExpression = Expression.Constant(onTableCreatedUserOrgId);

                MethodCallExpression dataScopesLeft = Expression.Call(Expression.Constant(dbContext), dbContext.GetType().GetMethod("GetDataScopes"));
                var firstOrDefaultCall = Expression.Call(typeof(EF), "Property", new Type[1]
                    {
                        typeof(object)
                    }, parameterExpression, constantExpression);

                var createdUserOrgIdQueryExpression = Expression.Call(dataScopesLeft, typeof(List<object>).GetMethod("Contains"), firstOrDefaultCall);

                finialExpression = Expression.Or(finialExpression, createdUserOrgIdQueryExpression);
            }

            return Expression.Lambda(finialExpression, parameterExpression);
        }

        #endregion 数据权限
    }
}