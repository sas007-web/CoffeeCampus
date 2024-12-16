namespace CoffeeCampus
{
    public class HoseChange
    {
        public int HoseChangeID { get; set; }
        public DateTime Date { get; set; }
        public int CoffeeMachineID { get; set; }
        public CoffeeMachine CoffeeMachine { get; set; }
    }
}
