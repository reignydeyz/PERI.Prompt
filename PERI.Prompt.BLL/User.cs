using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PERI.Prompt.EF;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace PERI.Prompt.BLL
{
    [HandleException]
    public class User : ISampleData<EF.User>
    {
        private readonly IUnitOfWork unitOfWork;

        public User(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public enum Roles { Admin = 1, User = 2 };

        public async Task Activate(int[] ids)
        {
            var res = unitOfWork.UserRepository.Entities.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = null);

            await unitOfWork.CommitAsync();
        }

        public async Task<int> Add(EF.User args)
        {
            var salt = Core.Crypto.GenerateSalt();
            var enc = Core.Crypto.Hash(args.PasswordHash ?? Guid.NewGuid().ToString(), salt);

            // default RoleId
            var roleId = Convert.ToInt16((await unitOfWork.SettingRepository.Entities.FirstAsync(x => x.Key == "Default RoleId")).Value);

            // Generate ConfirmationCode
            Guid g = Guid.NewGuid();
            string guidString = Convert.ToBase64String(g.ToByteArray());
            guidString = guidString.Replace("=", "");
            guidString = guidString.Replace("+", "");

            args.PasswordHash = enc;
            args.PasswordSalt = Convert.ToBase64String(salt);
            args.RoleId = args.RoleId == 0 ? roleId : args.RoleId;
            args.LastSessionId = Guid.NewGuid().ToString();
            args.LastLoginDate = DateTime.Now;
            args.DateCreated = args.LastLoginDate.Value;
            args.ConfirmationCode = guidString;
            args.ConfirmationExpiry = DateTime.Now.AddHours(12);

            unitOfWork.UserRepository.Add(args);
            await unitOfWork.CommitAsync();

            return args.UserId;
        }

        public async Task Deactivate(int[] ids)
        {
            var res = unitOfWork.UserRepository.Entities.Where(x => ids.Contains(x.UserId));

            await res.ForEachAsync(x => x.DateInactive = DateTime.Now);

            await unitOfWork.CommitAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(int[] ids)
        {
            unitOfWork.UserRepository.RemoveRange(unitOfWork.UserRepository.Entities.Where(x => ids.Contains(x.UserId)));
            await unitOfWork.CommitAsync();
        }

        public Task Delete(EF.User args)
        {
            throw new NotImplementedException();
        }

        public async Task Edit(EF.User args)
        {
            var rec = unitOfWork.UserRepository.Entities.First(x => x.UserId == args.UserId);

            if (args.PasswordHash != rec.PasswordHash)
            {
                var salt = Core.Crypto.GenerateSalt();
                var enc = Core.Crypto.Hash(args.PasswordHash, salt);

                rec.PasswordHash = enc;
                rec.PasswordSalt = Convert.ToBase64String(salt);
            }
            rec.RoleId = args.RoleId;
            rec.LastPasswordChanged = args.LastPasswordChanged;
            rec.DateConfirmed = args.DateConfirmed;
            rec.ConfirmationCode = args.ConfirmationCode;
            rec.ConfirmationExpiry = args.ConfirmationExpiry;
            rec.DateInactive = args.DateInactive;

            await unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<EF.User>> Find(EF.User args)
        {
            return await unitOfWork.UserRepository.Entities
            .Include(x => x.Role)
            .Where(x => x.FirstName.Contains(args.FirstName ?? x.FirstName) 
            && x.LastName.Contains(args.LastName ?? x.LastName) 
            && x.Email == (args.Email ?? x.Email)).ToListAsync();
        }

        public async Task<EF.User> Get(EF.User args)
        {
            var rec = await unitOfWork.UserRepository.Entities.Include(x => x.Role).FirstOrDefaultAsync(x => x.UserId == (args.UserId == 0 ? x.UserId : args.UserId)
            && x.Email == (args.Email ?? x.Email));

            return rec;
        }
    }
}
