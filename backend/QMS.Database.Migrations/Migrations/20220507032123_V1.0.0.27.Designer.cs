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
    [Migration("20220507032123_V1.0.0.27")]
    partial class V10027
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QMS.Core.Entity.SsuIssue", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<long?>("CC")
                        .HasColumnType("bigint")
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

                    b.Property<long?>("Verifier")
                        .HasColumnType("bigint")
                        .HasComment("验证人");

                    b.Property<string>("VerifierPlace")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("验证地点");

                    b.HasKey("Id");

                    b.ToTable("ssu_issue");

                    b.HasComment("问题记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssueDetail", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint")
                        .HasComment("问题编号");

                    b.Property<string>("Attachments")
                        .HasColumnType("longtext");

                    b.Property<string>("Batch")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasComment("验证批次");

                    b.Property<string>("Comment")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)")
                        .HasComment("备注");

                    b.Property<int?>("Count")
                        .HasColumnType("int")
                        .HasComment("验证数量");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("问题详情");

                    b.Property<string>("ExtendAttribute")
                        .HasMaxLength(1500)
                        .HasColumnType("varchar(1500)")
                        .HasComment("扩展属性");

                    b.Property<string>("HangupReason")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("挂起情况");

                    b.Property<string>("Measures")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("解决措施");

                    b.Property<string>("Reason")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("原因分析");

                    b.Property<string>("Result")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("验证情况");

                    b.Property<string>("SolveVersion")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)")
                        .HasComment("解决版本");

                    b.HasKey("Id");

                    b.ToTable("ssu_issue_detail");

                    b.HasComment("详细问题记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssueExtendAttribute", b =>
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

                    b.ToTable("ssu_issue_extend_attribute");

                    b.HasComment("问题扩展属性");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssueExtendAttributeValue", b =>
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

                    b.ToTable("ssu_issue_extend_attribute_value");

                    b.HasComment("问题扩展属性值");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssueOperation", b =>
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

                    b.ToTable("ssu_issue_operation");

                    b.HasComment("问题操作记录");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssueDetail", b =>
                {
                    b.HasOne("QMS.Core.Entity.SsuIssue", "Issue")
                        .WithOne("SsuIssueDetail")
                        .HasForeignKey("QMS.Core.Entity.SsuIssueDetail", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("QMS.Core.Entity.SsuIssue", b =>
                {
                    b.Navigation("SsuIssueDetail");
                });
#pragma warning restore 612, 618
        }
    }
}
