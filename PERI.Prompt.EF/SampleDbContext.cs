using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace PERI.Prompt.EF
{
    public partial class SampleDbContext : DbContext
    {
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogCategory> BlogCategory { get; set; }
        public virtual DbSet<BlogPhoto> BlogPhoto { get; set; }
        public virtual DbSet<BlogSortOrder> BlogSortOrder { get; set; }
        public virtual DbSet<BlogTag> BlogTag { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ChildMenuItem> ChildMenuItem { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventPhoto> EventPhoto { get; set; }
        public virtual DbSet<Gallery> Gallery { get; set; }
        public virtual DbSet<GalleryPhoto> GalleryPhoto { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<PagePhoto> PagePhoto { get; set; }
        public virtual DbSet<Photo> Photo { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<SectionItem> SectionItem { get; set; }
        public virtual DbSet<SectionItemPhoto> SectionItemPhoto { get; set; }
        public virtual DbSet<SectionItemProperty> SectionItemProperty { get; set; }
        public virtual DbSet<SectionProperty> SectionProperty { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<Template> Template { get; set; }
        public virtual DbSet<TemplateSetting> TemplateSetting { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Core.Setting.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.Property(e => e.Body)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.DatePublished).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.CategoryId });

                entity.HasIndex(e => new { e.BlogId, e.CategoryId })
                    .HasName("UQ_BlogCategory_BlogId_CategoryId")
                    .IsUnique();

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogCategory)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogCategory_Blog");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BlogCategory)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_BlogCategory_Category");
            });

            modelBuilder.Entity<BlogPhoto>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.PhotoId });

                entity.HasIndex(e => new { e.BlogId, e.PhotoId })
                    .HasName("UQ_BlogPhoto_BlogId_PhotoId")
                    .IsUnique();

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogPhoto)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogPhoto_Blog");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.BlogPhoto)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_BlogPhoto_Photo");
            });

            modelBuilder.Entity<BlogSortOrder>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<BlogTag>(entity =>
            {
                entity.HasKey(e => new { e.BlogId, e.TagId });

                entity.HasIndex(e => new { e.BlogId, e.TagId })
                    .HasName("UQ_BlogTag_BlogId_TagId")
                    .IsUnique();

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogTag)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogTag_Blog");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.BlogTag)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK_BlogTag_Tag");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Category_Name")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.BlogSortOrder)
                    .WithMany(p => p.Category)
                    .HasForeignKey(d => d.BlogSortOrderId)
                    .HasConstraintName("FK_Category_BlogSortOrder");
            });

            modelBuilder.Entity<ChildMenuItem>(entity =>
            {
                entity.HasIndex(e => new { e.MenuItemId, e.Label })
                    .HasName("UQ_ChildMenuItem_MenuItemId_Label")
                    .IsUnique();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.ChildMenuItem)
                    .HasForeignKey(d => d.MenuItemId)
                    .HasConstraintName("FK_ChildMenuItem_MenuItem");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Location)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Time).HasColumnType("datetime");
            });

            modelBuilder.Entity<EventPhoto>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.PhotoId });

                entity.HasIndex(e => new { e.EventId, e.PhotoId })
                    .HasName("UQ_EventPhoto_EventId_PhotoId")
                    .IsUnique();

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.EventPhoto)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_EventPhoto_Event");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.EventPhoto)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_EventPhoto_Photo");
            });

            modelBuilder.Entity<Gallery>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GalleryPhoto>(entity =>
            {
                entity.HasKey(e => new { e.GalleryId, e.PhotoId });

                entity.HasIndex(e => new { e.GalleryId, e.PhotoId })
                    .HasName("UQ_GalleryPhoto_GalleryId_PhotoId")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Gallery)
                    .WithMany(p => p.GalleryPhoto)
                    .HasForeignKey(d => d.GalleryId)
                    .HasConstraintName("FK_GalleryPhoto_Gallery");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.GalleryPhoto)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_GalleryPhoto_Photo");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Menu_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasIndex(e => new { e.MenuId, e.Label })
                    .HasName("UQ_MenuItem_MenuId_Label")
                    .IsUnique();

                entity.Property(e => e.Label)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.MenuItem)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_MenuItem_Menu");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.HasIndex(e => e.Permalink)
                    .HasName("UQ__Page__58FFE96490270724")
                    .IsUnique();

                entity.HasIndex(e => e.Title)
                    .HasName("UQ__Page__2CB664DC087B0D73")
                    .IsUnique();

                entity.Property(e => e.Content)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Permalink)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PagePhoto>(entity =>
            {
                entity.HasKey(e => new { e.PageId, e.PhotoId });

                entity.HasIndex(e => new { e.PageId, e.PhotoId })
                    .HasName("UQ_PagePhoto_PageId_PhotoId")
                    .IsUnique();

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PagePhoto)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_PagePhoto_Page");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.PagePhoto)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_PagePhoto_Photo");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasIndex(e => new { e.TemplateId, e.Name })
                    .HasName("UQ_Section_TemplateId_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.Section)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK_Template_TemplateId");
            });

            modelBuilder.Entity<SectionItem>(entity =>
            {
                entity.Property(e => e.Body)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.SectionItem)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_SectionItem_Section");
            });

            modelBuilder.Entity<SectionItemPhoto>(entity =>
            {
                entity.HasKey(e => new { e.SectionItemId, e.PhotoId });

                entity.HasIndex(e => new { e.SectionItemId, e.PhotoId })
                    .HasName("UQ_SectionItemPhoto_SectionId_PhotoId")
                    .IsUnique();

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.SectionItemPhoto)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_SectionItemPhoto_Photo");

                entity.HasOne(d => d.SectionItem)
                    .WithMany(p => p.SectionItemPhoto)
                    .HasForeignKey(d => d.SectionItemId)
                    .HasConstraintName("FK_SectionItemPhoto_Section");
            });

            modelBuilder.Entity<SectionItemProperty>(entity =>
            {
                entity.HasKey(e => new { e.SectionPropertyId, e.SectionItemId });

                entity.HasIndex(e => new { e.SectionPropertyId, e.SectionItemId })
                    .HasName("UQ_SectionItemProperty_SectionPropertyId_SectionItemId")
                    .IsUnique();

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SectionItem)
                    .WithMany(p => p.SectionItemProperty)
                    .HasForeignKey(d => d.SectionItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SectionItemProperty_SectionItem");

                entity.HasOne(d => d.SectionProperty)
                    .WithMany(p => p.SectionItemProperty)
                    .HasForeignKey(d => d.SectionPropertyId)
                    .HasConstraintName("FK_SectionItemProperty_SectionProperty");
            });

            modelBuilder.Entity<SectionProperty>(entity =>
            {
                entity.HasIndex(e => new { e.SectionId, e.Name })
                    .HasName("UQ_Section_SectionId_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.SectionProperty)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_SectionProperty_Section");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Group)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Tag_Name")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Template>(entity =>
            {
                entity.HasIndex(e => e.Name)
                    .HasName("UQ_Template_Name")
                    .IsUnique();

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TemplateSetting>(entity =>
            {
                entity.HasKey(e => e.SettingId);

                entity.HasIndex(e => new { e.TemplateId, e.Key })
                    .HasName("UQ_TemplateSetting_TemplateId_Key")
                    .IsUnique();

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Template)
                    .WithMany(p => p.TemplateSetting)
                    .HasForeignKey(d => d.TemplateId)
                    .HasConstraintName("FK_TemplateSetting_Template");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("UQ_User_Email")
                    .IsUnique();

                entity.HasIndex(e => new { e.FirstName, e.LastName })
                    .HasName("UQ_User_FirstName_LastName")
                    .IsUnique();

                entity.Property(e => e.ConfirmationCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmationExpiry).HasColumnType("datetime");

                entity.Property(e => e.DateConfirmed).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastFailedPasswordAttempt).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.LastPasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.LastSessionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordExpiry).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });
        }
    }
}
