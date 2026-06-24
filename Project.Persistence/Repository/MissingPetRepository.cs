using Microsoft.EntityFrameworkCore;
using Project.Data.DBEntities;
using Project.Models.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project.Persistence.Repository
{
    public  class MissingPetRepository : GenericRepository<MissingPets>
    {

        private readonly ProjectDbContext _db;

        public MissingPetRepository(ProjectDbContext dbContext) : base(dbContext)
        {
            _db = dbContext;
        }


        //Get Pet List DB Work
        public List<MissingPetsViewModel> GetAllMissingPet()
        {

            var missingPets = _db.MissingPet
                    .Select(x => new MissingPetsViewModel()
                    {
                        Id = x.Id,
                        Address = x.Address,
                        Lat = x.Lat,
                        Long = x.Long,
                        LostDate = x.LostDate,
                        PetId = x.PetId != null ? x.PetId.Value : Guid.Empty,
                        FoundBy = x.FoundBy
                    }).ToList();
            return missingPets;
        }


        /// <summary>
        /// Get Missing Pet Data
        /// </summary>
        /// <returns></returns>
        public async Task<MissingPets> GetMissingPet(Guid petId)
        {
            var response = await _db.MissingPet.FirstOrDefaultAsync(p => p.PetId == petId);
            return response;
        }

        /// <summary>
        /// Get Missing Pet Data By MicrochipNumber
        /// </summary>
        /// <returns></returns>
        public async Task<MissingPets> GetMissingPet(String microChipNumber)
        {
            var response = await _db.MissingPet.FirstOrDefaultAsync(p => p.MicrochipNumber == microChipNumber);
            return response;
        }



        /// <summary>
        /// Add Missing Pet Data
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> AddMissingPet(MissingPetRequestViewModel petData)
        {

            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            var petModel = new MissingPets
            {
                Description = petData.Description,
                CreatedOn = DateTime.Now,
                Address = petData.Address,
                Lat = petData.Lat,
                Long = petData.Long,
                LostDate = petData.LostDate,
                PetId = petData.PetId,
                Status = petData.Status,
                
            };

            // Add the pet model to the database
            await this._db.Set<MissingPets>().AddAsync(petModel);

            // Save changes and determine success
            var affectedRows = await this._db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }




        /// <summary>
        /// Add Missing Pet Data
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> AddMissingPetLogs(MissingPets petData)
        {

            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            var petModel = new MissingPetsLogs
            {
                Description = petData.Description,
                CreatedOn = DateTime.Now,
                Address = petData.Address,
                Lat = petData.Lat,
                Long = petData.Long,
                LostDate = petData.LostDate,
                PetId = petData.PetId,
                Status = petData.Status,
                FoundBy    = petData.FoundBy,
                 FoundAddress = petData.FoundAddress,
                 FoundLong = petData.FoundLong,
                 FoundLat = petData.FoundLat
            };

            // Add the pet model to the database
            await this._db.Set<MissingPetsLogs>().AddAsync(petModel);

            // Save changes and determine success
            var affectedRows = await this._db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }


        public async Task<bool> UpdateMissingPetPetDataAsync(MissingPetRequestViewModel petData)
        {
            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            // Find the existing pet record by ID (assuming PetOwnerId is the key)
            var petModel = await _db.MissingPet.FirstOrDefaultAsync(p => p.PetId == petData.PetId);

            if (petModel == null)
            {
                throw new InvalidOperationException("Pet record not found for the given ID.");
            }

            // Update the relevant properties
            petModel.Status = petData.Status;
            petModel.Address = petData.Address;
            petModel.Lat = petData.Lat;
            petModel.Long = petData.Long;
            petModel.LostDate = petData.LostDate;
            

            // Save changes
            var affectedRows = await _db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }

        public MissingPets GetMissingPetData(Guid petId)
        {
            return _db.MissingPet.Where(x => x.PetId == petId).FirstOrDefault();
           
        }

        public async Task<bool> ReportMissingPet(MissingPetRequestViewModel petData)
        {

            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            var MissingPetData = this.GetMissingPetData(petData.PetId);

            if (MissingPetData is null)
            {
                this.AddMissingPet(petData);
            }
            else {
                //Add Missing Data in Logs
                this.AddMissingPetLogs(MissingPetData);

                //Update Missing Pet Data
            }


            var petModel = new MissingPets
            {
                Description = petData.Description,
                CreatedOn = DateTime.Now,
                Address = petData.Address,
                Lat = petData.Lat,
                Long = petData.Long,
                LostDate = petData.LostDate,
                PetId = petData.PetId,
                Status = petData.Status,
            };

            // Add the pet model to the database
            await this._db.Set<MissingPets>().AddAsync(petModel);

            // Save changes and determine success
            var affectedRows = await this._db.SaveChangesAsync();
            return affectedRows > 0; // Return true if at least one row was affected
        }


        /// <summary>
        /// Work for ID Check users (Guest User)
        /// Steps :
        /// 1. Add Guest user in user table
        /// 2. Get UserId and add this as FoundBy Id
        /// 3. Get Missing Pet Data by petId
        /// 4. Add this data in pet Logs table
        /// 5. Update the missing pet data 
        /// 6. Don't change the status of pet
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> GuestUserPetFoundDataAsync(FoundMissingPetRequest petData)
        {

            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            // Fetch existing pet data
            var missingPetData = this.GetMissingPetData(petData.PetId);
            if (missingPetData == null)
            {
                throw new InvalidOperationException($"Pet data with ID {petData.PetId} was not found.");
            }

            // Add logs for the missing pet if applicable
            await this.AddMissingPetLogs(missingPetData);


            //Update Required Entries
            missingPetData.Status = PetStatus.IDCheck;
            missingPetData.FoundBy = petData.FoundBy;
            missingPetData.FoundAddress = petData?.Address;
            missingPetData.FoundLat = petData?.Lat;
            missingPetData.FoundLong = petData?.Long;


            if (missingPetData.CreatedOn == default)
            {
                missingPetData.CreatedOn = DateTime.Now; // Only set if not already defined
            }

            try
            {
                // Save changes
                var affectedRows = await _db.SaveChangesAsync();
                return affectedRows > 0; // Return true if at least one row was affected
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "Failed to update pet status.");

                throw new InvalidOperationException("Failed to update pet status.", ex);
            }
        }


        /// <summary>
        /// Pet Found Data Data add
        /// Owner change the status of missing pet
        /// </summary>
        /// <param name="petData"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> PetFoundDataAsync(FoundMissingPetRequest petData)
        {
            // Validate input
            if (petData == null)
            {
                throw new ArgumentNullException(nameof(petData), "Pet data cannot be null");
            }

            // Fetch existing pet data
            var missingPetData = this.GetMissingPetData(petData.PetId);
            if (missingPetData == null)
            {
                throw new InvalidOperationException($"Pet data with ID {petData.PetId} was not found.");
            }

            // Add logs for the missing pet if applicable
            await this.AddMissingPetLogs(missingPetData);

            // Update pet status and timestamps
            missingPetData.Status = PetStatus.Found;

            if (missingPetData.CreatedOn == default)
            {
                missingPetData.CreatedOn = DateTime.Now; // Only set if not already defined
            }

            try
            {
                // Save changes
                var affectedRows = await _db.SaveChangesAsync();
                return affectedRows > 0; // Return true if at least one row was affected
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                // _logger.LogError(ex, "Failed to update pet status.");

                throw new InvalidOperationException("Failed to update pet status.", ex);
            }
        }
    }
}
