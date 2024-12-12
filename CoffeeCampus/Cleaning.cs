namespace CoffeeCampus
{
    public class Cleaning
    {
        public int CleaningID { get; set; }
        public DateTime DateTime { get; set; }
        public string Responsible  { get; set; } 

        public int MachineID { get; set; } 

        public Machine Machine { get; set; }    

    }
}
