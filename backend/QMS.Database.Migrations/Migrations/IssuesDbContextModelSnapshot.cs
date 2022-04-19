﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QMS.EntityFramework.Core;

#nullable disable

namespace QMS.Database.Migrations.Migrations
{
    [DbContext(typeof(IssuesDbContext))]
    partial class IssuesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("QMS.Core.Entity.SsuIssues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("问题编号");

                    b.Property<DateTime>("CloseTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("关闭日期");

                    b.Property<long>("Consequence")
                        .HasColumnType("bigint")
                        .HasComment("问题性质");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("提出日期");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint")
                        .HasComment("提出人");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("问题描述");

                    b.Property<DateTime>("DisconverTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("发现日期");

                    b.Property<long>("Discover")
                        .HasColumnType("bigint")
                        .HasComment("发现人");

                    b.Property<DateTime>("DispatchTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("分发日期");

                    b.Property<long>("Dispatcher")
                        .HasColumnType("bigint")
                        .HasComment("分发人");

                    b.Property<long>("Executor")
                        .HasColumnType("bigint")
                        .HasComment("解决人");

                    b.Property<DateTime>("ForecastSolveTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("预计完成日期");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)")
                        .HasComment("软删除");

                    b.Property<long>("IssueClassification")
                        .HasColumnType("bigint")
                        .HasComment("问题分类");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint")
                        .HasComment("产品编号");

                    b.Property<long>("ProjectId")
                        .HasColumnType("bigint")
                        .HasComment("项目编号");

                    b.Property<DateTime>("SolveTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("解决日期");

                    b.Property<long>("Source")
                        .HasColumnType("bigint")
                        .HasComment("问题来源");

                    b.Property<long>("Stage")
                        .HasColumnType("bigint")
                        .HasComment("问题模块");

                    b.Property<long>("Status")
                        .HasColumnType("bigint")
                        .HasComment("问题状态");

                    b.Property<string>("Title")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("问题简述");

                    b.Property<DateTime>("ValidateTime")
                        .HasColumnType("datetime(6)")
                        .HasComment("验证日期");

                    b.Property<long>("Verifier")
                        .HasColumnType("bigint")
                        .HasComment("验证人");

                    b.Property<string>("VerifierPlace")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)")
                        .HasComment("验证地点");

                    b.HasKey("Id");

                    b.ToTable("ssu_issues");

                    b.HasComment("问题记录");
                });
#pragma warning restore 612, 618
        }
    }
}
