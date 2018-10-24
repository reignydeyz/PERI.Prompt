using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PERI.Prompt.EF;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class Attachment : ISampleData<EF.Attachment>
    {
        private readonly IUnitOfWork unitOfWork;

        public Attachment(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Adds and uploads the file to the upload folder
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<int> Add(IHostingEnvironment environment, IFormFile file)
        {
            var guid = Guid.NewGuid().ToString("N");

            var attachment = new EF.Attachment
            {
                Url = "uploads/" + guid + "/" + file.FileName
            };
            await unitOfWork.AttachmentRepository.AddAsync(attachment);
            await unitOfWork.CommitAsync();

            // Upload                
            if (file.Length > 0)
            {
                // Create folder
                Directory.CreateDirectory(Path.Combine(environment.WebRootPath, "uploads", guid));

                using (var fileStream = new FileStream(Path.Combine(environment.WebRootPath, "uploads", guid, file.FileName), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return attachment.AttachmentId;
        }

        public async Task Delete(int id, IHostingEnvironment environment)
        {
            var attachment = unitOfWork.AttachmentRepository.Entities.First(x => x.AttachmentId == id);

            // https://stackoverflow.com/questions/1616353/how-can-i-get-a-directory-from-a-uri
            Uri baseAddress = new Uri(Path.Combine(environment.WebRootPath, attachment.Url));
            Uri directory = new Uri(baseAddress, "."); // "." == current dir, like MS-DOS
            Console.WriteLine(directory.OriginalString);

            // Remove the attachment physical file
            await Task.Run(() => Directory.Delete(directory.OriginalString, true));

            unitOfWork.AttachmentRepository.Remove(attachment);
            await unitOfWork.CommitAsync();
        }

        public Task Activate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task<int> Add(EF.Attachment args)
        {
            throw new NotImplementedException();
        }

        public Task Deactivate(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int[] ids)
        {
            throw new NotImplementedException();
        }

        public Task Delete(EF.Attachment args)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EF.Attachment args)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EF.Attachment>> Find(EF.Attachment args)
        {
            throw new NotImplementedException();
        }

        public Task<EF.Attachment> Get(EF.Attachment args)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids, IHostingEnvironment environment)
        {
            var res = unitOfWork.AttachmentRepository.Entities.Where(x => ids.Contains(x.AttachmentId));

            // Remove the attachments
            foreach (var p in res)
            {
                // https://stackoverflow.com/questions/1616353/how-can-i-get-a-directory-from-a-uri
                Uri baseAddress = new Uri(Path.Combine(environment.WebRootPath, p.Url));
                Uri directory = new Uri(baseAddress, "."); // "." == current dir, like MS-DOS
                Console.WriteLine(directory.OriginalString);

                // Remove the attachment physical file
                await Task.Run(() => Directory.Delete(directory.OriginalString, true));
            }

            unitOfWork.AttachmentRepository.RemoveRange(res);
            await unitOfWork.CommitAsync();
        }
    }
}
