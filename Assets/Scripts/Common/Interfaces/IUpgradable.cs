namespace Common.Interfaces
{
    public interface IUpgradable
    {
        int SellValue { get; protected set; }
        int UpgradeCost { get; protected set; } 
        void UpgradeTower();
        int SellTower();
    }
}