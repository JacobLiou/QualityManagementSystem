﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QMS.EntityFramework.Core;

#nullable disable

namespace QMS.Database.Migrations.Migrations.IssuesDb
{
    [DbContext(typeof(IssuesDbContext))]
    [Migration("20220518072847_V1.0.0.45")]
    partial class V10045
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QMS.Core.Entity.Issue", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<string>("CCs")
                        .HasColumnType("longtext")
                        .HasComment("被抄送人");

                    b.Property<DateTime?>("CloseTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("关闭日期");

                    b.Property<int>("Consequence")
                        .HasColumnType("int")
                        .HasComment("问题性质");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("提出日期");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint")
                        .HasComment("提出人");

                    b.Property<long?>("CurrentAssignment")
                        .HasColumnType("bigint")
                        .HasComment("当前指派给");

                    b.Property<long?>("Discover")
                        .HasColumnType("bigint")
                        .HasComment("发现人");

                    b.Property<DateTime?>("DiscoverTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("发现日期");

                    b.Property<DateTime?>("DispatchTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("分发日期");

                    b.Property<long?>("Dispatcher")
                        .HasColumnType("bigint")
                        .HasComment("分发人");

                    b.Property<long?>("Executor")
                        .HasColumnType("bigint")
                        .HasComment("解决人");

                    b.Property<DateTime?>("ForecastSolveTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("预计完成日期");

                    b.Property<long?>("HangupId")
                        .HasColumnType("bigint")
                        .HasComment("挂起人");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("软删除");

                    b.Property<int>("IssueClassification")
                        .HasColumnType("int")
                        .HasComment("问题分类");

                    b.Property<int>("Module")
                        .HasColumnType("int")
                        .HasComment("问题模块");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasComment("产品编号");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint")
                        .HasComment("项目编号");

                    b.Property<DateTime?>("SolveTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("解决日期");

                    b.Property<int>("Source")
                        .HasColumnType("int")
                        .HasComment("问题来源");

                    b.Property<int>("Status")
                        .HasColumnType("int")
                        .HasComment("问题状态");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("问题简述");

                    b.Property<DateTime?>("ValidateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("验证日期");

                    b.Property<int>("ValidationStatus")
                        .HasColumnType("int")
                        .HasComment("回归验证状态");

                    b.Property<long?>("Verifier")
                        .HasColumnType("bigint")
                        .HasComment("验证人");

                    b.Property<string>("VerifierPlace")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("验证地点");

                    b.HasKey("Id");

                    b.ToTable("issue");

                    b.HasComment("问题记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueColumnDisplay", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("用户编号");

                    b.Property<string>("Columns")
                        .HasMaxLength(600)
                        .HasColumnType("varchar(600)")
                        .HasComment("列名集合");

                    b.HasKey("UserId");

                    b.ToTable("issue_column_display");

                    b.HasComment("问题列表显示列明记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueDetail", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<string>("Attachments")
                        .HasColumnType("longtext")
                        .HasComment("附件信息");

                    b.Property<string>("Batch")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("验证批次");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext")
                        .HasComment("备注");

                    b.Property<int?>("Count")
                        .HasColumnType("int")
                        .HasComment("验证数量");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasComment("问题详情");

                    b.Property<string>("ExtendAttribute")
                        .HasColumnType("longtext")
                        .HasComment("扩展属性");

                    b.Property<string>("HangupReason")
                        .HasColumnType("longtext")
                        .HasComment("挂起情况");

                    b.Property<string>("Measures")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("解决措施");

                    b.Property<string>("ReCheckResult")
                        .HasColumnType("longtext")
                        .HasComment("复核情况");

                    b.Property<string>("Reason")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("原因分析");

                    b.Property<string>("Result")
                        .HasColumnType("longtext")
                        .HasComment("验证情况");

                    b.Property<string>("SolveVersion")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("解决版本");

                    b.HasKey("Id");

                    b.ToTable("issue_detail");

                    b.HasComment("详细问题记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueExtendAttribute", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasComment("字段编号");

                    b.Property<string>("AttibuteName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasComment("字段名");

                    b.Property<string>("AttributeCode")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasComment("字段代码");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("创建时间");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint")
                        .HasComment("创建人");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("软删除");

                    b.Property<int>("Module")
                        .HasColumnType("int")
                        .HasComment("模块编号");

                    b.Property<int>("Sort")
                        .HasColumnType("int")
                        .HasComment("排序优先级");

                    b.Property<long>("UpdateId")
                        .HasColumnType("bigint")
                        .HasComment("更新人");

                    b.Property<DateTime>("UpdateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("提出日期");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)")
                        .HasComment("字段值类型");

                    b.HasKey("Id");

                    b.ToTable("issue_ext_attr");

                    b.HasComment("问题扩展属性");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueExtendAttributeValue", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("字段编号");

                    b.Property<long>("IssueNum")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<string>("AttibuteValue")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("字段值");

                    b.HasKey("Id", "IssueNum");

                    b.ToTable("issue_ext_attr_val");

                    b.HasComment("问题扩展属性值");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueOperation", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("Id主键");

                    b.Property<string>("Content")
                        .HasColumnType("longtext")
                        .HasComment("内容");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("软删除");

                    b.Property<long>("IssueId")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<DateTime>("OperationTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("时间");

                    b.Property<int>("OperationTypeId")
                        .HasMaxLength(200)
                        .HasColumnType("int")
                        .HasComment("操作类型");

                    b.Property<string>("OperatorName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .HasComment("操作人");

                    b.HasKey("Id");

                    b.ToTable("issue_operation");

                    b.HasComment("问题操作记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.IssueDetail", b =>
                {
                    b.HasOne("QMS.Core.Entity.Issue", "Issue")
                        .WithOne("SsuIssueDetail")
                        .HasForeignKey("QMS.Core.Entity.IssueDetail", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("QMS.Core.Entity.Issue", b =>
                {
                    b.Navigation("SsuIssueDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
