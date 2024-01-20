using System;
using AsyncProg.AsyncAndSync.Models;

namespace AsyncProg.AsyncAndSync.SyncToAsync
{
	public class HealthcareService
	{
        public async Task<Patient> GetPatientInfoAsync(int patientId)
        {
            await Task.Delay(2000);

            Patient patient = new()
            {
                Id = patientId,
                Name = "Smith, Terry",
                PrimaryCareProvider = new Provider
                {
                    Id = 999,
                    Name = "Dr. Amy Ng"
                },
                Medications = new List<Medication>
                {
                    new Medication { Id = 1, Name = "acetaminophen" },// 对乙酰氨基酚
                    new Medication { Id = 2, Name = "hydrocortisone cream" }// 氢化可的松乳膏
                }
            };

            return patient;
        }
    }
}

