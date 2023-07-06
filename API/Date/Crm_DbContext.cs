using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using crm_api.Models;

#nullable disable

namespace crm_api.Date
{
    public partial class Crm_DbContext : DbContext
    {
        public Crm_DbContext()
        {
        }

        public Crm_DbContext(DbContextOptions<Crm_DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ActionPlan> ActionPlans { get; set; }
        public virtual DbSet<ActionRecord> ActionRecords { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<CusContactInfo> CusContactInfos { get; set; }
        public virtual DbSet<CusContractInfo> CusContractInfos { get; set; }
        public virtual DbSet<CusExtensionInfo> CusExtensionInfos { get; set; }
        public virtual DbSet<CusPayRecord> CusPayRecords { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<SalesGroup> SalesGroups { get; set; }
        public virtual DbSet<Salesman> Salesmen { get; set; }
        public virtual DbSet<SignerInfo> SignerInfos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=127.0.0.01;userid=root;pwd=123456;port=3306;database=customer_relationship_management;sslmode=none", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<ActionPlan>(entity =>
            {
                entity.HasKey(e => e.PlanId)
                    .HasName("PRIMARY");

                entity.ToTable("action_plan");

                entity.HasIndex(e => e.SalesManId, "FK_写计划");

                entity.HasIndex(e => e.CustomerId, "FK_客户对应的计划");

                entity.Property(e => e.PlanId)
                    .HasMaxLength(32)
                    .HasColumnName("Plan_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.PlanEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Plan_EndDate");

                entity.Property(e => e.PlanInfo)
                    .HasColumnType("text")
                    .HasColumnName("Plan_Info");

                entity.Property(e => e.PlanStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Plan_StartDate");

                entity.Property(e => e.SalesManId).HasColumnName("SalesMan_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ActionPlans)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_客户对应的计划");

                entity.HasOne(d => d.SalesMan)
                    .WithMany(p => p.ActionPlans)
                    .HasForeignKey(d => d.SalesManId)
                    .HasConstraintName("FK_写计划");
            });

            modelBuilder.Entity<ActionRecord>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .HasName("PRIMARY");

                entity.ToTable("action_record");

                entity.HasIndex(e => e.CustomerId, "FK_被记录");

                entity.HasIndex(e => e.SalesManId, "FK_记录服务");

                entity.Property(e => e.RecordId)
                    .HasMaxLength(32)
                    .HasColumnName("Record_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.RecordDate)
                    .HasColumnType("date")
                    .HasColumnName("Record_Date");

                entity.Property(e => e.RecordInfo)
                    .HasColumnType("text")
                    .HasColumnName("Record_Info");

                entity.Property(e => e.RecordWay).HasColumnName("Record_Way");

                entity.Property(e => e.SalesManId).HasColumnName("SalesMan_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ActionRecords)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_被记录");

                entity.HasOne(d => d.SalesMan)
                    .WithMany(p => p.ActionRecords)
                    .HasForeignKey(d => d.SalesManId)
                    .HasConstraintName("FK_记录服务");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.AdminId)
                    .ValueGeneratedNever()
                    .HasColumnName("Admin_Id");

                entity.Property(e => e.AdminPwd)
                    .HasMaxLength(255)
                    .HasColumnName("Admin_Pwd");
            });

            modelBuilder.Entity<Complaint>(entity =>
            {
                entity.ToTable("complaint");

                entity.HasIndex(e => e.SalesManId, "FK_处理投诉");

                entity.HasIndex(e => e.CustomerId, "FK_投诉");

                entity.Property(e => e.ComplaintId)
                    .HasMaxLength(32)
                    .HasColumnName("Complaint_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.ComplaintExplain)
                    .HasColumnType("text")
                    .HasColumnName("Complaint_Explain");

                entity.Property(e => e.ComplaintHandleTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Complaint_HandleTime");

                entity.Property(e => e.ComplaintInfo)
                    .HasColumnType("text")
                    .HasColumnName("Complaint_Info");

                entity.Property(e => e.ComplaintOverTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Complaint_OverTime");

                entity.Property(e => e.ComplaintResult)
                    .HasColumnType("text")
                    .HasColumnName("Complaint_Result");

                entity.Property(e => e.ComplaintState).HasColumnName("Complaint_State");

                entity.Property(e => e.ComplaintStyle).HasColumnName("Complaint_Style");

                entity.Property(e => e.ComplaintTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Complaint_Time");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.SalesManId).HasColumnName("SalesMan_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_投诉");

                entity.HasOne(d => d.SalesMan)
                    .WithMany(p => p.Complaints)
                    .HasForeignKey(d => d.SalesManId)
                    .HasConstraintName("FK_处理投诉");
            });

            modelBuilder.Entity<CusContactInfo>(entity =>
            {
                entity.HasKey(e => e.ContactId)
                    .HasName("PRIMARY");

                entity.ToTable("cus_contact_info");

                entity.HasIndex(e => e.CustomerId, "FK_拥有");

                entity.Property(e => e.ContactId)
                    .ValueGeneratedNever()
                    .HasColumnName("Contact_Id");

                entity.Property(e => e.ContactAddress)
                    .HasColumnType("text")
                    .HasColumnName("Contact_Address");

                entity.Property(e => e.ContactEmail)
                    .HasMaxLength(50)
                    .HasColumnName("Contact_Email");

                entity.Property(e => e.ContactName)
                    .HasMaxLength(50)
                    .HasColumnName("Contact_Name");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(20)
                    .HasColumnName("Contact_Phone")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CusContactInfos)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_拥有");
            });

            modelBuilder.Entity<CusContractInfo>(entity =>
            {
                entity.HasKey(e => e.ContractId)
                    .HasName("PRIMARY");

                entity.ToTable("cus_contract_info");

                entity.HasIndex(e => e.SalesManId, "FK_出售合同");

                entity.HasIndex(e => e.SingerId, "FK_签订合约");

                entity.Property(e => e.ContractId)
                    .HasMaxLength(32)
                    .HasColumnName("Contract_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.ContractAmount).HasColumnName("Contract_Amount");

                entity.Property(e => e.ContractEndDate)
                    .HasColumnType("date")
                    .HasColumnName("Contract_EndDate");

                entity.Property(e => e.ContractInfo)
                    .HasColumnType("text")
                    .HasColumnName("Contract_Info");

                entity.Property(e => e.ContractName)
                    .HasColumnType("text")
                    .HasColumnName("Contract_Name");

                entity.Property(e => e.ContractStartDate)
                    .HasColumnType("date")
                    .HasColumnName("Contract_StartDate");

                entity.Property(e => e.ContractState).HasColumnName("Contract_State");

                entity.Property(e => e.SalesManId).HasColumnName("SalesMan_Id");

                entity.Property(e => e.SingerId).HasColumnName("Singer_Id");

                entity.HasOne(d => d.SalesMan)
                    .WithMany(p => p.CusContractInfos)
                    .HasForeignKey(d => d.SalesManId)
                    .HasConstraintName("FK_出售合同");

                entity.HasOne(d => d.Singer)
                    .WithMany(p => p.CusContractInfos)
                    .HasForeignKey(d => d.SingerId)
                    .HasConstraintName("FK_签订合约");
            });

            modelBuilder.Entity<CusExtensionInfo>(entity =>
            {
                entity.HasKey(e => e.ExtendId)
                    .HasName("PRIMARY");

                entity.ToTable("cus_extension_info");

                entity.HasIndex(e => e.CustomerId, "FK_拥有多个扩展信息");

                entity.Property(e => e.ExtendId)
                    .HasMaxLength(32)
                    .HasColumnName("Extend_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.ExtendInfo)
                    .HasColumnType("text")
                    .HasColumnName("Extend_Info");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.CusExtensionInfos)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_拥有多个扩展信息");
            });

            modelBuilder.Entity<CusPayRecord>(entity =>
            {
                entity.HasKey(e => e.CuspayId)
                    .HasName("PRIMARY");

                entity.ToTable("cus_pay_record");

                entity.HasIndex(e => e.ContractId, "FK_回款记录");

                entity.Property(e => e.CuspayId)
                    .HasMaxLength(32)
                    .HasColumnName("Cuspay_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.ContractId)
                    .HasMaxLength(32)
                    .HasColumnName("Contract_Id")
                    .IsFixedLength(true);

                entity.Property(e => e.CuspayAmount).HasColumnName("Cuspay_Amount");

                entity.Property(e => e.CuspayTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Cuspay_Time");

                entity.HasOne(d => d.Contract)
                    .WithMany(p => p.CusPayRecords)
                    .HasForeignKey(d => d.ContractId)
                    .HasConstraintName("FK_回款记录");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("customer");

                entity.HasIndex(e => e.SalesManId, "FK_服务");

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("Customer_Id");

                entity.Property(e => e.CustomerAddress)
                    .HasColumnType("text")
                    .HasColumnName("Customer_Address");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Name");

                entity.Property(e => e.CustomerOccupation)
                    .HasMaxLength(50)
                    .HasColumnName("Customer_Occupation");

                entity.Property(e => e.CustomerPhoto)
                    .HasColumnType("text")
                    .HasColumnName("Customer_Photo");

                entity.Property(e => e.CustomerPwd)
                    .HasMaxLength(255)
                    .HasColumnName("Customer_Pwd");

                entity.Property(e => e.SalesManId).HasColumnName("SalesMan_Id");

                entity.HasOne(d => d.SalesMan)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.SalesManId)
                    .HasConstraintName("FK_服务");
            });

            modelBuilder.Entity<SalesGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId)
                    .HasName("PRIMARY");

                entity.ToTable("sales_group");

                entity.Property(e => e.GroupId)
                    .ValueGeneratedNever()
                    .HasColumnName("Group_Id");

                entity.Property(e => e.GroupManager)
                    .HasMaxLength(20)
                    .HasColumnName("Group_Manager");

                entity.Property(e => e.GroupName)
                    .HasMaxLength(20)
                    .HasColumnName("Group_Name");
            });

            modelBuilder.Entity<Salesman>(entity =>
            {
                entity.ToTable("salesman");

                entity.HasIndex(e => e.GroupId, "FK_分组");

                entity.Property(e => e.SalesManId)
                    .ValueGeneratedNever()
                    .HasColumnName("SalesMan_Id");

                entity.Property(e => e.GroupId).HasColumnName("Group_Id");

                entity.Property(e => e.SalesManEmail)
                    .HasMaxLength(50)
                    .HasColumnName("SalesMan_Email");

                entity.Property(e => e.SalesManImg)
                    .HasColumnType("text")
                    .HasColumnName("SalesMan_Img");

                entity.Property(e => e.SalesManName)
                    .HasMaxLength(50)
                    .HasColumnName("SalesMan_Name");

                entity.Property(e => e.SalesManPhone)
                    .HasMaxLength(20)
                    .HasColumnName("SalesMan_Phone")
                    .IsFixedLength(true);

                entity.Property(e => e.SalesManPwd)
                    .HasMaxLength(255)
                    .HasColumnName("SalesMan_Pwd");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Salesmen)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_分组");
            });

            modelBuilder.Entity<SignerInfo>(entity =>
            {
                entity.HasKey(e => e.SingerId)
                    .HasName("PRIMARY");

                entity.ToTable("signer_info");

                entity.HasIndex(e => e.CustomerId, "FK_客户的签单人");

                entity.Property(e => e.SingerId)
                    .ValueGeneratedNever()
                    .HasColumnName("Singer_Id");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_Id");

                entity.Property(e => e.SignatureImg)
                    .HasColumnType("text")
                    .HasColumnName("Signature_Img");

                entity.Property(e => e.SignerEmail)
                    .HasMaxLength(50)
                    .HasColumnName("Signer_Email");

                entity.Property(e => e.SignerImg)
                    .HasColumnType("text")
                    .HasColumnName("Signer_Img");

                entity.Property(e => e.SignerName)
                    .HasMaxLength(50)
                    .HasColumnName("Signer_Name");

                entity.Property(e => e.SignerPhone)
                    .HasMaxLength(50)
                    .HasColumnName("Signer_Phone");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SignerInfos)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_客户的签单人");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
