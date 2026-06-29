using Microsoft.EntityFrameworkCore;
using Project.Data.DBEntities;
using Project.Models.Pets;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Project.Persistence.Repository
{
    public class PetRepository : GenericRepository<PetInfo>
    {

        private readonly ProjectDbContext _db;

        public PetRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }

        //Get Pet List DB Work
        public List<PetsViewModel> GetPetAll()
        {

            var petInfoList = _db.PetInfo
                    .Select(x => new PetsViewModel()
                    {
                        Address = x.Address,
                        ContactNumber = x.ContactNumber,
                        PetOwnerId = x.UserID != null ? x.UserID.Value : Guid.Empty,
                        IsMissing = x.IsMissing,
                        PName = x.PName,
                        PSex = x.PSex,
                        Id = x.Id,
                        Lat = x.Lat,
                        Long = x.Long,
                        CreatedOn = x.CreatedOn,
                        MicrochipNumber = x.MicrochipNumber,
                        PetTypeId = x.PetTypeId,
                        LicenceNumber = x.LicenceNumber,
                        IssuingAuthority = x.IssuingAuthority,
                        Colour = x.Colour,
                        Breeder = x.Breeder,
                        BreedDescription = x.BreedDescription,
                        DateOfBirth = x.DateOfBirth,
                        PetTypeName = x.PetTypeMaster != null ? x.PetTypeMaster.TypeName : null,
                    }).ToList();
            return petInfoList;
        }

        //Get Pet List DB Work
        public List<PetsViewModel> GetPetList(Guid userId)
        {
            var petInfoList = _db.PetInfo
                    .Where(x => x.UserID == userId)
                    .Select(x => new PetsViewModel()
                    {
                        Address = x.Address,
                        ContactNumber = x.ContactNumber,
                        PetOwnerId = x.UserID.Value,
                        IsMissing = x.IsMissing,
                        PName = x.PName,
                        PSex = x.PSex,
                        Id = x.Id,
                        Lat = x.Lat,
                        Long = x.Long,
                        NoseImagePath = x.NoseImagePath,
                        FullBodyImagePath = x.FullBodyImagePath,
                        FaceImagePath = x.FaceImagePath,
                        CreatedOn = x.CreatedOn,
                        MicrochipNumber = x.MicrochipNumber,
                        PetTypeId = x.PetTypeId,
                        LicenceNumber = x.LicenceNumber,
                        IssuingAuthority = x.IssuingAuthority,
                        Colour = x.Colour,
                        Breeder = x.Breeder,
                        BreedDescription = x.BreedDescription,
                        DateOfBirth = x.DateOfBirth,
                        PetTypeName = x.PetTypeMaster != null ? x.PetTypeMaster.TypeName : null,
                    }).ToList();
            return petInfoList;
        }

        //public PetInfo GetPetData(string dataSecienceId)
        //{
        //    return _db.PetInfo.Where(x => x.PDataScienceId == dataSecienceId).FirstOrDefault();
        //}


        public PetsViewModel GetPetData(Guid petid)
        {
            var petData = _db.PetInfo.Include(x => x.PetTypeMaster).Where(x => x.Id == petid).FirstOrDefault();
            if (petData is not null)
            {
                return new PetsViewModel()
                {
                    Address = petData.Address,
                    ContactNumber = petData.ContactNumber,
                    PetOwnerId = petData.UserID.Value,
                    IsMissing = petData.IsMissing,
                    PName = petData.PName,
                    PSex = petData.PSex,
                    Id = petData.Id,
                    Lat = petData.Lat,
                    Long = petData.Long,
                    NoseImagePath = petData.NoseImagePath,
                    FullBodyImagePath = petData.FullBodyImagePath,
                    FaceImagePath = petData.FaceImagePath,
                    MicrochipNumber = petData.MicrochipNumber,
                    PetTypeId = petData.PetTypeId,
                    LicenceNumber = petData.LicenceNumber,
                    IssuingAuthority = petData.IssuingAuthority,
                    Colour = petData.Colour,
                    Breeder = petData.Breeder,
                    BreedDescription = petData.BreedDescription,
                    DateOfBirth = petData.DateOfBirth,
                    PetTypeName = petData.PetTypeMaster?.TypeName,
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get Pet Detail For Admin
        /// </summary>
        /// <param name="petid"></param>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public PetsViewModel GetPetDataByStringId(string petid, string baseURL)
        {
            var petData = _db.PetInfo.Where(x => x.Id.ToString() == petid).FirstOrDefault();
            if (petData is not null)
            {
                return new PetsViewModel()
                {
                    Address = petData.Address,
                    ContactNumber = petData.ContactNumber,
                    PetOwnerId = petData.UserID.Value,
                    IsMissing = petData.IsMissing,
                    PName = petData.PName,
                    PSex = petData.PSex,
                    Id = petData.Id,
                    Lat = petData.Lat,
                    Long = petData.Long,
                    MicrochipNumber = petData.MicrochipNumber,
                    PetTypeId = petData.PetTypeId,
                    LicenceNumber = petData.LicenceNumber,
                    IssuingAuthority = petData.IssuingAuthority,
                    Colour = petData.Colour,
                    Breeder = petData.Breeder,
                    BreedDescription = petData.BreedDescription,
                    DateOfBirth = petData.DateOfBirth,
                    NoseImagePath = baseURL + petData.NoseImagePath,
                    FullBodyImagePath = baseURL + petData.FullBodyImagePath
                };
            }
            else
            {
                return null;
            }
        }

        public PetsViewModel GetPetData(String microChipNumber)
        {
            var petData = _db.PetInfo.Where(x => x.MicrochipNumber == microChipNumber).FirstOrDefault();
            if (petData is not null)
            {
                return new PetsViewModel()
                {
                    Address = petData.Address,
                    ContactNumber = petData.ContactNumber,
                    PetOwnerId = petData.UserID.Value,
                    IsMissing = petData.IsMissing,
                    PName = petData.PName,
                    PSex = petData.PSex,
                    Id = petData.Id,
                    Lat = petData.Lat,
                    Long = petData.Long,
                    MicrochipNumber = petData.MicrochipNumber,
                    PetTypeId = petData.PetTypeId,
                    LicenceNumber = petData.LicenceNumber,
                    IssuingAuthority = petData.IssuingAuthority,
                    Colour = petData.Colour,
                    Breeder = petData.Breeder,
                    BreedDescription = petData.BreedDescription,
                    DateOfBirth = petData.DateOfBirth,
                };
            }
            else
            {
                return null;
            }
        }


        public PetsViewModel GetPetDataByPetId(Guid petId)
        {
            var petData = _db.PetInfo.Where(x => x.Id == petId).FirstOrDefault();
            if (petData is not null)
            {
                return new PetsViewModel()
                {
                    Address = petData.Address,
                    ContactNumber = petData.ContactNumber,
                    PetOwnerId = petData.UserID.Value,
                    IsMissing = petData.IsMissing,
                    PName = petData.PName,
                    PSex = petData.PSex,
                    Id = petData.Id,
                    Lat = petData.Lat,
                    Long = petData.Long,
                    MicrochipNumber = petData.MicrochipNumber,
                    PetTypeId = petData.PetTypeId,
                    LicenceNumber = petData.LicenceNumber,
                    IssuingAuthority = petData.IssuingAuthority,
                    Colour = petData.Colour,
                    Breeder = petData.Breeder,
                    BreedDescription = petData.BreedDescription,
                    DateOfBirth = petData.DateOfBirth,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> UpdateIsMissingAsync(Guid petId, bool status)
        {
            // Create a new instance of the entity with the primary key
            var petToUpdate = new PetInfo { Id = petId };

            // Attach the entity to the context
            _db.PetInfo.Attach(petToUpdate);

            // Update only the IsMissing field
            petToUpdate.IsMissing = status;

            // Mark the property as modified
            _db.Entry(petToUpdate).Property(p => p.IsMissing).IsModified = true;

            // Save changes to the database
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0;
        }

        public async Task<string> UpdatePetIsMissingNReturnName(Guid petId, bool status)
        {
            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var petModel = await _db.PetInfo.FirstOrDefaultAsync(p => p.Id == petId);

            if (petModel != null)
            {
                // Update the relevant properties
                petModel.IsMissing = status;
            }
            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0 ? petModel.PName : ""; // Return true if at least one row was affected
        }

        public async Task<string> SavePetDataAsync(PetsViewModel petData)
        {
            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            var petModel = new PetInfo
            {
                PName = petData.PName,
                ContactNumber = petData.ContactNumber,
                Address = petData.Address,
                CreatedOn = DateTime.UtcNow,
                PSex = petData.PSex,
                UserID = petData.PetOwnerId,
                IsMissing = false,
                PetTypeId = petData.PetTypeId,
                NoseImagePath = petData.NoseImagePath,
                FullBodyImagePath = petData.FullBodyImagePath,
                FaceImagePath = petData.FaceImagePath,
                Lat = petData.Lat,
                Long = petData.Long,
                MicrochipNumber = petData.MicrochipNumber,
                LicenceNumber = petData.LicenceNumber,
                IssuingAuthority = petData.IssuingAuthority,
                Colour = petData.Colour,
                Breeder = petData.Breeder,
                DateOfBirth = petData.DateOfBirth.HasValue ? petData.DateOfBirth.Value : new DateTime(),
                BreedDescription = petData.BreedDescription,
            };

            // Add the pet model to the database
            var res = await this._db.Set<PetInfo>().AddAsync(petModel);


            // Save changes and determine success
            var affectedRows = await this._db.SaveChangesAsync();
            return affectedRows > 0 ? res.Entity.Id.ToString() : ""; // Return true if at least one row was affected
        }



        public async Task<bool> UpdatePetDataAsync(PetsViewModel petData)
        {
            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var petModel = await _db.PetInfo.FirstOrDefaultAsync(p => p.Id == petData.Id);

            if (petModel == null)
            {
                throw new InvalidOperationException("Pet record not found for the given ID.");
            }

            // Update the relevant properties
            petModel.PName = petData.PName;
            petModel.ContactNumber = petData.ContactNumber;
            petModel.Address = petData.Address;
            petModel.PSex = petData.PSex;
            petModel.Lat = petData.Lat;
            petModel.Long = petData.Long;
            petModel.Colour = petData?.Colour;
            petModel.Breeder = petData?.Breeder;
            petModel.BreedDescription = petData?.BreedDescription;
            petModel.IssuingAuthority = petData?.IssuingAuthority;
            petModel.LicenceNumber = petData?.LicenceNumber;
            petModel.PetTypeId = petData.PetTypeId;
            petModel.FaceImagePath = petData.FaceImagePath;
            petModel.NoseImagePath = petData.NoseImagePath;
            petModel.FullBodyImagePath = petData.FullBodyImagePath;

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }


        public async Task<bool> DeletePetAsync(Guid petId)
        {
            // Validate input
            if (petId == Guid.Empty)
            {
                throw new ArgumentException("Pet Id cannot be empty.", nameof(petId));
            }

            // Find the pet in the database
            var petData = await _db.PetInfo.FirstOrDefaultAsync(x => x.Id == petId);
            if (petData == null)
            {
                // Pet not found
                return false;
            }

            // Remove the pet
            _db.PetInfo.Remove(petData);

            // Save changes and determine success
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if the deletion was successful
        }
    }
}
