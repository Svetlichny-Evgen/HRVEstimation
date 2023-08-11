using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.Threading.Tasks;

namespace HRVEstimation
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<PersonModel>();
            db.CreateTableAsync<MedicalExamResultModel>();
        }
        public async Task<int> CreatePerson(PersonModel person)
        {
            return await db.InsertAsync(person);
        }
        public async Task<List<PersonModel>> ReadPersons()
        {
            return await db.Table<PersonModel>().ToListAsync();
        }
        public Task<int> UpdatePerson(PersonModel person)
        {
            return db.UpdateAsync(person);
        }
        public Task DeletePerson(PersonModel person)
        {
            return db.DeleteAsync(person);
        }

        public async Task<int> CreateResult(MedicalExamResultModel result)
        {
            return await db.InsertAsync(result);
        }

        public async Task<MedicalExamResultModel> ReadResult(int resultId)
        {
            return await db.FindAsync<MedicalExamResultModel>(resultId);
        }

        public Task<int> UpdateResult(MedicalExamResultModel result)
        {
            return db.UpdateAsync(result);
        }

        public Task DeleteResult(MedicalExamResultModel result)
        {
            return db.DeleteAsync(result);
        }

        public void AddPersonAndMedicalExamResult(PersonModel person, MedicalExamResultModel medicalExamResult)
        {
            db.InsertAsync(person);

            medicalExamResult.PersonId = person.Id;
            db.InsertAsync(medicalExamResult);
        }

        public async Task<MedicalExamResultModel> GetMedicalExamResultByPersonId(int personId)
        {
            return await db.Table<MedicalExamResultModel>()
                                   .FirstOrDefaultAsync(result => result.PersonId == personId);
        }


    }
}
