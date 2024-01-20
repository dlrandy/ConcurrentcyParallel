using System;
using AsyncProg.AsyncAndSync.Models;

namespace AsyncProg.AsyncAndSync.AsyncToSync
{
	public class PatientLoader
	{

         private PatientService _patientService;
        public PatientLoader()
        {
            _patientService = new PatientService();
        }
        public async Task<Patient?> GetPatientAndMedsAsync(int patientId)
        {
            Patient? patient = null;
            try
            {
                patient = await Task.Run(() => _patientService.GetPatientInfo(patientId));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading patient. Message: {e.Message}");
            }

            if (patient != null)
            {
                patient = await ProcessPatientInfoAsync(patient);
                return patient;
            }
        
                return null;
            
        }
        private async Task<Patient> ProcessPatientInfoAsync(Patient patient)
        {
            await Task.Delay(100);
            // Add additional processing here.
            return patient;
        }
    }
}

