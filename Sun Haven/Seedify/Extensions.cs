namespace Seedify;

public static class CropInfoExtensions
{
    public static CropInfo Clone(this CropInfo src)
    {
        return new CropInfo
        {
          
            cropStages = (int[])src.cropStages.Clone(),
        
            alwaysWatered = src.alwaysWatered,
            neverWatered = src.neverWatered,
            isFlower = src.isFlower,
            farmType = src.farmType,
         
            seasons = [..src.seasons],
            pickUpAble = src.pickUpAble,
            manaInfusable = src.manaInfusable,
            regrowable = src.regrowable
        };
    }
}