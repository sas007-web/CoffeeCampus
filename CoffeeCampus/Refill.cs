namespace CoffeeCampus
{
    public class Refill
    {
        public int RefillID { get; set; }
        public DateTime DateTime { get; set; }
        public string RefillType { get; set; }
        public string Responsible { get; set; }
        public int CoffeeMachineID { get; set; }
        public CoffeeMachine CoffeeMachine { get; set; }
    }
}
