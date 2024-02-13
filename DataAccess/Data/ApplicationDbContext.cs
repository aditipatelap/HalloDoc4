using System;
using System.Collections.Generic;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Adminregion> Adminregions { get; set; }

    public virtual DbSet<Aspnetrole> Aspnetroles { get; set; }

    public virtual DbSet<Aspnetuser> Aspnetusers { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Blockrequest> Blockrequests { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Casetag> Casetags { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Emaillog> Emaillogs { get; set; }

    public virtual DbSet<Healthprofessional> Healthprofessionals { get; set; }

    public virtual DbSet<Healthprofessionaltype> Healthprofessionaltypes { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Physician> Physicians { get; set; }

    public virtual DbSet<Physicianlocation> Physicianlocations { get; set; }

    public virtual DbSet<Physiciannotification> Physiciannotifications { get; set; }

    public virtual DbSet<Physicianregion> Physicianregions { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Requestbusiness> Requestbusinesses { get; set; }

    public virtual DbSet<Requestclient> Requestclients { get; set; }

    public virtual DbSet<Requestclosed> Requestcloseds { get; set; }

    public virtual DbSet<Requestconcierge> Requestconcierges { get; set; }

    public virtual DbSet<Requestnote> Requestnotes { get; set; }

    public virtual DbSet<Requeststatuslog> Requeststatuslogs { get; set; }

    public virtual DbSet<Requesttype> Requesttypes { get; set; }

    public virtual DbSet<Requestwisefile> Requestwisefiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Rolemenu> Rolemenus { get; set; }

    public virtual DbSet<Shift> Shifts { get; set; }

    public virtual DbSet<Shiftdetail> Shiftdetails { get; set; }

    public virtual DbSet<Shiftdetailregion> Shiftdetailregions { get; set; }

    public virtual DbSet<Smslog> Smslogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("User ID = postgres;Password=2211;Server=localhost;Port=5432;Database=Hallo_Doc;Integrated Security=true;Pooling=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Adminid).HasName("pk_admin");

            entity.Property(e => e.Adminid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.AdminAspnetusers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_admin");

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.AdminCreatedbyNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_admin2");

            entity.HasOne(d => d.Region).WithMany(p => p.Admins).HasConstraintName("fk_admin3");
        });

        modelBuilder.Entity<Adminregion>(entity =>
        {
            entity.HasKey(e => e.Adminregionid).HasName("pk_adminregion");

            entity.Property(e => e.Adminregionid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Admin).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_adminregion");

            entity.HasOne(d => d.Region).WithMany(p => p.Adminregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_adminregion1");
        });

        modelBuilder.Entity<Aspnetrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_aspnetrole");
        });

        modelBuilder.Entity<Aspnetuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_aspnetusers");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("pk_aspnetuserrole");

            entity.HasOne(d => d.User).WithMany(p => p.Aspnetuserroles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aspnetuserrole");
        });

        modelBuilder.Entity<Blockrequest>(entity =>
        {
            entity.HasKey(e => e.Blockrequestid).HasName("pk_blockrequests");

            entity.Property(e => e.Blockrequestid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.Businessid).HasName("pk_business");

            entity.Property(e => e.Businessid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.BusinessCreatedbyNavigations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_business1");

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.BusinessModifiedbyNavigations).HasConstraintName("fk_business2");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses).HasConstraintName("fk_business");
        });

        modelBuilder.Entity<Casetag>(entity =>
        {
            entity.Property(e => e.Casetagid)
                .ValueGeneratedOnAdd()
                .UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.Conciergeid).HasName("pk_concierge");

            entity.Property(e => e.Conciergeid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_concierge");
        });

        modelBuilder.Entity<Emaillog>(entity =>
        {
            entity.HasKey(e => e.Emaillogid).HasName("pk_emaillog");

            entity.Property(e => e.Emaillogid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Admin).WithMany(p => p.Emaillogs).HasConstraintName("fk_emaillog2");

            entity.HasOne(d => d.Physician).WithMany(p => p.Emaillogs).HasConstraintName("fk_emaillog3");

            entity.HasOne(d => d.Request).WithMany(p => p.Emaillogs).HasConstraintName("fk_emaillog1");
        });

        modelBuilder.Entity<Healthprofessional>(entity =>
        {
            entity.HasKey(e => e.Vendorid).HasName("pk_healthprofessionals");

            entity.Property(e => e.Vendorid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.ProfessionNavigation).WithMany(p => p.Healthprofessionals).HasConstraintName("fk_healthprofessional");
        });

        modelBuilder.Entity<Healthprofessionaltype>(entity =>
        {
            entity.HasKey(e => e.Healthprofessionalid).HasName("pk_healthprofessionaltype");

            entity.Property(e => e.Healthprofessionalid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Menuid).HasName("pk_menu");

            entity.Property(e => e.Menuid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_orderdetails");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Request).WithMany(p => p.Orderdetails).HasConstraintName("fk_orderdetails1");

            entity.HasOne(d => d.Vendor).WithMany(p => p.Orderdetails).HasConstraintName("fk_orderdetails");
        });

        modelBuilder.Entity<Physician>(entity =>
        {
            entity.HasKey(e => e.Physicianid).HasName("pk_physician");

            entity.Property(e => e.Physicianid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Physicians).HasConstraintName("fk_physician");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicians).HasConstraintName("fk_physician1");
        });

        modelBuilder.Entity<Physicianlocation>(entity =>
        {
            entity.HasKey(e => e.Locationid).HasName("pk_physicianlocation");

            entity.Property(e => e.Locationid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianlocations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianlocation");
        });

        modelBuilder.Entity<Physiciannotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_physiciannotification");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Pysician).WithMany(p => p.Physiciannotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physiciannotification");
        });

        modelBuilder.Entity<Physicianregion>(entity =>
        {
            entity.HasKey(e => e.Physicianregionid).HasName("pk_physicianregion");

            entity.Property(e => e.Physicianregionid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Physician).WithMany(p => p.Physicianregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianregion");

            entity.HasOne(d => d.Region).WithMany(p => p.Physicianregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_physicianregion1");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Regionid).HasName("pk_region");

            entity.Property(e => e.Regionid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("pk_request");

            entity.Property(e => e.Requestid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Physician).WithMany(p => p.Requests).HasConstraintName("fk_request2");

            entity.HasOne(d => d.Requesttype).WithMany(p => p.Requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_request");

            entity.HasOne(d => d.User).WithMany(p => p.Requests).HasConstraintName("fk_request1");
        });

        modelBuilder.Entity<Requestbusiness>(entity =>
        {
            entity.HasKey(e => e.Requestbusinessid).HasName("pk_requestbusiness");

            entity.Property(e => e.Requestbusinessid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Business).WithMany(p => p.Requestbusinesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestbusiness1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestbusinesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestbusiness");
        });

        modelBuilder.Entity<Requestclient>(entity =>
        {
            entity.HasKey(e => e.Requestclientid).HasName("pk_requestclient");

            entity.Property(e => e.Requestclientid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Region).WithMany(p => p.Requestclients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclient1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestclients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclient");
        });

        modelBuilder.Entity<Requestclosed>(entity =>
        {
            entity.HasKey(e => e.Requestclosedid).HasName("pk_requestclosed");

            entity.Property(e => e.Requestclosedid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Request).WithMany(p => p.Requestcloseds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclosed");

            entity.HasOne(d => d.Requeststatuslog).WithMany(p => p.Requestcloseds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestclosed1");
        });

        modelBuilder.Entity<Requestconcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_requestconcierge");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Concierge).WithMany(p => p.Requestconcierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestconcierge1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestconcierges)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestconcierge");
        });

        modelBuilder.Entity<Requestnote>(entity =>
        {
            entity.HasKey(e => e.Requestnotesid).HasName("pk_requestnotes");

            entity.Property(e => e.Requestnotesid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Request).WithMany(p => p.Requestnotes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestnotes");
        });

        modelBuilder.Entity<Requeststatuslog>(entity =>
        {
            entity.HasKey(e => e.Requeststatuslogid).HasName("pk_requeststatuslog");

            entity.Property(e => e.Requeststatuslogid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Admin).WithMany(p => p.Requeststatuslogs).HasConstraintName("fk_requeststatuslog2");

            entity.HasOne(d => d.Physician).WithMany(p => p.RequeststatuslogPhysicians).HasConstraintName("fk_requeststatuslog1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requeststatuslogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requeststatuslog");

            entity.HasOne(d => d.Transtophysician).WithMany(p => p.RequeststatuslogTranstophysicians).HasConstraintName("fk_requeststatuslog4");
        });

        modelBuilder.Entity<Requesttype>(entity =>
        {
            entity.HasKey(e => e.Requesttypeid).HasName("pk_requesttype");

            entity.Property(e => e.Requesttypeid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Requestwisefile>(entity =>
        {
            entity.HasKey(e => e.Requestwisefileid).HasName("pk_requestwisefile");

            entity.Property(e => e.Requestwisefileid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Admin).WithMany(p => p.Requestwisefiles).HasConstraintName("fk_requestwisefile2");

            entity.HasOne(d => d.Physician).WithMany(p => p.Requestwisefiles).HasConstraintName("fk_requestwisefile1");

            entity.HasOne(d => d.Request).WithMany(p => p.Requestwisefiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_requestwisefile");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("pk_role");

            entity.Property(e => e.Roleid).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Rolemenu>(entity =>
        {
            entity.HasKey(e => e.Rolemenuid).HasName("pk_rolemenu");

            entity.Property(e => e.Rolemenuid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Menu).WithMany(p => p.Rolemenus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rolemenu2");
        });

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.HasKey(e => e.Shiftid).HasName("pk_shift");

            entity.Property(e => e.Shiftid).UseIdentityAlwaysColumn();
            entity.Property(e => e.Weekdays).IsFixedLength();

            entity.HasOne(d => d.CreatedbyNavigation).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shift1");

            entity.HasOne(d => d.Physician).WithMany(p => p.Shifts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shift");
        });

        modelBuilder.Entity<Shiftdetail>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailid).HasName("pk_shiftdetail");

            entity.Property(e => e.Shiftdetailid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.ModifiedbyNavigation).WithMany(p => p.Shiftdetails).HasConstraintName("fk_shiftdetails1");

            entity.HasOne(d => d.Shift).WithMany(p => p.Shiftdetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetails");
        });

        modelBuilder.Entity<Shiftdetailregion>(entity =>
        {
            entity.HasKey(e => e.Shiftdetailregionid).HasName("pk_shiftdetailregion");

            entity.Property(e => e.Shiftdetailregionid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Region).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetailregion1");

            entity.HasOne(d => d.Shiftdetail).WithMany(p => p.Shiftdetailregions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_shiftdetailregion");
        });

        modelBuilder.Entity<Smslog>(entity =>
        {
            entity.HasKey(e => e.Smslogid).HasName("pk_smslog");

            entity.Property(e => e.Smslogid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Admin).WithMany(p => p.Smslogs).HasConstraintName("fk_smslog1");

            entity.HasOne(d => d.Physician).WithMany(p => p.Smslogs).HasConstraintName("fk_smslog3");

            entity.HasOne(d => d.Request).WithMany(p => p.Smslogs).HasConstraintName("fk_smslog2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("pk_user");

            entity.Property(e => e.Userid).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Aspnetuser).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_aspnetusers");

            entity.HasOne(d => d.Region).WithMany(p => p.Users).HasConstraintName("fk_users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
