using CoffeeCampus.Data;
using CoffeeCampus.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


namespace CoffeeCampus.Services
{
    public class NotificationService
    {
        private readonly CoffeeCampusDbContext _context;


        public NotificationService(CoffeeCampusDbContext context)
        {
            _context = context;
        }


        public async Task<string> GetHoseChangeNotificationAsync(int coffeeMachineId)
        {
            var coffeeMachine = await _context.CoffeeMachines
                .Include(m => m.HoseChanges)
                .FirstOrDefaultAsync(m => m.CoffeeMachineID == coffeeMachineId);


            if (coffeeMachine == null) { return "Coffee machine not found"; }


            var lastChange = coffeeMachine.HoseChanges
                 .OrderByDescending(hc => hc.ChangeDate)
                  .FirstOrDefault();


            if (lastChange == null)
            {
                return "Slangen skal skiftes! Denne er første gang."; // No previous changes.
            }


            DateTime nextChangeDate = lastChange.ChangeDate.AddMonths(3);


            if (DateTime.Now >= nextChangeDate)
            {
                return "Slangen skal skiftes!";
            }
            else
            {
                TimeSpan timeRemaining = nextChangeDate - DateTime.Now;
                return $"Slangen skal skiftes om {timeRemaining.Days} dage";
            }
        }



        public async Task MarkHoseChangeAsync(int coffeeMachineId)
        {
            var coffeeMachine = await _context.CoffeeMachines.FirstOrDefaultAsync(m => m.CoffeeMachineID == coffeeMachineId);
            if (coffeeMachine == null) return;  // Handle missing machine

            var hoseChange = new HoseChange { ChangeDate = DateTime.Now, CoffeeMachineId = coffeeMachineId };
            _context.HoseChanges.Add(hoseChange);
            await _context.SaveChangesAsync(); // Save changes

        }
    }
}