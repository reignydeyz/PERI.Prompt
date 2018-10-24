using Microsoft.EntityFrameworkCore;
using PERI.Prompt.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EF.SampleDbContext _dbContext;

        public IRepository<EF.Attachment> AttachmentRepository => new GenericRepository<EF.Attachment>(_dbContext);
        public IRepository<EF.Blog> BlogRepository => new GenericRepository<EF.Blog>(_dbContext);
        public IRepository<EF.BlogAttachment> BlogAttachmentRepository => new GenericRepository<EF.BlogAttachment>(_dbContext);
        public IRepository<EF.BlogCategory> BlogCategoryRepository => new GenericRepository<EF.BlogCategory>(_dbContext);
        public IRepository<EF.BlogPhoto> BlogPhotoRepository => new GenericRepository<EF.BlogPhoto>(_dbContext);
        public IRepository<EF.BlogSortOrder> BlogSortOrderRepository => new GenericRepository<EF.BlogSortOrder>(_dbContext);
        public IRepository<EF.BlogTag> BlogTagRepository => new GenericRepository<EF.BlogTag>(_dbContext);
        public IRepository<EF.Category> CategoryRepository => new GenericRepository<EF.Category>(_dbContext);
        public IRepository<EF.ChildMenuItem> ChildMenuItemRepository => new GenericRepository<EF.ChildMenuItem>(_dbContext);
        public IRepository<EF.Event> EventRepository => new GenericRepository<EF.Event>(_dbContext);
        public IRepository<EF.EventPhoto> EventPhotoRepository => new GenericRepository<EF.EventPhoto>(_dbContext);
        public IRepository<EF.Gallery> GalleryRepository => new GenericRepository<EF.Gallery>(_dbContext);
        public IRepository<EF.GalleryPhoto> GalleryPhotoRepository => new GenericRepository<EF.GalleryPhoto>(_dbContext);
        public IRepository<EF.Menu> MenuRepository => new GenericRepository<EF.Menu>(_dbContext);
        public IRepository<EF.MenuItem> MenuItemRepository => new GenericRepository<EF.MenuItem>(_dbContext);
        public IRepository<EF.Page> PageRepository => new GenericRepository<EF.Page>(_dbContext);
        public IRepository<EF.PagePhoto> PagePhotoRepository => new GenericRepository<EF.PagePhoto>(_dbContext);
        public IRepository<EF.Photo> PhotoRepository => new GenericRepository<EF.Photo>(_dbContext);
        public IRepository<EF.Role> RoleRepository => new GenericRepository<EF.Role>(_dbContext);
        public IRepository<EF.Section> SectionRepository => new GenericRepository<EF.Section>(_dbContext);
        public IRepository<EF.SectionItem> SectionItemRepository => new GenericRepository<EF.SectionItem>(_dbContext);
        public IRepository<EF.SectionItemPhoto> SectionItemPhotoRepository => new GenericRepository<EF.SectionItemPhoto>(_dbContext);
        public IRepository<EF.SectionItemProperty> SectionItemPropertyRepository => new GenericRepository<EF.SectionItemProperty>(_dbContext);
        public IRepository<EF.SectionProperty> SectionPropertyRepository => new GenericRepository<EF.SectionProperty>(_dbContext);
        public IRepository<EF.Setting> SettingRepository => new GenericRepository<EF.Setting>(_dbContext);
        public IRepository<EF.Tag> TagRepository => new GenericRepository<EF.Tag>(_dbContext);
        public IRepository<EF.Template> TemplateRepository => new GenericRepository<EF.Template>(_dbContext);
        public IRepository<EF.TemplateSetting> TemplateSettingRepository => new GenericRepository<EF.TemplateSetting>(_dbContext);
        public IRepository<EF.User> UserRepository => new GenericRepository<EF.User>(_dbContext);
        public IRepository<EF.Visibility> VisibilityRepository => new GenericRepository<EF.Visibility>(_dbContext);

        public UnitOfWork(EF.SampleDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
        public void RejectChanges()
        {
            foreach (var entry in _dbContext.ChangeTracker.Entries()
                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
