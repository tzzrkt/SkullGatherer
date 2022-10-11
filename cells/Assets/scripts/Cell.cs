public class Cell
{
    public bool isWater;
    public bool isSand;
    public bool isGrass;
    public bool hasTrees;
    public bool hasBuild;
    public bool excludeSk;

    public Cell(bool isWater, bool isSand, bool isGrass, bool hasTrees, bool hasBuild, bool excludeSkull) {
        this.isWater = isWater;
        this.isSand = isSand;
        this.isGrass = isGrass;
        this.hasTrees = hasTrees;
        this.hasBuild = hasBuild;
        this.excludeSk = excludeSkull;
    }
}