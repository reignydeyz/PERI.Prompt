using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using Microsoft.EntityFrameworkCore;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Photo : ISampleData<EF.Photo>
    {
        private readonly IUnitOfWork unitOfWork;

        public Photo(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Deletes an specfic image
        /// </summary>
        /// <param name="id">PhotoId</param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            var rec = unitOfWork.PhotoRepository.Entities.First(x => x.PhotoId == id);
            unitOfWork.PhotoRepository.Remove(rec);
            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Delete images
        /// </summary>
        /// <param name="ids">PhotoId</param>
        /// <returns></returns>
        public async Task Delete(int[] ids)
        {
            var res = unitOfWork.PhotoRepository.Entities.Where(x => ids.Contains(x.PhotoId));

            unitOfWork.PhotoRepository.RemoveRange(res);
            await unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Adds and uploads the image to the upload folder
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<int> Add(IHostingEnvironment environment, IFormFile file)
        {
            // Rename the file
            var newFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

            var photo = new EF.Photo();
            photo.Url = "uploads/" + newFileName;
            unitOfWork.PhotoRepository.Add(photo);
            await unitOfWork.CommitAsync();

            // Upload                
            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(environment.WebRootPath, "uploads", file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Rename the file
                System.IO.File.Move(Path.Combine(environment.WebRootPath, "uploads", file.FileName), Path.Combine(environment.WebRootPath, "uploads", newFileName));
            }

            return photo.PhotoId;
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Photo args)
        {
            throw new NotImplementedException();
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Photo args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Photo args)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EF.Photo>> Find(EF.Photo args)
        {
            return await unitOfWork.PhotoRepository.Entities.ToListAsync();
        }

        public async Task<EF.Photo> Get(EF.Photo args)
        {
            return await unitOfWork.PhotoRepository.Entities.FirstAsync(x => x.PhotoId == args.PhotoId);
        }

        public async Task Delete(int id, IHostingEnvironment environment)
        {
            var photo = unitOfWork.PhotoRepository.Entities.First(x => x.PhotoId == id);

            // Remove the photo
            await Task.Run(() => System.IO.File.Delete(Path.Combine(environment.WebRootPath, photo.Url)));

            unitOfWork.PhotoRepository.Remove(photo);
            await unitOfWork.CommitAsync();
        }

        public async Task Delete(int[] ids, IHostingEnvironment environment)
        {
            var res = unitOfWork.PhotoRepository.Entities.Where(x => ids.Contains(x.PhotoId));

            // Remove the photos
            foreach (var p in res)
                await Task.Run(() => System.IO.File.Delete(Path.Combine(environment.WebRootPath, p.Url)));

            unitOfWork.PhotoRepository.RemoveRange(res);
            await unitOfWork.CommitAsync();
        }
    }
}
