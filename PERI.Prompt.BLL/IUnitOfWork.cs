using PERI.Prompt.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PERI.Prompt.BLL
{
    public interface IUnitOfWork
    {
        IRepository<EF.Attachment> AttachmentRepository { get; }
        IRepository<EF.Blog> BlogRepository { get; }
        IRepository<EF.BlogAttachment> BlogAttachmentRepository { get; }
        IRepository<EF.BlogCategory> BlogCategoryRepository { get; }
        IRepository<EF.BlogPhoto> BlogPhotoRepository { get; }
        IRepository<EF.BlogSortOrder> BlogSortOrderRepository { get; }
        IRepository<EF.BlogTag> BlogTagRepository { get; }
        IRepository<EF.Category> CategoryRepository { get; }
        IRepository<EF.ChildMenuItem> ChildMenuItemRepository { get; }
        IRepository<EF.Event> EventRepository { get; }
        IRepository<EF.EventPhoto> EventPhotoRepository { get; }
        IRepository<EF.Gallery> GalleryRepository { get; }
        IRepository<EF.GalleryPhoto> GalleryPhotoRepository { get; }
        IRepository<EF.Menu> MenuRepository { get; }
        IRepository<EF.MenuItem> MenuItemRepository { get; }
        IRepository<EF.Page> PageRepository { get; }
        IRepository<EF.PagePhoto> PagePhotoRepository { get; }
        IRepository<EF.Photo> PhotoRepository { get; }
        IRepository<EF.Role> RoleRepository { get; }
        IRepository<EF.Section> SectionRepository { get; }
        IRepository<EF.SectionItem> SectionItemRepository { get; }
        IRepository<EF.SectionItemPhoto> SectionItemPhotoRepository { get; }
        IRepository<EF.SectionItemProperty> SectionItemPropertyRepository { get; }
        IRepository<EF.SectionProperty> SectionPropertyRepository { get; }
        IRepository<EF.Setting> SettingRepository { get; }
        IRepository<EF.Tag> TagRepository { get; }
        IRepository<EF.Template> TemplateRepository { get; }
        IRepository<EF.TemplateSetting> TemplateSettingRepository { get; }
        IRepository<EF.User> UserRepository { get; }
        IRepository<EF.Visibility> VisibilityRepository { get; }

        /// <summary>
        /// Commits all changes
        /// </summary>
        void Commit();

        Task CommitAsync();

        /// <summary>
        /// Discards all changes that has not been commited
        /// </summary>
        void RejectChanges();
        void Dispose();
    }
}
