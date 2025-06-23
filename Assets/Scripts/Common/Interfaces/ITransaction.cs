namespace Common.Interfaces
{
    public interface ITransaction
    {
        public void SpendMoney(int cost);

        public bool CanBuy(int cost);
    }
}
