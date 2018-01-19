using System;
using System.Collections.Generic;
using System.IO;
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
        EF.SampleDbContext context;

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
            await context.Attachment.AddAsync(attachment);
            await context.SaveChangesAsync();

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

        public Attachment(EF.SampleDbContext dbcontext)
        {
            context = dbcontext;
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
    }
}
